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

        public void UpdateCustomer(CustomerDataObject customer)
        {
            
        }

        public void DeleteCustomer(CustomerDataObject customer)
        {
            
        }

        public CommonDataObject GetCommonData()
        {
            using (
                var conn = new SqlCeConnection(ConfigurationManager.ConnectionStrings["ContoDatabase"].ConnectionString)
                )
            {
                conn.Open();
                return conn.Query<CommonDataObject>("SELECT * FROM Common").FirstOrDefault();
            }
        }

        public List<MaterialDataObject> GetMaterials()
        {
            using (
                var conn = new SqlCeConnection(ConfigurationManager.ConnectionStrings["ContoDatabase"].ConnectionString)
                )
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
