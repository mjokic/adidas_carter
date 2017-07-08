using AdidasBot.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;

namespace AdidasCarterPro.Model
{
    public class SplashTask
    {

        public SplashTask(Proxy proxy)
        {
            this.count = 0;

            this.Proxy = proxy;
            this.timer = new DispatcherTimer();
            this.timer.Tick += new EventHandler(timer_tick);
            this.timer.Interval = new TimeSpan(0, 0, 1);

        }

        public int count { get; set; }
        public Proxy Proxy { get; set; }
        public DispatcherTimer timer { get; set; }
        public Button btn { get; set; }


        private void timer_tick(object sender, EventArgs e)
        {
            if (count >= 5) timer.Stop();
            Console.WriteLine("Timer counting down...");

            btn.Content = count;
            
            count++;
        }

    }
}
