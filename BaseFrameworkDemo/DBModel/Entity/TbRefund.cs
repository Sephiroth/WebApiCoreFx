using System;
using System.Collections.Generic;

namespace DBModel.Entity
{
    public partial class TbRefund
    {
        public string Id { get; set; }
        public DateTime CreateTime { get; set; }
        public string OrderId { get; set; }
        public int RefundAmount { get; set; }
        public string UserId { get; set; }
        public string Reason { get; set; }
        public int State { get; set; }
        public string Photo { get; set; }
        public string Remarks { get; set; }
    }
}
