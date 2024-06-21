namespace vidoeMVC.Models
{
    public class Video : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public ICollection<Category> Vcategories { get; set; }
        public AppUser User { get; set; }
    }
}
