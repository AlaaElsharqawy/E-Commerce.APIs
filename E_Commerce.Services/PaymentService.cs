using E_Commerce.Core.Entities.Basket_Module;
using E_Commerce.Core.Entities.Order_Module;
using E_Commerce.Core.Entities.Product_Module;
using E_Commerce.Core.Interfaces;
using E_Commerce.Core.Interfaces.Repositories;
using E_Commerce.Core.Interfaces.Services;
using E_Commerce.Core.Specifications;
using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product = E_Commerce.Core.Entities.Product_Module.Product;

namespace E_Commerce.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly IBasketRepository _basketRepo;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IConfiguration configuration, IBasketRepository basketRepo
            , IUnitOfWork unitOfWork)
        {
            this._configuration = configuration;
            this._basketRepo = basketRepo;
            this._unitOfWork = unitOfWork;
        }
        #region Create Or Update PaymentIntent
        public async Task<CustomerBasket?> CreateOrUpdatePaymentIntentAsync(string basketId)
        {
            //Secret Key (speak to stripe)
            StripeConfiguration.ApiKey = _configuration["StripeKeys:Secretkey"];
            //Get Basket
            var Basket = await _basketRepo.GetBasketAsync(basketId);
            if (Basket is null) return null;
            //create or update paymentIntent Dependence on
            //Total=subtotal+DeliveryMethodCost(shippingPrice)
            var shippingPrice = 0M;
            if (Basket.DeliveryMethodId.HasValue)
            {

                var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(Basket.DeliveryMethodId.Value);
                shippingPrice = deliveryMethod.Cost;
            }


            //subtotal=price*quantity
            if (Basket.Items.Count > 0)
            {
                foreach (var item in Basket.Items)
                {
                    var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                    if (item.Price != product.Price)
                        item.Price = product.Price;
                }

            }
            var subtotal = Basket.Items.Sum(item => item.Price * item.Quantity);


            //create or update PaymentIntent
            var Service = new PaymentIntentService();
            PaymentIntent paymentIntent;
            if (string.IsNullOrEmpty(Basket.PaymentIntentId))//create
            {
                var CreatedOptions = new PaymentIntentCreateOptions()
                {
                    Amount = (long)(subtotal * 100 + shippingPrice * 100),
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" }
                };
                paymentIntent = await Service.CreateAsync(CreatedOptions);
                Basket.PaymentIntentId = paymentIntent.Id;
                Basket.ClientSecret = paymentIntent.ClientSecret;
               
            }
            else
            {
                //update
                var UpdatedOptions = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)(subtotal * 100 + shippingPrice * 100),

                };

                paymentIntent = await Service.UpdateAsync(Basket.PaymentIntentId, UpdatedOptions);

                Basket.PaymentIntentId = paymentIntent.Id;
                Basket.ClientSecret = paymentIntent.ClientSecret;

                


            }

            await _basketRepo.UpdateBasketAsync(Basket);
            return Basket;
        }


        #endregion
        public async Task<Order?> UpdatePaymentIntentToSucceedOrFailedAsync(string PaymentIntentId, bool flag)
        {
            var spec = new OrderWithPaymentIntentSpec(PaymentIntentId);
            var order = await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(spec);
            if(order is null)  return null; 

            if (flag)
            {
                order.Status = OrderStatus.PaymentReceived;
            }
            else
            {
                order.Status = OrderStatus.PaymentFailed;
            }
            _unitOfWork.Repository<Order>().Update(order);
           await _unitOfWork.CompleteAsync();
            return order;
        }




    }
}
