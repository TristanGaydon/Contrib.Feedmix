using System.Collections.Generic;
using System.ServiceModel.Syndication;
using DeftIndustries.FeedMix.Models;
using Orchard.ContentManagement;

namespace Orchard.Tags.Services {
    public interface IFeedService : IDependency {

        void CreateFeed(FeedRecord feedRecord);
        void DeleteFeed(int id);
        SyndicationFeed GetFeedMix(string friendlyFeedName);
    }
}