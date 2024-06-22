using vidoeMVC.Enums;
using System.Collections.Generic;

namespace vidoeMVC.Models
{
    public class Video : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string VideoUrl { get; set; }
        public string TumbnailUrl { get; set; }
        public string AuthorId { get; set; }
        public AppUser Author { get; set; }
        public VideoStatus Privacy { get; set; }  
        public ICollection<Language>? Languages { get; set; }
        public ICollection<Category>? VCategories { get; set; }
        public ICollection<AppUser>? Cast { get; set; }
        public ICollection<Tag>? Tags { get; set; }
    }
}
