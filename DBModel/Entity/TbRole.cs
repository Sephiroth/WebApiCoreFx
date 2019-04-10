using System;
using System.Collections.Generic;

namespace DBModel.Entity
{
    public partial class TbRole
    {
        public string Id { get; set; }
        public string RoleName { get; set; }
        public string RoleDesc { get; set; }
        public int? RoleType { get; set; }
        public DateTime Createdate { get; set; }
        public int Flag { get; set; }
    }
}
