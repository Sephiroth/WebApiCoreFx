using System;
using System.Collections.Generic;

namespace DBModel.Entity
{
    public partial class TbOrder
    {
        public string Id { get; set; }
        public string OrderNo { get; set; }
        public string StationId { get; set; }
        public string PileId { get; set; }
        public string UserId { get; set; }
        public DateTime CreateTime { get; set; }
        public int WithdrawalMonetary { get; set; }
        public int DiscountMonetary { get; set; }
        public double? Power { get; set; }
        public int? ActualServiceCharge { get; set; }
        public int? ActualElectricCharge { get; set; }
        public double? Duration { get; set; }
        public string Datail { get; set; }
        public int State { get; set; }
        public int InvoiceState { get; set; }
        public string Remark { get; set; }
    }
}
