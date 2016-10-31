using System.Collections.Generic;
using DeftIndustries.FeedMix.Models;

namespace DeftIndustries.FeedMix.ViewModels {

    public class FeedsIndexViewModel
    {
        public IList<FeedEntry> Feeds { get; set; }
        public dynamic Pager { get; set; }

        public FeedsIndexViewModel() {
            Feeds = new List<FeedEntry>();
        }
    }

    public class FeedEntry
    {
        public FeedMixPart FeedMix { get; set; }
        public bool IsChecked { get; set; }
    }
}
