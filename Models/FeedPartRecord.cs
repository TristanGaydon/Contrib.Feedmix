namespace DeftIndustries.FeedMix.Models
{
    using Orchard.ContentManagement.Records;

    public class FeedPartRecord : ContentPartRecord
    {
        public virtual string FeedUrl { get; set; }
        public virtual string WebsiteUrl { get; set; }
        public virtual string Title { get; set; }
        public virtual string Author { get; set; }

        public virtual FeedMixPartRecord FeedMixPartRecord { get; set; } 
    }
}