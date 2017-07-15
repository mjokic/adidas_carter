using AdidasBot;
using AdidasCarterPro.Model;
using CefSharp;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Interaction logic for CefBrowserWindow.xaml
    /// </summary>
    public partial class CefBrowserWindow : MetroWindow
    {

        private string url;

        public CefBrowserWindow(string url, SplashTask st)
        {
            this.url = url;

            InitializeComponent();

            Cef.UIThreadTaskFactory.StartNew(delegate
            {
                browser1.RequestContext = st.RC;
                Console.WriteLine("RC set up!");

            });


            browser1.Address = url;

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            browser1.Load(this.url);
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            browser1.Load("http://whatismyip.com/");
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            // refresh page
            browser1.Reload(true);
        }
    }
}
