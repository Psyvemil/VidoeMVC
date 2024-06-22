using vidoeMVC.Enums;
using vidoeMVC.Models;

namespace vidoeMVC.ViewModels.Videos
{
    public class VideoUploadVM
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string VideoUrl { get; set; }
        public string ThumbnailUrl { get; set; }
        public string AuthorId { get; set; }
        public AppUser Author { get; set; }
        public VideoStatus Privacy { get; set; }
        public ICollection<Language>? Languages { get; set; }
        public ICollection<Category>? VCategories { get; set; }
        public ICollection<AppUser>? Cast { get; set; }
        public ICollection<Tag>? Tags { get; set; }
    }
}

//duzeltmek
public class VideoUploadVM
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile Video { get; set; }
        public IFormFile Tumbnail { get; set; }  
        public string AuthorId { get; set; }
      
    }