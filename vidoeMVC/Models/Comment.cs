namespace vidoeMVC.Models
{
    public class Comment :BaseEntity
    {       
        public string Text { get; set; }    
       
        public ICollection<VideoComment>? Comments { get; set; }
        
    }
}
