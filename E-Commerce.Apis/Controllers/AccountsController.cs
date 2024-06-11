using AutoMapper;
using E_Commerce.Apis.DTOs;
using E_Commerce.Apis.Errors;
using E_Commerce.Apis.Extensions;
using E_Commerce.Core.Entities.Identity_Module;
using E_Commerce.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Commerce.Apis.Controllers
{
   
    public class AccountsController : ApiBaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenServices _tokenServices;
        private readonly IMapper _mapper;

        public AccountsController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager
            ,ITokenServices tokenServices,IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenServices = tokenServices;
            _mapper = mapper;
        }



        [HttpPost("Register")]

        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
            if(CheckEmailExists(model.Email).Result.Value)
            {
                return BadRequest(new ApiExceptionResponse(400," This Email Already Exists "));
            }



            var User = new AppUser() 
            { 
             DisplayName = model.DisplayName,
             Email = model.Email,
              UserName = model.Email.Split("@")[0],
              PhoneNumber = model.PhoneNumber,
            
            };

          var Result=   await _userManager.CreateAsync(User,model.Password);
            if (!Result.Succeeded) return BadRequest(new ApiExceptionResponse(400));

            return Ok(new UserDto
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                Token = await _tokenServices.CreateTokenAsync(User,_userManager)
            }); ;


        }



        //Login 
        [HttpPost("Login")]

        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null) return Unauthorized(new ApiExceptionResponse(401));
           var Result= await _signInManager.CheckPasswordSignInAsync(user,model.Password,false);
            if (!Result.Succeeded) return Unauthorized(new ApiExceptionResponse(401));
            return Ok(new UserDto
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenServices.CreateTokenAsync(user, _userManager)
            });
        }



        [Authorize]
        [HttpGet("CurrentUser")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
           var email= User.FindFirstValue(ClaimTypes.Email);
            var user=await _userManager.FindByEmailAsync(email);
            var MappedUser = new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenServices.CreateTokenAsync(user,_userManager)

            };
            return Ok(MappedUser);
        }



        [Authorize]
        [HttpGet("Address")]
        public async Task<ActionResult<AddressDto>> GetCurrentUserAddress()
        {
         var user=  await _userManager.FindUserWithAddressAsync(User);
            var MappedAddress = _mapper.Map<Address,AddressDto>(user.Address);
            return Ok(MappedAddress);
           
        }


       [Authorize]
        [HttpPut("Address")]
        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto UpdatedAddress)
        {
            var user =await _userManager.FindUserWithAddressAsync(User);
            var MappedAddress = _mapper.Map<AddressDto, Address>(UpdatedAddress);
            MappedAddress.Id=user.Address.Id;
            user.Address=MappedAddress;
            var Result= await _userManager.UpdateAsync(user);
            if(!Result.Succeeded) return BadRequest(new ApiExceptionResponse(400));
            return Ok(UpdatedAddress);

        }




        [HttpGet("emailExists")]
        public async Task<ActionResult<bool>> CheckEmailExists(string email)
        {
            var user =await _userManager.FindByEmailAsync(email);
            return user is not null?true:false;
        }

    }
}
