using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Conto.Data.PdfHelpers;
using Dapper;
using NUnit.Framework;

namespace Conto.Data.TestFixtures
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Fiscal_Code { get; set; }
        public string Vat_Code { get; set; }
    }

    [TestFixture]
    public class CustomersTests
    {
        [Test]
        public void GetCustomers()
        {
            using (
                var conn = new SqlCeConnection(ConfigurationManager.ConnectionStrings["ContoDatabase"].ConnectionString)
                )
            {
                conn.Open();
                var customerList = conn.Query<Customer>("SELECT * FROM Customer").ToList();
            }
        }

        [Test]
        public void CreatePdf()
        {
            ItextObjectExport.CreateFirstPdf();
        }
    }
}
