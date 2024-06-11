using AutoMapper;
using E_Commerce.Apis.DTOs;
using E_Commerce.Core.Entities.Order_Module;

namespace E_Commerce.Apis.Helpers
{
    public class OrderPictureUrlResolver : IValueResolver<OrderItem, OrderItemDto, string>
    {
        private readonly IConfiguration _configuration;

        public OrderPictureUrlResolver(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.Product.PictureUrl))
            {
                return $"{_configuration["ApiBaseUrl"]}{source.Product.PictureUrl}";
            }

            return string.Empty;
        }
    }
}


