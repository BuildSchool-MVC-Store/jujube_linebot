using Dapper;
using OSLibrary.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSLibrary.ADO.NET.Repositories
{
    public class CategoryRepository : IRepository<Category>
    {
        private SqlConnection connection = new SqlConnection(SqlConnect.str);
        public void Create(Category model)
        {
            var sql = ("INSERT INTO Category (CategoryName) VALUES (@CategoryName)");
            var exec = connection.Execute(sql);
        }

        public IEnumerable<Category> GetAll()
        {
            var sql = ("SELECT * FROM Category");
            return connection.Query<Category>(sql);
        }
    }
}
