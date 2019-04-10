using System;
using System.Collections.Generic;

namespace DBModel.Entity
{
    public partial class TbRecharge
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public int Amount { get; set; }
        public DateTime CreateTime { get; set; }
        public sbyte Method { get; set; }
        public string Remark { get; set; }
        public int State { get; set; }
    }
}
