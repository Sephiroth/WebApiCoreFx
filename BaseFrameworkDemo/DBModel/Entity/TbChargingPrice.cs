using System;
using System.Collections.Generic;

namespace DBModel.Entity
{
    public partial class TbChargingPrice
    {
        public string Id { get; set; }
        public int Type { get; set; }
        public string Name { get; set; }
        public float ElectricPrice { get; set; }
        public float? ServicePrice { get; set; }
        public string Remark { get; set; }
    }
}
