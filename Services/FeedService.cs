using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Xml;
using Contrib.FeedMix.Models;
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

namespace Contrib.FeedMix.Services
{
    using System.Collections.Concurrent;
    using System.Net;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using NHibernate.Hql.Ast.ANTLR.Util;

    public class FeedService : IFeedService
    {
        private readonly IRepository<FeedMixPartRecord> _feedMixRepository;
        private readonly IRepository<FeedPartRecord> _feedRepository;
        private readonly INotifier _notifier;
        private readonly IOrchardServices _orchardServices;

        public FeedService(IRepository<FeedMixPartRecord> feedMixRepository,
                          IRepository<FeedPartRecord> feedRepository,
                          INotifier notifier,
                          IOrchardServices orchardServices) {
            _feedMixRepository = feedMixRepository;
            _feedRepository = feedRepository;
            _notifier = notifier;
            _orchardServices = orchardServices;
            Logger = NullLogger.Instance;
            T = NullLocalizer.Instance;
        }

        public ILogger Logger { get; set; }
        public Localizer T { get; set; }

        public void GetAllFeedMixes() {
            
        }

        public void CreateFeed(FeedPartRecord feedPartRecord)
        {
            _feedRepository.Create(feedPartRecord);
        }

        public IContentQuery<FeedPart, FeedPartRecord> GetFeedsForFeedMix(int feedMixId)
        {
            return _orchardServices.ContentManager
                      .Query<FeedPart, FeedPartRecord>()
                      .Where(fpr => fpr.FeedMixPartRecord.Id == feedMixId);
        }

        public FeedPartRecord GetFeed(int feedRecordId)
        {
            return _feedRepository.Get(feedRecordId);
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

        public DateTime? GetLastPostDate(string url)
        {
            try
            {
                Uri feedUri = new Uri(url);
                SyndicationFeed syndicationFeed;

                var request = (HttpWebRequest) WebRequest.Create(feedUri);
                request.Accept = "application/rss+xml, application/atom+xml, text/xml */*;q=0.1";
                using (var response = request.GetResponse())
                using (var responseStream = response.GetResponseStream())
                using (var reader = XmlReader.Create(responseStream))
                {
                    syndicationFeed = SyndicationFeed.Load(reader);
                    var posts = syndicationFeed.Items.OrderByDescending(p => p.PublishDate);
                    var lastPost = posts.FirstOrDefault();

                    if(lastPost != null)
                        return lastPost.PublishDate.LocalDateTime;
                }
            }
            catch (Exception)
            {
                return null;
            }

            return null;
        }

        public SyndicationFeed GetFeedMix(string friendlyFeedName)
        {
            SyndicationFeed mainFeed = new SyndicationFeed();
            FeedMixPart feedMixMix = this.GetRssFeeds(friendlyFeedName);

            if(feedMixMix == null)
                return mainFeed;

            var feeds = new ConcurrentBag<IEnumerable<SyndicationItem>>();
            Parallel.ForEach(feedMixMix.Feeds, feed =>
            {
                try
                {
                    Uri feedUri = new Uri(feed.FeedUrl);
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

                            if (creator == null)
                            {
                                item.ElementExtensions.Add(new SyndicationElementExtension("creator", "http://purl.org/dc/elements/1.1/", feed.Author));
                            }

                            if (creator != null && item.Authors.Count == 0)
                            {
                                item.Authors.Add(new SyndicationPerson(creator.GetObject<string>()));
                            }
                            else if (creator == null && item.Authors.Count == 0)
                            {
                                item.Authors.Add(new SyndicationPerson(feed.Author));
                            }

                            if (string.IsNullOrEmpty(item.Authors.First().Email))
                            {
                                item.Authors.First().Email = item.Authors.First().Name;
                            }
                        }

                        feeds.Add(syndicationFeed.Items);
                    }
                }
                catch (Exception)
                {

                }
            });

            var feedItems = new List<SyndicationItem>();
            feeds.ToList().ForEach(f => feedItems.AddRange(f));
            feedItems = feedItems.OrderByDescending(u => u.PublishDate).Take(100).ToList();

            mainFeed = new SyndicationFeed(feedItems);
            mainFeed.Title = new TextSyndicationContent(feedMixMix.Title);
            return mainFeed;
        }

        private FeedMixPart GetRssFeeds(string friendlyFeedName) 
        {
           return _orchardServices.ContentManager
                .Query<FeedMixPart, FeedMixPartRecord>()
                .Where(f => f.Path == friendlyFeedName)
                .List()
                .FirstOrDefault();
        }
    }
}
