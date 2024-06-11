using AutoMapper;
using E_Commerce.Apis.DTOs;
using E_Commerce.Core.Entities.Basket_Module;
using E_Commerce.Core.Entities.Identity_Module;
using E_Commerce.Core.Entities.Order_Module;
using E_Commerce.Core.Entities.Product_Module;

namespace E_Commerce.Apis.Helpers
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product,ProductToReturnDto>().ForMember(d=>d.ProductType,o=>o.MapFrom(s=>s.ProductType.Name)).
           ForMember(d => d.ProductBrand, o => o.MapFrom(s => s.ProductBrand.Name))
           .ForMember(d=>d.PictureUrl,o=>o.MapFrom<ProductPictureUrlResolver>());


            CreateMap<Core.Entities.Identity_Module.Address, AddressDto>().ReverseMap();
            CreateMap<Core.Entities.Order_Module.Address, AddressDto>().ReverseMap();
            CreateMap<CustomerBasketDto, CustomerBasket>().ReverseMap();
            CreateMap<BasketItemDto, BasketItem>().ReverseMap();

            CreateMap<Order, OrderToReturnDto>().
                ForMember(d => d.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod.ShortName)).
                 ForMember(d => d.DeliveryMethodCost, o => o.MapFrom(s => s.DeliveryMethod.Cost));
            CreateMap<OrderItem, OrderItemDto>().
                ForMember(d => d.ProductId, o => o.MapFrom(s => s.Product.ProductId)).
                ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product.ProductName)).
                  ForMember(d => d.PictureUrl, o => o.MapFrom(s => s.Product.PictureUrl)).
                  ForMember(d => d.PictureUrl, o => o.MapFrom<OrderPictureUrlResolver>());


        }
    }
}
