using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AdidasCarterPro.Model
{
    public class Updater
    {
        private HttpClient client;

        public Updater()
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.UseProxy = false;
            //handler.Proxy = new WebProxy();
            this.client = new HttpClient(handler);
        }


        public async Task<bool> checkForUpdates()
        {
            bool status = false;
            string url = "http://adidascarter.club/check.php";

            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("type", "pro");

            var data = new FormUrlEncodedContent(dict);

            using (HttpResponseMessage response = await this.client.PostAsync(url, data))
            {
                string content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(content);

                try
                {
                    JObject json = JObject.Parse(content);

                    bool update = bool.Parse(json.GetValue("update").ToString());
                    string hash = json.GetValue("hash").ToString();

                    string checkHash = GetMD5();

                    if (update == true && hash != checkHash)
                    {
                        status = true;
                    }
                }
                catch (Exception)
                {
                    status = false;
                }
                
            }

            return status;

        }

        public async Task<bool> downloadUpdater()
        {
            var downloadFileUrl = "http://adidascarter.club/adidas_pro/updater/AdidasCarterUpdater.exe";
            var destinationFilePath = Path.GetFullPath("updater.exe");

            using (var client = new Download(downloadFileUrl, destinationFilePath))
            {
                client.ProgressChanged += (totalFileSize, totalBytesDownloaded, progressPercentage) => {
                    //Console.WriteLine($"{progressPercentage}% ({totalBytesDownloaded}/{totalFileSize})");
                    //textBlockProgress.Text = progressPercentage.ToString();

                    //progressBar1.Value = (double)progressPercentage;


                };

                await client.StartDownload();
            }

            return true;
        }

        public static string GetMD5()
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            FileStream stream = new FileStream(Process.GetCurrentProcess().MainModule.FileName,
                FileMode.Open, FileAccess.Read);

            md5.ComputeHash(stream);

            stream.Close();

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < md5.Hash.Length; i++)
                sb.Append(md5.Hash[i].ToString("x2"));

            return sb.ToString();
        }

    }

}
