using vidoeMVC.Models;



    public class VideoReport
    {
        public int Id { get; set; }
        public int VideoId { get; set; }
        public string UserId { get; set; }
        public string Reason { get; set; }
        public DateTime ReportedAt { get; set; } = DateTime.UtcNow;

        public Video Video { get; set; }
        public AppUser User { get; set; }
    }

