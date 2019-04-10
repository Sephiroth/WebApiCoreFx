using System;
using System.Collections.Generic;

namespace DBModel.Entity
{
    public partial class TbPriceModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int ElectricPrice { get; set; }
        public int ServicePrice { get; set; }
        public string Remark { get; set; }
        public int? ModelSign { get; set; }
    }
}
