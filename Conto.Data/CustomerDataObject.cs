using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conto.Data
{
    public class CustomerDataObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Fiscal_Code { get; set; }
        public string Vat_Code { get; set; }
    }
}
