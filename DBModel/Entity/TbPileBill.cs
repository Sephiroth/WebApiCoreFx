using System;
using System.Collections.Generic;

namespace DBModel.Entity
{
    public partial class TbPileBill
    {
        public string Id { get; set; }
        public string UserCard { get; set; }
        public int? Power { get; set; }
        public int? ElectricCharge { get; set; }
        public int? ServiceCharge { get; set; }
        public int? Reason { get; set; }
    }
}
