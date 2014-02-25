using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Conto.Data
{
    public class ContoData
    {
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
    }
}
