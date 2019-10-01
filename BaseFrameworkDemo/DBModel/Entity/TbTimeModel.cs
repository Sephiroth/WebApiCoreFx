using System;
using System.Collections.Generic;

namespace DBModel.Entity
{
    public partial class TbTimeModel
    {
        public string Id { get; set; }
        public string PriceModelId { get; set; }
        public int? Duration { get; set; }
        public int? ModelSign { get; set; }
    }
}
