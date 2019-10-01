using System;
using System.Collections.Generic;

namespace DBModel.Entity
{
    public partial class TbCollection
    {
        public string Id { get; set; }
        public byte[] Logo { get; set; }
        public string StationId { get; set; }
        public string UserId { get; set; }
        public DateTime CreateDate { get; set; }
        public string Remark { get; set; }
    }
}
