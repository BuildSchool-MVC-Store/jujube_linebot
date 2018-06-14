using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OSLibrary.ViewModels
{
    public class ProductModel
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string CategoryName { get; set; }
        public string Gender { get; set; }
        public string ImageUrl { get; set; }
    }
}