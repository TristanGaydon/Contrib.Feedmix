using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DeftIndustries.FeedMix.Models;
using Orchard.ContentManagement;

namespace DeftIndustries.FeedMix.ViewModels {
    public class FeedEditViewModel  {
        [Required]
        public string Title {
            get { return Feed.As<FeedPart>().Record.Title; }
            set { Feed.As<FeedPart>().Record.Title = value; }
        }

        public IEnumerable<FeedRecord> Feeds {
            get { return Feed.As<FeedPart>().Feeds; }
        }

        public IContent Feed { get; set; }
    }
}