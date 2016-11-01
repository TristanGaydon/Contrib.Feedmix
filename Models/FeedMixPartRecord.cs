using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement.Records;
using Orchard.Data.Conventions;

namespace Contrib.FeedMix.Models
{
    public class FeedMixPartRecord : ContentPartRecord
    {
        public virtual string Title { get; set; }
        public virtual string Description { get; set; }
        public virtual string Path { get; set; }
        [Aggregate]
        public virtual IList<FeedPartRecord> Feeds { get; set; }
    }
}