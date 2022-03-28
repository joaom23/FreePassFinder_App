using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreePasses_API.Models
{
    public static class SeedFreePasses
    {
        public async static void Seed(APIContext context)
        {
            if(context.FreePasses.Any() == false)
            {
                context.FreePasses.Add(new FreePass {IdFP = "1" ,  DiscoName = "BClub", CourseName = "NEEI", TotalNumber = 50}) ;
                context.FreePasses.Add(new FreePass {IdFP = "2", DiscoName = "Duplex", CourseName = "NEEI", TotalNumber = 50 });
                context.FreePasses.Add(new FreePass {IdFP = "3", DiscoName = "BClub", CourseName = "NEMEC", TotalNumber = 50 });
                context.FreePasses.Add(new FreePass {IdFP = "4", DiscoName = "Duplex", CourseName = "NEMEC", TotalNumber = 50 });
                await context.SaveChangesAsync();
            }
        }
    }
}
