using Newtonsoft.Json.Linq;

namespace AdidasBot.Model.Captchas.AntiCaptchaAPI
{
    public interface IAnticaptchaTaskProtocol
    {
        JObject GetPostData();
        string GetTaskSolution();
    }
}