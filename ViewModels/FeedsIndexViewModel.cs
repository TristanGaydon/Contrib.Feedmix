using System.Collections.Generic;
using DeftIndustries.FeedMix.Models;

namespace DeftIndustries.FeedMix.ViewModels {

    public class FeedsIndexViewModel  {
        public IList<FeedEntry> Feeds { get; set; }
        public UserIndexOptions Options { get; set; }
        public dynamic Pager { get; set; }

        public FeedsIndexViewModel() {
            Feeds = new List<FeedEntry>();
        }
    }

    public class FeedEntry {
        public FeedPartRecord Feed { get; set; }
        public bool IsChecked { get; set; }
    }

    public class UserIndexOptions {
        public string Search { get; set; }
        public UsersOrder Order { get; set; }
        public UsersFilter Filter { get; set; }
        public UsersBulkAction BulkAction { get; set; }
    }

    public enum UsersOrder {
        Name,
        Email
    }

    public enum UsersFilter {
        All,
        Approved,
        Pending,
        EmailPending
    }

    public enum UsersBulkAction {
        None,
        Delete,
        Disable,
        Approve,
        ChallengeEmail
    }
}
