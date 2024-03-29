using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Web.Mvc;
using System.Xml;
using Orchard;
using Orchard.DisplayManagement;
using Orchard.Localization;
using Orchard.Tags.Services;

namespace Contrib.FeedMix.Controllers
{
    [ValidateInput(false)]
    public class FeedController : Controller
    {
        private readonly IFeedService _feedService;

        public FeedController(
            IOrchardServices services,
            IShapeFactory shapeFactory,
            IFeedService feedService)
        {
            Services = services;
            _feedService = feedService;
            T = NullLocalizer.Instance;
            Shape = shapeFactory;
        }

        dynamic Shape { get; set; }
        public IOrchardServices Services { get; set; }
        public Localizer T { get; set; }

        public RssActionResult Index(string id) {
            // Debugger.Launch();
            var mainFeed = _feedService.GetFeedMix(id);
            return new RssActionResult() { Feed = mainFeed };
        }
    }
}
