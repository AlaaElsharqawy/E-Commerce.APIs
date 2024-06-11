using E_Commerce.Apis.Errors;
using E_Commerce.Apis.Helpers;
using E_Commerce.Core.Interfaces;
using E_Commerce.Core.Interfaces.Repositories;
using E_Commerce.Core.Interfaces.Services;
using E_Commerce.Repository;
using E_Commerce.Repository.Repositories;
using E_Commerce.Service;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Apis.Extensions
{
    public static class ApplicationServicesExtension
    {


        public static IServiceCollection AddApplicationServices(this IServiceCollection Services)
        {
            Services.AddScoped(typeof(IResponseCacheService), typeof(ResponseCacheService));
            Services.AddScoped(typeof(IPaymentService), typeof(PaymentService));
            Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            Services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));
            Services.AddScoped(typeof(IOrderService), typeof(OrderService));

            //Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));


            Services.AddAutoMapper(typeof(MappingProfiles));

            #region Handling Error
            //ModelState=>[KeyValuePair]
            //Key=>parameter =>catch context for request
            //Value =.Errors
            Services.Configure<ApiBehaviorOptions>(options =>
              {
                  options.InvalidModelStateResponseFactory = (ActionContext) =>
                  {

                      var errors = ActionContext.ModelState.Where(A => A.Value.Errors.Count > 0).SelectMany(A => A.Value.Errors)
                     .Select(E => E.ErrorMessage);


                      var ValidationErrors = new ApiValidationExceptionResponse
                      {

                          Errors = errors

                      };

                      return new BadRequestObjectResult(ValidationErrors);


                  };
              });

            #endregion




            return Services;




        }
    }
}


