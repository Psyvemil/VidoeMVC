using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CloudinaryDotNet;
using vidoeMVC.DAL;
using vidoeMVC.Models;
using vidoeMVC.Services;
using vidoeMVC.Enums;

namespace vidoeMVC
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            ConfigureServices(builder.Services, builder.Configuration);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            Configure(app);

            // Ensure roles are created
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                await EnsureRolesCreated(roleManager);
            }

            app.Run();
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllersWithViews();

            // Configure DbContext
            services.AddDbContext<VidoeDBContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("CodeFirst")));

            // Configure Identity
            services.AddIdentity<AppUser, IdentityRole>(opt =>
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
            var cloudinaryConfig = configuration.GetSection("Cloudinary");
            var cloudinaryAccount = new Account(
                cloudinaryConfig["CloudName"],
                cloudinaryConfig["ApiKey"],
                cloudinaryConfig["ApiSecret"]
            );
            services.AddSingleton(new Cloudinary(cloudinaryAccount));
            services.AddTransient<CloudinaryService>();
        }

        private static async Task EnsureRolesCreated(RoleManager<IdentityRole> roleManager)
        {
            foreach (UserRole role in Enum.GetValues(typeof(UserRole)))
            {
                if (!await roleManager.RoleExistsAsync(role.ToString()))
                {
                    await roleManager.CreateAsync(new IdentityRole { Name = role.ToString() });
                }
            }
        }

        private static void Configure(WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Category}/{action=Index}/{id?}");
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        }
    }
}
