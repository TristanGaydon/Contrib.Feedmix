using System.Collections.Generic;
using Contrib.FeedMix.Models;

namespace Contrib.FeedMix.ViewModels
{
    public class FeedMixWidgetViewModel
    {
        public int FeedMixId { get; set; }
        public IEnumerable<FeedMixPart> FeedMixes { get; set; }
    }
}