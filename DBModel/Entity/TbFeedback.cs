using System;
using System.Collections.Generic;

namespace DBModel.Entity
{
    public partial class TbFeedback
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string Url { get; set; }
        public string UserId { get; set; }
        public DateTime CreateTime { get; set; }
        public string Opinion { get; set; }
        public sbyte? OpinionState { get; set; }
    }
}
