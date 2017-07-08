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
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;

namespace AdidasCarterPro.Model
{
    public class SplashTask : INotifyPropertyChanged
    {

        public SplashTask(Proxy proxy)
        {
            this.seconds = 600;
            this.btnContent = "00:00";

            this.Proxy = proxy;

            this.timer = new DispatcherTimer();
            this.timer.Tick += new EventHandler(timer_tick);
            this.timer.Interval = new TimeSpan(0, 0, 1);

        }


        public void startTask()
        {
            // starting splash bypass stuff


            // if successfully bypassed splash, start timer
            this.timer.Start();
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
        private void runIt(string url)
        {
            WebClient webClient = new WebClient(BrowserVersion.CHROME);
            webClient.Options.JavaScriptEnabled = true;
            webClient.Options.CssEnabled = false;
            webClient.Options.AppletEnabled = false;
            webClient.Options.Timeout = 30000;
            webClient.Options.RedirectEnabled = true;
            webClient.Options.ThrowExceptionOnFailingStatusCode = false;
            webClient.Options.ThrowExceptionOnScriptError = false;
            Console.WriteLine(webClient.CookieManager.IsCookiesEnabled());

            HtmlPage page = webClient.GetPage("http://adidas.co.uk") as HtmlPage;

            //Manager.debugSave("page.html", page.AsXml());
            //Manager.debugSave("page_"+ new Random(99999).NextDouble()+".html", page.WebResponse.ContentAsString);

            var cookiez = webClient.GetCookies(new java.net.URL("http://adidas.co.uk"));
            Console.WriteLine(cookiez.Count);


            HtmlPage page2 = (HtmlPage)webClient.GetPage(url);
            var cookiez1 = webClient.GetCookies(new java.net.URL("http://adidas.co.uk"));
            Console.WriteLine(cookiez.Count);


            ICollection<Cookie> map = webClient.GetCookies(new java.net.URL("http://adidas.co.uk"));
            Console.WriteLine(map.Count + "map size");
            foreach (var item in map)
            {
                Console.WriteLine(item);
            }

            Manager.debugSave("page2.html", page2.AsXml());

            Console.WriteLine("DONE!");
        }



        #region Properties
        public int seconds { get; set; }
        public Proxy Proxy { get; set; }
        public DispatcherTimer timer { get; set; }

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
