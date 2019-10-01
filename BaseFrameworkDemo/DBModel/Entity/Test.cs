using System;
using System.Collections.Generic;

namespace DBModel.Entity
{
    public partial class Test
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? Birthday { get; set; }
        public double? Income { get; set; }
    }
}
