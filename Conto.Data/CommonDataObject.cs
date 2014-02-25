using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conto.Data
{
    public class CommonDataObject
    {
        public int Work_Year { get; set; }
        public long Invoice_Number { get; set; }
        public decimal Max_Self_Invoice_cost { get; set; }
        public int Self_Invoice_Customer { get; set; }
    }
}
