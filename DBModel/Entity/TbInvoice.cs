using System;
using System.Collections.Generic;

namespace DBModel.Entity
{
    public partial class TbInvoice
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public int InvoiceType { get; set; }
        public DateTime CreateTime { get; set; }
        public sbyte TitleType { get; set; }
        public string Title { get; set; }
        public string TaxNum { get; set; }
        public int? InvoiceNum { get; set; }
        public string InvoiceContent { get; set; }
        public string ReceiverName { get; set; }
        public int? ReceiverPhone { get; set; }
        public string ReceiverMail { get; set; }
        public int State { get; set; }
        public string Remarks { get; set; }
    }
}
