using System;
using System.Collections.Generic;

namespace DBModel.Entity
{
    public partial class TbCoupon
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}
