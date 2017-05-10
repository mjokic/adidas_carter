using AdidasBot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for ManualCaptchaSolvingWindow.xaml
    /// </summary>
    public partial class ManualCaptchaSolvingWindow : Window
    {

        public string CaptchaSolution { get; set; }
        public Job Job { get; set; }

        public ManualCaptchaSolvingWindow(Job job)
        {
            this.Job = job;
            InitializeComponent();

        }


        private void loadPage()
        {
            string content = @"
                            <html>
                            <head>
                            <style>
                            form {
                                text-align: center;
                            }
                            body {
                                text-align: center;
                                background-color:#ECEEE3;
  
                            }

                            h1 {
                                text-align: center;
                            }
                            h3 {
                                text-align: center;
                            }
                            div-captcha {
                                    text-align: center;
                            }
                                .g-recaptcha {
                                    display: inline-block;
                                }
                            </style>
                            <h1> Adidas Bot Manual Captcha</h1>

                            <br>
                            Nothing special here. Just solve captcha.</h3>
                            <br><br>
                            <meta name=""referrer"" content=""never""> <script type='text/javascript' src='https://www.google.com/recaptcha/api.js'></script></head>
                            <body oncontextmenu=""return false""><div id=""div-captcha"">
                            <div style=""opacity: 0.9"" class=""g-recaptcha"" data-sitekey=""";

            string part2 = @""">
                            </div></div> <br>
                            <script>

                            window.setInterval(function(){
                                var token = document.getElementById('g-recaptcha-response').value;
                                document.getElementById('g-recaptcha-response').value = '';
                                if(token != '')
                                {
                                var mi = document.createElement(""input"");
                                mi.setAttribute('type', 'text');
                                mi.setAttribute('value', '<!--' + token + '-->');
                                document.body.appendChild(mi);
                            }
                            }, 500);
                            </script>
                            </body></html>
                            ";

            string url = "http://www." + Manager.selectedProfile.Domain.Replace("global.", "");
            //chrome.LoadHtml(content + Manager.siteKey + part2, url);
        }
       

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

    }
}
