using E_Commerce.Core.Entities.Basket_Module;
using E_Commerce.Core.Interfaces.Repositories;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace E_Commerce.Repository.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;
        public BasketRepository(IConnectionMultiplexer radis)//ask clr for object from class implement IConnectionMultiplexer
        {
            _database=radis.GetDatabase();
        }



        public async Task<bool> DeleteBasketAsync(string BasketId)
        {
            return await _database.KeyDeleteAsync(BasketId);
        }

        public async Task<CustomerBasket?> GetBasketAsync(string BasketId)
        {
           var Basket = await _database.StringGetAsync(BasketId);


            return Basket.IsNull ? null : JsonSerializer.Deserialize<CustomerBasket>(Basket);//json=>customerBasket
        }                                  

        public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket Basket)
        {
            var JsonBasket=JsonSerializer.Serialize(Basket);//customerBasket => json
            var CreatedOrUpdated= await _database.StringSetAsync (Basket.Id, JsonBasket,TimeSpan.FromDays(1));
            if (!CreatedOrUpdated) return null;
            return await GetBasketAsync(Basket.Id);
        }
    }
}
