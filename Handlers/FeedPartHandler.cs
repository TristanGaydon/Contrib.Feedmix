namespace DeftIndustries.FeedMix.Handlers
{
    using Models;
    using Orchard.ContentManagement.Handlers;
    using Orchard.Data;

    public class FeedPartHandler : ContentHandler
    {
        public FeedPartHandler(IRepository<FeedPartRecord> repository)
        {
            Filters.Add(new ActivatingFilter<FeedPart>("Feed"));
            Filters.Add(StorageFilter.For(repository));
            Filters.Add(new TemplateFilterForRecord<FeedPartRecord>("Feed", "Parts/GoogleMapsSettings"));
        }
    }
}