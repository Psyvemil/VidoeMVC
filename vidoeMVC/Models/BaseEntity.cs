namespace vidoeMVC.Models
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatedTime { get; set; }

        public DateTime UpdatedTime { get; set; }

        //public string CreatedBy { get; set; }

        //public string UpdatedBy { get; set;}

        public bool IsDeleted { get; set; }=false;

    }
}
