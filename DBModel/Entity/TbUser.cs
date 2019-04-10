using System;
using System.Collections.Generic;

namespace DBModel.Entity
{
    public partial class TbUser
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Nickname { get; set; }
        public string Phone { get; set; }
        public string WechatId { get; set; }
        public int DiscountBalance { get; set; }
        public int WithdrawalBalance { get; set; }
        public string CarNum { get; set; }
        public string Openid { get; set; }
        public string Pwd { get; set; }
        public string Photo { get; set; }
        public string Email { get; set; }
        public int? Gender { get; set; }
        public int State { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
