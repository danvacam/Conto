using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlServerCe;
using System.Linq;
using Dapper;

namespace Conto.Data
{
    public class ContoData
    {
        private SqlCeConnection Connection
        {
            get
            {
                return new SqlCeConnection( ConfigurationManager.ConnectionStrings["ContoDatabase"].ConnectionString);
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
                return conn.Query<Material>("SELECT * FROM Materials").ToList();
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
                return conn.Query<Measures>("SELECT * FROM Measures WHERE Id = @Id", new {Id = id}).SingleOrDefault();
            }
        }

        #endregion

        #region SELFINVOICES

        
//SELECT        SelfInvoices.InvoiceGroupId, Materials.Description AS MaterialDescription, SUM(SelfInvoices.Quantity) AS Quantity, SUM(SelfInvoices.InvoiceCost) AS Cost
//FROM            SelfInvoices INNER JOIN
//                         Materials ON SelfInvoices.MaterialId = Materials.Id
//GROUP BY SelfInvoices.InvoiceGroupId, Materials.Description

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
                conn.Execute(
                    "UPDATE SelfInvoices SET InCashFlow = 1 WHERE InvoiceGroupId = @invoiceGroupId",
                    new
                    {
                        invoiceGroupId= selfInvoice.InvoiceGroupId
                    });
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
                    conn.Query<CommonDataObject>("SELECT * FROM Common WHERE work_year = @year", new {year})
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
                        new {month, year}).ToList();
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
