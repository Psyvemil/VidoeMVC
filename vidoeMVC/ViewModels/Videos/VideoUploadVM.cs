using vidoeMVC.Enums;
using vidoeMVC.Models;

namespace vidoeMVC.ViewModels.Videos
{
    public class VideoUploadVM
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile Video { get; set; }
        public IFormFile Tumbnail { get; set; }    
        public string AuthorId { get; set; }       
        //public List<VideoStatus> Privacy { get; set; }
        //public List<Language> Language { get; set; }
        public int[] CategoriesIDs { get; set; }
        public int[] TagsIDs { get; set; }
        public string[] CastIDs { get; set; }
        public DateTime DateTime { get; set; }
    }
}



