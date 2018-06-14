using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSLibrary.ViewModels
{
    public class OrderAndDetail
    {
        public int OrderID { get; set; }
        public DateTime Order_Date { get; set; }
        public string Account { get; set; }
        public string Pay { get; set; }
        public string Transport { get; set; }
        public string Order_Check { get; set; }
        public decimal TotalMoney { get; set; }
        public List<Detail> product { get; set; }
    }
    public class Detail
    {
        public int Product_ID { get; set; }
        public string Product_Name { get; set; }
        public int Quantity { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Price { get; set; }
    }
}
