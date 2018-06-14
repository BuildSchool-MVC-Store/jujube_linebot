using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSLibrary.ViewModels
{
    public class ProductDetail
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public Dictionary<string, List<string>> ColorSize { get; set; }
        public List<string> Image { get; set; }
        public string Comments { get; set; }
        public List<string> Color { get; set; }
        public List<string> Size { get; set; }
    }
}
