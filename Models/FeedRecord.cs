using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Data.Conventions;

namespace DeftIndustries.FeedMix.Models
{
    public class FeedRecord
    {
        public virtual int Id { get; set; }
        
        public virtual string URL { get; set; }
        public virtual string Title { get; set; }
        public virtual string Author { get; set; }

        public virtual FeedPartRecord FeedPartRecord { get; set; } 
    }
}