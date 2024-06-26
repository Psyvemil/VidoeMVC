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
        public DbSet<AppUser> Users { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<UserFollow> UserFollows { get; set; }
        public DbSet<VideoCategory> VideoCategories { get; set; }
        public DbSet<VideoTag> VideoTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserFollow>()
                .HasKey(uf => new { uf.FollowerId, uf.FolloweeId });

            modelBuilder.Entity<UserFollow>()
                .HasOne(uf => uf.Follower)
                .WithMany(u => u.Followees)
                .HasForeignKey(uf => uf.FollowerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserFollow>()
                .HasOne(uf => uf.Followee)
                .WithMany(u => u.Followers)
                .HasForeignKey(uf => uf.FolloweeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<VideoCategory>()
                .HasKey(vc => new { vc.VideoId, vc.CategoryId });

            modelBuilder.Entity<VideoCategory>()
                .HasOne(vc => vc.Video)
                .WithMany(v => v.VCategories)
                .HasForeignKey(vc => vc.VideoId);

            modelBuilder.Entity<VideoCategory>()
                .HasOne(vc => vc.Category)
                .WithMany(c => c.VideoCategories)
                .HasForeignKey(vc => vc.CategoryId);

            modelBuilder.Entity<VideoTag>()
                .HasKey(vt => new { vt.VideoId, vt.TagId });

            modelBuilder.Entity<VideoTag>()
                .HasOne(vt => vt.Video)
                .WithMany(v => v.Tags)
                .HasForeignKey(vt => vt.VideoId);

            modelBuilder.Entity<VideoTag>()
                .HasOne(vt => vt.Tag)
                .WithMany(t => t.VideoTags)
                .HasForeignKey(vt => vt.TagId);
            modelBuilder.Entity<AppUser>()
           .HasMany(u => u.Videos)
           .WithOne(v => v.Author) 
           .HasForeignKey(v => v.AuthorId);
        }
    }
}

