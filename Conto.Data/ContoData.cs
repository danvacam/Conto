using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlServerCe;
using System.Globalization;
using System.Linq;
using Dapper;

namespace Conto.Data
{
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

        public int MaterialsCountGet()
        {
            using (var conn = Connection)
            {
                conn.Open();
                var ret = conn.Query<int>("SELECT Count(*) FROM Materials");
                return ret != null ? ret.First() : 0;
            }
        }

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

        public bool MaterialDelete(Material material)
        {
            using (var conn = new SqlCeConnection(
                ConfigurationManager.ConnectionStrings["ContoDatabase"].ConnectionString))
            {
                conn.Open();

                var ret = conn.Query<int>("SELECT Count(*) FROM SelfInvoices WHERE MaterialId = @Id", new
                {
                    material.Id
                });

                var totUsedMaterials = ret != null ? ret.First() : 0;
                if (totUsedMaterials > 0)
                    return false;
                
                conn.Execute("DELETE FROM Materials WHERE Id = @Id",
                    new
                    {
                        material.Id
                    });
            }
            return true;
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
                return conn.Query<SelfInvoicesMaster>("SELECT SelfInvoices.InvoiceGroupId, Materials.Description AS MaterialDescription, SUM(SelfInvoices.Quantity) AS Quantity, SUM(SelfInvoices.InvoiceCost) AS Cost, SelfInvoices.InvoiceDate, COUNT(*) AS InvoiceCount, SelfInvoices.MeasureId, Measures.Description AS MeasuresDescription, SelfInvoices.MaterialId, SelfInvoices.VatExcept, SelfInvoices.InvoiceYear FROM SelfInvoices INNER JOIN Materials ON SelfInvoices.MaterialId = Materials.Id INNER JOIN Measures ON Measures.Id = SelfInvoices.MeasureId WHERE SelfInvoices.InCashFlow = 0 GROUP BY SelfInvoices.InvoiceGroupId, Materials.Description, SelfInvoices.InvoiceDate, SelfInvoices.MeasureId, Measures.Description, SelfInvoices.MaterialId, SelfInvoices.VatExcept, SelfInvoices.InvoiceYear ORDER BY SelfInvoices.InvoiceDate DESC").ToList();
            }
        }

        public bool SelfInvoiceDelete(SelfInvoicesMaster selfInvoice)
        {
            using (var conn = new SqlCeConnection(
                ConfigurationManager.ConnectionStrings["ContoDatabase"].ConnectionString))
            {
                conn.Open();

                var selfInvoices = conn.Query<SelfInvoices>("SELECT * FROM SelfInvoices WHERE InvoiceGroupId = @InvoiceGroupId AND CashFlowId IS NULL", new
                {
                   selfInvoice.InvoiceGroupId
                });

                var totInCashFlow = selfInvoices != null ? selfInvoices.Count() : 0;
                if (totInCashFlow == 0)
                    return false;

                conn.Execute("DELETE FROM SelfInvoices WHERE InvoiceGroupId = @InvoiceGroupId",
                    new
                    {
                        selfInvoice.InvoiceGroupId
                    });
            }
            return true;
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

                var invoiceYear = selfInvoices[0].InvoiceYear;
                var common =
                    conn.Query<Common>("SELECT * FROM Common WHERE WorkYear = @WorkYear", new { WorkYear = invoiceYear })
                        .SingleOrDefault();
                long invoiceNumber = 1;
                if (common != null)
                {
                    invoiceNumber = common.CurrentAvailableInvoiceNumber;
                }
                else
                {
                    conn.Execute(
                        "INSERT INTO Common (CurrentAvailableInvoiceNumber, WorkYear) VALUES (@CurrentAvailableInvoiceNumber, @WorkYear)",
                        new {invoiceNumber, invoiceYear});
                }

                foreach (var selfInv in selfInvoices)
                {
                    conn.Execute(
                        "INSERT INTO CashFlow (Cash, Description, FlowDate, CashFlowType) VALUES (@Cash, @Description, @FlowDate, @CashFlowType)",
                        new
                        {
                            Cash = selfInv.InvoiceCost,
                            Description =
                                string.Format("Pagata autofattura {0}/{1}", invoiceNumber,
                                    selfInv.InvoiceYear.ToString(CultureInfo.InvariantCulture).Substring(2)),
                            FlowDate = selfInv.InvoiceDate,
                            CashFlowType = "SelfInvoice"
                        });

                    var id = conn.Query("SELECT @@IDENTITY AS id").SingleOrDefault();

                    if (id != null)
                        conn.Execute(
                            "UPDATE SelfInvoices SET InCashFlow = 1, CashFlowId = @CashFlowId, InvoiceNumber = @InvoiceNumber WHERE InvoiceGroupId = @InvoiceGroupId AND InvoiceId = @InvoiceId",
                            new
                            {
                                CashFlowId = (long) id.id,
                                selfInv.InvoiceGroupId,
                                selfInv.InvoiceId,
                                InvoiceNumber = invoiceNumber
                            });

                    invoiceNumber++;
                }

                conn.Execute("UPDATE Common SET CurrentAvailableInvoiceNumber = @CurrentAvailableInvoiceNumber WHERE WorkYear = @WorkYear",
                    new { CurrentAvailableInvoiceNumber = invoiceNumber, WorkYear = invoiceYear });
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
                    "INSERT INTO SelfInvoices (MaterialId, Quantity, VatExcept, InvoiceYear, MeasureId, InCashFlow, InvoiceDate, InvoiceCost, InvoiceGroupId) VALUES (@MaterialId, @Quantity, @VatExcept, @InvoiceYear, @MeasureId, @InCashFlow, @InvoiceDate, @InvoiceCost, @InvoiceGroupId)",
                    new
                    {
                        selfInvoice.MaterialId,
                        selfInvoice.Quantity,
                        selfInvoice.VatExcept,
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

        public Client ClientGet(long id)
        {
            using (var conn = new SqlCeConnection(
                ConfigurationManager.ConnectionStrings["ContoDatabase"].ConnectionString))
            {
                conn.Open();
                return conn.Query<Client>("SELECT * FROM Clients WHERE Id = @Id", new { Id = id }).SingleOrDefault();
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

        public void ClientUpdate(Client client)
        {
            using (var conn = new SqlCeConnection(
                ConfigurationManager.ConnectionStrings["ContoDatabase"].ConnectionString))
            {
                conn.Open();
                conn.Execute(
                    "UPDATE Clients SET Name = @Name, Address = @Address, PostalCode = @PostalCode, FiscalCode = @FiscalCode, VatCode = @VatCode WHERE Id = @Id",
                    new
                    {
                        client.Name,
                        client.Address,
                        client.City,
                        client.PostalCode,
                        client.FiscalCode,
                        client.VatCode,
                        client.Id
                    });
            }
        }

        public bool ClientDelete(Client client)
        {
            using (var conn = new SqlCeConnection(
                ConfigurationManager.ConnectionStrings["ContoDatabase"].ConnectionString))
            {
                conn.Open();

                //var ret = conn.Query<int>("SELECT Count(*) FROM SelfInvoices WHERE MaterialId = @Id", new
                //{
                //    client.Id
                //});

                //var totUsedMaterials = ret != null ? ret.First() : 0;
                //if (totUsedMaterials > 0)
                //    return false;

                conn.Execute("DELETE FROM Clients WHERE Id = @Id",
                    new
                    {
                        client.Id
                    });
            }
            return true;
        }


        #endregion

        #region COMMON

        public List<Common> CommonsGet()
        {
            using (var conn = Connection)
            {
                conn.Open();
                return conn.Query<Common>("SELECT * FROM Common").ToList();
            }
        }

        public Common CommonGet(int year)
        {
            using (var conn = Connection)
            {
                conn.Open();
                return
                    conn.Query<Common>("SELECT * FROM Common WHERE WorkYear = @WorkYear", new {WorkYear = year})
                        .SingleOrDefault();
            }
        }

        public void CommonUpdate(Common common)
        {
            using (var conn = Connection)
            {
                conn.Open();
                conn.Execute("UPDATE Common SET CurrentAvailableInvoiceNumber = @CurrentAvailableInvoiceNumber WHERE WorkYear = @WorkYear",
                    new {common.CurrentAvailableInvoiceNumber, common.WorkYear});
            }
        }

        public void CommonInsert(Common common)
        {
            using (var conn = Connection)
            {
                conn.Open();
                conn.Execute("INSERT INTO Common (CurrentAvailableInvoiceNumber, WorkYear) VALUES (@CurrentAvailableInvoiceNumber, @WorkYear)",
                    new {common.CurrentAvailableInvoiceNumber, common.WorkYear});
            }
        }

        #endregion









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
