﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXAM_ProductShop.Models
{
    public class ClientBasketProduct
    {
        public int Id { get; set; }
        public string Product { get; set; }
        public int Price { get; set; }
        public int Count { get; set; }

        public ClientBasketProduct()
        {

        }
    }
}
