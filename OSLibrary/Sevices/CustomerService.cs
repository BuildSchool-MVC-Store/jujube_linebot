using OSLibrary.ADO.NET.Repositories;
using OSLibrary.Containers;
using OSLibrary.Models;
using OSLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSLibrary.Sevices
{
    public class CustomerService
    {
        public bool SearchAccount(string account)
        {
            if (RepositoryContainer.GetInstance<CustomerRepository>().GetByAccount(account) == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool CheckAccount(string Account, string Password)
        {
            CustomerRepository repository = new CustomerRepository();
            try
            {
                if (repository.GetByAccount(Account).Password == Password)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public string GetName(string Account)
        {
            CustomerRepository repository = new CustomerRepository();
            return repository.GetByAccount(Account).Name;
        }
        public bool CreateAccount(string Account,string Email , string Password)
        {
            CustomerRepository customerRepository = new CustomerRepository();
            if(customerRepository.GetByAccount(Account) == null)
            {
                try
                {
                    Customers customers = new Customers()
                    {
                        Account = Account,
                        Password = Password,
                        Email = Email,
                    };
                    customerRepository.Create(customers);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }
        public CustomerDetail GetCustomerDetail(string Account)
        {
            var C_repository = RepositoryContainer.GetInstance<CustomerRepository>();
            var O_repostiory = RepositoryContainer.GetInstance<OrdersRepository>();
            var OD_repostiory = RepositoryContainer.GetInstance<Order_DetailsRepository>();
            var result = C_repository.GetByAccount(Account);
            var customer = new CustomerDetail {
                CustomerName = result.Name,
                Address = result.Address,
                Phone = result.Phone,
                Email = result.Email,
                Birthday = result.Birthday
            };
            customer.Order = O_repostiory.GetByAccountOfPresonOrder(Account).ToList();
            foreach (var item in customer.Order)
            {
                item.details = OD_repostiory.GetByOrder_IDOfView(item.Order_ID).ToList();
            }
            return customer;
        }
        public bool UpdateCustomerDetail(string Account,string Name,string Email, string Phone, string Address)
        {
            try
            {
                var C_repository = RepositoryContainer.GetInstance<CustomerRepository>();
                C_repository.Update(new Customers {
                    Account = Account,
                    Address = Address,
                    Email = Email,
                    Name = Name,
                    Phone = Phone
                });
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool UpdateCustomerDetail(string Account,string Password, string Name, string Email, string Phone, string Address)
        {
            try
            {
                var C_repository = RepositoryContainer.GetInstance<CustomerRepository>();
                C_repository.UpdatePassword(Account,Password);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public IEnumerable<Customers> GetAll()
        {
            return RepositoryContainer.GetInstance<CustomerRepository>().GetAll();
        }
    }
}
