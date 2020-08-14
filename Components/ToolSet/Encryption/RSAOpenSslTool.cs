using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace ToolSet.Encryption
{
    public class RSAOpenSslTool : IDisposable
    {
        private static readonly object lockObj = new object();
        private static RSAOpenSslTool instance;
        public static RSAOpenSslTool INSTANCE
        {
            get { return GetSingleton(); }
        }

        private bool disposed = false;
        private RSA rsa;
        private byte[] privateKey;
        private byte[] publicKey;

        /// <summary>
        /// 公钥
        /// </summary>
        public string PublicKey { get; private set; }

        public RSAOpenSslTool()
        {
            InitRSA();
        }

        /// <summary>
        /// 获取RSAOpenSslTool的单例对象
        /// </summary>
        /// <returns></returns>
        private static RSAOpenSslTool GetSingleton()
        {
            if (instance == null)
            {
                lock (lockObj)
                {
                    instance ??= new RSAOpenSslTool();
                };
            }
            return instance;
        }

        private void InitRSA()
        {
            //X509Certificate2 x509Certificate2 = new X509Certificate2("");
            rsa = RSA.Create();
            privateKey = rsa.ExportRSAPrivateKey();
            publicKey = rsa.ExportRSAPublicKey();
            PublicKey = Convert.ToBase64String(publicKey);
#if DEBUG
            Console.WriteLine($"EncryptionTool.OpenSsl.RSAOpenSslTool.INSTANCE.publicKey:{PublicKey}");
            Console.WriteLine($"EncryptionTool.OpenSsl.RSAOpenSslTool.INSTANCE.privateKey:{Convert.ToBase64String(privateKey)}");
#endif
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="encryptStr">UTF8字符串</param>
        /// <returns>加密的base64字符串</returns>
        public string Encrypt(string encryptStr)
        {
            if (string.IsNullOrEmpty(encryptStr)) { throw new ArgumentNullException(nameof(encryptStr)); }
            byte[] data = rsa.Encrypt(Encoding.UTF8.GetBytes(encryptStr), RSAEncryptionPadding.OaepSHA256);
            return Convert.ToBase64String(data);
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="decryptStr">加密的base64字符串</param>
        /// <returns>UTF8字符串</returns>
        public string Decrypt(string decryptStr)
        {
            if (string.IsNullOrEmpty(decryptStr)) { throw new ArgumentNullException(nameof(decryptStr)); }
            byte[] data = rsa.Decrypt(Convert.FromBase64String(decryptStr), RSAEncryptionPadding.OaepSHA256);
            return Encoding.UTF8.GetString(data);
        }

        ~RSAOpenSslTool() => Dispose(false);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }
            if (disposing)
            {
                rsa?.Dispose();
            }
            disposed = true;
        }
    }
}
