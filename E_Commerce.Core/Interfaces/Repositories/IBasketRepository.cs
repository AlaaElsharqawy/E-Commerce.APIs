using E_Commerce.Core.Entities.Basket_Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Interfaces.Repositories
{
     public interface IBasketRepository
    {

        Task<CustomerBasket?> GetBasketAsync(string BasketId);

        Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket Basket);

        Task<bool> DeleteBasketAsync(string BasketId);
    }
}
