using AdidasBot.Model;
using CefSharp;
using CefSharp.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AdidasCarterPro.Windows
{
    /// <summary>
    /// Interaction logic for NewBrowserWindow.xaml
    /// </summary>
    public partial class NewBrowserWindow : Window
    {

        public Proxy proxy { get; set; }
        private int counter = 0;
        private ICollection<Cookie> cookies;

        public NewBrowserWindow(Proxy proxy, ICollection<Cookie> cookies)
        {
            this.proxy = proxy;
            this.cookies = cookies;

            InitializeComponent();

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //Cef.Shutdown();
        }

        private void browsi_IsBrowserInitializedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if(browsi.IsInitialized == true && counter < 1)
            {

                var cookieManager = Cef.GetGlobalCookieManager();

                foreach (Cookie cookie in cookies)
                {
                    cookieManager.SetCookie("http://adidas.com", cookie);
                }


                Cef.UIThreadTaskFactory.StartNew(delegate
                {
                    string ip = proxy.IP;
                    string port = proxy.Port;
                    var rc = browsi.GetBrowser().GetHost().RequestContext;
                    var dict = new Dictionary<string, object>();
                    dict.Add("mode", "fixed_servers");
                    dict.Add("server", "" + ip + ":" + port + "");
                    string error;
                    bool success = rc.SetPreference("proxy", dict, out error);

                });

                //browsi.Address = "https://www.whatismyip.com/";
                browsi.Address = "https://adidas.com/";

                counter++;

            }
        }
    }
}
