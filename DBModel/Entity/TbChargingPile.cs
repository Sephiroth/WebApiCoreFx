using System;
using System.Collections.Generic;

namespace DBModel.Entity
{
    public partial class TbChargingPile
    {
        public string Id { get; set; }
        public string Sn { get; set; }
        public sbyte Type { get; set; }
        public string TerminalType { get; set; }
        public string ChargeInterface { get; set; }
        public string StationId { get; set; }
        public int State { get; set; }
        public string OrderType { get; set; }
    }
}
