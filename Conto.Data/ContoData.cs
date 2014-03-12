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
                return conn.Query<Material>("SELECT * FROM Material").ToList();
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





        #region CUSTOMERS

        public List<CustomerDataObject> GetCustomers()
        {
            using (
                var conn = new SqlCeConnection(ConfigurationManager.ConnectionStrings["ContoDatabase"].ConnectionString)
                )
            {
                conn.Open();
                return conn.Query<CustomerDataObject>("SELECT * FROM Customer").ToList();
            }
        }

        public void AddCustomer(CustomerDataObject customer)
        {
            using (var conn = new SqlCeConnection(
                ConfigurationManager.ConnectionStrings["ContoDatabase"].ConnectionString))
            {
                conn.Open();
                conn.Execute(
                    "INSERT INTO Customer (name, address, fiscal_code, vat_code) VALUES (@name, @address, @fiscal_code, @vat_code);",
                    new
                    {
                        name = customer.Name,
                        address = customer.Address,
                        fiscal_code = customer.Fiscal_Code,
                        vat_code = customer.Vat_Code
                    });
            }
        }

        public void UpdateCustomer(CustomerDataObject customer)
        {
            using (var conn = new SqlCeConnection(
                ConfigurationManager.ConnectionStrings["ContoDatabase"].ConnectionString))
            {
                conn.Open();
                conn.Execute(
                    "UPDATE Customer SET name = @name, address = @address, fiscal_code = @fiscal_code, vat_code = @vat_code WHERE id = @id",
                    new
                    {
                        name = customer.Name,
                        address = customer.Address,
                        fiscal_code = customer.Fiscal_Code,
                        vat_code = customer.Vat_Code,
                        id = customer.Id
                    });
            }
        }

        public void DeleteCustomer(CustomerDataObject customer)
        {
            using (var conn = new SqlCeConnection(
                ConfigurationManager.ConnectionStrings["ContoDatabase"].ConnectionString))
            {
                conn.Open();
                conn.Execute(
                    "DELETE Customer WHERE id = @id);",
                    new
                    {
                        id = customer.Id
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

        public IEnumerable<SelfInvoiceDataObject> CashFlowSelfInvoices()
        {
            using (var conn = new SqlCeConnection(
                ConfigurationManager.ConnectionStrings["ContoDatabase"].ConnectionString))
            {
                conn.Open();
                return conn.Query<SelfInvoiceDataObject>("SELECT TOP 50 * FROM SelfInvoice WHERE InCashFlow = 0 ORDER BY Invoicedate ASC").ToList();
            }
        }

        public void CashFlowAdd(CashFlowDataObject cashFlow)
        {
            using (var conn = new SqlCeConnection(
                ConfigurationManager.ConnectionStrings["ContoDatabase"].ConnectionString))
            {
                conn.Open();
                var res = conn.Execute("INSERT INTO CashFlow (Cash, Description, FlowDate) VALUES (@Cash, @Description, @FlowDate)", new { Cash = cashFlow.Cash, Description = cashFlow.Description, FlowDate = cashFlow.FlowDate });
            }
        }

        #endregion
    }
}
