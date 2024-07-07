using vidoeMVC.Enums;
using System.Collections.Generic;
using Microsoft.Identity.Client;

namespace vidoeMVC.Models
{
    public class Video : BaseEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string VideoUrl { get; set; }
        public string TumbnailUrl { get; set; }
        public string AuthorId { get; set; }
        public string Duration { get; set; } = "0:00";
        public int ViewCount { get; set; }
        
        public AppUser? Author { get; set; }
        public List<VideoStatus>? Privacy { get; set; }
        public List<Language>? Languages { get; set; }
        public ICollection<VideoLike>? Like { get; set; }
        public ICollection<VideoComment>? Comments { get; set; }
        public ICollection<VideoCategory>? VCategories { get; set; }
        public ICollection<VideoTag>? Tags { get; set; }
        public ICollection<VideoCast>? Casts { get; set; }
        public ICollection<VideoReport>? VideoReports { get; set; }

    }
}
