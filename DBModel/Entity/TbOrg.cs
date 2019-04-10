using System;
using System.Collections.Generic;

namespace DBModel.Entity
{
    public partial class TbOrg
    {
        public string Id { get; set; }
        public string OrgName { get; set; }
        public string ParentId { get; set; }
        public string OrgDesc { get; set; }
        public string Flag { get; set; }
        public int State { get; set; }
        public DateTime? CreateTime { get; set; }
    }
}
