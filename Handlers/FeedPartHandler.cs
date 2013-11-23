using DeftIndustries.FeedMix.Models;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;

namespace DeftIndustries.FeedMix.Handlers {
    public class FeedPartHandler : ContentHandler {
        public FeedPartHandler(IRepository<FeedPartRecord> repository)
        {
            Filters.Add(new ActivatingFilter<FeedPart>("Feed"));
            Filters.Add(StorageFilter.For(repository));
            Filters.Add(new TemplateFilterForRecord<FeedPartRecord>("Feed", "Parts/GoogleMapsSettings"));
        }
    }
}