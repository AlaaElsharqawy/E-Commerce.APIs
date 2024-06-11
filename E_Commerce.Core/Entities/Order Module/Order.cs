﻿using E_Commerce.Core.Entities.Product_Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Entities.Order_Module
{
    public class Order:BaseEntity
    {
        public Order()
        {
            
        }
        public Order(string buyerEmail,   Address shippingAddress, DeliveryMethod deliveryMethod, ICollection<OrderItem> items, decimal subTotal,string paymentIntentId)
        {
            BuyerEmail = buyerEmail;

            ShippingAddress = shippingAddress;
            DeliveryMethod = deliveryMethod;
            Items = items;
            SubTotal = subTotal;
            PaymentIntentId = paymentIntentId;
        }

        public string BuyerEmail { get; set; }

        public DateTimeOffset OrderDate { get; set; }= DateTimeOffset.Now;

        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public Address ShippingAddress { get; set; }//Aggregate relationShip

        public DeliveryMethod DeliveryMethod { get; set; }//Navigational Property

        public ICollection<OrderItem> Items { get; set; }=new HashSet<OrderItem>();

        public decimal SubTotal { get; set; }

        public decimal GetTotal()
        {
            return SubTotal + DeliveryMethod.Cost;
        }

        public string PaymentIntentId { get; set; } 


    }
}
