namespace vidoeMVC.Models
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }    
        public ICollection<Video>? Videos { get; set; }
    }
}
