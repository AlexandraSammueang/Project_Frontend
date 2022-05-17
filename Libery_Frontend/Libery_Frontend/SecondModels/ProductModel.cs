﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Libery_Frontend.SecondModels
{
    public class ProductModel
    {
        public string Image { get; set; } = default;
        public string Name { get; set; } = default;
        public string Info { get; set; } = default;
        public string Type { get; set; } = default;
        public int ProId { get; set; } = default;
        public int UnitPrice { get; set; } = default;

    }

    public class GroupedProducts
    {
        public string ProductType { get; set; } = default;
        public List<Product> Products { get; set; } = new List<Product>();
    }
}