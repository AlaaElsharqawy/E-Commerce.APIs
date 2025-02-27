﻿using E_Commerce.Core.Entities.Product_Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Entities.Order_Module
{
   public class OrderItem:BaseEntity
    {
        public OrderItem()
        {
            
        }
        public OrderItem(ProductItemOrdered product, decimal price, int quantity)
        {
            Product = product;
            Price = price;
            Quantity = quantity;
        }

        public ProductItemOrdered Product {  get; set; }

        public decimal Price { get; set; }
        public int Quantity { get; set; }

    }
}
