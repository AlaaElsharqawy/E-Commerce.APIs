using E_Commerce.Core.Entities.Order_Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Interfaces.Services
{
    public interface IOrderService
    {
        Task<Order?> CreateOrderAsync(string buyerEmail,string basketId,int DeliveryMethodId,Address ShippingAddress);

        Task<IReadOnlyList<Order>> GetOrdersForSpecificUserAsync(string buyerEmail);

        Task<Order> GetOrderByIdForSpecificUserAsync(string buyerEmail, int OrderId);

    }
}
