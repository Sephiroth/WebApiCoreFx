using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using WxAppUtil.Model;

namespace WxAppUtil.Util
{
    public static class WxUtils
    {
        private static string wxLoginLink = "https://api.weixin.qq.com/sns/jscode2session?appid={0}&secret={1}&js_code={2}&grant_type=authorization_code";

        /// <summary>
        /// 获取OpenId(建议通过IHttpClientFactory注入HttpClient)
        /// </summary>
        /// <param name="loginParam"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        public static async Task<OpenIdParam> GetOpenIdAsync(WxLoginParam loginParam, HttpClient client = null)
        {
            // 获取openid连接
            string address = string.Format(wxLoginLink, loginParam.AppId, loginParam.Secret, loginParam.Code);
            string jsonStr = null;
            OpenIdParam openIdParam = null;

            // 自定义HttpClient
            bool selfClient = false;
            if (client == null)
            {
                client = new HttpClient();
                selfClient = true;
            }
            using (HttpResponseMessage message = await client.GetAsync(address))
            {
                if (message.IsSuccessStatusCode && message.StatusCode == HttpStatusCode.OK)
                {
                    jsonStr = await message.Content.ReadAsStringAsync();
                }
            }
            if (selfClient)
            {
                client.Dispose();
            }
            if (!string.IsNullOrEmpty(jsonStr))
            {
                openIdParam = JsonConvert.DeserializeObject<OpenIdParam>(jsonStr);
                if (openIdParam == null || string.IsNullOrEmpty(openIdParam.session_key))
                {
                    throw new System.Exception(jsonStr);
                }
            }
            return openIdParam;
        }

    }
}