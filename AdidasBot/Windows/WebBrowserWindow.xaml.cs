using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AdidasBot.Windows
{
    /// <summary>
    /// Interaction logic for WebBrowserWindow.xaml
    /// </summary>
    public partial class WebBrowserWindow : Window
    {

         
        // for deleting cookies
        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int dwBufferLength);
        private const int INTERNET_OPTION_SUPPRESS_BEHAVIOR = 81;
        private const int INTERNET_SUPPRESS_COOKIE_PERSIST = 3;


        // for setting cookie
        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool InternetSetCookie(string UrlName, string CookieName, string CookieData);


        public WebBrowserWindow(Job job)
        {
            deleteCookies();
            InitializeComponent();
            this.Title = "Browser - ID:" + job.PID + "  Size:" + job.Size;

            // not work for proxy with user/pass
            if(job.Proxy != null && job.Proxy.Username == null && job.Proxy.Password == null) setupProxy(job.Proxy.IP + ":" + job.Proxy.Port);


            string url = "http://www." + Manager.selectedProfile.Domain.Replace("global.", "");

            CookieCollection cookies = job.Handler.CookieContainer.GetCookies(new Uri(url));

            foreach (Cookie koki in cookies)
            {
                string kokiName = Uri.UnescapeDataString(koki.Name);
                string kokiVal = Uri.UnescapeDataString(koki.Value);

                Console.WriteLine(kokiName);
                Console.WriteLine(kokiVal);
                InternetSetCookie(url, koki.Name, koki.Value);
            }


            string cartUrl = url + "/on/demandware.store/" + Manager.selectedProfile.InUrlLong + "/" 
                + Manager.selectedProfile.InUrlShort + "/Cart-Show";
            Console.WriteLine(cartUrl);
            webBrowser.Navigate(cartUrl, null, null, "User-Agent:"+job.UserAgent);


            // hiding js alert bullshit?
            dynamic activeX = webBrowser.GetType().InvokeMember("ActiveXInstance",
                    BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                    null, webBrowser, new object[] { });

            activeX.Silent = true;

        }

        // cookies delete
        private void deleteCookies()
        {
            var lpBuffer = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(int)));
            Marshal.StructureToPtr(INTERNET_SUPPRESS_COOKIE_PERSIST, lpBuffer, true);

            InternetSetOption(IntPtr.Zero, INTERNET_OPTION_SUPPRESS_BEHAVIOR, lpBuffer, sizeof(int));

            Marshal.FreeCoTaskMem(lpBuffer);
        }

        // proxy setup
        private void setupProxy(string strProxy)
        {
            const int INTERNET_OPTION_PROXY = 38;
            const int INTERNET_OPEN_TYPE_PROXY = 3;

            Struct_INTERNET_PROXY_INFO struct_IPI;


            // Filling in structure
            struct_IPI.dwAccessType = INTERNET_OPEN_TYPE_PROXY;
            struct_IPI.proxy = Marshal.StringToHGlobalAnsi(strProxy);
            struct_IPI.proxyBypass = Marshal.StringToHGlobalAnsi("local");

            // Allocating memory
            IntPtr intptrStruct = Marshal.AllocCoTaskMem(Marshal.SizeOf(struct_IPI));

            // Converting structure to IntPtr
            Marshal.StructureToPtr(struct_IPI, intptrStruct, true);

            bool iReturn = InternetSetOption(IntPtr.Zero, INTERNET_OPTION_PROXY, intptrStruct, Marshal.SizeOf(struct_IPI));
        }

        public struct Struct_INTERNET_PROXY_INFO
        {
            public int dwAccessType;
            public IntPtr proxy;
            public IntPtr proxyBypass;
        };



        private void webBrowser_Navigating(object sender, System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            //e.Cancel = true;
            //Console.WriteLine("Radim magiju....");
            ////Console.WriteLine(e.Uri.AbsoluteUri);

            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://google.com/");
            //HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            //webBrowser.NavigateToStream(response.GetResponseStream());

            //if (e.Uri.AbsolutePath != "blank")
            //{


                //string url = "http://www." + Manager.selectedProfile.Domain.Replace("global.", "");
                //Uri currentUri = new Uri(url);

                //currentUri = new Uri(currentUri, e.Uri.AbsolutePath);
                //HttpWebRequest myRequest = (HttpWebRequest)HttpWebRequest.Create(currentUri);

                //HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();

                //Console.WriteLine(currentUri);

                //webBrowser.NavigateToStream(myResponse.GetResponseStream());
                //e.Cancel = true;
            //}
        }

    }
}
