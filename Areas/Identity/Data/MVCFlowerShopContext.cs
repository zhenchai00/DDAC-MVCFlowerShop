using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MVCFlowerShop.Areas.Identity.Data;
using MVCFlowerShop.Models;

namespace MVCFlowerShop.Data;

public class MVCFlowerShopContext : IdentityDbContext<MVCFlowerShopUser>
{
    public MVCFlowerShopContext(DbContextOptions<MVCFlowerShopContext> options)
        : base(options)
    {
    }

    // create a statement to define the class is use for which table
    public DbSet<FlowerTable> FlowerList { get; set; }
    public DbSet<PurchaseTable> PurchaseList { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}
