using System;

namespace Conto.Data
{
    public class SelfInvoicesMaster
    {
        public Guid InvoiceGroupId { get; set; }
        public string MaterialDescription { get; set; }
        public decimal Quantity { get; set; }
        public decimal Cost { get; set; }
        public DateTime InvoiceDate { get; set; }
        public long InvoiceCount { get; set; }
    }
}
