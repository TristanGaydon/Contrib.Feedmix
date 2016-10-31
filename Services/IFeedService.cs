namespace Orchard.Tags.Services
{
    using System.ServiceModel.Syndication;
    using DeftIndustries.FeedMix.Models;
    using ContentManagement;
    using System;

    public interface IFeedService : IDependency
    {

        void CreateFeed(FeedPartRecord feedPartRecord);
        FeedPartRecord GetFeed(int feedRecordId);
        void DeleteFeed(int id);
        SyndicationFeed GetFeedMix(string friendlyFeedName);
        IContentQuery<FeedPart, FeedPartRecord> GetFeedsForFeedMix(int feedMixId);
        DateTime? GetLastPostDate(string url);
    }
}