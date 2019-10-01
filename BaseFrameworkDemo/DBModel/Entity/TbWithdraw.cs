using System;
using System.Collections.Generic;

namespace DBModel.Entity
{
    public partial class TbWithdraw
    {
        public string Id { get; set; }
        public DateTime WithdrawTime { get; set; }
        public int WithdrawAmount { get; set; }
        public string UserId { get; set; }
        public string Remarks { get; set; }
        public int State { get; set; }
    }
}
