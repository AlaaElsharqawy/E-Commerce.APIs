using E_Commerce.Core.Entities.Order_Module;
using E_Commerce.Core.Entities.Product_Module;
using E_Commerce.Core.Interfaces;
using E_Commerce.Core.Interfaces.Repositories;
using E_Commerce.Core.Interfaces.Services;
using E_Commerce.Core.Specifications;
using E_Commerce.Core.Specifications.Order_Spec;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;

        public OrderService(IBasketRepository basketRepo,IUnitOfWork unitOfWork,IPaymentService paymentService)
        {
            this._basketRepo = basketRepo;
            _unitOfWork = unitOfWork;
            this._paymentService = paymentService;
        }


        #region CreateOrder
        public async Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int DeliveryMethodId, Address ShippingAddress)
        {

            // 1.Get Basket From Basket Repo
            var Basket = await _basketRepo.GetBasketAsync(basketId);


            //2.Get Selected Items at Basket From Product Repo
            var OrderItems = new List<OrderItem>();//productItem=ProductItemOrdered+Price+Quantity
            if (Basket?.Items.Count > 0)
            {
                foreach (var item in Basket.Items)
                {
                    var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                    var ProductItemOrdered = new ProductItemOrdered(product.Id, product.Name, product.PictureUrl);

                    var OrderItem = new OrderItem(ProductItemOrdered, product.Price, item.Quantity);
                    OrderItems.Add(OrderItem);

                }
            }

            //3.Calculate SubTotal //Product of price*quantity
            var SubTotal = OrderItems.Sum(item => item.Price * item.Quantity);


            //4.Get Delivery Method From DeliveryMethod Repo

            var DeliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(DeliveryMethodId);
            //5.Create Order

            var spec = new OrderWithPaymentIntentSpec(Basket?.PaymentIntentId);
            var ExOrder = await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(spec);
            if (ExOrder is not null)
            {
                _unitOfWork.Repository<Order>().Delete(ExOrder);
                await _paymentService.CreateOrUpdatePaymentIntentAsync(basketId);
            }


            var order = new Order(buyerEmail, ShippingAddress, DeliveryMethod, OrderItems, SubTotal,Basket.PaymentIntentId);
            //6.Add Order Locally

            await _unitOfWork.Repository<Order>().AddAsync(order);
            //7.Save Order To Database[ToDo]
            var Result = await _unitOfWork.CompleteAsync();
            if (Result <= 0) return null;
            return order;

        }

        #endregion


        public async Task<Order> GetOrderByIdForSpecificUserAsync(string buyerEmail, int OrderId)
        {
            var OrderSpec=new OrderSpecifications(buyerEmail,OrderId);
            var Order = await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(OrderSpec);
            return Order;
        }



        public async Task<IReadOnlyList<Order>> GetOrdersForSpecificUserAsync(string buyerEmail)
        {
            var OrderSpec=new OrderSpecifications(buyerEmail);
            var Orders = await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(OrderSpec);
           return Orders;
        }
    }
}
