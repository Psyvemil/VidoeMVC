using Microsoft.EntityFrameworkCore;
using vidoeMVC.DAL;

namespace vidoeMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<VidoeDBContext>(optinons => optinons.UseSqlServer(builder.Configuration.GetConnectionString("CodeFirst")));


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.MapControllerRoute("areas", "{area:exists}/{controller=Category}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
