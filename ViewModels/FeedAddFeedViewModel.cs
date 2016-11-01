namespace Contrib.FeedMix.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    public class FeedAddFeedViewModel
    {
        [Required]
        public string FeedUrl { get; set; }

        [Required]
        public string WebsiteUrl { get; set; }

        public string Author { get; set; }

        public string Title { get; set; }    
    }
}