using AutoMapper;
using E_Commerce.Apis.DTOs;
using E_Commerce.Apis.Errors;
using E_Commerce.Core.Entities.Basket_Module;
using E_Commerce.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Apis.Controllers
{
    
    public class BasketController :ApiBaseController
    {
        private readonly IBasketRepository _basketRepo;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepository basketRepository,IMapper mapper)
        {
         _basketRepo = basketRepository;
            this._mapper = mapper;
        }

        //Get or ReCreate Basket
      
        [HttpGet("{BasketId}")]

        public async Task<ActionResult<CustomerBasket>> GetCustomerBasket(string  BasketId)
        {
          var Basket= await _basketRepo.GetBasketAsync(BasketId);
            return Basket == null ? new CustomerBasket(BasketId) : Ok(Basket);
        }


        //Update or Create Basket

        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateCustomerBasket(CustomerBasketDto basket)
        {
            var MappedBasket=_mapper.Map<CustomerBasketDto,CustomerBasket>(basket);
            var CreatedOrUpdatedBasket= await _basketRepo.UpdateBasketAsync(MappedBasket );
            return CreatedOrUpdatedBasket is null ? BadRequest(new ApiExceptionResponse(400)) :Ok(CreatedOrUpdatedBasket);
        }


        //Delete Basket
        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteCustomerBasket(string BasketId)
        {
        return await  _basketRepo.DeleteBasketAsync(BasketId);

        }





    }
}
