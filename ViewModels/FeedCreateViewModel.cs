using System.ComponentModel.DataAnnotations;

namespace Contrib.FeedMix.ViewModels {
    public class FeedCreateViewModel  {
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public string Path { get; set; }
    }
}