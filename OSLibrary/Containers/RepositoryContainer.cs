using OSLibrary.ADO.NET.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSLibrary.Containers
{
    public class RepositoryContainer
    {
        public static RContainer _container;
        public static RContainer Container
        {
            get
            {
                if(_container == null)
                {
                    CreateContainer();
                }
                return _container;
            }
        }

        public static T GetInstance<T>()
        {
            return Container.GetInstance<T>(); 
        }

        private static void CreateContainer()
        {
            _container = new RContainer();
            _container.Register<CustomerRepository, CustomerRepository>();
            _container.Register<OrdersRepository, OrdersRepository>();
            _container.Register<Order_DetailsRepository, Order_DetailsRepository>();
            _container.Register<ProductsRepository, ProductsRepository>();
            _container.Register<ProductImageRepository, ProductImageRepository>();
            _container.Register<StockRepository, StockRepository>();
            _container.Register<EmployeesRepository, EmployeesRepository>();
            _container.Register<ShoppingCartRepository, ShoppingCartRepository>();
            _container.Register<CategoryRepository, CategoryRepository>();
        }
    }
}
