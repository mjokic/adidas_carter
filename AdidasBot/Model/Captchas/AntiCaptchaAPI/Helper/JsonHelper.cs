using System.Globalization;
using Newtonsoft.Json;
using System.Reflection;

namespace AdidasBot.Model.Captchas.AntiCaptchaAPI.Helper
{
    [ObfuscationAttribute(Exclude = true)]
    public class JsonHelper
    {
        [ObfuscationAttribute(Exclude = true)]
        public static string ExtractStr(dynamic json, string firstLevel, string secondLevel = null, bool silent = false)
        {
            var path = firstLevel + (secondLevel == null ? "" : "=>" + secondLevel);

            try
            {
                object result = json[firstLevel];

                if (result != null && secondLevel != null)
                {
                    result = json[firstLevel][secondLevel];
                }

                if (result == null)
                {
                    if (!silent)
                    {
                        DebugHelper.JsonFieldParseError(path, json);
                    }

                    return null;
                }

                return result.ToString();
            }
            catch
            {
                if (!silent)
                {
                    DebugHelper.JsonFieldParseError(path, json);
                }

                return null;
            }
        }

        [ObfuscationAttribute(Exclude = true)]
        public static string AsString(dynamic json)
        {
            return JsonConvert.SerializeObject(json, Formatting.Indented);
        }

        [ObfuscationAttribute(Exclude = true)]
        public static double? ExtractDouble(dynamic json, string firstLevel, string secondLevel = null)
        {
            double outDouble;
            string numberAsStr = ExtractStr(json, firstLevel, secondLevel);

            if (numberAsStr == null ||
                !double.TryParse(numberAsStr.Replace(",", "."), NumberStyles.Number, CultureInfo.InvariantCulture,
                    out outDouble))
            {
                var path = firstLevel + (secondLevel == null ? "" : "=>" + secondLevel);
                DebugHelper.JsonFieldParseError(path, json);

                return null;
            }

            return outDouble;
        }

        [ObfuscationAttribute(Exclude = true)]
        public static int? ExtractInt(dynamic json, string firstLevel, string secondLevel = null)
        {
            int outInt;
            string numberAsStr = JsonHelper.ExtractStr(json, firstLevel, secondLevel);

            if (!int.TryParse(numberAsStr, out outInt))
            {
                var path = firstLevel + (secondLevel == null ? "" : "=>" + secondLevel);
                DebugHelper.JsonFieldParseError(path, json);

                return null;
            }

            return outInt;
        }


    }
}