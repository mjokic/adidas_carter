using AdidasBot;
using CefSharp;
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
    /// Interaction logic for CefBrowserWindow.xaml
    /// </summary>
    public partial class CefBrowserWindow : Window
    {

        private string url;
        private string cookieString;

        public CefBrowserWindow(string url, string cookieString)
        {
            this.url = url;
            this.cookieString = cookieString;

            InitializeComponent();

            //ICookieManager cookieManager = Cef.GetGlobalCookieManager();

            //string[] tmp1 = cookieString.Split(';');

            //for (int i = 0; i < tmp1.Length - 1; i++)
            //{
            //    string[] tmp2 = tmp1[i].Replace(" ", "").Split(new char[] { '=' }, 2);

            //    // add this cookie to cookie container
            //    //Cookie cookie =
            //    //    new Cookie(tmp2[0], tmp2[1], "/", Manager.selectedProfile.Domain.Replace("global", ""));

            //    Cookie c = new Cookie();
            //    c.Name = tmp2[0];
            //    c.Value = tmp2[1];
            //    c.Path = "/";
            //    c.Domain = Manager.selectedProfile.Domain.Replace("global", "");

            //    cookieManager.SetCookie(c.Domain, c);
            //}

            browser1.Address = url;

           

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            var t = browser1.EvaluateScriptAsync("document.cookie");
            t.ContinueWith(resp =>
            {
                string resultat = resp.Result.Result as string;
                Console.WriteLine(resultat + "<-- RESULTAT");
            });
        }

        private void browser1_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            if(!e.IsLoading)
            {
                browser1.LoadingStateChanged -= browser1_LoadingStateChanged;

                // execute javascript to loadcookies
                //var task = browser1.EvaluateScriptAsync("document.cookie = " + cookieString);

                //task.ContinueWith(respon =>
                //{
                //    browser1.Address = url;
                //});


            }
        }

        private void browser1_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {

           if(e.Frame.IsMain)
            {

                browser1.FrameLoadEnd -= browser1_FrameLoadEnd;
                
                // execute javascript to loadcookies
                var task = browser1.EvaluateScriptAsync("document.cookie = " + cookieString);

                task.ContinueWith(respon =>
                {

                    respon.ContinueWith(x =>
                    {
                        App.Current.Dispatcher.Invoke((Action)delegate
                        {
                            browser1.Address = url;
                            Console.WriteLine("SHOULD BE READY NOW!");
                        });
                    });
                });
            }
        }
    }
}
