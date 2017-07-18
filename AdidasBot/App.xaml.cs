using CefSharp;
using CefSharp.OffScreen;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace AdidasBot
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        [STAThread]
        public static void Main()
        {
            try
            {
                var application = new App();
                application.InitializeComponent();
                application.Run();
            }catch(Exception ex)
            {
                File.WriteAllLines("error_log.txt", new string[] { ex.Message, ex.StackTrace, ex.Source });
                MessageBox.Show("SOMETHING FUCKED UP!");
                return;
            }
            
        }

        public App()
        {
            CefSettings settings = new CefSettings();
            settings.BrowserSubprocessPath = @"x86\CefSharp.BrowserSubprocess.exe";

            //settings.MultiThreadedMessageLoop = false;
            //Cef.Initialize(settings, performDependencyCheck: false, browserProcessHandler: null);
            Cef.Initialize(settings, true, null);
        }

    }
}
