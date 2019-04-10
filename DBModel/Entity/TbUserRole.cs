using System;
using System.Collections.Generic;

namespace DBModel.Entity
{
    public partial class TbUserRole
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string RoleId { get; set; }
        public int Flag { get; set; }
    }
}
