using System;
using System.Collections.Generic;

namespace DBModel.Entity
{
    public partial class TbUserOrg
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string OrgId { get; set; }
        public int? Flag { get; set; }
        public string OrgName { get; set; }
    }
}
