using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using vidoeMVC.Models;


namespace vidoeMVC.DAL
{
    public class VidoeDBContext : IdentityDbContext
    {
        public VidoeDBContext(DbContextOptions<VidoeDBContext> options) : base(options)
        {
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<UserFollow> UserFollows { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<Tag> Tags { get; set; }
        

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserFollow>()
                .HasKey(uf => new { uf.FollowerId, uf.FolloweeId });

            builder.Entity<UserFollow>()
                .HasOne(uf => uf.Follower)
                .WithMany(u => u.Followees)
                .HasForeignKey(uf => uf.FollowerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<UserFollow>()
                .HasOne(uf => uf.Followee)
                .WithMany(u => u.Followers)
                .HasForeignKey(uf => uf.FolloweeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Video>()
          .HasOne(v => v.Author)
          .WithMany(a => a.Videos);
          

        }
    }
}

