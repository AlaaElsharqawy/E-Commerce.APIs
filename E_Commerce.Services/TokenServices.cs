using E_Commerce.Core.Entities.Identity_Module;
using E_Commerce.Core.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Service
{
    public class TokenServices : ITokenServices
    {
        private readonly IConfiguration _configuration;

        public TokenServices(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public async Task<string> CreateTokenAsync(AppUser user,UserManager<AppUser> userManager)
        {
            //token =header(algorithm,TokenType)+payload(private Claims,Register Claims)+Key


            //private Claims(user (Define))
            var privateClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.GivenName,user.DisplayName),
                new Claim(ClaimTypes.Email,user.Email)
            };

            var RolesUser= await userManager.GetRolesAsync(user);

            foreach (var Role in RolesUser)
            {
                privateClaims.Add(new Claim (ClaimTypes.Role,Role));
            }

            //Key
            var TokenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));


            var ObjectToken = new JwtSecurityToken
                (
                
                
                 //Register Claims
                 issuer: _configuration["Jwt:issuerValidation"],
                 audience: _configuration["Jwt:audienceValidation"],
                 expires:DateTime.Now.AddDays(double.Parse( _configuration["Jwt:expiresValidation"])),
                 //Private Claims
                 claims: privateClaims,
                 signingCredentials: new SigningCredentials(TokenKey, SecurityAlgorithms.HmacSha256Signature)
                  );

             return new JwtSecurityTokenHandler().WriteToken(ObjectToken);






        }
    }
}
