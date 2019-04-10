using System;
using System.Collections.Generic;

namespace DBModel.Entity
{
    public partial class TbPileSession
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public int? Info { get; set; }
        public string Sessionid { get; set; }
        public int? Power { get; set; }
        public int? Pilestate { get; set; }
        public int? Gunstate { get; set; }
    }
}
