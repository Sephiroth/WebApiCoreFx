using DBModel.WxModel;
using Newtonsoft.Json.Linq;
using System;
using System.Security.Cryptography;
using System.Text;

namespace LogicLayer.Util
{
    public class WxAppEncryptUtil
    {
        internal static MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static string MD5(string txt)
        {
            var bytearr = UTF8Encoding.Default.GetBytes(txt);
            string encTxt = BitConverter.ToString(md5.ComputeHash(bytearr), 4, 8);
            encTxt = encTxt.Replace("-", "");
            return encTxt;
        }

        /// <summary>
        /// 解密小程序的encryptedData
        /// </summary>
        /// <param name="encryptedData">加密的信息</param>
        /// <param name="sessionKey">key</param>
        /// <param name="iv">加密算法的初始向量</param>
        public static WxPhoneModel DescodeWxSHA1(string encryptedData, string sessionKey, string iv)
        {
            WxPhoneModel model = null;
            string res = AESDecrypt(encryptedData, sessionKey, iv);
            if (!string.IsNullOrEmpty(res))
            {
                model = JObject.Parse(res).ToObject<WxPhoneModel>();
            }
            return model;
        }

        /// <summary>
        /// 解密encryptedData(容易报错)
        /// </summary>
        /// <param name="encryptedData"></param>
        /// <param name="sessionKey"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static string AESDecrypt(string encryptedData, string sessionKey, string iv)
        {
            //16进制数据转换成byte
            var encryptedDataByte = Convert.FromBase64String(encryptedData);  // strToToHexByte(text);
            var rijndaelCipher = new RijndaelManaged
            {
                Key = Convert.FromBase64String(sessionKey),
                IV = Convert.FromBase64String(iv),
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7
            };
            var transform = rijndaelCipher.CreateDecryptor();
            var plainText = transform.TransformFinalBlock(encryptedDataByte, 0, encryptedDataByte.Length);
            return Encoding.Default.GetString(plainText);
        }

        public static WxPhoneModel GetEncryptedDataStr(string encryptedDataStr, string key, string iv)
        {
            WxPhoneModel model = null;
            //判断是否是16位 如果不够补0
            //text = tests(text);
            //16进制数据转换成byte
            byte[] encryptedData = Convert.FromBase64String(encryptedDataStr);  // strToToHexByte(text);
            RijndaelManaged rijndaelCipher = new RijndaelManaged();
            rijndaelCipher.Key = Convert.FromBase64String(key); // Encoding.UTF8.GetBytes(AesKey);
            rijndaelCipher.IV = Convert.FromBase64String(iv);// Encoding.UTF8.GetBytes(AesIV);
            rijndaelCipher.Mode = CipherMode.CBC;
            rijndaelCipher.Padding = PaddingMode.PKCS7;
            ICryptoTransform transform = rijndaelCipher.CreateDecryptor();
            byte[] plainText = transform.TransformFinalBlock(encryptedData, 0, encryptedData.Length);
            string result = Encoding.Default.GetString(plainText);
            //int index = result.LastIndexOf('>');
            //result = result.Remove(index + 1);
            // 转换实体类
            if (!string.IsNullOrEmpty(result))
            {
                model = JObject.Parse(result).ToObject<WxPhoneModel>();
            }
            return model;
        }

    }
}