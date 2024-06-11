using AutoMapper;
using E_Commerce.Apis.DTOs;
using E_Commerce.Apis.Errors;
using E_Commerce.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Stripe;

namespace E_Commerce.Apis.Controllers
{
  
    public class PaymentsController :ApiBaseController
    {
        private readonly IPaymentService _paymentServices;
        private readonly IMapper _mapper;
        const string endpointSecret = "whsec_ee8375a12cf0b37091e8b4495d51dd43ab049991e3dc95a033ad137b4a53cc3b";
                                              
        public PaymentsController(IPaymentService paymentServices,IMapper mapper)
        {
            this._paymentServices = paymentServices;
            this._mapper = mapper;
        }

        //Create Or Update Payment
        [ProducesResponseType(typeof(CustomerBasketDto),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiExceptionResponse), StatusCodes.Status400BadRequest)]
        [Authorize]
        [HttpPost("{basketId}")]
       
        public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var CustomerBasket=await _paymentServices.CreateOrUpdatePaymentIntentAsync(basketId);
            if (CustomerBasket is null) return BadRequest(new ApiExceptionResponse(400));
            var MappedCustomerBasket = _mapper.Map<CustomerBasketDto>(CustomerBasket);
            return Ok(MappedCustomerBasket);
        }





        [HttpPost("webhook")]
        public async Task<IActionResult> StripeWebHook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json,
               Request.Headers["Stripe-Signature"], endpointSecret);

                var PaymentIntent=stripeEvent.Data.Object as PaymentIntent;
                // Handle the event
                if (stripeEvent.Type == Events.PaymentIntentPaymentFailed)
                {
                  await  _paymentServices.UpdatePaymentIntentToSucceedOrFailedAsync(PaymentIntent.Id,false);
                }
                else if (stripeEvent.Type == Events.PaymentIntentSucceeded)
                {
                    await _paymentServices.UpdatePaymentIntentToSucceedOrFailedAsync(PaymentIntent.Id, true);
                }

                // ... handle other event types
                else
                {
                    Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                }

                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest();
            }

        }


    }
}
