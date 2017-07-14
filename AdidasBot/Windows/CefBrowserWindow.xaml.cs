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
        private List<Cookie> cookies;

        public CefBrowserWindow(string url, List<Cookie> cookies, RequestContext rc)
        {
            this.url = url;

            InitializeComponent();

            Cef.UIThreadTaskFactory.StartNew(delegate
            {
                browser1.RequestContext = rc;
                Console.WriteLine("RC set up!");

            });

            //browser1.RequestContext = new RequestContext();
            //var CM = browser1.RequestContext.GetDefaultCookieManager(null);

            //foreach (Cookie cookie in cookies)
            //{
            //    CM.SetCookieAsync("adidas.com", cookie);
            //}

            //Console.WriteLine("COOKIES SET!");

            browser1.Address = url;

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            //var t = browser1.EvaluateScriptAsync("document.cookie");
            //t.ContinueWith(resp =>
            //{
            //    string resultat = resp.Result.Result as string;
            //    Console.WriteLine(resultat + "<-- RESULTAT");
            //});

            var CM = browser1.RequestContext.GetDefaultCookieManager(null);

            var t = CM.VisitAllCookiesAsync();
            t.ContinueWith(x =>
            {
                Console.WriteLine("===");
                List<Cookie> cookies = t.Result;
                foreach (Cookie c in cookies)
                {
                    Console.WriteLine(c.Name + "=" + c.Value + ";");
                }
                Console.WriteLine("===");
            });


        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            browser1.Load("http://whatismyip.com/");
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
                //var task = browser1.EvaluateScriptAsync("document.cookie = " + cookieString);
                var task = browser1.EvaluateScriptAsync("document.cookie = ");

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
