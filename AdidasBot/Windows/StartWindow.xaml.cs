using AdidasBot;
using AdidasCarterPro.Model;
using Cryptlex;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            try { 
                Mutex m = new Mutex(true, "{c1ac06c5-fd46-4423-8999-acfe7667d009}");

                if (m.WaitOne(TimeSpan.Zero, true))
                {
                    bool status = checkUpdates();
                    if (status == false)
                    {
                        m.ReleaseMutex();
                        checkLicense();
                    }
                    else
                    {
                        // open updater and close this process
                        Process.Start("updater.exe");
                        Application.Current.Shutdown();
                    }
                } else
                {
                    App.Current.Shutdown();
                }
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show(ex.StackTrace);
                App.Current.Shutdown();
            }
        }

        private void checkLicense()
        {
            LexActivator.SetVersionGUID("44F2F567-E715-1F19-93D5-E376D65434A5", LexActivator.PermissionFlags.LA_USER);
            LexActivator.SetDayIntervalForServerCheck(1);
            LexActivator.SetGracePeriodForNetworkError(1);

            int statusG;
            statusG = LexActivator.IsProductGenuine();
            Console.WriteLine(statusG);

            if (statusG == LexActivator.LA_OK)
            {
                MainWindow mw = new MainWindow();
                mw.Show();
            } else
            {
                ActivateWindow aw = new ActivateWindow();
                aw.Show();
            }

            this.Close();

        }


        private bool checkUpdates()
        {
            bool status = false;
            Updater updater = new Updater();

            // check if new update is available
            Task t = Task.Run(async () => status = await updater.checkForUpdates());
            t.Wait();

            if (status == true)
            {

                // check if updater already exists


                Console.WriteLine("DOWNLOAD UPDATER CALLED...");
                // if it is download updater and return true
                t = Task.Run(async () => status = await updater.downloadUpdater());
                t.Wait();


            }else
            {
                // if it's not return false
                return false;
            }


            return status;
        }

    }
}
