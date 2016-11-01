namespace Contrib.FeedMix.Models
{
    using Orchard.ContentManagement;

    public class FeedMixWidgetPart : ContentPart<FeedMixWidgetPartRecord>
    {
        public int FeedMixPartRecordId
        {
            get { return Record.FeedMixPartRecord_Id; }
            set { Record.FeedMixPartRecord_Id = value; }
        }
    }
}