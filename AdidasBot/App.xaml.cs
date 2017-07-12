using CefSharp;
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
            //settings.MultiThreadedMessageLoop = false;
            //Cef.Initialize(settings, performDependencyCheck: false, browserProcessHandler: null);
            Cef.Initialize(settings);
        }

    }
}
