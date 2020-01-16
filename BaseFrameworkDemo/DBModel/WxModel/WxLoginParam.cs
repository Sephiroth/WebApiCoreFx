using System;
using System.Collections.Generic;
using System.Text;

namespace DBModel.WxModel
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