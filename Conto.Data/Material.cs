using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conto.Data
{
    public class Material
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }
        public int MeasureId { get; set; }
    }
}
