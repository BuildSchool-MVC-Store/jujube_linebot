using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSLibrary.ViewModels
{
    public class StockDetail
    {
        public int Product_ID { get; set; }
        public string Product_Name { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public int Quantity { get; set; }
    }
}
