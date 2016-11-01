using System.Collections.Generic;
using Orchard.ContentManagement;
using Orchard.Security;
using System.Linq;

namespace DeftIndustries.FeedMix.Models
{
    using Orchard.ContentManagement.Utilities;

    public sealed class FeedMixPart : ContentPart<FeedMixPartRecord>
    {
        private readonly LazyField<IList<FeedPart>> _feeds = new LazyField<IList<FeedPart>>();

        public LazyField<IList<FeedPart>> FeedsField { get { return _feeds; } }

        public string Title
        {
            get { return Record.Title; }
            set { Record.Title = value; }
        }

        public string Description
        {
            get { return Record.Description; }
            set { Record.Description = value; }
        }

        public string Path
        {
            get { return Record.Path; }
            set { Record.Path = value; }
        }

        public IList<FeedPart> Feeds
        {
            get { return _feeds.Value.OrderBy(f => f.DisplayName).ToList(); }
            set { _feeds.Value = value; }
        }
    }
}
