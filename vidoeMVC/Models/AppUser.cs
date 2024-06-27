using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace vidoeMVC.Models
{
    public class AppUser : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime? BirthDate { get; set; }
        public virtual ICollection<UserFollow>? Followers { get; set; }
        public virtual ICollection<UserFollow>? Followees { get; set; }
        public ICollection<Video>? Videos { get; set; }
        public ICollection<VideoCast>? VideoCasts { get; set; }

    }
}
