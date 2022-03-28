using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreePasses_API.Services
{
    public static class SeedRoles
    {
        public static void Seed(RoleManager<IdentityRole> roleManager)
        {
            if(roleManager.Roles.Any() == false)
            {
                roleManager.CreateAsync(new IdentityRole("Nucleo")).Wait();
                roleManager.CreateAsync(new IdentityRole("Cliente")).Wait();
            }
        }
    }
}
