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
                .WithMany(u => u.Videos)
                .HasForeignKey(v => v.AuthorId)
                .IsRequired();
            builder.Entity<VideoTag>()
              .HasKey(vt => new { vt.VideoId, vt.TagId });

            builder.Entity<VideoCategory>()
                .HasKey(vc => new { vc.VideoId, vc.CategoryId });

            builder.Entity<Tag>()
                .HasMany(t => t.VideoTags)
                .WithOne(vt => vt.Tag)
                .HasForeignKey(vt => vt.TagId);

            builder.Entity<Video>()
                .HasMany(v => v.Tags)
                .WithOne(vt => vt.Video)
                .HasForeignKey(vt => vt.VideoId);

            builder.Entity<Category>()
                .HasMany(c => c.VideoCategories)
                .WithOne(vc => vc.Category)
                .HasForeignKey(vc => vc.CategoryId);

            builder.Entity<Video>()
                .HasMany(v => v.VCategories)
                .WithOne(vc => vc.Video)
                .HasForeignKey(vc => vc.VideoId);

        }
    }
}

