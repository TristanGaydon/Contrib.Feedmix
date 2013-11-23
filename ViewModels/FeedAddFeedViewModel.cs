using System.ComponentModel.DataAnnotations;

namespace DeftIndustries.FeedMix.ViewModels {
    public class FeedAddFeedViewModel  {
        [Required]
        public string URL { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        
    }
}