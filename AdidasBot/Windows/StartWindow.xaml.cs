using AdidasBot;
using Cryptlex;
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

namespace AdidasCarterPro.Windows
{
    /// <summary>
    /// Interaction logic for StartWindow.xaml
    /// </summary>
    public partial class StartWindow : Window
    {
        public StartWindow()
        {
            Mutex m = new Mutex(true, "{c1ac06c5-fd46-4423-8999-acfe7667d009}");
            
            if(m.WaitOne(TimeSpan.Zero, true))
            {
                check();
                m.ReleaseMutex();
            }else
            {
                App.Current.Shutdown();
            }

        }

        private void check()
        {
            LexActivator.SetVersionGUID("44F2F567-E715-1F19-93D5-E376D65434A5", LexActivator.PermissionFlags.LA_USER);

            int statusG;
            statusG = LexActivator.IsProductGenuine();
            LexActivator.SetDayIntervalForServerCheck(1);
            LexActivator.SetGracePeriodForNetworkError(1);
            
            if (statusG == LexActivator.LA_OK)
            {
                MainWindow mw = new MainWindow();
                mw.Show();
            }else
            {
                ActivateWindow aw = new ActivateWindow();
                aw.Show();
            }

            this.Close();

        }

    }
}
