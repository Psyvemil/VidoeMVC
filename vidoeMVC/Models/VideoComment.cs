namespace vidoeMVC.Models
{
    public class VideoComment
    {
        public int VideoId { get; set; }
        public int CommentId { get; set; }

        public string UserId { get; set; }

        public AppUser? User { get; set; }
        public Video? Video { get; set; }
        public Comment? Comment { get; set; }
    }
}
