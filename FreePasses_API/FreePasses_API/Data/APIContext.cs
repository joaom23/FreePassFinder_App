using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FreePasses_API;
using FreePasses_API.Models;

public class APIContext : IdentityDbContext
{
    public APIContext(DbContextOptions<APIContext> options)
        : base(options)
    {
    }

    public DbSet<Nucleo> Nucleos { get; set; }
    public DbSet<FreePass> FreePasses { get; set; }
    public DbSet<NomesDeNucleos> NomesDeNucleos { get; set; }
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<ClientBuyFreePasse> ClientBuyFreePasses { get; set; }

}
