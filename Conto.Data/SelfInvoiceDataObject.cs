using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conto.Data
{
    public class SelfInvoiceDataObject
    {
        public long CustomerId { get; set; }
        public DateTime Invoicedate { get; set; }
        public int InvoiceYear { get; set; }
        public long MaterialId { get; set; }
        public decimal Quantity { get; set; }
        public bool VatExempt { get; set; }
        public bool InCashFlow { get; set; }
        public long InvoiceNumber { get; set; }
    }
}
