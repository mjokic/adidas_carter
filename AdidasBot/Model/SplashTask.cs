using AdidasBot;
using AdidasBot.Model;
using NHtmlUnit;
using NHtmlUnit.Html;
using NHtmlUnit.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace AdidasCarterPro.Model
{
    public class SplashTask : INotifyPropertyChanged
    {

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


        public async Task startTask(string url)
        {
            this.Status = "Running..";

            // starting splash bypass stuff
            WebClient webClient = new WebClient(BrowserVersion.CHROME);
            ProxyConfig proxyConfig = new ProxyConfig(this.Proxy.IP, int.Parse(this.Proxy.Port));
            webClient.Options.ProxyConfig = proxyConfig;
            webClient.Options.JavaScriptEnabled = true;
            webClient.Options.CssEnabled = false;
            webClient.Options.AppletEnabled = false;
            webClient.Options.Timeout = 30000;
            webClient.Options.RedirectEnabled = true;
            webClient.Options.ThrowExceptionOnFailingStatusCode = false;
            webClient.Options.ThrowExceptionOnScriptError = false;
            Console.WriteLine(webClient.CookieManager.IsCookiesEnabled());

            HtmlPage page = null;
            try
            {
                page = webClient.GetPage("http://adidas.com") as HtmlPage;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.Message);
                this.Status = "Something fuckd up!";
                return;
            }

            //HtmlPage page = webClient.GetPage("https://www.whatismyip.com") as HtmlPage;

            //Manager.debugSave("test_" + this.Proxy.IP + "_" + this.Proxy.Port + ".html", page.AsXml());


            HtmlPage page2 = null;
            try
            {
                page2 = (HtmlPage)webClient.GetPage(url);

            }catch(InvalidOperationException ex)
            {
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.Message);
                this.Status = "Something fuckd up!";
                return;
            }



            bool status = runIt(page2, url);

            while (!status)
            {
                status = runIt(page2, url);
            }

            ICollection<Cookie> cookies = webClient.GetCookies(new java.net.URL("http://adidas.com"));
            Console.WriteLine(cookies.Count + "map size");

            this.CookieString = "";
            foreach (Cookie cookie in cookies)
            {
                this.CookieString += cookie.Name + "=" + cookie.Value + ";";
                Console.WriteLine(cookie);
            }

            // if successfully bypassed splash, start timer
            this.Status = "Bypassed!";
            this.timer.Start();
            this.TextColor = Brushes.Green;
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
        private bool runIt(HtmlPage page, string url)
        {
            //HtmlPage page2 = (HtmlPage)webClient.GetPage(url);
            //var cookiez1 = webClient.GetCookies(new java.net.URL("http://adidas.co.uk"));
            //Console.WriteLine(cookiez1.Count);

            //ICollection<Cookie> map = webClient.GetCookies(new java.net.URL("http://adidas.co.uk"));
            //Console.WriteLine(map.Count + "map size");
            //foreach (var item in map)
            //{
            //    Console.WriteLine(item);
            //}
            page.Refresh();
            Console.WriteLine("PAge refreSHed!");

            //Manager.debugSave("page_" + new Random(int.MaxValue).Next() + ".html", page.AsXml());

            Regex r = new Regex("data-sitekey=\"(.*?)\"");
            MatchCollection mc = r.Matches(page.AsXml());

            string siteKey;
            try
            {
                siteKey = mc[0].Groups[1].Value;
            }catch(ArgumentOutOfRangeException ex)
            {
                Console.WriteLine("DO IT AGAIN!");
                return false;
            }

            if (siteKey != null)
            {
                Console.WriteLine("SITE KEY FOUND, splash bypassed?!");
                return true;
            }

            Console.WriteLine("DO IT AGAIN!");
            return false;
            // check for site key..
            // data-sitekey="(.*?)"

        }


        #region Properties
        public int seconds { get; set; }
        public Proxy Proxy { get; set; }
        public DispatcherTimer timer { get; set; }
        public string CookieString { get; set; }

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

    }
}
