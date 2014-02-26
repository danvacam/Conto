using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conto.Data
{
    public class SelfInvoiceDataObject
    {
        public CustomerDataObject Customer { get; set; }
        public DateTime InvoiceDate { get; set; }
        public long InvoiceNumber { get; set; }
        public int InvoiceYear { get; set; }
        public MaterialDataObject Material { get; set; }
        public decimal Quantity { get; set; }
        public bool VatExempt { get; set; }
    }
}
