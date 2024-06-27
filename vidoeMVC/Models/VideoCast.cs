namespace vidoeMVC.Models
{

    public class VideoCast
    {
        public string UserId { get; set; }
        public int VideoId { get; set; }
        public Video? Video { get; set; }
        public AppUser? User { get; set; }
    }



}
