using System.Data;
using System.Data.SqlClient;
using TechnicalTest.Models;

namespace TechnicalTest.Process
{
    public class ProcessCustomers
    {

        public Customers getCustomer()
        {
            Customers objCustomer = new Customers();

            string connect = Startup.mainConnection;

            SqlConnection connection = new SqlConnection(connect);
            string query = "SELECT * FROM dbo.Customers WHERE idCustomer = 1";
            DataTable tb = new DataTable();

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                using (SqlDataReader dr = command.ExecuteReader())
                {
                    tb.Load(dr);
                }
            }
            connection.Close();

            foreach (DataRow row in tb.Rows)
            {
                objCustomer.customerId = (int)row["customerId"];
                objCustomer.name1 = row["name1"].ToString();
                objCustomer.name2 = row["name2"].ToString();
                objCustomer.lastName1 = row["lastname1"].ToString();
                objCustomer.lastName2 = row["lastName2"].ToString();
            }

            return objCustomer;
        }
    }
}
