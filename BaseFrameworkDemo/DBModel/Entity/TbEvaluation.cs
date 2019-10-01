using System;
using System.Collections.Generic;

namespace DBModel.Entity
{
    public partial class TbEvaluation
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string OrderId { get; set; }
        public string StationId { get; set; }
        public int EvaluateLever { get; set; }
        public string EvaluateContent { get; set; }
        public DateTime CreateTime { get; set; }
        public string Remark { get; set; }
    }
}
