using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Xml;
using DeftIndustries.FeedMix.Models;
using JetBrains.Annotations;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Core.Common.Models;
using Orchard.Data;
using Orchard.Localization;
using Orchard.Logging;
using Orchard.Security;
using Orchard.Tags.Services;
using Orchard.UI.Notify;
using System.Linq;

namespace DeftIndustries.FeedMix.Services
{
    using System.Net;
    using System.Xml.Linq;

    [UsedImplicitly]
    public class FeedService : IFeedService
    {
        private readonly IRepository<FeedRecord> _feedRepository;
        private readonly INotifier _notifier;
        private readonly IOrchardServices _orchardServices;

        public FeedService(IRepository<FeedRecord> feedRepository,
                          INotifier notifier,
                          IOrchardServices orchardServices)
        {
            _feedRepository = feedRepository;
            _notifier = notifier;
            _orchardServices = orchardServices;
            Logger = NullLogger.Instance;
            T = NullLocalizer.Instance;
        }

        public ILogger Logger { get; set; }
        public Localizer T { get; set; }


        public void CreateFeed(FeedRecord feedRecord)
        {
            _feedRepository.Create(feedRecord);
        }

        public void DeleteFeed(int id) {
            var feedRecord = _feedRepository.Get(id);
            _feedRepository.Delete(feedRecord);
        }

        public bool IsFeedValid(string feedURL)
        {
            try
            {
                Uri feedUri = new Uri(feedURL);
                SyndicationFeed syndicationFeed;

                var request = (HttpWebRequest)WebRequest.Create(feedUri);
                request.Accept = "application/rss+xml, application/atom+xml, text/xml */*;q=0.1";
                using (var response = request.GetResponse())
                using (var responseStream = response.GetResponseStream())
                using (var reader = XmlReader.Create(responseStream))
                {
                    syndicationFeed = SyndicationFeed.Load(reader);
                    return true;
                }
            }
            catch(Exception) { }

            return false;
        }

        public SyndicationFeed GetFeedMix(string friendlyFeedName)
        {
            SyndicationFeed mainFeed = new SyndicationFeed();


            FeedPart feedMix = this.GetRssFeeds(friendlyFeedName);

            if(feedMix == null)
                return mainFeed;

            foreach (var feed in feedMix.Feeds)
            {
                try
                {
                    Uri feedUri = new Uri(feed.URL);
                    SyndicationFeed syndicationFeed;

                    var request = (HttpWebRequest)WebRequest.Create(feedUri);
                    request.Accept = "application/rss+xml, application/atom+xml, text/xml */*;q=0.1";
                    using (var response = request.GetResponse())
                    using (var responseStream = response.GetResponseStream())
                    using (var reader = XmlReader.Create(responseStream))
                    {
                        syndicationFeed = SyndicationFeed.Load(reader);
                        syndicationFeed.Id = feed.Title;

                        foreach (var item in syndicationFeed.Items)
                        {
                            const string dc = "http://purl.org/dc/elements/1.1/";
                            
                            var creator = item.ElementExtensions.Where(p => p.OuterName == "creator").FirstOrDefault();

                            if (creator == null && item.Authors.Count == 0)
                            {
                                foreach (SyndicationPerson person in syndicationFeed.Authors)
                                {
                                    item.Authors.Add(person);
                                }
                            }

                            if (creator != null && item.Authors.Count == 0)
                            {
                                item.Authors.Add(new SyndicationPerson(creator.GetObject<string>()));
                            }
                            else if(creator == null && item.Authors.Count == 0)
                            {
                                item.Authors.Add(new SyndicationPerson(feed.Author));
                            }

                            if(string.IsNullOrEmpty(item.Authors.First().Email))
                            {
                                item.Authors.First().Email = item.Authors.First().Name;
                            }
                        }

                        SyndicationFeed tempFeed = new SyndicationFeed(
                            mainFeed.Items.Union(syndicationFeed.Items).OrderByDescending(u => u.PublishDate));

                        mainFeed = tempFeed;
                    }
                }
                catch (Exception)
                {
                    
                }
            }
            mainFeed.Title = new TextSyndicationContent(feedMix.Title);
            return mainFeed;
        }

        private FeedPart GetRssFeeds(string friendlyFeedName) 
        {
           return _orchardServices.ContentManager
                .Query<FeedPart, FeedPartRecord>()
                .Where(f => f.URL == friendlyFeedName)
                .List()
                .FirstOrDefault();
        }
    }
}
