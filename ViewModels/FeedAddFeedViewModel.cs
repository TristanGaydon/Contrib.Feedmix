namespace DeftIndustries.FeedMix.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    public class FeedAddFeedViewModel
    {
        [Required]
        public string FeedUrl { get; set; }

        [Required]
        public string WebsiteUrl { get; set; }

        [Required]
        public string Author { get; set; }

        [Required]
        public string Title { get; set; }    
    }
}