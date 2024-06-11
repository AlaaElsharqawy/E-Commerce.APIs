using AutoMapper;
using E_Commerce.Apis.DTOs;
using E_Commerce.Apis.Errors;
using E_Commerce.Core.Entities.Order_Module;
using E_Commerce.Core.Interfaces;
using E_Commerce.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Commerce.Apis.Controllers
{
   
    public class OrdersController : ApiBaseController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public OrdersController(IOrderService orderService,IMapper mapper,IUnitOfWork unitOfWork)
        {
            this._orderService = orderService;
            this._mapper = mapper;
            this._unitOfWork = unitOfWork;
        }


        #region Create Order
        //Create Order
        [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiExceptionResponse), StatusCodes.Status400BadRequest)]
        
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Order?>> CreateOrder(OrderDto orderDto)
        {
            var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var MappedAddress = _mapper.Map<AddressDto, Address>(orderDto.ShippingAddress);

            var CreatedOrder = await _orderService.CreateOrderAsync(BuyerEmail, orderDto.basketId, orderDto.deliveryMethodId, MappedAddress);

            if (CreatedOrder is null) return BadRequest(new ApiExceptionResponse(400, "There is A problem with createdOrder"));
            return CreatedOrder;
        }
        #endregion




        #region Get Orders For User
        [ProducesResponseType(typeof(IReadOnlyList<OrderToReturnDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiExceptionResponse), StatusCodes.Status404NotFound)]
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrdersForUser()
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var Orders = await _orderService.GetOrdersForSpecificUserAsync(buyerEmail);
            if (Orders is null) return NotFound(new ApiExceptionResponse(404, "No Orders For This User"));
            var MappedOrders = _mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDto>>(Orders);
            return Ok(MappedOrders);
        }
        #endregion




        #region Get Order By Id For Specific User Async
        [ProducesResponseType(typeof(OrderToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiExceptionResponse), StatusCodes.Status404NotFound)]
        [Authorize]
        [HttpGet("{id}")]

        public async Task<ActionResult<OrderToReturnDto>> GetOrderByIdForUser(int id)
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var Order = await _orderService.GetOrderByIdForSpecificUserAsync(Email, id);
            if (Order is null) return NotFound(new ApiExceptionResponse(400, "No Order For This User"));
            var MappedOrder = _mapper.Map<Order,OrderToReturnDto>(Order);
            return Ok(MappedOrder);
        }
        #endregion



        #region Get Delivery Methods
        [HttpGet("DeliveryMethods")]
        public async Task<ActionResult<DeliveryMethod>> GetDeliveryMethods()
        {
            var DeliveryMethods = await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
            return Ok(DeliveryMethods);
        } 
        #endregion

    }
}
