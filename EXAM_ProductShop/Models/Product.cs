using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXAM_ProductShop.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public int Count { get; set; }

        public Product()
        {

        }
    }
}
