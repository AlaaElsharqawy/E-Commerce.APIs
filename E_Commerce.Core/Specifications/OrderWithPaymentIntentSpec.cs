using E_Commerce.Core.Entities.Order_Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Specifications
{
   public class OrderWithPaymentIntentSpec:BaseSpecifications<Order>
    {
        public OrderWithPaymentIntentSpec(string? paymentIntentId):base(o=>o.PaymentIntentId==paymentIntentId)
        {
            Includes.Add(p => p.DeliveryMethod);
            Includes.Add(p => p.Items);
        }
    }
}
