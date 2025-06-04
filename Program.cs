using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MVCFlowerShop.Areas.Identity.Data;
using MVCFlowerShop.Data;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("MVCFlowerShopContextConnection") ?? throw new InvalidOperationException("Connection string 'MVCFlowerShopContextConnection' not found.");;

builder.Services.AddDbContext<MVCFlowerShopContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<MVCFlowerShopUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<MVCFlowerShopContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages();

app.Run();
