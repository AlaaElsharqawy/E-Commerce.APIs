using E_Commerce.Core.Entities.Basket_Module;
using E_Commerce.Core.Entities.Order_Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Interfaces.Services
{
  public interface IPaymentService
    {
        //function  =>speak to stripe through

        Task<CustomerBasket?> CreateOrUpdatePaymentIntentAsync(string basketId);
        Task<Order?> UpdatePaymentIntentToSucceedOrFailedAsync(string PaymentIntentId,bool flag);
    }
}
