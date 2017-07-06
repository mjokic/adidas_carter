using AdidasBot.Model;
using AdidasBot.Model.Captchas;
using AdidasBot.Model.Captchas.AntiCaptchaAPI.Api;
using AdidasBot.Model.Captchas.AntiCaptchaAPI.Helper;
using AdidasBot.SplashTool;
using AdidasBot.Windows;
using AdidasCarterPro.Model;
using AdidasCarterPro.Windows;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.CSharp;
using Microsoft.Win32;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using wyDay.TurboActivate;

namespace AdidasBot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {

        public MainWindow()
        {

            Manager.initialize();

            try
            {
                InitializeComponent();
            }catch(Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
                MessageBox.Show(ex.Source);
                throw;
            }

            //loadVariables();
            loadServerVars();
            radioButton2Captcha.IsChecked = true;

            Task.Run(() => checkForUpdate());


            #region Site Profiles
            Manager.siteProfiles.Add(new SiteProfile("United Kingdom", Manager.adidasVar + ".co.uk", "Sites-adidas-GB-Site", "en_GB", Manager.sizesEnglish));
            Manager.siteProfiles.Add(new SiteProfile("United States", "global."+Manager.adidasVar+".com", "Sites-adidas-US-Site", "en_US", Manager.sizesAmerica));
            Manager.siteProfiles.Add(new SiteProfile("Australia", Manager.adidasVar + ".com.au", "Sites-adidas-AU-Site", "en_AU", Manager.sizesAmerica));
            Manager.siteProfiles.Add(new SiteProfile("Canada", Manager.adidasVar + ".ca", "Sites-adidas-CA-Site", "en_CA", Manager.sizesAmerica));

            Manager.siteProfiles.Add(new SiteProfile("Austria", Manager.adidasVar + ".at", "Sites-adidas-AT-Site", "de_AT", Manager.sizesEurope));
            Manager.siteProfiles.Add(new SiteProfile("Belgium", Manager.adidasVar + ".be", "Sites-adidas-BE-Site", "fr_BE", Manager.sizesEurope));
            Manager.siteProfiles.Add(new SiteProfile("Czech Republic", Manager.adidasVar + ".cz", "Sites-adidas-CZ-Site", "cs_CZ", Manager.sizesEurope));
            Manager.siteProfiles.Add(new SiteProfile("Denmark", Manager.adidasVar + ".dk", "Sites-adidas-DK-Site", "da_DK", Manager.sizesEurope));
            Manager.siteProfiles.Add(new SiteProfile("Germany", Manager.adidasVar + ".de", "Sites-adidas-DE-Site", "de_DE", Manager.sizesEurope));
            Manager.siteProfiles.Add(new SiteProfile("Greece", Manager.adidasVar + ".gr", "Sites-adidas-MLT-Site", "en_GR", Manager.sizesEurope));
            Manager.siteProfiles.Add(new SiteProfile("Spain", Manager.adidasVar + ".es", "Sites-adidas-ES-Site", "es_ES", Manager.sizesEurope));
            Manager.siteProfiles.Add(new SiteProfile("France", Manager.adidasVar + ".fr", "Sites-adidas-FR-Site", "fr_FR", Manager.sizesEurope));
            Manager.siteProfiles.Add(new SiteProfile("Italia", Manager.adidasVar + ".it", "Sites-adidas-IT-Site", "it_IT", Manager.sizesEurope));
            Manager.siteProfiles.Add(new SiteProfile("Ireland", Manager.adidasVar + ".ie", "Sites-adidas-IE-Site", "en_IE", Manager.sizesEnglish));
            Manager.siteProfiles.Add(new SiteProfile("Netherlands", Manager.adidasVar + ".nl", "Sites-adidas-NL-Site", "nl_NL", Manager.sizesEurope));
            Manager.siteProfiles.Add(new SiteProfile("Norway", Manager.adidasVar + ".no", "Sites-adidas-NO-Site", "en_NO", Manager.sizesEurope));
            Manager.siteProfiles.Add(new SiteProfile("Poland", Manager.adidasVar + ".pl", "Sites-adidas-PL-Site", "pl_PL", Manager.sizesEurope));
            //Manager.siteProfiles.Add(new SiteProfile("Russia", Manager.adidasVar + ".ru", "Sites-adidas-RU-Site", "ru_RU", ));
            Manager.siteProfiles.Add(new SiteProfile("Switzerland", Manager.adidasVar + ".ch", "Sites-adidas-CH-Site", "de_CH", Manager.sizesEurope));
            Manager.siteProfiles.Add(new SiteProfile("Slovakia", Manager.adidasVar + ".sk", "Sites-adidas-SK-Site", "sk_SK", Manager.sizesEurope));
            Manager.siteProfiles.Add(new SiteProfile("Finland", Manager.adidasVar + ".fi", "Sites-adidas-FI-Site", "fi_FI", Manager.sizesEurope));
            Manager.siteProfiles.Add(new SiteProfile("Sweeden", Manager.adidasVar + ".se", "Sites-adidas-SE-Site", "sv_SE", Manager.sizesEurope));
            #endregion

            
            // setting up comboBoxSizes
            comboBoxSizes.ItemsSource = Manager.sizes.Keys;

            // setting up comboBoxSites
            comboBoxSites.ItemsSource = Manager.siteProfiles;
            comboBoxSites.DisplayMemberPath = "Name";


            #region dataGridJobs Settings
            dataGridJobs.ItemsSource = Manager.jobs;
            dataGridJobs.AutoGenerateColumns = false;
            dataGridJobs.IsReadOnly = true;
            dataGridJobs.SelectionMode = DataGridSelectionMode.Single;

            DataGridTextColumn c = new DataGridTextColumn();
            c.Header = "PID";
            c.Binding = new Binding("PID");
            c.Width = new DataGridLength(1, DataGridLengthUnitType.Auto);
            dataGridJobs.Columns.Add(c);

            c = new DataGridTextColumn();
            c.Header = "Size";
            c.Binding = new Binding("Size");
            c.Width = new DataGridLength(0, DataGridLengthUnitType.SizeToHeader);
            dataGridJobs.Columns.Add(c);

            c = new DataGridTextColumn();
            c.Header = "Quantity";
            c.Binding = new Binding("Quantity");
            c.Width = new DataGridLength(0, DataGridLengthUnitType.SizeToHeader);
            dataGridJobs.Columns.Add(c);

            c = new DataGridTextColumn();
            c.Header = "Account";
            c.Binding = new Binding("Acc");
            c.Width = new DataGridLength(1, DataGridLengthUnitType.Auto);
            dataGridJobs.Columns.Add(c);

            c = new DataGridTextColumn();
            c.Header = "Proxy";
            c.Binding = new Binding("Proxy");
            c.Width = new DataGridLength(1, DataGridLengthUnitType.Auto);
            dataGridJobs.Columns.Add(c);

            c = new DataGridTextColumn();
            c.Header = "Status";
            c.Binding = new Binding("Status");
            c.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            dataGridJobs.Columns.Add(c);

            c = new DataGridTextColumn();
            c.Header = "Retries";
            c.Binding = new Binding("Retries");
            c.Width = new DataGridLength(1, DataGridLengthUnitType.Auto);
            dataGridJobs.Columns.Add(c);
            #endregion


            #region dataGridInCart Settings
            dataGridInCart.ItemsSource = Manager.inCartJobs;
            dataGridInCart.AutoGenerateColumns = false;
            dataGridInCart.IsReadOnly = true;
            dataGridInCart.SelectionMode = DataGridSelectionMode.Single;

            c = new DataGridTextColumn();
            c.Header = "PID";
            c.Binding = new Binding("PID");
            c.Width = new DataGridLength(1, DataGridLengthUnitType.Auto);
            dataGridInCart.Columns.Add(c);

            c = new DataGridTextColumn();
            c.Header = "Size";
            c.Binding = new Binding("Size");
            c.Width = new DataGridLength(0, DataGridLengthUnitType.SizeToHeader);
            dataGridInCart.Columns.Add(c);

            c = new DataGridTextColumn();
            c.Header = "Quantity";
            c.Binding = new Binding("Quantity");
            c.Width = new DataGridLength(0, DataGridLengthUnitType.SizeToHeader);
            dataGridInCart.Columns.Add(c);

            c = new DataGridTextColumn();
            c.Header = "Account";
            c.Binding = new Binding("Acc");
            c.Width = new DataGridLength(1, DataGridLengthUnitType.Auto);
            dataGridInCart.Columns.Add(c);

            c = new DataGridTextColumn();
            c.Header = "Proxy";
            c.Binding = new Binding("Proxy");
            c.Width = new DataGridLength(1, DataGridLengthUnitType.Auto);
            dataGridInCart.Columns.Add(c);

            c = new DataGridTextColumn();
            c.Header = "Status";
            c.Binding = new Binding("Status");
            c.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            dataGridInCart.Columns.Add(c);

            #endregion


            #region dataGridAccounts Settings
            dataGridAccounts.ItemsSource = Manager.accounts;
            dataGridAccounts.AutoGenerateColumns = false;
            dataGridAccounts.IsReadOnly = true;
            dataGridAccounts.SelectionMode = DataGridSelectionMode.Single;

            c = new DataGridTextColumn();
            c.Header = "Email";
            c.Binding = new Binding("Username");
            c.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            dataGridAccounts.Columns.Add(c);

            c = new DataGridTextColumn();
            c.Header = "Password";
            c.Binding = new Binding("Password");
            c.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            dataGridAccounts.Columns.Add(c);
            #endregion


            #region dataGridProxies Settings
            dataGridProxies.ItemsSource = Manager.proxies;
            dataGridProxies.AutoGenerateColumns = false;
            dataGridProxies.IsReadOnly = true;
            dataGridProxies.SelectionMode = DataGridSelectionMode.Single;

            c = new DataGridTextColumn();
            c.Header = "IP";
            c.Binding = new Binding("IP");
            c.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            dataGridProxies.Columns.Add(c);

            c = new DataGridTextColumn();
            c.Header = "Port";
            c.Binding = new Binding("Port");
            c.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            dataGridProxies.Columns.Add(c);

            c = new DataGridTextColumn();
            c.Header = "Username";
            c.Binding = new Binding("Username");
            c.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            dataGridProxies.Columns.Add(c);

            c = new DataGridTextColumn();
            c.Header = "Password";
            c.Binding = new Binding("Password");
            c.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            dataGridProxies.Columns.Add(c);

            c = new DataGridTextColumn();
            c.Header = "Status";
            c.Binding = new Binding("Status");
            c.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            dataGridProxies.Columns.Add(c);
            #endregion


            #region dataGridCustomHeaders Settings
            dataGridCustomHeaders.ItemsSource = Manager.customHeaders;
            dataGridCustomHeaders.AutoGenerateColumns = false;
            dataGridCustomHeaders.IsReadOnly = true;

            c = new DataGridTextColumn();
            c.Header = "Name";
            c.Binding = new Binding("Key");
            c.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            dataGridCustomHeaders.Columns.Add(c);

            c = new DataGridTextColumn();
            c.Header = "Value";
            c.Binding = new Binding("Value");
            c.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            dataGridCustomHeaders.Columns.Add(c);
            #endregion


        }


        // buton clicks
        // TASKS TAB
        private async void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            string pid = textBoxPId.Text;
            string size = comboBoxSizes.SelectedValue as string;
            string quantityTmp = textBoxQuantity.Text;
            int quantity;

            if (!int.TryParse(quantityTmp, out quantity))
            {
                await this.ShowMessageAsync("Error!", "Quantity needs to be a number!", MessageDialogStyle.Affirmative);
                return;
            }
            else if (size == null)
            {
                await this.ShowMessageAsync("Error!", "Select site from options tab!", MessageDialogStyle.Affirmative);
                OptionsTab.IsSelected = true;
                return;
            }

            // getting sizeCode and creating object
            // setting size property value (display only)
            string sizeCode;
            if (textBoxCustomSize.IsVisible)
            {
                sizeCode = textBoxCustomSize.Text;
                size = sizeCode;
            }
            else
            {
                Manager.sizes.TryGetValue(size, out sizeCode);
            }
            Job job = new Job(pid, sizeCode, quantity);
            job.Size = size;

            Manager.jobs.Add(job);

        }

        private void buttonStart_Click(object sender, RoutedEventArgs e)
        {
            if (buttonStart.Content as string == "Start")
            {

                // display error if site is not selected
                if(Manager.selectedProfile == null) { this.ShowMessageAsync("Error!", "Please select site in options tab!", MessageDialogStyle.Affirmative); return; }
                if(Manager.jobs.Count == 0) { this.ShowMessageAsync("Error!", "No tasks to run!", MessageDialogStyle.Affirmative); return; }
                buttonStart.Content = "Stop";

                Manager.cts = new CancellationTokenSource();
                Manager.ct = Manager.cts.Token;

                foreach (Job j in Manager.jobs)
                {
                    Task t = null;

                    if (Manager.cartAfterCaptcha)
                    {
                        // cartAfterCaptcha Option
                        t = Task.Run(() => cartAfterCaptcha(j), Manager.ct);

                    }
                    else
                    {
                        // normal cart Option
                        t = Task.Run(() => addToCart(j), Manager.ct);

                    }

                    Manager.runningTasks.Add(t);
                    t.ContinueWith(task => checkTasks());


                }

            }
            else
            {
                // stop all tasks here
                Manager.cts.Cancel();
                buttonStart.Content = "Start";

            }

        }

        private void buttonSolveCaptchas_Click(object sender, RoutedEventArgs e)
        {
            //Manager.stopAllTask = false;

            // for each job solve captcha on new thread
            foreach (Job j in Manager.jobs)
            {
                Task.Run(() => solveCaptcha(j));
            }

        }

        private async void buttonRemoveJobs_Click(object sender, RoutedEventArgs e)
        {
            Job selectedJob = dataGridJobs.SelectedItem as Job;
            
            if(selectedJob != null)
            {
                Manager.jobs.Remove(selectedJob);
            }else
            {

                var mb = await this.ShowMessageAsync("Are you sure?", "All tasks will be deleted", MessageDialogStyle.AffirmativeAndNegative, Manager.mdsQustion);

                if (mb == MessageDialogResult.Affirmative)
                {
                    foreach (Job j in Manager.jobs.ToList())
                    {
                        Manager.jobs.Remove(j);
                    }
                }


            }


        }

        private void buttonAssingProxiesWindow_Click(object sender, RoutedEventArgs e)
        {
            if(Manager.proxies.Count == 0)
            {
                this.ShowMessageAsync("Error!", "You don't have proxies!", MessageDialogStyle.Affirmative);
                return;
            }else if(Manager.jobs.Count == 0)
            {
                this.ShowMessageAsync("Error!", "You don't have tasks!", MessageDialogStyle.Affirmative);
                return;
            }

            AssingProxiesWindow apWindow;
            Job selectedJob = dataGridJobs.SelectedItem as Job;
            if(selectedJob != null)
            {
                apWindow = new AssingProxiesWindow(selectedJob);
            }else
            {
                apWindow = new AssingProxiesWindow();
            }

            apWindow.ShowDialog();
            dataGridJobs.SelectedIndex = -1;

        }

        private void buttonAssingAccosWindow_Click(object sender, RoutedEventArgs e)
        {
            if(Manager.accounts.Count == 0)
            {
                this.ShowMessageAsync("Error!", "You don't have any accounts!", MessageDialogStyle.Affirmative);
                return;
            }

            Queue accQueue = new Queue();
            foreach (Account acc in Manager.accounts)
            {
                accQueue.Enqueue(acc);
            }

            foreach (Job j in Manager.jobs)
            {
                if (accQueue.Count == 0) break;
                j.Acc = accQueue.Dequeue() as Account;
                j.Acc.Handler = j.Handler; // nova izmena

                Thread t = new Thread(() => accountLogin(j));
                t.Start();

            }

        }

        private void buttonSendOnSite_Click(object sender, RoutedEventArgs e)
        {
            foreach (Job j in Manager.inCartJobs)
            {
                if (j.Status == "OK" && j.Acc != null)
                {
                    Task.Run(() => sendToSite(j));
                }
            }
        }

        // ACCOUNTS TAB
        private void buttonImportAccs_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Select your accounts file";
            ofd.Filter = "txt files (*.txt)|*.txt";
            ofd.ShowDialog();

            string path = ofd.FileName;

            if (path == "") return;

            string[] lines = File.ReadAllLines(path);


            foreach (string line in lines)
            {
                if (!parseAccount(line))
                {
                    this.ShowMessageAsync("Error!", "Wrong accounts format!", MessageDialogStyle.Affirmative);
                    break;
                }
            }

        }

        private async void buttonRemoveAccounts_Click(object sender, RoutedEventArgs e)
        {
            Account selectedAccount = dataGridAccounts.SelectedItem as Account;
            if (selectedAccount == null && Manager.accounts.Count != 0)
            {
                var mbr = await this.ShowMessageAsync("Are you sure?", "You'll delete all accounts", MessageDialogStyle.AffirmativeAndNegative, Manager.mdsQustion);
                if (mbr == MessageDialogResult.Affirmative)
                {
                    foreach (Account acc in Manager.accounts.ToList())
                    {
                        Manager.accounts.Remove(acc);
                    }
                }
            }
            else
            {
                Manager.accounts.Remove(selectedAccount);
            }




        }

        private void buttonAddAccount_Click(object sender, RoutedEventArgs e)
        {
            string email = textBoxAccountUsername.Text;
            string password = passBoxAccountPassword.Password;

            if(email != "" && password != "")
            {
                Manager.accounts.Add(new Account(email, password));
            }
            else
            {
                this.ShowMessageAsync("Error!", "Enter username/password!", MessageDialogStyle.Affirmative);
            }

        }


        // PROXIES TAB
        private void buttonImportProxies_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Select your proxies file";
            ofd.Filter = "txt files (*.txt)|*.txt";
            ofd.ShowDialog();

            string path = ofd.FileName;

            if (path == "") return;

            string[] lines = File.ReadAllLines(path);


            foreach (string line in lines)
            {
                if (!parseProxy(line))
                {
                    this.ShowMessageAsync("Error!", "Wrong proxy format!", MessageDialogStyle.Affirmative);
                    break;
                }
            }



        }

        private async void buttonRemoveProxy_Click(object sender, RoutedEventArgs e)
        {
            Proxy selectedProxy = dataGridProxies.SelectedItem as Proxy;

            if (selectedProxy == null)
            {

                // if there's not selected proxy
                // we remove all proxies
                var dr = await this.ShowMessageAsync("Are you sure?", "All proxies will be removed", MessageDialogStyle.AffirmativeAndNegative, Manager.mdsQustion);

                if (dr == MessageDialogResult.Affirmative)
                {
                    foreach (Proxy p in Manager.proxies.ToList())
                    {
                        Manager.proxies.Remove(p);

                    }
                }

            }
            else
            {
                Manager.proxies.Remove(selectedProxy);
            }

        }

        private void buttonRemoveFalse_Click(object sender, RoutedEventArgs e)
        {
            foreach (Proxy proxy in Manager.proxies.ToList())
            {
                if(proxy.Status == "False")
                {
                    Manager.proxies.Remove(proxy);
                }
            }
        }

        private void buttonCheckroxy_Click(object sender, RoutedEventArgs e)
        {
            Queue proxyQueue = new Queue();
            foreach (Proxy p in Manager.proxies)
            {
                proxyQueue.Enqueue(p);
            }

            // number of threads - make it customizable
            int tnum = int.Parse(textBlockProxyCheckThreads.Text);

            for(int i = 0; i <= tnum; i++)
            {
                Thread t = new Thread(() => proxyCheckWorker(proxyQueue));
                t.Start();

                //Task taks = Task.Run(async () =>
                //{
                //    await proxyCheckWorker(proxyQueue);

                //});


            }

        }
       

        // OPTIONS TAB
        private void buttonSaveSettings_Click(object sender, RoutedEventArgs e)
        {
            if (radioButton2Captcha.IsChecked == true)
            {
                Manager.use2Captcha = true;
                Manager.useAntiCaptcha = false;
                Manager.api2CaptchaKey = textBoxCaptchaApiKey.Password;
                Manager.myKey = Manager.api2CaptchaKey;
            }
            else if(radioButtonAntiCaptcha.IsChecked == true)
            {
                Manager.use2Captcha = false;
                Manager.useAntiCaptcha = true;
                Manager.apiAntiCaptchaKey = textBoxCaptchaApiKey.Password;
                Manager.myKey = Manager.apiAntiCaptchaKey;
            }



            string captchaSiteKey = textBoxSiteKey.Text;
            Manager.siteKey = captchaSiteKey;

            SiteProfile selectedProfile = comboBoxSites.SelectedItem as SiteProfile;
            if (selectedProfile == null)
            {
                this.ShowMessageAsync("Error!", "Please select country!", MessageDialogStyle.Affirmative);
                return;
            }

            Manager.selectedProfile = selectedProfile;
            Manager.sizes = Manager.selectedProfile.Sizes;

            comboBoxSizes.ItemsSource = Manager.sizes.Keys;

            Manager.customPage = textBoxCustomPage.Text;
            if(Manager.customPage == null || Manager.customPage == string.Empty)
            {
                Manager.customPage = "http://www." + Manager.selectedProfile.Domain;
            }

            Manager.saveToRegistry("siteKey", captchaSiteKey);

            Manager.saveToRegistry("2captchaApiKey", Manager.api2CaptchaKey);
            Manager.saveToRegistry("antiCaptchaApiKey", Manager.apiAntiCaptchaKey);


            this.ShowMessageAsync("Saved!", "Setting saved successfully!", MessageDialogStyle.Affirmative);

        }

        private void buttonTest_Click(object sender, RoutedEventArgs e)
        {
            getBalance();
        }

        private void buttonHeaderAdd_Click(object sender, RoutedEventArgs e)
        {
            string customHeaderName = textBoxHeaderName.Text;
            string customHeaderValue = textBoxHeaderValue.Text;

            if(Manager.customHeaders.ContainsKey(customHeaderName))
            {
                Manager.customHeaders[customHeaderName] = customHeaderValue;
            }else
            {
                Manager.customHeaders.Add(customHeaderName, customHeaderValue);
            }


            dataGridCustomHeaders.Items.Refresh();
            textBoxHeaderName.Text = "";
            textBoxHeaderValue.Text = "";

        }

        private async void dataGridCustomHeaders_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedItem = dataGridCustomHeaders.SelectedItem;
            if (selectedItem != null)
            {
                var mbr = await this.ShowMessageAsync("Are you sure?", "All custom headers will be removed", MessageDialogStyle.AffirmativeAndNegative, Manager.mdsQustion);

                if(mbr == MessageDialogResult.Affirmative)
                {

                    foreach (var key in Manager.customHeaders.Keys.ToList())
                    {
                        Manager.customHeaders.Remove(key);
                    }
                }
            }

            dataGridCustomHeaders.Items.Refresh();
        }

        private async void buttonDeactivateLicense_Click(object sender, RoutedEventArgs e)
        {
            var setts = new MetroDialogSettings { AnimateHide = false };
            var what = await this.ShowMessageAsync("Are you sure?", "Your license will be deactivated!", MessageDialogStyle.AffirmativeAndNegative, Manager.mdsQustion);

            if (what != MessageDialogResult.Affirmative) return;


            try { 
                Manager.TA.Deactivate();
                await this.ShowMessageAsync("Success!", "Your license is successfully deactivated!", MessageDialogStyle.Affirmative, setts);
                Application.Current.Shutdown();

            }catch(Exception ex)
            {
                await this.ShowMessageAsync("Error!", "Error while deactivating your license!", MessageDialogStyle.Affirmative, setts);
                Application.Current.Shutdown();
            }
        }


        // methods
        private void solveCaptcha(Job j)
        {
            string captchaResponse = null;
            j.Status = "Getting captcha response...";

            if (Manager.use2Captcha)
            {
                _2Captcha captcha = new _2Captcha(Manager.myKey, Manager.siteKey);
                captchaResponse = captcha.solveCaptcha();
            }
            else if(Manager.useAntiCaptcha)
            {
                AntiCaptcha captcha = new AntiCaptcha(Manager.myKey, Manager.siteKey);
                captchaResponse = captcha.solveCaptcha();
            }


            if (captchaResponse == "false")
            {
                j.Status = "Error Getting Captcha!";

            }
            else
            {
                //_status = true;
                j.Status = "Got captcha response!";
                j.CaptchaResponse = captchaResponse;

            }


        }

        [ObfuscationAttribute(Exclude = true)]
        public bool addToCart(Job j)
        {
            j.Retries += 1;
            bool _status = false;

            _status = j.addToCart2().Result;

            if (_status)
            {
                App.Current.Dispatcher.Invoke((Action) delegate
                {
                    if (checkBoxSendToSite.IsChecked == true && j.Acc != null)
                    {
                        if (sendToSite(j))
                            j.Status = "On Site";
                    }

                    Manager.jobs.Remove(j);
                    Manager.inCartJobs.Add(j);

                });

            }


            return _status;

        }


        [ObfuscationAttribute(Exclude = true)]
        private void cartAfterCaptcha(Job j)
        {
            bool _status = false;
            //while (_status == false && !Manager.stopAllTask)
            while (_status == false && !Manager.ct.IsCancellationRequested)
            {
                string captchaResponse = null;

                j.Status = "Getting captcha response...";

                if (Manager.use2Captcha)
                {
                    _2Captcha captcha = new _2Captcha(Manager.myKey, Manager.siteKey);
                    captchaResponse = captcha.solveCaptcha();
                }
                else if (Manager.useAntiCaptcha)
                {
                    AntiCaptcha captcha = new AntiCaptcha(Manager.myKey, Manager.siteKey);
                    captchaResponse = captcha.solveCaptcha();
                }


                if (captchaResponse == "false")
                {
                    j.Status = "Error Getting Captcha!";

                }
                else
                {
                    j.Status = "Got captcha response!";
                    j.CaptchaResponse = captchaResponse;

                    j.Retries += 1;
                    _status = j.addToCart2().Result;


                    Console.WriteLine("TREBA JOB DA SE ODRADI...!");
                    Console.WriteLine(_status);
                    //if (_status && !Manager.stopAllTask)
                    if (_status)
                    {
                        Console.WriteLine("STEP 2...");
                        App.Current.Dispatcher.Invoke((Action) delegate
                        {
                            if (checkBoxSendToSite.IsChecked == true && j.Acc != null)
                            {
                                //API api = new API(j.Size, Manager.selectedProfile.Domain, j.Acc.Username, j.Acc.Password, j.PID);
                                //if (await api.SendCart()) j.Status = "On Site";
                                if(sendToSite(j)) j.Status = "On Site";


                            }
                            Manager.jobs.Remove(j);
                            Manager.inCartJobs.Add(j);
                            Console.WriteLine("KAO JE ODRADJENO!");

                        });

                    }

                    if (!Manager.retryOutOfStock) break;

                }
            }

            //return _status;

        }


        private void accountLogin(Job j)
        {
            j.Status = "Logging to account";
            bool? loginStatus = false;
            Task t = Task.Run(async () => loginStatus = await j.Acc.Login());
            t.Wait();

            if (loginStatus == true)
            {
                //j.Handler.CookieContainer = j.Acc.Handler.CookieContainer;
                j.Status = "Logged in";
            }else if (loginStatus == null)
            {
                j.Status = "You've been blocked!";
                j.Acc = null;
            } else
            {
                j.Status = "Failed login to " + j.Acc.ToString();
                j.Acc = null;
            }

        }


        private bool parseProxy(string proxy)
        {
            string[] proxyParts = null;
            string ip;
            string port;
            try
            {
                proxyParts = proxy.Split(':');
                ip = proxyParts[0];
                port = proxyParts[1];
            }
            catch (Exception)
            {
                return false;
            }

            string username = null;
            string password = null;

            try
            {
                if(proxyParts.Length == 4)
                {
                    username = proxyParts[2];
                    password = proxyParts[3];
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }


            Proxy p = new Proxy(ip, port, username, password);
            Manager.proxies.Add(p);
            return true;


        }

        private bool parseAccount(string account)
        {
            string[] accParts = null;
            string username;
            string password;

            try
            {
                accParts = account.Split(':');
                username = accParts[0];
                password = accParts[1];
            }
            catch (Exception)
            {
                return false;
            }
            
            Account acc = new Account(username, password);
            Manager.accounts.Add(acc);
            return true;

        }


        private async void proxyCheckWorker(Queue proxyQueue)
        {
            while (proxyQueue.Count != 0)
            {
                Proxy proxy = proxyQueue.Dequeue() as Proxy;
                bool status = false;
                //Task t = Task.Run(async () => status = await proxy.check());
                //t.Wait();
                status = await proxy.check();

                proxy.Status = status.ToString();

                //Console.WriteLine("Konacni proxy status " + status);
            }

        }


        private async void getBalance()
        {
            double balance = 0;
            string title = null;

            if (radioButton2Captcha.IsChecked == true)
            {
                _2Captcha twoCaptcha = new _2Captcha(Manager.myKey, Manager.siteKey);
                balance = await twoCaptcha.getBalance();
                title = "2Captcha";

            }
            else if(radioButtonAntiCaptcha.IsChecked == true)
            {
                AntiCaptcha antiCaptcha = new AntiCaptcha(Manager.myKey, Manager.siteKey);
                balance = antiCaptcha.getBalance();
                title = "Anti-Captcha";

            }

            await this.ShowMessageAsync(title, "Balance " + balance, MessageDialogStyle.Affirmative);

        }


        private void loadVariables()
        {
            textBlockLoggedInUser.Text = Manager.Username;
            textBlockExpires.Text = Manager.daysLeft.ToString();
            textBlockLicenseType.Text = Manager.LicenseType;
            
            Manager.api2CaptchaKey = Manager.readFromRegistry("2captchaApiKey");
            Manager.apiAntiCaptchaKey = Manager.readFromRegistry("antiCaptchaApiKey");

            string captchaSiteKey = Manager.readFromRegistry("siteKey");
            Manager.siteKey = captchaSiteKey;
            textBoxSiteKey.Text = captchaSiteKey;

            //loadServerVars();

        }


        private void loadServerVars()
        {
            Manager.adidasVar = Manager.TA.GetFeatureValue("adidasVar");
            Manager.addToCartFunction = Manager.TA.GetFeatureValue("addToCartFunction");
            Manager.atcUrl = Manager.TA.GetFeatureValue("atcUrl");
            Manager.atcUrlPart = Manager.TA.GetFeatureValue("atcUrlPart");
            //Manager.ExpireDate = Convert.ToDateTime(Manager.TA.GetFeatureValue("expire"));
            //Manager.daysLeft = ((Manager.ExpireDate - DateTime.Today).TotalDays).ToString();

            loadVariables();
        }


        private bool sendToSite(Job j)
        {
            bool status = false;
            API api =
                new API(j.Size, Manager.selectedProfile.Domain, j.Acc.Username, j.Acc.Password, j.PID);

            if (api.SendCart().Result)
            {
                j.Status = "On Site";
            }

            return status;
        }


        private async void checkForUpdate()
        {
            Updater updater = new Updater();

            while (true)
            {
                Thread.Sleep(600000);

                //int status = LexActivator.IsProductActivated();
                IsGenuineResult licenseStatus = Manager.TA.IsGenuine();

                Console.WriteLine("Product Status: " + licenseStatus);
                if (licenseStatus != IsGenuineResult.Genuine || Manager.dateCheck() == true)
                {
                    Environment.Exit(0);
                    return;
                }

                bool status = await updater.checkForUpdates();

                if(status == true)
                {
                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        buttonUpdate.IsEnabled = true;
                        textBlock10.Text = "New version available!";
                        textBlock10.Foreground = Brushes.Red;
                    });
                    break;

                }

            }

        }
       

        // Menu Buttons
        private void MenuButtonExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MenuButtonSplashTool_Click(object sender, RoutedEventArgs e)
        {

            var n = MessageBox.Show("Under development!", "Do not use it!", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (n == MessageBoxResult.OK)
            {
                SplashToolWindow stw = new SplashToolWindow();
                stw.Show();
            }

        }


        // events
        private void dataGridJobs_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Job j = dataGridJobs.SelectedItem as Job;
           
            if(j != null)
            {
                this.ShowMessageAsync("Error!", "Under developments", MessageDialogStyle.Affirmative);
            }

        }

        private void dataGridInCart_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Job selectedJob = dataGridInCart.SelectedItem as Job;
            if (selectedJob == null) return;

            // if there's running browsers close them all
            if(Manager.openedBrowser.Count != 0)
            {
                foreach (WebBrowserWindow wbw in Manager.openedBrowser)
                {
                    wbw.Close();
                }
            }

            WebBrowserWindow wb = new WebBrowserWindow(selectedJob);
            Manager.openedBrowser.Add(wb);
            wb.Show();

        }

        private async void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            MessageDialogResult what;

            MetroDialogSettings settings = new MetroDialogSettings {
                AnimateHide = false,
                AffirmativeButtonText = "YES",
                NegativeButtonText = "NO"
            };

            what = await this.ShowMessageAsync("Are you sure?", "You're about to close application", MessageDialogStyle.AffirmativeAndNegative, settings);

            if (what == MessageDialogResult.Affirmative)
            {
                Application.Current.Shutdown();
            }

        }


        private void checkBoxCartAfterCaptcha_Click(object sender, RoutedEventArgs e)
        {
            if (Manager.cartAfterCaptcha)
            {
                Manager.cartAfterCaptcha = false;
                checkBoxRetryOutOfStock.IsEnabled = false;
                buttonSolveCaptchas.IsEnabled = true;

            }
            else
            {
                Manager.cartAfterCaptcha = true;
                checkBoxRetryOutOfStock.IsEnabled = true;
                buttonSolveCaptchas.IsEnabled = false;

            }
        }

        private void checkBoxRetryOutOfStock_Click(object sender, RoutedEventArgs e)
        {
            if (Manager.retryOutOfStock)
            {
                Manager.retryOutOfStock = false;
            }
            else
            {
                Manager.retryOutOfStock = true;
            }
        }

        private void radioButton2Captcha_Checked(object sender, RoutedEventArgs e)
        {
            textBoxCaptchaApiKey.Password = Manager.api2CaptchaKey;
            Manager.myKey = Manager.api2CaptchaKey;
        }

        private void radioButtonAntiCaptcha_Checked(object sender, RoutedEventArgs e)
        {
            textBoxCaptchaApiKey.Password = Manager.apiAntiCaptchaKey;
            Manager.myKey = Manager.apiAntiCaptchaKey;
        }

        private void comboBoxSizes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedItem = comboBoxSizes.SelectedItem as string;

            if(selectedItem == "Custom")
            {
                textBoxCustomSize.Visibility = Visibility.Visible;
            }else
            {
                textBoxCustomSize.Visibility = Visibility.Hidden;
            }
        }

        private void comboBoxSites_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SiteProfile siteProfile = (SiteProfile) comboBoxSites.SelectedItem;
            string customPage = "http://www." + siteProfile.Domain;
            textBoxCustomPage.Text = customPage;
        }

        private async void buttonUpdate_Click(object sender, RoutedEventArgs e)
        {

            MessageDialogResult status = await this.ShowMessageAsync("Are you sure?", "You're about to update software",
                MessageDialogStyle.AffirmativeAndNegative, Manager.mdsQustion);

            if(status == MessageDialogResult.Affirmative)
            {
                Updater updater = new Updater();
                await updater.downloadUpdater();

                // open updater and close this process
                Process.Start("updater.exe");
                Application.Current.Shutdown();
            }

        }


        // here
        private void checkTasks()
        {
            
            for (int i = 0; i < Manager.runningTasks.Count; i++)
            {
                Task task = Manager.runningTasks[i];

                if (task.IsCompleted)
                {
                    Manager.runningTasks.Remove(task);
                }
            }

            if (Manager.runningTasks.Count == 0)
            {
                App.Current.Dispatcher.Invoke((Action) delegate
                {
                    buttonStart.Content = "Start";
                });
            }
        }
    }
}
