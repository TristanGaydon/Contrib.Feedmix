namespace Contrib.FeedMix.Models
{
    using System;
    using Orchard.ContentManagement;
    using Orchard.Tags.Services;

    public class FeedPart : ContentPart<FeedPartRecord>
    {
        private IFeedService _feedService;
        
        public string FeedUrl
        {
            get { return Record.FeedUrl; }
            set { Record.FeedUrl = value; }
        }

        public string WebsiteUrl
        {
            get { return Record.WebsiteUrl; }
            set { Record.WebsiteUrl = value; }
        }

        public string Title
        {
            get { return Record.Title; }
            set { Record.Title = value; }
        }

        public string Author
        {
            get { return Record.Author; }
            set { Record.Author = value; }
        }

        public virtual FeedMixPartRecord FeedMixPartRecord
        {
            get { return Record.FeedMixPartRecord; }
            set { Record.FeedMixPartRecord = value; }
        }

        public string DisplayName
        {
            get
            {
                if (!string.IsNullOrEmpty(Record.Author))
                    return Record.Author;

                return Record.Title;
            }
        }
    }
}