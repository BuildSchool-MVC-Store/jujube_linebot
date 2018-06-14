using OSLibrary.Models;
using OSLibrary.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Configuration;

namespace OSLibrary.ADO.NET.Repositories
{
    public class EmployeesRepository : IRepository<Employees>
    {
        private SqlConnection connection = new SqlConnection(SqlConnect.str);
        public void Create(Employees model)
        {
            var sql = "INSERT INTO Employees VALUES (@Account, @Password, @Name, @Birthday, @Email, @Phone, @Address)";
            var exec = connection.Execute(sql, model);
        }
        public void Update(Employees model)
        {

            var sql = "UPDATE Employees SET Account = @Account, Password = @Password, Name = @Name, Birthday = @Birthday, Email = @Email, Phone = @Phone, Address = @Address WHERE Account = @Account";
            var exec = connection.Execute(sql, model);

        }
        public void Delete(string Account)
        {

            var sql = "DELETE FROM Employees WHERE Account = @Account";
            var exec = connection.Execute(sql, new { Account });

        }
        public Employees GetByEmployeesAccount(string account)
        {
            var sql = "SELECT * FROM Employees WHERE Account = @Account";
            return connection.QueryFirstOrDefault<Employees>(sql, new { Account = account });

        }
        public IEnumerable<Employees> GetAll()
        {

            var sql = "SELECT * FROM Employees";
            return connection.Query<Employees>(sql);
        }
    }
}
