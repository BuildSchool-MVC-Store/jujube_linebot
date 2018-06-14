using OSLibrary.ADO.NET.Repositories;
using OSLibrary.Containers;
using OSLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSLibrary.Sevices
{
    public class EmployeeService
    {
        public bool GetEmployee(string account,string password)
        {
            var E_repository = RepositoryContainer.GetInstance<EmployeesRepository>();
            var employee = E_repository.GetByEmployeesAccount(account);
            if (employee != null && employee.Password == password)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public IEnumerable<Employees> GetAll()
        {
            return RepositoryContainer.GetInstance<EmployeesRepository>().GetAll();
        }
        public Employees GetEmployeeDetail(string Account)
        {
            return RepositoryContainer.GetInstance<EmployeesRepository>().GetByEmployeesAccount(Account);
        }
    }
}
