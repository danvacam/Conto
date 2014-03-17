using System;

namespace Conto.Data
{
    public class SelfInvoices
    {
        public long Id { get; set; }
        public long MaterialId { get; set; }
        public decimal Quantity { get; set; }
        public bool VatExcept { get; set; }
        public long InvoiceNumber { get; set; }
        public int InvoiceYear { get; set; }
        public int MeasureId { get; set; }
        public bool InCashFlow { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal InvoiceCost { get; set; }
        public Guid InvoiceGroupId { get; set; }
    }

    public class SelfInvoicesMaster
    {
        public Guid InvoiceGroupId { get; set; }
        public string MaterialDescription { get; set; }
        public decimal Quantity { get; set; }
        public decimal Cost { get; set; }
    }
}
