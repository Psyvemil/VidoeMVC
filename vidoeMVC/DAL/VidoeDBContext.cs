using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using vidoeMVC.Models;


namespace vidoeMVC.DAL
{
    public class VidoeDBContext : IdentityDbContext
    {
        public VidoeDBContext(DbContextOptions<VidoeDBContext> options) : base(options)
        {
        }
        public DbSet<Category> Categories { get; set; }
    }
}
