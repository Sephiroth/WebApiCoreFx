namespace WxAppUtil.Model
{
    public class WxLoginParam
    {
        public string AppId { get; set; }

        public string Secret { get; set; }

        public string Code { get; set; }

        public string EncryptedData { get; set; }

        public string Iv { get; set; }
    }
}