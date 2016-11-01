namespace DeftIndustries.FeedMix.Handlers
{
    using Models;
    using Orchard.ContentManagement.Handlers;
    using Orchard.Data;

    public class FeedMixWidgetPartHandler : ContentHandler
    {
        public FeedMixWidgetPartHandler(IRepository<FeedMixWidgetPartRecord> repository)
        {
            Filters.Add(StorageFilter.For(repository));   
        }
    }
}