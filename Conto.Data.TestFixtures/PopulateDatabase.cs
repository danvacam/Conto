using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using NUnit.Framework;

namespace Conto.Data.TestFixtures
{
    [TestFixture]
    public class PopulateDatabase
    {
        [Test]
        public void AddCashFlow()
        {
            using (var conn = new SqlCeConnection(
                ConfigurationManager.ConnectionStrings["ContoDatabase"].ConnectionString))
            {
                conn.Open();
                var res =
                    conn.Execute(
                        "INSERT INTO CashFlow (Cash, Description, FlowDate) VALUES (@Cash, @Description, @FlowDate)",
                        new {Cash = 50000, Description = "Prelievo pre cassa", FlowDate = DateTime.Now});
            }
        }

        [Test]
        public void CustomerAddSampleData()
        {
            using (var conn = new SqlCeConnection(
                    ConfigurationManager.ConnectionStrings["ContoDatabase"].ConnectionString))
            {
                conn.Open();
                for (var i = 0; i < 25; i++)
                {

                    var res = conn.Execute(@"INSERT INTO Customer(name, address, fiscal_code, vat_code)
                        VALUES (@name, @address, @fiscal_code, @vat_code);",
                        new
                        {
                            name = "prova " + i,
                            address = "indirizzo " + i,
                            fiscal_code = "1234567890123456",
                            vat_code = "1234567890123456"
                        });
                }
            }
        }

        [Test]
        public void CustomerTruncateTable()
        {
            using (
                var conn = new SqlCeConnection(ConfigurationManager.ConnectionStrings["ContoDatabase"].ConnectionString)
                )
            {
                conn.Open();
                conn.Execute("DELETE FROM Customer;");
                conn.Execute("ALTER TABLE Customer ALTER COLUMN id IDENTITY (1,1);");
            }
        }

        [Test]
        public void MaterialsAddSampleData()
        {
            using (var conn = new SqlCeConnection(
                    ConfigurationManager.ConnectionStrings["ContoDatabase"].ConnectionString))
            {
                conn.Open();
                for (var i = 0; i < 25; i++)
                {

                    var res = conn.Execute(@"INSERT INTO Material(name, price)
                        VALUES (@name, @price);",
                        new
                        {
                            name = "prova " + i,
                            price = 1800
                        });
                }
            }
        }

        [Test]
        public void MaterialsTruncateTable()
        {
            using (
                var conn = new SqlCeConnection(ConfigurationManager.ConnectionStrings["ContoDatabase"].ConnectionString)
                )
            {
                conn.Open();
                conn.Execute("DELETE FROM Material;");
                conn.Execute("ALTER TABLE Material ALTER COLUMN id IDENTITY (1,1);");
            }
        }
    }
}
