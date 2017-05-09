using AdidasBot;
using CefSharp;
using CefSharp.Wpf;
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
        //private ChromiumWebBrowser chrome;

        public ManualCaptchaSolvingWindow(Job job)
        {
            this.Job = job;
            InitializeComponent();

            //var settings = new CefSettings();
            //settings.BrowserSubprocessPath = @"x86\CefSharp.BrowserSubprocess.exe";

            //Cef.Initialize(settings, performDependencyCheck: false, browserProcessHandler: null);


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
            chrome.LoadHtml(content + Manager.siteKey + part2, url);
        }

        private async void getSolution()
        {
            Thread.Sleep(500);
            Regex r = new Regex("<!--(.*?)-->");

            while (true)
            {
                string source = null;
                try
                {
                    source = await chrome.GetSourceAsync();
                }
                catch (Exception)
                {
                    return;
                }

                var tmp = r.Matches(source);
                try
                {
                    this.CaptchaSolution = tmp[1].Groups[1].Value;
                    this.Job.CaptchaResponse = this.CaptchaSolution;
                    this.Job.Status = "Got captcha response!";
                    Console.WriteLine("FOUND?");
                    // closing z window
                    App.Current.Dispatcher.Invoke((Action)delegate {
                        this.Close();
                    });
                    return;
                }
                catch (ArgumentOutOfRangeException)
                {
                    Console.WriteLine("NOT FOUND!");
                }

                Thread.Sleep(500);
            }


        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Thread.Sleep(500);
            loadPage();

            Task t = Task.Run(() => getSolution());
        }

        //private async void button_Click(object sender, RoutedEventArgs e)
        //{
        //    string source = await chrome.GetSourceAsync();

        //    //string source = "aaaa<!--mkflalfjaakfjakfmak-->a<!--marko-->aabbask";

        //    Regex r = new Regex("<!--(.*?)-->");

        //    var tmp = r.Matches(source);
        //    try
        //    {
        //        this.CaptchaSolution = tmp[1].Groups[1].Value;
        //        this.Job.CaptchaResponse = this.CaptchaSolution;
        //        this.Job.Status = this.CaptchaSolution;
        //        Console.WriteLine("FOUND");
        //        this.Close();
        //    }
        //    catch (ArgumentOutOfRangeException)
        //    {
        //        Console.WriteLine("NOT FOUND!");
        //    }

        //    Thread.Sleep(500);

        //}


    }
}
