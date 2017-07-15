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
        private ChromiumWebBrowser browser;
        public CancellationToken token { get; set; }

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


        public void startTask(string url, CancellationToken token)
        {
            this.Retries = 0;
            this.Status = "Initializing...";

            this.url = url;
            this.token = token;

            // starting splash bypass stuff
            this.browser = new ChromiumWebBrowser(address: this.url, requestContext: new RequestContext());

            browser.BrowserInitialized += Browser_BrowserInitialized;
            browser.LoadingStateChanged += BrowserLoadingStateChanged;
            
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

        private bool findSiteKey(string html)
        {
            this.Retries++;

            Regex r = new Regex("data-sitekey=\"(.*?)\"");
            MatchCollection mc = r.Matches(html);

            string siteKey = null;
            try
            {
                siteKey = mc[0].Groups[1].Value;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine("DO IT AGAIN!");
                //this.Status = "FAIL!";
            }

            if (siteKey != null)
            {
                Console.WriteLine("SITE KEY FOUND, splash bypassed?!");
                //this.Status = "SITE KEY FOUND, splash bypassed?!";
                return true;
            }

            this.Status = "Attempting to pass splash...";
            return false;
        }



        private void Browser_BrowserInitialized(object sender, EventArgs e)
        {
            this.Status = "Initialization Completed!";

            //this.browser.Load("https://www.whatismyip.com/");
            //this.browser.Load(url);

            Cef.UIThreadTaskFactory.StartNew(delegate
            {
                string ip = this.Proxy.IP;
                string port = this.Proxy.Port;
                var rc = this.browser.GetBrowser().GetHost().RequestContext;
                var dict = new Dictionary<string, object>();
                dict.Add("mode", "fixed_servers");
                dict.Add("server", "" + ip + ":" + port + "");
                string error;
                bool success = rc.SetPreference("proxy", dict, out error);

            });
            

        }

        private void BrowserLoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            // Check to see if loading is complete - this event is called twice, one when loading starts
            // second time when it's finished
            // (rather than an iframe within the main frame).
            if (!e.IsLoading)
            {

                if(token.IsCancellationRequested)
                {
                    this.browser.LoadingStateChanged -= BrowserLoadingStateChanged;
                    this.Status = "Stopped!";
                    return;
                }

                // Remove the load event handler, because we only want one snapshot of the initial page.
                //this.browser.LoadingStateChanged -= BrowserLoadingStateChanged;

                var task = this.browser.GetSourceAsync();

                task.ContinueWith(response =>
                  {
                      bool status = findSiteKey(response.Result);

                      Console.WriteLine(status + "<-- status...");

                      if (status)
                      {

                          var cm = this.browser.RequestContext.GetDefaultCookieManager(null);
                          var tt = cm.VisitAllCookiesAsync();
                          tt.ContinueWith(x =>
                          {
                              List<Cookie> listaKolaca = x.Result;
                              this.Cookies = listaKolaca;
                             
                              // if successfully bypassed splash, start timer
                              this.timer.Start();
                              this.TextColor = Brushes.Green;

                              this.Status = "Past Splash!";

                          });
                          
                        this.RC = this.browser.RequestContext;
                        this.browser.LoadingStateChanged -= BrowserLoadingStateChanged;
                        return;
                      }

                      this.browser.Reload(true);
                      return;
                      
                  });

            }
        }


        #region Properties
        public string url { get; private set; }
        public int seconds { get; set; }
        public Proxy Proxy { get; set; }
        public DispatcherTimer timer { get; set; }
        public List<Cookie> Cookies { get; set; }
        public RequestContext RC { get; set; }

        private int retries;

        public int Retries
        {
            get { return retries; }
            set { retries = value;
                OnPropertyChanged("Retries");
            }
        }


        private string status;

        public string Status
        {
            get { return status; }
            set { status = value;
                OnPropertyChanged("Status");
            }
        }

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
            set
            {
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


    }
}
