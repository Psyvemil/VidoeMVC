namespace vidoeMVC.Models
{
    public class VideoLike
    {
        public int VideoId { get; set; }
        public int LikeDislikeId { get; set; }
        public string UserId { get; set; }

        public AppUser? User { get; set; }
        public Video? Video { get; set; }
        public LikeDislike? LikeDislike { get; set; }
    }
}
