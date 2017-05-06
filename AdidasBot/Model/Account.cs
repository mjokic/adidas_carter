using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdidasBot.Model
{
    public class Account : INotifyPropertyChanged
    {

        private string csrfToken;
        private HttpClient client;

        public Account(string username, string password)
        {
            this.username = username;
            this.password = password;

        }


        private async Task getLoginScreen()
        {
            //string url = "https://cp."+Manager.selectedProfile.Domain.Replace("global.","")+"/web/eCom/"+Manager.selectedProfile.InUrlShort+"/loadsignin?target=account";
            string url = "https://cp." + Manager.selectedProfile.Domain.Replace("global.", "") + "/web/eCom/" + Manager.selectedProfile.InUrlShort + "/loadsignin";
            //string url = "https://cp.adidas.co.uk/web/eCom/en_GB/loadsignin?target=account";
            //string url = "https://cp.adidas.com/web/eCom/en_US/loadsignin?target=account";
            //string url = "http://requestb.in/11c4ol21";

            Console.WriteLine(url);

            if (handler == null)
            {
                this.handler = new HttpClientHandler();
                this.handler.UseCookies = true;
                this.handler.CookieContainer = new CookieContainer();
                this.handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                this.client = new HttpClient(handler);
                Console.WriteLine("Im here");
            }
            else
            {
                try
                {
                    this.handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                    this.client = new HttpClient(this.handler);
                }
                catch (Exception)
                {
                    throw;
                }
                
                Console.WriteLine("Im FUCKING here!!!");
            }

            //this.client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (X11; Ubuntu; Linux x86_64; rv:51.0) Gecko/20100101 Firefox/51.0");
            this.client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
            this.client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
            this.client.DefaultRequestHeaders.Add("DNT", "1");
            this.client.DefaultRequestHeaders.Add("Connection", "keep-alive");
            this.client.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");

            using (HttpResponseMessage response = await this.client.GetAsync(url))
            {
                string content = await response.Content.ReadAsStringAsync();
                //Manager.debugSave("test" + new Random().Next(1, 999999) + ".html", content);

                Regex r = new Regex("name=\"CSRFToken\" value=\"(.*?)\"");
                var tmp = r.Match(content);
                this.csrfToken = tmp.Groups[1].Value;
                Console.WriteLine("CSRF TOKEN:"+this.csrfToken);
                //Console.WriteLine("CSRF Token: " + this.csrfToken);


            }



        }


        public async Task<bool?> Login()
        {
            bool status = true;
            try
            {
                await getLoginScreen();
            }
            catch (Exception)
            {
                return null;
            }


            string url = "https://cp."+ Manager.selectedProfile.Domain.Replace("global.", "") + "/idp/startSSO.ping";
            Console.WriteLine(url);

            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("username", username);
            dict.Add("password", password);
            dict.Add("signinSubmit", "Sign+in");
            dict.Add("IdpAdapterId", "adidasIdP10");
            dict.Add("SpSessionAuthnAdapterId", "https://cp.adidas.com/web/");
            dict.Add("PartnerSpId", "sp:demandware");
            dict.Add("validator_id", "adieComDWgb");
            dict.Add("TargetResource", "https://www."+ Manager.selectedProfile.Domain.Replace("global.", "") + "/on/demandware.store/"+ Manager.selectedProfile.InUrlLong+ "/"+ Manager.selectedProfile.InUrlShort+ "/MyAccount-ResumeLogin?target=account");
            dict.Add("InErrorResource", "https://www." + Manager.selectedProfile.Domain.Replace("global.", "") + "/on/demandware.store/" + Manager.selectedProfile.InUrlLong + "/" + Manager.selectedProfile.InUrlShort + "/null");
            dict.Add("loginUrl", "https://cp."+ Manager.selectedProfile.Domain.Replace("global.", "") + "/web/eCom/"+Manager.selectedProfile.InUrlShort+"/loadsignin");
            dict.Add("cd", "eCom|"+Manager.selectedProfile.InUrlShort+"|cp."+Manager.selectedProfile.InUrlShort+"|null");
            dict.Add("remembermeParam", "");
            dict.Add("app", "eCom");
            dict.Add("locale", Manager.selectedProfile.InUrlShort);
            dict.Add("domain", "cp."+ Manager.selectedProfile.Domain.Replace("global.", ""));
            dict.Add("email", "");
            dict.Add("pfRedirectBaseURL_test", "https://cp."+ Manager.selectedProfile.Domain.Replace("global.", ""));
            dict.Add("pfStartSSOURL_test", "https://cp."+ Manager.selectedProfile.Domain.Replace("global.", "") + "/idp/startSSO.ping");
            dict.Add("resumeURL_test", "");
            dict.Add("FromFinishRegistraion", "");
            dict.Add("CSRFToken", this.csrfToken);

            var data = new FormUrlEncodedContent(dict);


            //HttpClient client = new HttpClient(this.handler);
            string nextUrl;

            string relayState;
            string samplResp;

            string REF;
            string tarRes;

            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (X11; Ubuntu; Linux x86_64; rv:51.0) Gecko/20100101 Firefox/51.0");
            client.DefaultRequestHeaders.ExpectContinue = false;
            //client.DefaultRequestHeaders.Add("Connection", "close");
            

            using (HttpResponseMessage response = await client.PostAsync(url, data))
            {
                string content = await response.Content.ReadAsStringAsync();
                //Manager.debugSave("login.html", content); // creshuje ovde...

                if (content.Contains("Consumer authenticated but required minimum age not confirmed for the given country."))
                {
                    return false;
                }else if(response.StatusCode == HttpStatusCode.Forbidden)
                {
                    return null;
                }

                // getting next url from source
                string pattern = "var resURL = '(.*?)';";
                Regex r = new Regex(pattern);
                var tmp = r.Match(content);

                nextUrl = tmp.Groups[1].Value;
                Console.WriteLine("NEXT URL: "+nextUrl);
                if (nextUrl == null || nextUrl == String.Empty) return null;

            }


            using (HttpResponseMessage response = await client.GetAsync(nextUrl))
            {
                string content = await response.Content.ReadAsStringAsync();
                //debugSave("onemore.html", content);

                Regex r = new Regex("action=\"(.*?)\"");
                var tmp = r.Match(content);
                //nextUrl = "https://cp.adidas.co.uk" + tmp.Groups[1].Value;
                var domen = Manager.selectedProfile.Domain.Split(new[] { "adidas." }, StringSplitOptions.None);
                nextUrl = "https://cp.adidas." + domen.Last() + tmp.Groups[1].Value;


                Regex r1 = new Regex("name=\"RelayState\" value=\"(.*?)\"");
                var tmp1 = r1.Match(content);
                relayState = tmp1.Groups[1].Value;

                Regex r2 = new Regex("name=\"SAMLResponse\" value=\"(.*?)\"");
                var tmp2 = r2.Match(content);
                samplResp = tmp2.Groups[1].Value;


            }

            dict = new Dictionary<string, string>();
            dict.Add("RelayState", relayState);
            dict.Add("SAMLResponse", samplResp);

            var data1 = new FormUrlEncodedContent(dict);


            using (HttpResponseMessage response = await client.PostAsync(nextUrl, data1))
            {
                string content = await response.Content.ReadAsStringAsync();
                //debugSave("lolz.html", content);

                Regex r = new Regex("action=\"(.*?)\">");
                var tmp = r.Match(content);
                nextUrl = tmp.Groups[1].Value;
                if (nextUrl == null || nextUrl == String.Empty) return null;


                Regex r1 = new Regex("name=\"REF\" value=\"(.*?)\"");
                var tmp1 = r1.Match(content);
                REF = tmp1.Groups[1].Value;

                Regex r2 = new Regex("name=\"TargetResource\" value=\"(.*?)\"");
                var tmp2 = r2.Match(content);
                tarRes = tmp2.Groups[1].Value;

            }

            dict = new Dictionary<string, string>();
            dict.Add("REF", REF);
            dict.Add("TargetResource", tarRes);

            var data2 = new FormUrlEncodedContent(dict);


            using (HttpResponseMessage response = await client.PostAsync(nextUrl, data2))
            {
                string content = await response.Content.ReadAsStringAsync();
                //debugSave("final.html", content);

            }


            string userPageUrl = "https://www." + Manager.selectedProfile.Domain + "/on/demandware.store/"+Manager.selectedProfile.InUrlLong+"/"+Manager.selectedProfile.InUrlShort+"/MyAccount-Show";
            using (HttpResponseMessage response = await client.GetAsync(userPageUrl))
            {
                string content = await response.Content.ReadAsStringAsync();
                //Manager.debugSave("loggedIn.html", content);

            }


            return status;

        }


        #region Properties
        private string username;

        public string Username
        {
            get { return username; }
            set { username = value;
                OnPropertyChanged("Username");
            }
        }

        private string password;

        public string Password
        {
            get { return password; }
            set { password = value;
                OnPropertyChanged("Password");
            }
        }


        private HttpClientHandler handler;

        public HttpClientHandler Handler
        {
            get { return handler; }
            set { handler = value; }
        }


        #endregion


        #region Data binding stuff
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propName)
        {
            PropertyChangedEventHandler h = PropertyChanged;

            if (h != null)
            {
                h(this, new PropertyChangedEventArgs(propName));
            }

        }
        #endregion


        public override string ToString()
        {
            return this.username;
        }

    }
}
