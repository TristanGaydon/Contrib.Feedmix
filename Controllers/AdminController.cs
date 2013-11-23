using System.Linq.Expressions;
using System.Web.Mvc;
using DeftIndustries.FeedMix.Models;
using DeftIndustries.FeedMix.ViewModels;
using Orchard;
using Orchard.DisplayManagement;
using Orchard.Localization;
using Orchard.Security;
using Orchard.ContentManagement;
using Orchard.Tags.Services;
using Orchard.UI.Notify;
using System.Linq;

namespace DeftIndustries.FeedMix.Controllers
{
    [ValidateInput(false)]
    public class AdminController : Controller, IUpdateModel {
        private readonly IFeedService _feedService;

        public AdminController(
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

        [HttpGet]
        public ActionResult Index() {

            var feeds = Services.ContentManager
                .Query<FeedPart, FeedPartRecord>();

            var results = feeds.Slice(0, 10)
                .ToList();

            var model = new FeedsIndexViewModel {
                Feeds = results
                    .Select(x => new FeedEntry { Feed = x.Record })
                    .ToList()
            };
            return View(model);
        }

        public ActionResult Create()
        {
            var user = Services.ContentManager.New<FeedPart>("Feed");
            var editor = Shape.EditorTemplate(TemplateName: "Parts/Feed.Create", Model: new FeedCreateViewModel(), Prefix: null);
            editor.Metadata.Position = "2";
            dynamic model = Services.ContentManager.BuildEditor(user);
            model.Content.Add(editor);

            // Casting to avoid invalid (under medium trust) reflection over the protected View method and force a static invocation.
            return View((object)model);
        }

        [HttpPost, ActionName("Create")]
        public ActionResult CreatePOST(FeedCreateViewModel createModel)
        {
            var feedPart = Services.ContentManager.New<FeedPart>("Feed");

            dynamic model = Services.ContentManager.UpdateEditor(feedPart, this);

            if (!ModelState.IsValid)
            {
                Services.TransactionManager.Cancel();

                var editor = Shape.EditorTemplate(TemplateName: "Parts/User.Create", Model: createModel, Prefix: null);
                editor.Metadata.Position = "2";
                model.Content.Add(editor);

                // Casting to avoid invalid (under medium trust) reflection over the protected View method and force a static invocation.
                return View((object)model);
            }

            feedPart.Title = createModel.Title;
            Services.ContentManager.Create(feedPart);

            Services.Notifier.Information(T("Feed mix created. Now add some new feeds"));
            return RedirectToAction("Edit", "Admin", new { feedPart.Id });
        }

        public ActionResult Edit(int id)
        {
            if (!Services.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not authorized to manage users")))
                return new HttpUnauthorizedResult();

            var feed = Services.ContentManager.Get<FeedPart>(id);
            var editor = Shape.EditorTemplate(TemplateName: "Parts/Feed.Edit", Model: new FeedEditViewModel { Feed = feed }, Prefix: null);
            editor.Metadata.Position = "2";
            dynamic model = Services.ContentManager.BuildEditor(feed);
            model.Content.Add(editor);

            // Casting to avoid invalid (under medium trust) reflection over the protected View method and force a static invocation.
            return View((object)model);
        }

        public ActionResult Delete(int id)
        {
            var feed = Services.ContentManager.Get<FeedPart>(id);
            Services.ContentManager.Remove(feed.ContentItem);

            return RedirectToAction("Index");
        }

        public ActionResult AddFeed(int id)
        {
            if (!Services.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not authorized to manage users")))
                return new HttpUnauthorizedResult();

            var feed = Services.ContentManager.Get<FeedPart>(id);
            var editor = Shape.EditorTemplate(TemplateName: "Parts/Feed.AddFeed", Model: new FeedAddFeedViewModel (), Prefix: null);
            editor.Metadata.Position = "2";
            dynamic model = Services.ContentManager.BuildEditor(feed);
            model.Content.Add(editor);

            // Casting to avoid invalid (under medium trust) reflection over the protected View method and force a static invocation.
            return View((object)model);
        }


        [HttpPost, ActionName("AddFeed")]
        public ActionResult AddFeedPOST(int id)
        {
            var feedAddViewModel = new FeedAddFeedViewModel ();
            if (!TryUpdateModel(feedAddViewModel)) 
            {
                return RedirectToAction("AddFeed", "Admin", new { id });
            }

            var feed = Services.ContentManager.Get<FeedPart>(id);
                var feedRecord = new FeedRecord {
                    FeedPartRecord = feed.Record,
                    URL = feedAddViewModel.URL,
                    Title = feedAddViewModel.Title,
                    Author = feedAddViewModel.Author
                };
            
            _feedService.CreateFeed(feedRecord);

            Services.Notifier.Information(T("Feed added"));
            return RedirectToAction("Edit", "Admin", new { id });

            /*
            dynamic model = Services.ContentManager.UpdateEditor(user, this);

            if (!ModelState.IsValid)
            {
                Services.TransactionManager.Cancel();

                var editor = Shape.EditorTemplate(TemplateName: "Parts/User.Create", Model: createModel, Prefix: null);
                editor.Metadata.Position = "2";
                model.Content.Add(editor);

                // Casting to avoid invalid (under medium trust) reflection over the protected View method and force a static invocation.
                return View((object)model);
            }
            */
        }

        public ActionResult DeleteFeed(int id, int feedId) {
            _feedService.DeleteFeed(feedId);
            Services.Notifier.Information(T("Feed added"));
            return RedirectToAction("Edit", "Admin", new { id });
        }

        bool IUpdateModel.TryUpdateModel<TModel>(TModel model, string prefix, string[] includeProperties, string[] excludeProperties)
        {
            return TryUpdateModel(model, prefix, includeProperties, excludeProperties);
        }

        public void AddModelError(string key, LocalizedString errorMessage)
        {
            ModelState.AddModelError(key, errorMessage.ToString());
        }
    }
}
