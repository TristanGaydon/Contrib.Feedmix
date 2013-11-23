using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement.Records;
using Orchard.Data.Conventions;

namespace DeftIndustries.FeedMix.Models
{
    public class FeedPartRecord : ContentPartRecord
    {
        public virtual string Title { get; set; }
        public virtual string About { get; set; }
        public virtual string URL { get; set; }
        [Aggregate]
        public virtual IList<FeedRecord> Feeds { get; set; }
    }
}