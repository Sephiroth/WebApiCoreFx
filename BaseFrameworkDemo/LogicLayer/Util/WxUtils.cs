using DBModel.WxModel;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.Util
{
    public static class WxUtils
    {
        public static async Task<OpenIdParam> GetOpenid(WxLoginParam loginParam)
        {
            // 获取openid连接
            string address = string.Format("https://api.weixin.qq.com/sns/jscode2session?appid={0}&secret={1}&js_code={2}&grant_type=authorization_code",
                loginParam.AppId, loginParam.Secret, loginParam.Code);
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(address);
            req.Method = "GET";
            OpenIdParam openIdParam = null;
            using (WebResponse wr = await req.GetResponseAsync())
            {
                using (Stream stream = wr.GetResponseStream())
                {
                    using (StreamReader sr = new StreamReader(stream, Encoding.UTF8))
                    {
                        string result = sr.ReadToEnd();
                        if (result.Contains("errcode"))
                        {
                            openIdParam = null;
                        }
                        else
                        {
                            openIdParam = JsonConvert.DeserializeObject<OpenIdParam>(result);
                        }
                    }
                }
            }
            return openIdParam;
        }

    }
}