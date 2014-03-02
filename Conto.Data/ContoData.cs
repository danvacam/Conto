using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        

        public List<MaterialDataObject> GetMaterials()
        {
            using (var conn = new SqlCeConnection(
                ConfigurationManager.ConnectionStrings["ContoDatabase"].ConnectionString))
            {
                conn.Open();
                return conn.Query<MaterialDataObject>("SELECT * FROM Material").ToList();
            }
        }

        public void UpdateMaterial(MaterialDataObject material)
        {

        }

        public void DeleteMaterial(MaterialDataObject material)
        {

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
