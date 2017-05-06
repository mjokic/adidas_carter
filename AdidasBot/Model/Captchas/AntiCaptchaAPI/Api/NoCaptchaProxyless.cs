using System;
using Newtonsoft.Json.Linq;
using System.Reflection;

namespace AdidasBot.Model.Captchas.AntiCaptchaAPI.Api
{
    [ObfuscationAttribute(Exclude = true)]
    public class NoCaptchaProxyless : AnticaptchaBase, IAnticaptchaTaskProtocol
    {
        [ObfuscationAttribute(Exclude = true)]
        public Uri WebsiteUrl { protected get; set; }
        [ObfuscationAttribute(Exclude = true)]
        public string WebsiteKey { protected get; set; }
        [ObfuscationAttribute(Exclude = true)]
        public string WebsiteSToken { protected get; set; }

        [ObfuscationAttribute(Exclude = true)]
        public override JObject GetPostData()
        {
            return new JObject
            {
                {"type", "NoCaptchaTaskProxyless"},
                {"websiteURL", WebsiteUrl},
                {"websiteKey", WebsiteKey},
                {"websiteSToken", WebsiteSToken}
            };
        }

        [ObfuscationAttribute(Exclude = true)]
        public string GetTaskSolution()
        {
            return TaskInfo.Solution.GRecaptchaResponse;
        }
    }
}