using E_Commerce.Core.Entities.Identity_Module;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace E_Commerce.Apis.Extensions
{
    public static class UserManagerExtension
    {
        public static Task<AppUser?> FindUserWithAddressAsync(this UserManager<AppUser> userManager, ClaimsPrincipal User)
        {
           var email= User.FindFirstValue(ClaimTypes.Email);

            var Users=userManager.Users.Include(u=>u.Address).FirstOrDefaultAsync(u=>u.Email==email);

            return Users;
        }
    }
}
