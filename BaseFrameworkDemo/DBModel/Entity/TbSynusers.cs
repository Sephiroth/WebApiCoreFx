using System;
using System.Collections.Generic;

namespace DBModel.Entity
{
    public partial class TbSynusers
    {
        public string Id { get; set; }
        public string LoginName { get; set; }
        public string Password { get; set; }
        public string RoleName { get; set; }
        public DateTime Createdate { get; set; }
        public int Flag { get; set; }
        public string NatureName { get; set; }
        public string Phone { get; set; }
        public int? Gender { get; set; }
    }
}
