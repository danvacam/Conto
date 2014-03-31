using System;
using System.Configuration;
using System.Data.SqlServerCe;
using System.Linq;
using Dapper;

namespace Conto.Data
{
    public class LazyInitializeContoData
    {
        private static readonly Lazy<LazyInitializeContoData> Instance =
            new Lazy<LazyInitializeContoData>(() => new LazyInitializeContoData());

        private SqlCeConnection Connection
        {
            get
            {
                return new SqlCeConnection(ConfigurationManager.ConnectionStrings["ContoDatabase"].ConnectionString);
            }
        }

        private LazyInitializeContoData()
        {
            using (var conn = Connection)
            {
                conn.Open();
                var settings = conn.Query<Settings>("SELECT * FROM Settings");
                if (!settings.Any())
                {
                    conn.Execute(
                        "INSERT INTO Settings (InvoiceOwnerName, InvoiceOwnerAddress, InvoiceOwnerCity, InvoiceOwnerPostalCode, InvoiceOwnerFiscalCode, InvoiceOwnerVatCode, MaxInvoiceValue) VALUES (@InvoiceOwnerName, @InvoiceOwnerAddress, @InvoiceOwnerCity, @InvoiceOwnerPostalCode, @InvoiceOwnerFiscalCode, @InvoiceOwnerVatCode, @MaxInvoiceValue)",
                        new { InvoiceOwnerName = "O.S. Trading S.r.l Soc. Unipersonale", InvoiceOwnerAddress = "Via Mascagni snc", InvoiceOwnerCity = "Usmate Velate", InvoiceOwnerPostalCode = "20040", InvoiceOwnerFiscalCode = "05962770961", InvoiceOwnerVatCode = "05962770961", MaxInvoiceValue = 990 });
                }

                var demoValues = ConfigurationManager.AppSettings["DemoValues"];
                if (demoValues == "true")
                {
                    
                }

            }
        }

        public static LazyInitializeContoData GetInstance
        {
            get { return Instance.Value; }
        }
    }
}
