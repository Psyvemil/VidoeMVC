using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CloudinaryDotNet;
using vidoeMVC.DAL;
using vidoeMVC.Models;
using System.Threading.Tasks;
using vidoeMVC.Services;

namespace vidoeMVC
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

            // Configure DbContext
            builder.Services.AddDbContext<VidoeDBContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("CodeFirst")));

            // Configure Identity
            builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
            {
                opt.User.RequireUniqueEmail = true;
                opt.Password.RequiredLength = 6;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireDigit = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
            })
            .AddEntityFrameworkStores<VidoeDBContext>()
            .AddDefaultTokenProviders();

            // Add Cloudinary configuration
            var cloudinaryConfig = builder.Configuration.GetSection("Cloudinary");
            var cloudinaryAccount = new Account(
                cloudinaryConfig["CloudName"],
                cloudinaryConfig["ApiKey"],
                cloudinaryConfig["ApiSecret"]
            );

            builder.Services.AddSingleton(new Cloudinary(cloudinaryAccount));
            builder.Services.AddTransient<CloudinaryService>();

            var app = builder.Build();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute("areas", "{area:exists}/{controller=Category}/{action=Index}/{id?}");
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
