using System;
using System.Collections.Generic;

namespace Conto.Data
{
    public class Invoice
    {
        public long Id { get; set; }
        public long ClientId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public long InvoiceNumber { get; set; }
        public int InvoiceYear { get; set; }
        public int MeasureId { get; set; }
        public IEnumerable<InvoiceContent> InvoiceContentRows { get; set; }
        public bool InCashFlow { get; set; }
        public long CashFlowId { get; set; }
    }

    public class InvoiceContent
    {
        public string Description { get; set; }
        public IEnumerable<InvoiceMaterialRow> Rows { get; set; }
    }

    public class InvoiceMaterialRow
    {
        public string Description { get; set; }
        public decimal MaterialQuantity { get; set; }
        public decimal MaterialPrice { get; set; }
        public decimal MaterialCost { get; set; }
    }
}
