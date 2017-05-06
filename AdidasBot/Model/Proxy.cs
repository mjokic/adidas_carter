using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AdidasBot.Model
{
    public class Proxy : INotifyPropertyChanged
    {

        public Proxy(string ip, string port, string username, string password)
        {
            this.ip = ip;
            this.port = port;
            this.username = username;
            this.password = password;

        }


        public async Task<bool> check()
        {
            bool status = false;

            HttpClientHandler handler = new HttpClientHandler();
            handler.UseProxy = true;
            WebProxy proxy = new WebProxy(this.ip + ":" + this.port, false);
            if(this.username != null && this.password != null) proxy.Credentials = new NetworkCredential(this.username, this.password);
            handler.Proxy = proxy;

            using (HttpClient client = new HttpClient(handler))
            {
                // timeout - make it cutomizable
                client.Timeout = TimeSpan.FromSeconds(60);

                try
                {
                    using (HttpResponseMessage response = await client.GetAsync("https://google.com"))
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            //Console.WriteLine("Proxy cool");
                            status = true;
                        }
                        else
                        {
                            //Console.WriteLine(response.StatusCode.ToString());
                        }
                    }
                }catch
                {
                    status = false;
                }
                
            }



            return status;
        }


        #region Properties
        private string ip;

        public string IP
        {
            get { return ip; }
            set { ip = value;
                OnPropertyChanged("IP");
            }
        }

        private string port;

        public string Port
        {
            get { return port; }
            set { port = value;
                OnPropertyChanged("Port");
            }
        }

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

        private string status;

        public string Status
        {
            get { return status; }
            set { status = value;
                OnPropertyChanged("Status");
            }
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
            return this.ip + ":" + this.port;
        }

    }
}
