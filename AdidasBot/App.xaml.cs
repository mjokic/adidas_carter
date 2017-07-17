using CefSharp;
using CefSharp.OffScreen;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
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
