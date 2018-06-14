using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSLibrary.ViewModels
{
    public class CustomerDetail
    {
        public string CustomerName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PassWord { get; set; }
        public DateTime Birthday { get; set; }
        public List<PersonOrder> Order { get; set; }
    }
    public class PersonOrder
    {
        public int Order_ID { get; set; }
        public DateTime Order_Date { get; set; }
        public decimal Total { get; set; }
        public string Order_Check { get; set; }
        public List<Person_OrderDetail> details { get; set; }
    }
    public class Person_OrderDetail
    {
        public int Product_ID { get; set; }
        public string Product_Name { get; set; }
        public string size { get; set; }
        public string Color { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
