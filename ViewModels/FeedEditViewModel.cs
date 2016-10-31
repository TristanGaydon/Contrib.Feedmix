namespace DeftIndustries.FeedMix.ViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Models;
    using Orchard.ContentManagement;
    using Orchard.Tags.Services;

    public class FeedEditViewModel
    {
        private IFeedService _feedService;

        public FeedEditViewModel()
        {
            
        }

        public FeedEditViewModel(IFeedService feedService)
        {
            _feedService = feedService;
        }

        [Required]
        public int Id
        {
            get { return Feed.As<FeedMixPart>().Record.Id; }
        }

        [Required]
        public string Title
        {
            get { return Feed.As<FeedMixPart>().Record.Title; }
            set { Feed.As<FeedMixPart>().Record.Title = value; }
        }

        public string Description
        {
            get { return Feed.As<FeedMixPart>().Record.Description; }
            set { Feed.As<FeedMixPart>().Record.Description = value; }
        }

        [Required]
        public string Path
        {
            get { return Feed.As<FeedMixPart>().Record.Path; }
            set { Feed.As<FeedMixPart>().Record.Path = value; }
        }

        public FeedEditOptions Options { get; set; }

        public IEnumerable<FeedEditFeedViewModel> Feeds
        {
            get
            {
                var feeds = new List<FeedEditFeedViewModel>();
                foreach (var feedPart in Feed.As<FeedMixPart>().Feeds)
                {
                    feeds.Add(new FeedEditFeedViewModel(_feedService) {Feed = feedPart });
                }

                switch (Options.Order)
                {
                    case FeedOrder.Author:
                        feeds = feeds.OrderBy(f => f.Author).ToList();
                        break;
                    case FeedOrder.LastPost:
                        feeds = feeds.OrderBy(f => f.LastPostDate).ToList();
                        break;
                }

                return feeds;
            }
        }

        public IContent Feed { get; set; }
    }

    public class FeedEditOptions
    {
        public FeedOrder Order { get; set; }
    }

    public enum FeedOrder
    {
        LastPost,
        Author
    }
}