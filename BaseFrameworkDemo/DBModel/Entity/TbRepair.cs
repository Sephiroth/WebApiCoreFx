using System;
using System.Collections.Generic;

namespace DBModel.Entity
{
    public partial class TbRepair
    {
        public string Id { get; set; }
        public string StationId { get; set; }
        public string Sn { get; set; }
        public string RepairTypeId { get; set; }
        public string Datail { get; set; }
        public DateTime CreateDate { get; set; }
        public string UserId { get; set; }
        public string Result { get; set; }
        public int State { get; set; }
        public string Remark { get; set; }
    }
}
