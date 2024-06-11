using E_Commerce.Core.Entities.Identity_Module;
using E_Commerce.Core.Interfaces.Services;
using E_Commerce.Repository.Identity;
using E_Commerce.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace E_Commerce.Apis.Extensions
{
    public static class IdentityServicesExtension
    {

        public static IServiceCollection AddIdentityServices(this IServiceCollection Services,IConfiguration configuration)
        {
        
           Services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<AppIdentityDbContext>();


            Services.AddScoped<ITokenServices,TokenServices>();

            Services.AddAuthentication(op =>
            {
                op.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                op.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;


            }).AddJwtBearer(options =>
             options.TokenValidationParameters = new TokenValidationParameters()
             {
                  ValidateIssuer = true,
                  ValidIssuer= configuration["Jwt:issuerValidation"],
                  ValidateAudience = true,
                  ValidAudience= configuration["Jwt:audienceValidation"],
                  ValidateLifetime = true,
                  ValidateIssuerSigningKey = true,
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),


        }
        ) ;



            return Services;
        }
    }
}
