using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlServerCe;
using System.Globalization;
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

    public class ContoData
    {
        public ContoData()
        {
            var inst = LazyInitializeContoData.GetInstance;
        }



        private SqlCeConnection Connection
        {
            get
            {
                return new SqlCeConnection(ConfigurationManager.ConnectionStrings["ContoDatabase"].ConnectionString);
            }
        }

        #region SETTINGS

        public Settings GetSettings()
        {
            using (var conn = new SqlCeConnection(
                ConfigurationManager.ConnectionStrings["ContoDatabase"].ConnectionString))
            {
                conn.Open();
                return conn.Query<Settings>("SELECT * FROM Settings").FirstOrDefault();
            }
        }

        public void SetSettings(Settings settings)
        {
            using (var conn = new SqlCeConnection(
                ConfigurationManager.ConnectionStrings["ContoDatabase"].ConnectionString))
            {
                conn.Open();
                conn.Execute("DELETE FROM Settings");
                conn.Execute(
                    "INSERT INTO Settings (InvoiceOwnerName, InvoiceOwnerAddress, InvoiceOwnerCity, InvoiceOwnerPostalCode, InvoiceOwnerFiscalCode, InvoiceOwnerVatCode, MaxInvoiceValue) VALUES (@InvoiceOwnerName, @InvoiceOwnerAddress, @InvoiceOwnerCity, @InvoiceOwnerPostalCode, @InvoiceOwnerFiscalCode, @InvoiceOwnerVatCode, @MaxInvoiceValue)",
                    new { settings.InvoiceOwnerName, settings.InvoiceOwnerAddress, settings.InvoiceOwnerCity, settings.InvoiceOwnerPostalCode, settings.InvoiceOwnerFiscalCode, settings.InvoiceOwnerVatCode, settings.MaxInvoiceValue });
            }
        }

        #endregion

        #region MATERIALS

        public List<Material> MaterialsGet()
        {
            using (var conn = new SqlCeConnection(
                ConfigurationManager.ConnectionStrings["ContoDatabase"].ConnectionString))
            {
                conn.Open();
                return conn.Query<Material>("SELECT Materials.Id, Materials.Description, Materials.Price, Materials.MeasureId, Measures.Description AS MeasureDescription FROM Materials JOIN Measures ON Materials.MeasureId = Measures.Id").ToList();
            }
        }

        public Material MaterialGet(long id)
        {
            using (var conn = new SqlCeConnection(
                ConfigurationManager.ConnectionStrings["ContoDatabase"].ConnectionString))
            {
                conn.Open();
                return conn.Query<Material>("SELECT * FROM Materials WHERE Id = @Id", new { Id = id}).SingleOrDefault();
            }
        }

        public void MaterialAdd(Material material)
        {
            using (var conn = new SqlCeConnection(
                ConfigurationManager.ConnectionStrings["ContoDatabase"].ConnectionString))
            {
                conn.Open();
                conn.Execute(
                    "INSERT INTO Materials (Description, Price, MeasureId) VALUES (@Description, @Price, @MeasureId)",
                    new
                    {
                        material.Description,
                        material.Price,
                        material.MeasureId
                    });
            }
        }

        public void MaterialUpdate(Material material)
        {
            using (var conn = new SqlCeConnection(
                ConfigurationManager.ConnectionStrings["ContoDatabase"].ConnectionString))
            {
                conn.Open();
                conn.Execute(
                    "UPDATE Materials SET Description = @Description, Price = @Price, MeasureId = @MeasureId WHERE Id = @Id",
                    new
                    {
                        material.Description,
                        material.Price,
                        material.MeasureId,
                        material.Id
                    });
            }
        }

        public void MaterialDelete(Material material)
        {
            using (var conn = new SqlCeConnection(
                ConfigurationManager.ConnectionStrings["ContoDatabase"].ConnectionString))
            {
                conn.Open();
                conn.Execute(
                    "DELETE Materials WHERE Id = @Id);",
                    new
                    {
                        material.Id
                    });
            }
        }

        #endregion

        #region MEASURES

        public List<Measures> MeasuresGet()
        {
            using (var conn = new SqlCeConnection(
                ConfigurationManager.ConnectionStrings["ContoDatabase"].ConnectionString))
            {
                conn.Open();
                return conn.Query<Measures>("SELECT * FROM Measures").ToList();
            }
        }

        public Measures MeasureGet(int id)
        {
            using (var conn = new SqlCeConnection(
                ConfigurationManager.ConnectionStrings["ContoDatabase"].ConnectionString))
            {
                conn.Open();
                return conn.Query<Measures>("SELECT * FROM Measures WHERE Id = @Id", new { Id = id }).SingleOrDefault();
            }
        }

        #endregion

        #region SELFINVOICES

        public List<SelfInvoicesMaster> SelfInvoicesMasterGet()
        {
            using (var conn = new SqlCeConnection(
                ConfigurationManager.ConnectionStrings["ContoDatabase"].ConnectionString))
            {
                conn.Open();
                return conn.Query<SelfInvoicesMaster>("SELECT SelfInvoices.InvoiceGroupId, Materials.Description AS MaterialDescription, SUM(SelfInvoices.Quantity) AS Quantity, SUM(SelfInvoices.InvoiceCost) AS Cost, SelfInvoices.InvoiceDate, COUNT(*) AS InvoiceCount FROM SelfInvoices INNER JOIN Materials ON SelfInvoices.MaterialId = Materials.Id WHERE SelfInvoices.InCashFlow = 0 GROUP BY SelfInvoices.InvoiceGroupId, Materials.Description, SelfInvoices.InvoiceDate ORDER BY SelfInvoices.InvoiceDate DESC").ToList();
            }
        }

        public void SelfInvoiceAddToCashFlow(SelfInvoicesMaster selfInvoice)
        {
            using (var conn = new SqlCeConnection(
                ConfigurationManager.ConnectionStrings["ContoDatabase"].ConnectionString))
            {
                conn.Open();

                var selfInvoices =
                    conn.Query<SelfInvoices>("SELECT * FROM SelfInvoices WHERE InvoiceGroupId = @invoiceGroupId", new
                    {
                        invoiceGroupId = selfInvoice.InvoiceGroupId
                    }).ToList();

                foreach (var selfInv in selfInvoices)
                {
                    conn.Execute(
                        "INSERT INTO CashFlow (Cash, Description, FlowDate, CashFlowType) VALUES (@Cash, @Description, @FlowDate, @CashFlowType)",
                        new
                        {
                            Cash = selfInv.InvoiceCost,
                            Description =
                                string.Format("Pagata autofattura {0}/{1}", selfInv.InvoiceNumber,
                                    selfInv.InvoiceYear.ToString(CultureInfo.InvariantCulture).Substring(2)),
                            FlowDate = selfInv.InvoiceDate,
                            CashFlowType = "SelfInvoice"
                        });

                    var id = conn.Query("SELECT @@IDENTITY AS id").SingleOrDefault();

                    if(id != null)
                    conn.Execute(
                    "UPDATE SelfInvoices SET InCashFlow = 1, CashFlowId = @CashFlowId WHERE InvoiceGroupId = @InvoiceGroupId AND InvoiceId = @InvoiceId",
                    new
                    {
                        CashFlowId = (long)id.id,
                        selfInv.InvoiceGroupId,
                        selfInv.InvoiceId
                    });
                }
            }
        }

        public SelfInvoices SelfInvoiceGetByCashFlow(long cashFlowId)
        {
            using (var conn = new SqlCeConnection(
                ConfigurationManager.ConnectionStrings["ContoDatabase"].ConnectionString))
            {
                conn.Open();
                return
                    conn.Query<SelfInvoices>("SELECT * FROM SelfInvoices WHERE CashFlowId = @CashFlowId",
                        new {CashFlowId = cashFlowId}).SingleOrDefault();
            }
        }

        public List<SelfInvoices> SelfInvoicesGet()
        {
            using (var conn = new SqlCeConnection(
                ConfigurationManager.ConnectionStrings["ContoDatabase"].ConnectionString))
            {
                conn.Open();
                return conn.Query<SelfInvoices>("SELECT * FROM SelfInvoices").ToList();
            }
        }

        public void SelfInvoicesAdd(SelfInvoices selfInvoice)
        {
            using (var conn = new SqlCeConnection(
                ConfigurationManager.ConnectionStrings["ContoDatabase"].ConnectionString))
            {
                conn.Open();
                conn.Execute(
                    "INSERT INTO SelfInvoices (MaterialId, Quantity, VatExcept, InvoiceNumber, InvoiceYear, MeasureId, InCashFlow, InvoiceDate, InvoiceCost, InvoiceGroupId) VALUES (@MaterialId, @Quantity, @VatExcept, @InvoiceNumber, @InvoiceYear, @MeasureId, @InCashFlow, @InvoiceDate, @InvoiceCost, @InvoiceGroupId)",
                    new
                    {
                        selfInvoice.MaterialId,
                        selfInvoice.Quantity,
                        selfInvoice.VatExcept,
                        selfInvoice.InvoiceNumber,
                        selfInvoice.InvoiceYear,
                        selfInvoice.MeasureId,
                        selfInvoice.InCashFlow,
                        selfInvoice.InvoiceDate,
                        selfInvoice.InvoiceCost,
                        selfInvoice.InvoiceGroupId
                    });
            }
        }

        #endregion


        #region COMMONDATA

        public CommonDataObject CommonGetForYear(int year)
        {
            using (var conn = new SqlCeConnection(
                ConfigurationManager.ConnectionStrings["ContoDatabase"].ConnectionString))
            {
                conn.Open();
                return
                    conn.Query<CommonDataObject>("SELECT * FROM Common WHERE work_year = @year", new { year })
                        .FirstOrDefault();
            }
        }

        public void CommonSetInvoiceNumberForYear(int year, long invoiceNumber)
        {
            using (var conn = new SqlCeConnection(
                ConfigurationManager.ConnectionStrings["ContoDatabase"].ConnectionString))
            {
                conn.Open();
                conn.Execute(
                    "UPDATE Common SET invoice_number = @invoiceNumber WHERE work_year = @year",
                    new
                    {
                        invoiceNumber,
                        year
                    });
            }
        }

        #endregion

        #region CLIENTS

        public List<Client> ClientsGet()
        {
            using (
                var conn = new SqlCeConnection(ConfigurationManager.ConnectionStrings["ContoDatabase"].ConnectionString)
                )
            {
                conn.Open();
                return conn.Query<Client>("SELECT * FROM Clients").ToList();
            }
        }

        public void ClientAdd(Client client)
        {
            using (var conn = new SqlCeConnection(
                ConfigurationManager.ConnectionStrings["ContoDatabase"].ConnectionString))
            {
                conn.Open();
                conn.Execute(
                    "INSERT INTO Clients (Name, Address, City, PostalCode, FiscalCode, VatCode) VALUES (@Name, @Address, @City, @PostalCode, @FiscalCode, @VatCode)",
                    new
                    {
                        client.Name,
                        client.Address,
                        client.City,
                        client.PostalCode,
                        client.FiscalCode,
                        client.VatCode
                    });
            }
        }

        #endregion












        public CommonDataObject GetCommonData()
        {
            using (var conn = new SqlCeConnection(
                ConfigurationManager.ConnectionStrings["ContoDatabase"].ConnectionString))
            {
                conn.Open();
                return conn.Query<CommonDataObject>("SELECT * FROM Common").FirstOrDefault();
            }
        }



        #region CASHFLOW

        public void CashFlowAdd(CashFlow cashFlow)
        {
            using (var conn = new SqlCeConnection(
                ConfigurationManager.ConnectionStrings["ContoDatabase"].ConnectionString))
            {
                conn.Open();
                conn.Execute(
                    "INSERT INTO CashFlow (Cash, Description, FlowDate) VALUES (@Cash, @Description, @FlowDate)",
                    new { cashFlow.Cash, cashFlow.Description, cashFlow.FlowDate });
            }
        }

        public List<CashFlow> CashFlowGetYear(int year)
        {
            using (var conn = new SqlCeConnection(
                ConfigurationManager.ConnectionStrings["ContoDatabase"].ConnectionString))
            {
                conn.Open();
                return
                    conn.Query<CashFlow>(
                        "SELECT * FROM CashFlow WHERE DATEPART(year, FlowDate) = @year ORDER BY FlowDate DESC",
                        new { year }).ToList();
            }
        }

        public List<CashFlow> CashFlowGetYearMonth(int year, int month)
        {
            using (var conn = new SqlCeConnection(
                ConfigurationManager.ConnectionStrings["ContoDatabase"].ConnectionString))
            {
                conn.Open();
                return
                    conn.Query<CashFlow>(
                        "SELECT * FROM CashFlow WHERE DATEPART(month, FlowDate) = @month AND DATEPART(year, FlowDate) = @year ORDER BY FlowDate DESC",
                        new { month, year }).ToList();
            }
        }

        public DateTime CashFlowGetLastDateTime()
        {
            using (var conn = Connection)
            {
                conn.Open();
                try
                {
                    var ret = conn.Query<DateTime>("SELECT MAX(FlowDate) FROM CashFlow");
                    return ret != null ? ret.First() : DateTime.Now;
                }
                catch
                {
                    return DateTime.Now;
                }
            }
        }

        public decimal CashFlowBalance()
        {
            using (var conn = Connection)
            {
                conn.Open();
                try
                {
                    var ret = conn.Query<decimal>("SELECT SUM(Cash) FROM CashFlow");
                    return ret != null ? ret.First() : 0;
                }
                catch
                {
                    return 0;
                }
            }
        }

        #endregion
    }
}
