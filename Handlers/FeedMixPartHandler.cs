namespace DeftIndustries.FeedMix.Handlers
{
    using System.Collections.Generic;
    using System.Linq;
    using Models;
    using Orchard.ContentManagement.Handlers;
    using Orchard.Data;
    using Orchard.Tags.Services;

    public class FeedMixPartHandler : ContentHandler
    {
        public FeedMixPartHandler(IRepository<FeedMixPartRecord> repository, IFeedService feedService)
        {
            OnInitializing<FeedMixPart>((ctx, part) => {
                part.Feeds = new List<FeedPart>();
            });

            OnLoading<FeedMixPart>((context, feedMix) =>
            {
                feedMix.FeedsField.Loader(() =>
                    feedService.GetFeedsForFeedMix(feedMix.Id).List().ToList());
            });

            Filters.Add(new ActivatingFilter<FeedMixPart>("FeedMix"));
            Filters.Add(StorageFilter.For(repository));
            Filters.Add(new TemplateFilterForRecord<FeedMixPartRecord>("FeedMix", "Parts/GoogleMapsSettings"));
        }
    }
}