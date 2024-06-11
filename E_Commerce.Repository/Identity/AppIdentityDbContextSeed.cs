using E_Commerce.Core.Entities.Identity_Module;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Repository.Identity
{
   public static class AppIdentityDbContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser>  userManager)
        {
            if(!userManager.Users.Any()) 
            { 
            var User = new AppUser
            {  
                DisplayName="Alaa Ali",
                Email="alaaali6101999@gmail.com",
                UserName= "alaaali6101999",
                PhoneNumber="01024108345"
 
            };
            await userManager.CreateAsync(User,"P@ssw0rd");
            }

        }


    }
}
