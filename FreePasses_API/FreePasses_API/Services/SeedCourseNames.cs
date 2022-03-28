using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreePasses_API.Models
{
    public static class SeedCourseNames
    {
        public async static void Seed(APIContext context)
        {
            if(context.NomesDeNucleos.Any() == false)
            {
                context.NomesDeNucleos.Add(new NomesDeNucleos("1","NEEI"));
                context.NomesDeNucleos.Add(new NomesDeNucleos("2","NEMEC"));
                await context.SaveChangesAsync();
            }
        }
    }
}
