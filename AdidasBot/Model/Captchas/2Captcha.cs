using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AdidasBot.Model.Captchas
{
    // only for solving recaptchas
    public class _2Captcha
    {

        private string apiKey;
        private string googleKey;
        private string url = "http://2captcha.com/res.php?key=";

        public _2Captcha(string apiKey, string googleKey)
        {
            this.apiKey = apiKey;
            this.googleKey = googleKey;

            this.url += this.apiKey;

        }


        public string solveCaptcha()
        {

            string captchaID = getCaptchaId().Result;
            Console.WriteLine("Captcha id: " + captchaID);

            if (captchaID == null) return "false";

            string captchaResponse = null;
            //while (captchaResponse == null && !Manager.stopAllTask)
            while (captchaResponse == null && !Manager.ct.IsCancellationRequested)
            {
                captchaResponse = getCaptchaResponse(captchaID).Result;
                Console.WriteLine(captchaResponse);
                if (captchaResponse != null) break;

                Thread.Sleep(5000);
                Console.WriteLine("Waiting 5 seconds...");

            }

            return captchaResponse;

        }


        private async Task<string> getCaptchaId()
        {
            string captchaID = null;
            string _url = "http://2captcha.com/in.php?key=" + this.apiKey
                + " &method=userrecaptcha&googlekey=" + this.googleKey
                + "&pageurl=" + Manager.selectedProfile.Domain;

            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(_url))
                {
                    string content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(content);

                    string[] data = content.Split('|');


                    string status = data[0];
                    if (status == "OK")
                    {
                        captchaID = data[1];
                        return captchaID;
                    }

                }

            }

            return captchaID;

        }


        private async Task<string> getCaptchaResponse(string captchaID)
        {
            string _url = this.url + "&action=get&id=" + captchaID;


            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(_url))
                {
                    string content = await response.Content.ReadAsStringAsync();

                    if (content == "CAPCHA_NOT_READY") return null;

                    string[] data = content.Split('|');

                    if (data[0] == "OK")
                    {
                        return data[1];
                    }

                    Console.WriteLine(content);
                    return "false";

                }
            }
        }


        public async Task<double> getBalance()
        {
            double _balance;
            string _url = this.url + "&action=getbalance";

            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(_url))
                {
                    string content = await response.Content.ReadAsStringAsync();
                    bool stat = double.TryParse(content, out _balance);

                    if(!stat) { _balance = -1; }
                    
                }
            }

            return _balance;

        }

    }

}
