using System.Collections.Generic;
using DeftIndustries.FeedMix.Models;

namespace DeftIndustries.FeedMix.ViewModels
{
    public class FeedMixWidgetViewModel
    {
        public int FeedMixId { get; set; }
        public IEnumerable<FeedMixPart> FeedMixes { get; set; }
    }
}