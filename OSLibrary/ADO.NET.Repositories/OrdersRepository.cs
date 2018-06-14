using OSLibrary.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSLibrary.Models;
using System.Configuration;
using OSLibrary.ViewModels;

namespace OSLibrary.ADO.NET.Repositories
{
    public class OrdersRepository : IRepository<Orders>
    {
        private string strConnection = "Server=140.126.146.49,7988;Database=2018Build;User Id=Build;Password=123456789;";
        public void Create(SqlConnection connection, Orders model, IDbTransaction transaction)
        {
            var sql = "INSERT INTO Orders (Order_Date, Account, Pay, Transport, Order_Check, Total, TranMoney) VALUES (@Order_Date, @Account, @Pay, @Transport, @Order_Check, @Total, @TranMoney)";
            connection.Execute(sql, model, transaction);
        }
        public void Update(Orders model)
        {
            using (SqlConnection connection = new SqlConnection(strConnection))
            {
                var sql = "UPDATE Orders SET Order_Date = @Order_Date, Account = @Account, Pay = @Pay, Transport = @Transport, Order_Check = @Order_Check, Total = @Total, TranMoney = @TranMoney WHERE Order_ID = @Order_ID";
                var exec = connection.Execute(sql, model);
            }
        }
        public void Delete(int Order_ID)
        {
            using (SqlConnection connection = new SqlConnection(strConnection))
            {
                var sql = "DELETE FROM Orders WHERE Order_ID = @Order_ID";
                var exec = connection.Execute(sql, new { Order_ID });
            }
        }
        public Orders GetByOrder_ID(int Order_ID)
        {
            using (SqlConnection connection = new SqlConnection(strConnection))
            {
                var sql = "SELECT * FROM Orders WHERE Order_ID = @Order_ID";
                return connection.QueryFirst<Orders>(sql, new { Order_ID });
            }
        }
       

        public IEnumerable<Orders> GetAll()
        {
            using (SqlConnection connection = new SqlConnection(strConnection))
            {

                var sql = "SELECT * FROM Orders ORDER BY Order_Date DESC";
                return connection.Query<Orders>(sql);
            }
        }
        public IEnumerable<Orders> GetByOrder_Date(DateTime from, DateTime to)
        {
            using (SqlConnection connection = new SqlConnection(strConnection))
            {
                var sql = "SELECT * FROM Orders WHERE Order_Date >= @from AND Order_Date<=@to";
                return connection.Query<Orders>(sql, new { from, to });
            }
        }
        public IEnumerable<Orders> GetByAccount(string Account)
        {
            using (SqlConnection connection = new SqlConnection(strConnection))
            {
                var sql = "SELECT * FROM Orders WHERE Account = @Account";
                return connection.Query<Orders>(sql, new { Account });
            }
        }
        public IEnumerable<PersonOrder> GetByAccountOfPresonOrder(string Account)
        {
            using (SqlConnection connection = new SqlConnection(strConnection))
            {
                var sql = "SELECT Order_ID ,Order_Date,Total,Order_Check FROM Orders WHERE Account = @Account ORDER BY Order_Date DESC";
                return connection.Query<PersonOrder>(sql, new { Account });
            }
        }
        public Orders GetLatestByAccount(SqlConnection connection, string Account, IDbTransaction transaction)
        {
            string sql = "SELECT TOP 1 * FROM Orders WHERE Account = @Account ORDER BY Order_Date DESC";
            return connection.QueryFirstOrDefault<Orders>(sql, new { Account }, transaction);
        }
        public Orders GetLatestByAccount(string Account)
        {
            using (SqlConnection connection = new SqlConnection(strConnection))
            {
                string sql = "SELECT TOP 1 * FROM Orders WHERE Account = @Account ORDER BY Order_Date DESC";
                return connection.QueryFirstOrDefault<Orders>(sql, new { Account });
            }
        }
        public void Update(int order_ID, decimal totalmoney, SqlConnection connection, SqlTransaction transaction)
        {
            var sql = "UPDATE Orders SET Total = @Total WHERE Order_ID = @Order_ID";
            connection.Execute(sql, new { Total = totalmoney, order_ID }, transaction);
        }
        public IEnumerable<Detail> GetAccountNewOrder(string Account)
        {
            using (SqlConnection connection = new SqlConnection(strConnection))
            {
                var sql = "SELECT p.Product_ID , p.Product_Name ,od.size,od.Color, od.Quantity ,p.UnitPrice,od.Price FROM Order_Details as od INNER JOIN Products as p on od.Product_ID = p.Product_ID WHERE Order_ID = (SELECT TOP 1 Order_ID FROM Orders WHERE Account = @Account order by Order_Date DESC) ";
                return connection.Query<Detail>(sql, new { Account });
            }
        }
        public IEnumerable<Orders> GetByStatus(string Order_Check)
        {
            using (SqlConnection connection = new SqlConnection(strConnection))
            {
                var sql = "SELECT * FROM Orders WHERE Order_Check = @Order_Check";
                return connection.Query<Orders>(sql, new { Order_Check });
            }
        }
    }
}
