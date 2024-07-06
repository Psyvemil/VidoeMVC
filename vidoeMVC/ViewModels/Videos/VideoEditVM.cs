using vidoeMVC.Enums;

namespace vidoeMVC.ViewModels.Videos
{
    public class VideoEditVM
    {

        public int id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
       
        public string TumbnailUrl { get; set; }

        public VideoStatus Privacy { get; set; }
        public List<Language> Language { get; set; }
        public int[] CategoriesIDs { get; set; }
        public string Tags { get; set; }
        public string[] CastIDs { get; set; }
        public DateTime DateTime { get; set; }
    }
}
