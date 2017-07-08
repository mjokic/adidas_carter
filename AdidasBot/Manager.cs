using AdidasBot.Model;
using AdidasBot.Model.Captchas;
using AdidasBot.Windows;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using wyDay.TurboActivate;

namespace AdidasBot
{
    [ObfuscationAttribute(Exclude = true)]
    public static class Manager
    {
        public static string siteKey { get; set; }

        public static string Username;
        public static DateTime ExpireDate;
        public static string LicenseType;
        public static double daysLeft;

        public static string myKey = null;
        public static string api2CaptchaKey = null;
        public static string apiAntiCaptchaKey = null;
        public static string customPage = null;

        //public static bool stopAllTask = false;

        public static List<Task> runningTasks = new List<Task>();
        public static CancellationTokenSource cts = null;
        public static CancellationToken ct;

        public static ObservableCollection<Job> jobs = new ObservableCollection<Job>();
        public static ObservableCollection<Job> inCartJobs = new ObservableCollection<Job>();
        public static ObservableCollection<Proxy> proxies = new ObservableCollection<Proxy>();
        public static ObservableCollection<Account> accounts = new ObservableCollection<Account>();
        public static ObservableCollection<SiteProfile> siteProfiles = new ObservableCollection<SiteProfile>();
        public static Dictionary<string, string> customHeaders = new Dictionary<string, string>();

        public static ObservableCollection<News> news = new ObservableCollection<News>();

        public static List<WebBrowserWindow> openedBrowser = new List<WebBrowserWindow>();

        public static ObservableCollection<Thread> splashTasks = new ObservableCollection<Thread>();

        public static SiteProfile selectedProfile;

        public static MetroDialogSettings mdsQustion = new MetroDialogSettings {
            AffirmativeButtonText = "YES", NegativeButtonText = "NO" };

        public static bool cartAfterCaptcha = false;
        public static bool retryOutOfStock = false;
        public static bool use2Captcha = false;
        public static bool useAntiCaptcha = false;

        public static Dictionary<string, string> sizes = new Dictionary<string, string>();

        public static TurboActivate TA = new TurboActivate("4de5d381591dfd48877143.36833305");


        #region Server Variables
        public static string addToCartFunction = null;
        public static string adidasVar = null;
        public static string atcUrl = null;
        public static string atcUrlPart = null;
        public static int tasksLimit = 0;
        #endregion


        #region English Sizes
        public static Dictionary<string, string> sizesEnglish = new Dictionary<string, string>
        {
            {"4","540"},
            {"4.5","550"},
            {"5","560"},
            {"5.5","570"},
            {"6","580"},
            {"6.5","590"},
            {"7","600"},
            {"7.5","610"},
            {"8","620"},
            {"8.5","630"},
            {"9","640"},
            {"9.5","650"},
            {"10","660"},
            {"10.5","670"},
            {"11","680"},
            {"11.5","690"},
            {"12","700"},
            {"12.5","710"},
            {"13","720"},
            {"13.5","730"},
            {"14","740"},
            {"14.5","750"},
            {"15","760"},
            {"15.5","770"},
            {"16","780"},
            {"XS","290"},
            {"S","310"},
            {"M","330"},
            {"L","350"},
            {"XL","370"},
            {"2XL", "390"},
            { "Custom", ""},

        };

        #endregion


        #region American Sizes (AU, US, CA)
        public static Dictionary<string, string> sizesAmerica = new Dictionary<string, string>
        {
            {"100", "100"},
            {"290", "290"},
            {"390", "390"},
            {"4", "530"},
            {"4.5", "540"},
            {"5", "550"},
            {"5.5", "560"},
            {"6", "570"},
            {"6.5", "580"},
            {"7", "590"},
            {"7.5", "600"},
            {"8", "610"},
            {"8.5", "620"},
            {"9", "630"},
            {"9.5", "640"},
            {"10", "650"},
            {"10.5", "660"},
            {"11", "670"},
            {"11.5", "680"},
            {"12", "690"},
            {"12.5", "700"},
            {"13", "710"},
            {"13.5", "720"},
            {"14", "730"},
            {"XS", "480"},
            {"S", "500"},
            {"M", "520"},
            {"L", "540"},
            {"XL", "560"},
            {"Custom", ""},

        };
        #endregion


        #region European Sizes
        public static Dictionary<string, string> sizesEurope = new Dictionary<string, string>
        {
            { "36", "530"},
            { "36 2/3", "540"},
            { "37 1/3", "550"},
            { "38", "560"},
            { "38 2/3", "570"},
            { "39 1/3", "580"},
            { "40", "590"},
            { "40 2/3", "600" },
            { "41 1/3", "610" },
            { "42", "620" },
            { "42 2/3", "630" },
            { "43 1/3", "640" },
            { "44", "650" },
            { "44 2/3", "660" },
            { "45 1/3", "670" },
            { "46", "680" },
            { "46 2/3", "690" },
            { "47 1/3", "700" },
            { "48", "710" },
            { "48 2/3", "720" },
            { "49 1/3", "730" },
            {"XS", "290"},
            {"S", "310"},
            {"M", "330"},
            {"L", "350"},
            {"XL", "370"},
            {"2XL", "390"},
            {"Custom", ""},
        };
        #endregion


        public static void debugSave(string fileName, string content)
        {
            File.WriteAllText(fileName, content);
        }


        public static void saveToRegistry(string registryName, string registryValue)
        {
            if (registryValue == null) registryValue = "";
            Microsoft.Win32.RegistryKey registryKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("adidasBotOptions");
            registryKey.SetValue(registryName, registryValue);
            registryKey.Close();

        }


        public static string readFromRegistry(string registryName)
        {
            string value = null;
            try
            {
                Microsoft.Win32.RegistryKey registryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("adidasBotOptions");
                value = registryKey.GetValue(registryName) as string;
                registryKey.Close();
            }
            catch (Exception)
            {
                value = "";
            }


            return value;

        }


        public static void initialize()
        {
            Username = TA.GetExtraData();
            LicenseType = "PRO";
        }

        public static bool dateCheck()
        {
            bool status = false;

            ExpireDate = Convert.ToDateTime(TA.GetFeatureValue("expire"));
            Console.WriteLine(ExpireDate);

            DateTime today = DateTime.Today;

            daysLeft = Math.Round((ExpireDate - today).TotalDays);
            if (daysLeft <= 0) status = true;

            return status;
        }

    }
}
