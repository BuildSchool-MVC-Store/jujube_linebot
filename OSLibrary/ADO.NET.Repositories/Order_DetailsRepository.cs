using Dapper;
using OSLibrary.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSLibrary.Models;
using System.Configuration;
using OSLibrary.ViewModels;

namespace OSLibrary.ADO.NET.Repositories
{
    public class Order_DetailsRepository : IRepository<Order_Details>
    {
        public void Create(SqlConnection connection, Order_Details model, IDbTransaction transaction)
        {
            var sql = "INSERT INTO Order_Details (Order_ID,Product_ID,Quantity,Price,size ,Color)  VALUES (@Order_ID, @Product_ID, @Quantity, @Price, @size,@Color)";
            connection.Execute(sql, model,transaction);
        }
        public void Create(Order_Details model)
        {
            using (SqlConnection connection = new SqlConnection(SqlConnect.str))
            {
                var sql = "INSERT INTO Order_Details (Order_ID,Product_ID,Quantity,Price,size,Color)  VALUES (@Order_ID, @Product_ID, @Quantity, @Price, @size,@Color)";
                connection.Execute(sql, model);
            }
        }
        public void Update(Order_Details model)
        {
            using (SqlConnection connection = new SqlConnection(SqlConnect.str))
            {
                var sql = "UPDATE Order_Details SET Order_ID = @Order_ID, Product_ID = @Product_ID, Quantity = @Quantity, Price = @Price, Discount = @Discount, size = @size ,Color = @Color WHERE Order_Details_ID = @Order_Details_ID";
                connection.Execute(sql, model);
            }
        }
        public void Delete(int Order_Details_ID)
        {
            using (SqlConnection connection = new SqlConnection(SqlConnect.str))
            {
                var sql = "DELETE FROM Order_Details WHERE Order_Details_ID = @Order_Details_ID";
                connection.Execute(sql, new { Order_Details_ID });
            }
        }

        public Order_Details GetByOrder_Details_ID(int Order_Details_ID)
        {
            using (SqlConnection connection = new SqlConnection(SqlConnect.str))
            {
                var sql = "SELECT * FROM Order_Details WHERE Order_Details_ID = @Order_Details_ID";
                return connection.QueryFirstOrDefault<Order_Details>(sql, new { Order_Details_ID = Order_Details_ID });
            }
        }
        public IEnumerable<Order_Details> GetAll()
        {
            using (SqlConnection connection = new SqlConnection(SqlConnect.str))
            {
                var sql = "SELECT * FROM Order_Details";
                return connection.Query<Order_Details>(sql);
            }
        }
        public IEnumerable<Order_Details> GetByOrderID(int OrderID)
        {
            using (SqlConnection connection = new SqlConnection(SqlConnect.str))
            {
                var sql = "SELECT * FROM Order_Details WHERE Order_ID = @OrderID";
                return connection.Query<Order_Details>(sql, new { OrderID });
            }
        }
        public IEnumerable<Person_OrderDetail> GetByOrder_IDOfView(int Order_ID)
        {
            using (SqlConnection connection = new SqlConnection(SqlConnect.str))
            {
                var sql = "SELECT od.Product_ID, p.Product_Name, Quantity,size,Color,Price FROM Order_Details as od INNER JOIN Products as p on p.Product_ID = od.Product_ID WHERE Order_ID = @Order_ID";
                return connection.Query<Person_OrderDetail>(sql, new { Order_ID });
            }
        }
    }
}
