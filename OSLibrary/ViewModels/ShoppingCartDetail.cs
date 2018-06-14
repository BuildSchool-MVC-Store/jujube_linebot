using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSLibrary.ViewModels
{
    public class ShoppingCartDetail
    {
        public int ShoppingCartID { get; set; }
        public string Account { get; set; }
        public int ProductID { get; set; }
        public string Name { get; set; } 
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal RowPrice { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public string imgurl { get; set; }
    }
}
