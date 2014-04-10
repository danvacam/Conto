using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conto.Data
{
    public class CashFlow
    {
        public long Id { get; set; }
        public decimal Cash { get; set; }
        public string Description { get; set; }
        public DateTime FlowDate { get; set; }
        public string CashFlowType { get; set; }
    }
}
