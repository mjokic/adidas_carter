using AdidasBot.Model.Captchas.AntiCaptchaAPI.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AdidasBot.Model.Captchas
{

    [ObfuscationAttribute(Exclude = true)]
    public class AntiCaptcha
    {

        [ObfuscationAttribute(Exclude = true)]
        private string apiKey = null;
        [ObfuscationAttribute(Exclude = true)]
        private string siteKey = null;


        public AntiCaptcha(string apiKey, string siteKey)
        {
            this.apiKey = apiKey;
            this.siteKey = siteKey;
        }


        [ObfuscationAttribute(Exclude = true)]
        public async Task<string> solveCaptcha()
        {
            string captchaResponse = await getCaptchaResponse();
            if (captchaResponse == null) return "false";

            return captchaResponse;
        }


        [ObfuscationAttribute(Exclude = true)]
        private async Task<string> getCaptchaResponse()
        {
            string captchaResponse = null;

            NoCaptchaProxyless api = new NoCaptchaProxyless
            {
                ClientKey = this.apiKey,
                WebsiteUrl = new Uri("http://" + Manager.selectedProfile.Domain),
                WebsiteKey = this.siteKey
            };

            if (!api.CreateTask())
            {
                //Console.WriteLine(api.ErrorMessage);
                Console.WriteLine("Something fucked up...");
            }
            else if (!api.WaitForResult())
            {
                Console.WriteLine("Could not solve the captcha.");
            }
            else
            {
                captchaResponse = api.GetTaskSolution();
                Console.WriteLine("Solution...:"+captchaResponse);
            }


            return captchaResponse;

        }


        [ObfuscationAttribute(Exclude = true)]
        public double getBalance()
        {
            double balance = 0;
            ImageToText api = new ImageToText
            {
                ClientKey = this.apiKey
            };

            double? tmpBala = api.GetBalance();

            if(tmpBala != null)
            {
                balance = double.Parse(tmpBala.ToString());
            }

            return balance;

        }


    }
}
