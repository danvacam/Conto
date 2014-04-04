using System;

namespace Conto.Data
{
    public class SelfInvoicesMaster
    {
        public Guid InvoiceGroupId { get; set; }
        public long MaterialId { get; set; }
        public string MaterialDescription { get; set; }
        public decimal Quantity { get; set; }
        public decimal Cost { get; set; }
        public DateTime InvoiceDate { get; set; }
        public long InvoiceCount { get; set; }
        public int MeasureId { get; set; }
        public string MeasuresDescription { get; set; }
        public bool VatExcept { get; set; }
        public int InvoiceYear { get; set; }
    }
}
