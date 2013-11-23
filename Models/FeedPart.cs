using System.Collections.Generic;
using Orchard.ContentManagement;
using Orchard.Security;
using System.Linq;

namespace DeftIndustries.FeedMix.Models {
    public sealed class FeedPart : ContentPart<FeedPartRecord> {
        public string Title
        {
            get { return Record.Title; }
            set { Record.Title = value; }
        }

        public string Description
        {
            get { return Record.About; }
            set { Record.About = value; }
        }

        public string Path
        {
            get { return Record.URL; }
            set { Record.URL = value; }
        }

        public IEnumerable<FeedRecord> Feeds { get { return Record.Feeds.OrderBy(f => f.Author); } }
    }
}
