using OSLibrary.ADO.NET.Repositories;
using OSLibrary.Containers;
using OSLibrary.Models;
using OSLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSLibrary.Sevices
{
    public class OrdersService
    {
        public IEnumerable<Orders> GetOrderByAccount(string Account)
        {
            return new OrdersRepository().GetByAccount(Account);
        }
        public string CreateOrder(string Account, string Pay, string Transport, decimal TranMoney)
        {
            var cart_R = RepositoryContainer.GetInstance<ShoppingCartRepository>();
            var orders_R = new OrdersRepository();
            var order_Details_R = new Order_DetailsRepository();
            var stock_R = new StockRepository();
            var products_R = new ProductsRepository();

            var now_time = DateTime.Now;
            SqlConnection connection = new SqlConnection(SqlConnect.str);
            connection.Open();
            var transaction = connection.BeginTransaction();
            try
            {
                orders_R.Create(connection, new Orders
                {
                    Account = Account,
                    Order_Check = "New",
                    Order_Date = now_time,
                    Pay = Pay,
                    TranMoney = TranMoney,
                    Transport = Transport,
                }, transaction);

                string errorMessage = "";
                var order = orders_R.GetLatestByAccount(connection,Account,transaction);
                var items = cart_R.GetByAccount(connection,Account,transaction);
                decimal totalmoney = 0;
                foreach (var item in items)
                {
                    if (stock_R.CheckInventory(item.Product_ID, item.size,item.Color,item.Quantity) == false)
                    {
                        errorMessage += "產品 :" + products_R.GetByProduct_ID(item.Product_ID).Product_Name + " 庫存不足\n";
                    }
                    else
                    {
                        totalmoney += item.Quantity * products_R.GetByProduct_ID(item.Product_ID).UnitPrice;
                        order_Details_R.Create(connection, new Order_Details()
                        {
                            Order_ID = order.Order_ID,
                            Product_ID = item.Product_ID,
                            Quantity = (short)item.Quantity,
                            size = item.size,
                            Price = item.Quantity * products_R.GetByProduct_ID(item.Product_ID).UnitPrice,
                            Color = item.Color
                        },transaction);
                        stock_R.Update(connection, new Stock()
                        {
                            Product_ID = item.Product_ID,
                            Size = item.size,
                            Quantity = stock_R.GetByPK(item.Product_ID, item.size ,item.Color).Quantity - item.Quantity,
                            Color = item.Color
                        },transaction);
                    }
                }
                if (errorMessage.Length <= 1)
                {
                    cart_R.DeleteByAccount(Account);
                    orders_R.Update(order.Order_ID, totalmoney+TranMoney,connection,transaction);
                    transaction.Commit();
                    connection.Close();
                    return "新訂單";
                }
                else
                {
                    throw new Exception(errorMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                transaction.Rollback();
                return ex.Message;
            }
        }
        public OrderAndDetail GetNewOrders(string Account)
        {
            OrdersRepository ordersRepository = new OrdersRepository();
            Order_DetailsRepository order_DetailsRepository = new Order_DetailsRepository();
            var order = ordersRepository.GetLatestByAccount(Account);
            var result = new OrderAndDetail
            {
                Account = order.Account,
                OrderID = order.Order_ID,
                Order_Check = order.Order_Check,
                Order_Date = order.Order_Date,
                Pay = order.Pay,
                Transport = order.Transport,
                TotalMoney = (decimal)order.Total+(decimal)order.TranMoney
            };
            result.product = ordersRepository.GetAccountNewOrder(Account).ToList();
            return result;
        }
        public IEnumerable<Orders> GetOrders()
        {
            return RepositoryContainer.GetInstance<OrdersRepository>().GetAll();
        }
        public IEnumerable<Orders> GetOrders(string status)
        {
            if(status == "all")
            {
                return RepositoryContainer.GetInstance<OrdersRepository>().GetAll();

            }
            else
            {
                return RepositoryContainer.GetInstance<OrdersRepository>().GetByStatus(status);
            }
        }
    }
}
