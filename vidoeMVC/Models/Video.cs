﻿using vidoeMVC.Enums;
using System.Collections.Generic;
using Microsoft.Identity.Client;

namespace vidoeMVC.Models
{
    public class Video : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string VideoUrl { get; set; }
        public string TumbnailUrl { get; set; }
        public string AuthorId { get; set; }
        public AppUser? Author { get; set; }
        //public List< VideoStatus>? Privacy { get; set; }  
        //public List<Language>? Languages { get; set; }
        public ICollection<VideoCategory>? VCategories { get; set; }
        public List<AppUser>? Cast { get; set; }
        public ICollection<VideoTag>? Tags { get; set; }
        
    }
}