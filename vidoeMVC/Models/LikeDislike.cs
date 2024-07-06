namespace vidoeMVC.Models
{
    public class LikeDislike : BaseEntity
    {
        public bool IsLike { get; set; } // True for like, false for dislike
        public ICollection<VideoLike> VideoLikes { get; set; }
    }
}
