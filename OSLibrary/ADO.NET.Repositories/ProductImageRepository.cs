using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Runtime.Serialization.Formatters.Binary;
using OSLibrary.Utils;
using Dapper;
using OSLibrary.Models;
using System.Configuration;

namespace OSLibrary.ADO.NET.Repositories
{
    public class ProductImageRepository : IRepository<Product_Image>
    {
        private SqlConnection connection = new SqlConnection(SqlConnect.str);
        public void Create(Product_Image model)
        {
            var sql = "INSERT INTO Product_Image VALUES (@Product_ID, @Picture, @Product_Image_Only)";
            var exec = connection.Execute(sql, model);
        }
        public void Update(Product_Image model)
        {
            var sql = "UPDATE Product_Image SET Product_ID = @Product_ID, Picture = @Picture, Product_Image_Only = @Product_Image_Only WHERE Product_Image_ID = @Product_Image_ID";
            var exec = connection.Execute(sql, model);
        }
        public void Delete(int Product_Image_ID)
        {
            var sql = "DELETE FROM Product_Image WHERE Product_Image_ID = @Product_Image_ID";
            var exec = connection.Execute(sql, new { Product_Image_ID });
        }
        public IEnumerable<Product_Image> GetByProduct_ID(int Product_ID)
        {
            var sql = "SELECT * FROM Product_Image WHERE Product_ID = @Product_ID";
            return connection.Query<Product_Image>(sql,new { Product_ID});
        }
        public IEnumerable<Product_Image> GetAll()
        {
            var sql = "SELECT * FROM Product_Image";
            return connection.Query<Product_Image>(sql);
        }
    }
}
