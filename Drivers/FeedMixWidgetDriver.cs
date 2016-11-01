using System.Diagnostics;
using System.Linq;
using Contrib.FeedMix.ViewModels;
using Orchard.ContentManagement;
using Orchard.Tags.Services;

namespace Contrib.FeedMix.Drivers
{
    using Models;
    using Orchard.ContentManagement.Drivers;

    public class FeedMixWidgetDriver : ContentPartDriver<FeedMixWidgetPart>
    {
        private readonly IFeedService _feedService;
        private readonly IContentManager _contentManager;

        public FeedMixWidgetDriver(IContentManager contentManager, IFeedService feedService)
        {
            _contentManager = contentManager;
            _feedService = feedService;
        }

        protected override DriverResult Display(FeedMixWidgetPart part, string displayType, dynamic shapeHelper)
        {
            return ContentShape("Parts_FeedMixWidget",
                              () => {
                                  var feedMix = _contentManager.Get<FeedMixPart>(part.FeedMixPartRecordId);

                                  if (feedMix == null)
                                      return null;

                                  return shapeHelper.Parts_FeedMixWidget(FeedMix: feedMix);
                              });
        }

        protected override DriverResult Editor(FeedMixWidgetPart part, dynamic shapeHelper) {

            var viewModel = new FeedMixWidgetViewModel {
                FeedMixId = part.FeedMixPartRecordId,
                FeedMixes = _contentManager.Query<FeedMixPart, FeedMixPartRecord>().List().ToList()
            };

            return ContentShape("Parts_FeedMixWidget_Edit",
                () => shapeHelper.EditorTemplate(
                    TemplateName: "Parts/FeedMixWidget",
                    Model: viewModel,
                    Prefix: Prefix));
        }

        protected override DriverResult Editor(FeedMixWidgetPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            var viewModel = new FeedMixWidgetViewModel();
            if (updater.TryUpdateModel(viewModel, Prefix, null, null))
            {
                part.FeedMixPartRecordId = viewModel.FeedMixId;
            }

            return Editor(part, shapeHelper);
        }
    }
}
