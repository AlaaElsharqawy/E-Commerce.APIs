using E_Commerce.Core.Entities.Order_Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Specifications.Order_Spec
{
    public class OrderSpecifications : BaseSpecifications<Order>
    {

        public OrderSpecifications(string email) : base(o => o.BuyerEmail == email)
        {
            Includes.Add(o => o.DeliveryMethod);
            Includes.Add(o => o.Items);//Eger loading

            ApplyOrderByDescending(o => o.OrderDate);//Newer to Older

        }




        public OrderSpecifications(string email, int OrderId) : base(o => o.BuyerEmail == email && o.Id == OrderId)
        {
            Includes.Add(o => o.DeliveryMethod);
            Includes.Add(o => o.Items);//Eger loading
        }
    }
}
