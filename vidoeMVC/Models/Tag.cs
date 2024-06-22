namespace vidoeMVC.Models
{
    public class Tag : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<Video>? Videos { get; set; }
    }
}
