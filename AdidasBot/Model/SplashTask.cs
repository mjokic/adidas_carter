using AdidasBot;
using AdidasBot.Model;
using CefSharp;
using CefSharp.OffScreen;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using System.Threading;
using System.Diagnostics;
using System.Windows;

namespace AdidasCarterPro.Model
{
    public class SplashTask : INotifyPropertyChanged
    {

        public ChromiumWebBrowser browser { get; set; }

        public SplashTask(Proxy proxy)
        {
            this.TextColor = Brushes.Red;
            this.seconds = 600;
            this.btnContent = "00:00";

            this.Proxy = proxy;

            this.timer = new DispatcherTimer();
            this.timer.Tick += new EventHandler(timer_tick);
            this.timer.Interval = new TimeSpan(0, 0, 1);

        }


        public void startTask(string url)
        {

            string url1 = "https://www.iplocation.net/find-ip-address";

            // starting splash bypass stuff

            //RequestContext rc = new RequestContext();
            //var dict = new Dictionary<string, object>();
            //dict["mode"] = "fixed_servers";
            //dict["server"] = "scheme://" + this.Proxy.IP + ":" + this.Proxy.Port;
            //string error;
            //rc.SetPreference("proxy", dict, out error);


            browser = new ChromiumWebBrowser(url1);
            //browser.BrowserInitialized += Browser_BrowserInitialized;
            browser.LoadingStateChanged += BrowserLoadingStateChanged;


            //Console.WriteLine(browser.IsBrowserInitialized);

            //while (!browser.IsBrowserInitialized)
            //{
            //    Thread.Sleep(500);
            //    Console.WriteLine("Not Initialized");
            //}

            //Thread.Sleep(1000);


            //await Cef.UIThreadTaskFactory.StartNew(delegate
            //{
            //    string ip = this.Proxy.IP;
            //    string port = this.Proxy.Port;
            //    var rc = this.browser.GetBrowser().GetHost().RequestContext;
            //    var dict = new Dictionary<string, object>();
            //    dict.Add("mode", "fixed_servers");
            //    dict.Add("server", "" + ip + ":" + port + "");
            //    string error;
            //    bool success = rc.SetPreference("proxy", dict, out error);

            //});



        }

        private void Browser_BrowserInitialized(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            Console.WriteLine("Browser initialized!?");

        }

        private void timer_tick(object sender, EventArgs e)
        {
            if (this.seconds < 0)
            {
                timer.Stop();
                //Manager.splashTasks.Remove(this); // don't need this, yet
                return;
            }
            
            TimeSpan t = TimeSpan.FromSeconds(this.seconds);
            this.BtnContent = t.ToString(@"mm\:ss");

            seconds--;
        }

        // temp
        private async void runIt()
        {
            // check for site key..
            // data-sitekey="(.*?)"
            
        }


        #region Properties
        public int seconds { get; set; }
        public Proxy Proxy { get; set; }
        public DispatcherTimer timer { get; set; }
        public string CookieString { get; set; }

        private string btnContent;

        public string BtnContent
        {
            get { return btnContent; }
            set
            {
                btnContent = value;
                OnPropertyChanged("BtnContent");
            }
        }

        private Brush textColor;

        public Brush TextColor
        {
            get { return textColor; }
            set {
                textColor = value;
                OnPropertyChanged("TextColor");
            }
        }

        #endregion

        #region Data binding stuff
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propName)
        {
            PropertyChangedEventHandler h = PropertyChanged;

            if (h != null)
            {
                h(this, new PropertyChangedEventArgs(propName));
            }

        }
        #endregion




        private async void BrowserLoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            // Check to see if loading is complete - this event is called twice, one when loading starts
            // second time when it's finished
            // (rather than an iframe within the main frame).
            if (!e.IsLoading)
            {
                // Remove the load event handler, because we only want one snapshot of the initial page.
                //browser.LoadingStateChanged -= BrowserLoadingStateChanged;

                var task = browser.GetSourceAsync();

                await task.ContinueWith(response =>
                 {
                     Manager.debugSave("aaa_" + this.Proxy.IP + "_" + this.Proxy.Port + ".html", response.Result);

                     App.Current.Dispatcher.Invoke((Action)delegate
                     {
                         Cef.Shutdown();
                     });

                     // if successfully bypassed splash, start timer
                     this.timer.Start();
                     this.TextColor = Brushes.Green;
                 });

            }
        }

    }
}
