using vidoeMVC.Models;

public class Tag : BaseEntity
{
    public string Name { get; set; }
    public ICollection<VideoTag> VideoTags { get; set; }
}
