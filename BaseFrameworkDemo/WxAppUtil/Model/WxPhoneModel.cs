namespace WxAppUtil.Model
{
    public class WxPhoneModel
    {
        /// <summary>
        /// 用户绑定的手机号（国外手机号会有区号）
        /// </summary>
        public string PhoneNumber { set; get; }

        /// <summary>
        /// 没有区号的手机号
        /// </summary>
        public string PurePhoneNumber { set; get; }

        /// <summary>
        /// 区号
        /// </summary>
        public string CountryCode { set; get; }

        /// <summary>
        /// 水印
        /// </summary>
        public WaterMarkModel WaterMark { set; get; }
    }
}