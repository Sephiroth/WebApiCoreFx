using System;
using System.Collections.Generic;

namespace DBModel.Entity
{
    public partial class TbOplist
    {
        public string Id { get; set; }
        public string OpCode { get; set; }
        public string OpName { get; set; }
        public int Flag { get; set; }
    }
}
