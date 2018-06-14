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
    public class StockRepository : IRepository<Stock>
    {
        private SqlConnection connection = new SqlConnection(SqlConnect.str);
        public void Create(Stock model)
        {
            var sql = "INSERT INTO Stock VALUES (@Product_ID,@Quantity,@Size,@Color)";
            var exec = connection.Execute(sql, model);
        }
        public void Update(SqlConnection connection, Stock model, IDbTransaction transaction)
        {
            var sql = "UPDATE Stock SET Quantity=@Quantity WHERE Product_ID = @Product_ID and Size=@Size and Color = @Color";
            connection.Execute(sql, model, transaction);
        }
        public void Update(Stock model)
        {
            var sql = "UPDATE Stock SET Quantity=@Quantity WHERE Product_ID = @Product_ID and Size=@Size and Color = @Color";
            connection.Execute(sql, model);
        }

        public void Delete(Stock model)
        {
            var sql = "DELETE FROM Stock WHERE Product_ID = @Product_ID and Size=@Size and Color = @Color";
            connection.Execute(sql, new { model.Product_ID, model.Size });
        }
        public Stock GetByPK(int Product_ID, string Size, string Color)
        {
            var sql = "SELECT * FROM Stock WHERE Product_ID = @Product_ID and Size=@Size and Color = @Color";
            return connection.QueryFirstOrDefault<Stock>(sql, new { Product_ID, Size, Color });
        }
        public IEnumerable<Stock> GetByProductID(int Product_ID)
        {
            var sql = "SELECT * FROM Stock WHERE Product_ID = @Product_ID";
            return connection.Query<Stock>(sql, new { Product_ID });
        }

        public IEnumerable<Stock> GetAll()
        {
            var sql = "SELECT * FROM Stock";
            return connection.Query<Stock>(sql);
        }
        public bool CheckInventory(int Product_ID, string Size, string Color, int needQuantity)
        {
            var sql = "SELECT * FROM Stock WHERE Product_ID = @Product_ID AND Size=@Size and Color = @Color";
            var item = connection.QueryFirstOrDefault<Stock>(sql, new { Product_ID, Size, Color });
            if (item.Quantity >= needQuantity)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public IEnumerable<StockDetail>GetAllOfBackStage()
        {
            var sql = "SELECT p.Product_ID , p.Product_Name , s.Color ,s.Size , s.Quantity FROM Stock as s INNER JOIN Products as p on s.Product_ID = p.Product_ID";
            return connection.Query<StockDetail>(sql);
        }
    }
}
