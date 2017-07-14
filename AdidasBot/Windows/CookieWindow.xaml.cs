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
    /// Interaction logic for CookieWindow.xaml
    /// </summary>
    public partial class CookieWindow : MetroWindow
    {
        public CookieWindow(List<Cookie> cookies)
        {
            InitializeComponent();

            string cookieString = "";

            foreach (Cookie cookie in cookies)
            {
                cookieString += cookie.Name + "=" + cookie.Value + ";";
            }

            textBoxSplashTaskCookie.Text = cookieString;
        }

        private void buttonCopySplashTaskCookie_Click(object sender, RoutedEventArgs e)
        {
            // copy to clipboard
            Clipboard.SetText(textBoxSplashTaskCookie.Text);
            buttonCopySplashTaskCookie.Content = "Copied";

        }
    }
}
