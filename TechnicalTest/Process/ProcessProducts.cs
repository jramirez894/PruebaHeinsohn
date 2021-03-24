using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text.Json;
using TechnicalTest.Models;

namespace TechnicalTest.Process
{
    public class ProcessProducts
    {
        public List<Products> allProducts()
        {
            List<Products> lstProducts = new List<Products>();
            Products objProduct = new Products();

            string connect = Startup.mainConnection;

            SqlConnection connection = new SqlConnection(connect);
            string query = "SELECT " + Environment.NewLine +
                            "   P.*" + Environment.NewLine +
                            "   ,C.catName" + Environment.NewLine +
                            "   ,WHP.stock AS quantity " + Environment.NewLine +
                            "FROM " + Environment.NewLine +
                            "   dbo.Products AS P " + Environment.NewLine +
                            "   INNER JOIN dbo.Categories AS C ON P.catId = C.catId" + Environment.NewLine +
                            "   INNER JOIN dbo.WareHouseProducts AS WHP ON P.pId = WHP.pId" + Environment.NewLine +
                            "WHERE " + Environment.NewLine +
                            "   pState = 1";
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
                objProduct = new Products();
                objProduct.pId = (int)row["pId"];
                objProduct.pName = row["pName"].ToString();
                objProduct.pCost = (decimal)row["pCost"];
                objProduct.pPrice = (decimal)row["pPrice"];
                objProduct.pState = (int)row["pState"];
                objProduct.catId = (int)row["catId"];
                objProduct.catName = row["catName"].ToString();
                objProduct.quantity = (int)row["quantity"];

                lstProducts.Add(objProduct);
            }

            return lstProducts;
        }


        public Products getProduct(string filter)
        {
            Products objProduct = new Products();

            string connect = Startup.mainConnection;

            SqlConnection connection = new SqlConnection(connect);
          
            string query = "SELECT TOP 1" + Environment.NewLine +
                            "   P.*" + Environment.NewLine +
                            "   ,C.catName" + Environment.NewLine +
                            "   ,WHP.stock AS quantity " + Environment.NewLine +
                            "FROM " + Environment.NewLine +
                            "   dbo.Products AS P " + Environment.NewLine +
                            "   INNER JOIN dbo.Categories AS C ON P.catId = C.catId" + Environment.NewLine +
                            "   INNER JOIN dbo.WareHouseProducts AS WHP ON P.pId = WHP.pId" + Environment.NewLine +
                            "WHERE " + Environment.NewLine +
                            "   pState = 1" +
                            "   AND (P.pName = '"+filter+ "' OR P.pId LIKE '%" + filter +"%')";
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
                objProduct.pId = (int)row["pId"];
                objProduct.pName = row["pName"].ToString();
                objProduct.pCost = (decimal)row["pCost"];
                objProduct.pPrice = (decimal)row["pPrice"];
                objProduct.pState = (int)row["pState"];
                objProduct.catId = (int)row["catId"];
                objProduct.catName = row["catName"].ToString();
                objProduct.quantity = (int)row["quantity"];
            }

            return objProduct;
        }

        public void SaveSale(string json)
        {
            List<RequestProducts> objProduct = new List<RequestProducts>();

            objProduct = JsonSerializer.Deserialize<List<RequestProducts>>(json);

            int sumTotSale = 0;
            for (int i = 0; i< objProduct.Count; i++)
            {
                sumTotSale += objProduct[i].quantity * objProduct[i].price;
            }

            string connect = Startup.mainConnection;

            SqlConnection connection = new SqlConnection(connect);
            DataTable tb = new DataTable();
            int shId = 0;

           string query = "INSERT INTO dbo.SalesHeader" + Environment.NewLine +
                            "(" + Environment.NewLine +
                            "   [customerId]" + Environment.NewLine +
                            "   ,totSale" + Environment.NewLine +
                            ")" + Environment.NewLine +
                            "VALUES" + Environment.NewLine +
                            "(" + Environment.NewLine +
                            "   1" + Environment.NewLine +
                            "   ," + sumTotSale + "" + Environment.NewLine +
                            ")" + Environment.NewLine +
                            "DECLARE @shId AS INT = (SELECT SCOPE_IDENTITY());" + Environment.NewLine +
                            "SELECT @shId";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                using (SqlDataReader dr = command.ExecuteReader())
                {
                    tb.Load(dr);
                    shId = (int)tb.Rows[0].ItemArray[0];
                }
            }

            for (int i = 0; i < objProduct.Count; i++)
            {
                string query2 = "INSERT INTO dbo.SalesDetails" + Environment.NewLine +
                            "(" + Environment.NewLine +
                            "   shId" + Environment.NewLine +
                            "   ,pId" + Environment.NewLine +
                            "   ,quantity" + Environment.NewLine +
                            "   ,pUnitPrice" + Environment.NewLine +
                            ")" + Environment.NewLine +
                            "VALUES" + Environment.NewLine +
                            "(" + Environment.NewLine +
                            "   " + shId + "" + Environment.NewLine +
                            "   ," + objProduct[i].pId + "" + Environment.NewLine +
                            "   ," + objProduct[i].quantity + "" + Environment.NewLine +
                            "   ," + objProduct[i].price + "" + Environment.NewLine +
                            ")" + Environment.NewLine +
                            "" + Environment.NewLine +
                            "" + Environment.NewLine +
                            "UPDATE WHP" + Environment.NewLine +
                            "SET" + Environment.NewLine +
                            "   stock = stock - " + objProduct[i].quantity + "" + Environment.NewLine +
                            "FROM" + Environment.NewLine +
                            "   dbo.WareHouseProducts  AS WHP" + Environment.NewLine +
                            "WHERE " + Environment.NewLine +
                            "   pId = " + objProduct[i].pId + "";
                using (SqlCommand command = new SqlCommand(query2, connection))
                {
                    using (SqlDataReader dr = command.ExecuteReader())
                    {
                        tb.Load(dr);
                    }
                }
            }
            connection.Close();


        }
    }
}
