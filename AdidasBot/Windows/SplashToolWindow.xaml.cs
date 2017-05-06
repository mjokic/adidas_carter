using AdidasBot.Model;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace AdidasBot.SplashTool
{
    /// <summary>
    /// Interaction logic for SplashToolWindow.xaml
    /// </summary>
    public partial class SplashToolWindow : Window
    {

        private bool? waitSeconds = false;
        private bool? refreshASAP = false;
        private int waitTime;


        public SplashToolWindow()
        {
           
            InitializeComponent();

            slider1.Maximum = Manager.proxies.Count;

            dataGridSplashTasks.ItemsSource = Manager.splashTasks;
            dataGridSplashTasks.IsReadOnly = true;
            dataGridSplashTasks.SelectionMode = DataGridSelectionMode.Single;
            //dataGridSplashTasks.AutoGenerateColumns = false;
           

        }




        // button actions
        private void buttonStart_Click(object sender, RoutedEventArgs e)
        {

            #region RadioButtons setup
            if (radioButtonRefreshASAP.IsChecked == true)
            {
                this.refreshASAP = true;
            }
            else
            {
                this.refreshASAP = false;
            }

            if (radioButtonRefreshEveryXsec.IsChecked == true)
            {
                this.waitSeconds = true;
            }
            else
            {
                this.waitSeconds = false;
            }
            #endregion

            #region WaitTime setup
            if (textBoxWaitSeconds.Text != "")
            {
                if (!int.TryParse(textBoxWaitSeconds.Text, out this.waitTime)) this.waitTime = 0;
            }
            #endregion


            List <Proxy> proxies = new List<Proxy>();

            string url = textBoxURL.Text;

            int minus = int.Parse(slider1.Value.ToString());
            int tojetaj = Manager.proxies.Count - minus;

            for (int i = 0; i < Manager.proxies.Count - tojetaj; i++)
            {
                proxies.Add(Manager.proxies[i]);
            }


            int threadsNumber = int.Parse(slider.Value.ToString());

            if(proxies.Count != 0)
            {
                foreach (Proxy proxy in proxies)
                {
                    for (int i = 0; i < threadsNumber; i++)
                    {
                        Thread t = new Thread(() => worker(url, proxy.ToString()));
                        t.Start();
                        Manager.splashTasks.Add(t);
                    }
                }

            }
            else
            {
                for (int i = 0; i < threadsNumber; i++)
                {
                    Thread t = new Thread(() => worker(url));
                    t.Start();
                    Manager.splashTasks.Add(t);
                }
            }




        }

        private void buttonStopAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (Thread t in Manager.splashTasks)
            {
                t.Abort();
            }
        }


        // methods
        public void worker(string url, string proxy = null)
        {
            //PhantomClass pc = new PhantomClass(proxy);
            //Manager.phantomTasks.Add(pc);


            // diff thread owns it
            if (this.refreshASAP == true)
            {
                while (true)
                {
                    //pc.getHmac(url);
                    Console.WriteLine("finished....!");
                }

            }else if(this.waitSeconds == true)
            {
                bool status = false;
                while(!status)
                {
                    //status = pc.getHmac(url);
                    Console.WriteLine("finished....!");
                    Console.WriteLine("waiting " + this.waitTime + "seconds..");
                    Thread.Sleep(this.waitTime * 1000);
                    
                }

            }

        }


        private void buttonApplyHmac_Click(object sender, RoutedEventArgs e)
        {
            // apply hmac cookie and siteKey value

            foreach (Job job in Manager.jobs)
            {
                job.Client.DefaultRequestHeaders.Add("Cookie", "Ovo je moj kokiiii");
            }

            //Manager.siteKey = siteKey; new Dummy().SiteKey = siteKey;


        }




        // events
        private void radioButtonRefreshEveryXsec_Checked(object sender, RoutedEventArgs e)
        {
            textBoxWaitSeconds.IsEnabled = true;
        }

        private void radioButtonRefreshASAP_Checked(object sender, RoutedEventArgs e)
        {
            textBoxWaitSeconds.IsEnabled = false;
        }

       
    }
}
