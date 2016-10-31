namespace DeftIndustries.FeedMix.ViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Models;
    using Orchard.ContentManagement;
    using Orchard.Tags.Services;

    public class FeedEditFeedViewModel
    {
        private IFeedService _feedService;

        public FeedEditFeedViewModel()
        {
        }

        public FeedEditFeedViewModel(IFeedService feedService)
        {
            _feedService = feedService;
        }

        public int Id
        {
            get { return Feed.As<FeedPart>().Record.Id; }
            set { Feed.As<FeedPart>().Record.Id = value; }
        }

        [Required]
        public string FeedUrl
        {
            get { return Feed.As<FeedPart>().Record.FeedUrl; }
            set { Feed.As<FeedPart>().Record.FeedUrl = value; }
        }

        [Required]
        public string WebsiteUrl
        {
            get { return Feed.As<FeedPart>().Record.WebsiteUrl; }
            set { Feed.As<FeedPart>().Record.WebsiteUrl = value; }
        }

        public string Author
        {
            get { return Feed.As<FeedPart>().Record.Author; }
            set { Feed.As<FeedPart>().Record.Author = value; }
        }

        public string Title
        {
            get { return Feed.As<FeedPart>().Record.Title; }
            set { Feed.As<FeedPart>().Record.Title = value; }
        }

        public DateTime? LastPostDate
        {
            get { return _feedService.GetLastPostDate(FeedUrl); }
        }

        public IContent Feed { get; set; }
    }
}