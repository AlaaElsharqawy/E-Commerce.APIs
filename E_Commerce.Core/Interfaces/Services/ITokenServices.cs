using E_Commerce.Core.Entities.Identity_Module;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Interfaces.Services
{
   public interface ITokenServices
    {
         Task<string> CreateTokenAsync(AppUser user,UserManager<AppUser> userManager);
    }
}
