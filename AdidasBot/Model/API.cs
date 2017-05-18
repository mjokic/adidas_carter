using AdidasBot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AdidasCarterPro.Model
{
    public class API
    {
        private string url = "https://www.strikecarts.com/carts.json";
        private string username = "3492jsdj";
        private string password = "hard";

        // params
        // are the parameters :size, :region, :adidasusername, :adidaspassword, :active, :product_id

        #region Properties
        public string Size { get; set; }
        public string Region { get; set; }
        public string AdidasUsername { get; set; }
        public string AdidasPassword { get; set; }
        public string ProductId { get; set; }
        #endregion


        public API(string size, string region, string adidasUsername, string adidasPassword, string productId)
        {
            this.Size = size;
            this.Region = region;
            this.AdidasUsername = adidasUsername;
            this.AdidasPassword = adidasPassword;
            this.ProductId = productId;
        }


        public async Task<bool> SendCart()
        {
            bool status = false;

            HttpClient client = new HttpClient();
            var textBytes = System.Text.Encoding.UTF8.GetBytes(this.username + ":" + this.password);
            client.DefaultRequestHeaders.Add("Authorization", "Basic " + System.Convert.ToBase64String(textBytes));

            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("cart[size]", this.Size);
            dict.Add("cart[region]", this.Region);
            dict.Add("cart[adidasusername]", this.AdidasUsername);
            dict.Add("cart[adidaspassword]", this.AdidasPassword);
            dict.Add("cart[active]", "");
            dict.Add("cart[product_id]", this.ProductId);

            var data = new FormUrlEncodedContent(dict);

            //using (HttpResponseMessage response = await client.GetAsync(this.url))
            using (HttpResponseMessage response = await client.PostAsync(this.url, data))
            {
                string content = await response.Content.ReadAsStringAsync();
                //Manager.debugSave("apiTest.html", content);

                Console.WriteLine(response.StatusCode); // StatusCode: Created
                if (response.StatusCode == System.Net.HttpStatusCode.Created) status = true;

            }

            return status;
        }


    }
}
