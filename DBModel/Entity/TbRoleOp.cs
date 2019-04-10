using System;
using System.Collections.Generic;

namespace DBModel.Entity
{
    public partial class TbRoleOp
    {
        public string Id { get; set; }
        public string OpId { get; set; }
        public string OpCode { get; set; }
        public string RoleId { get; set; }
        public int Flag { get; set; }
    }
}
