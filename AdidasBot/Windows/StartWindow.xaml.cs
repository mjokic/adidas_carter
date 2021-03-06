﻿using AdidasBot;
using AdidasCarterPro.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using wyDay.TurboActivate;

namespace AdidasCarterPro.Windows
{
    /// <summary>
    /// Interaction logic for StartWindow.xaml
    /// </summary>
    public partial class StartWindow : Window
    {
        public StartWindow()
        {
            try
            {
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show(ex.StackTrace);
                App.Current.Shutdown();
            }
        }

        private void checkLicense()
        {
            IsGenuineResult genuine = Manager.TA.IsGenuine(7, 7, true);

            if (genuine == IsGenuineResult.Genuine && Manager.dateCheck() == false)
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
