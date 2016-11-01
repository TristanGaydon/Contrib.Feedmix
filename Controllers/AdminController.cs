using System.Linq.Expressions;
using System.Web.Mvc;
using Contrib.FeedMix.Models;
using Contrib.FeedMix.ViewModels;
using Orchard;
using Orchard.DisplayManagement;
using Orchard.Localization;
using Orchard.Security;
using Orchard.ContentManagement;
using Orchard.Tags.Services;
using Orchard.UI.Notify;
using System.Linq;

namespace Contrib.FeedMix.Controllers
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

            var feeds = Services.ContentManager.Query<FeedMixPart, FeedMixPartRecord>();

            var results = feeds.Slice(0, 10).ToList();

            var model = new FeedsIndexViewModel {
                Feeds = results
                    .Select(x => new FeedEntry { FeedMix = x })
                    .ToList()
            };
            return View(model);
        }

        public ActionResult Create()
        {
            var user = Services.ContentManager.New<FeedMixPart>("FeedMix");
            var editor = Shape.EditorTemplate(TemplateName: "Parts/FeedMix.Create", Model: new FeedCreateViewModel(), Prefix: null);
            editor.Metadata.Position = "2";
            dynamic model = Services.ContentManager.BuildEditor(user);
            model.Content.Add(editor);

            // Casting to avoid invalid (under medium trust) reflection over the protected View method and force a static invocation.
            return View((object)model);
        }

        [HttpPost, ActionName("Create")]
        public ActionResult CreatePOST(FeedCreateViewModel createModel)
        {
            var feedPart = Services.ContentManager.New<FeedMixPart>("FeedMix");

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
            feedPart.Description = createModel.Description;
            feedPart.Path = createModel.Path;
            Services.ContentManager.Create(feedPart);

            Services.Notifier.Information(T("Feed mix created. Now add some new feeds"));
            return RedirectToAction("Edit", "Admin", new { feedPart.Id });
        }

        public ActionResult Edit(int id, FeedEditOptions options)
        {
            if (!Services.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not authorized to manage users")))
                return new HttpUnauthorizedResult();

            var feed = Services.ContentManager.Get<FeedMixPart>(id);
            
            // default options
            if (options == null)
                options = new FeedEditOptions();

            var editor = Shape.EditorTemplate(TemplateName: "Parts/FeedMix.Edit", Model: new FeedEditViewModel(_feedService) { Feed = feed, Options = options }, Prefix: null);
            editor.Metadata.Position = "2";
            dynamic model = Services.ContentManager.BuildEditor(feed);
            model.Content.Add(editor);

            // Casting to avoid invalid (under medium trust) reflection over the protected View method and force a static invocation.
            return View((object)model);
        }

        [HttpPost, ActionName("Edit")]
        public ActionResult EditPOST(int id, string title, string description, string path)
        {
            var feedPart = Services.ContentManager.Get<FeedMixPart>(id);

            dynamic model = Services.ContentManager.UpdateEditor(feedPart, this);

            if (!ModelState.IsValid)
            {
                Services.TransactionManager.Cancel();

                //var editor = Shape.EditorTemplate(TemplateName: "Parts/User.Create", Model: editModel, Prefix: null);
                // editor.Metadata.Position = "2";
                // model.Content.Add(editor);

                // Casting to avoid invalid (under medium trust) reflection over the protected View method and force a static invocation.
                return View((object)model);
            }

            feedPart.Title = title;
            feedPart.Description = description;
            feedPart.Path = path;

            Services.Notifier.Information(T("Feed mix updated."));
            return RedirectToAction("Index", "Admin", new { feedPart.Id });
        }

        public ActionResult Delete(int id)
        {
            var feed = Services.ContentManager.Get<FeedMixPart>(id);
            Services.ContentManager.Remove(feed.ContentItem);

            return RedirectToAction("Index");
        }

        public ActionResult AddFeed(int id)
        {
            if (!Services.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not authorized to manage users")))
                return new HttpUnauthorizedResult();

            var feed = Services.ContentManager.Get<FeedMixPart>(id);
            var editor = Shape.EditorTemplate(TemplateName: "Parts/FeedMix.AddFeed", Model: new FeedAddFeedViewModel (), Prefix: null);
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

            var feed = Services.ContentManager.Get<FeedMixPart>(id);
            var feedPart = Services.ContentManager.New<FeedPart>("Feed");
            feedPart.FeedUrl = feedAddViewModel.FeedUrl;
            feedPart.WebsiteUrl = feedAddViewModel.WebsiteUrl;
            feedPart.Title = feedAddViewModel.Title;
            feedPart.Author = feedAddViewModel.Author;
            feedPart.FeedMixPartRecord = feed.Record;
            Services.ContentManager.Create(feedPart);

            Services.Notifier.Information(T("Feed added"));
            return RedirectToAction("Edit", "Admin", new { id });
        }

        public ActionResult EditFeed(int id)
        {
            if (!Services.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not authorized to manage users")))
                return new HttpUnauthorizedResult();


            var feedPart = Services.ContentManager.Get<FeedPart>(id);
            var editor = Shape.EditorTemplate(TemplateName: "Parts/Feed.EditFeed", Model: new FeedEditFeedViewModel { Feed = feedPart }, Prefix: null);
            editor.Metadata.Position = "2";
            
            dynamic model = Services.ContentManager.BuildEditor(feedPart);
            model.Content.Add(editor);

            // Casting to avoid invalid (under medium trust) reflection over the protected View method and force a static invocation.
            return View((object)model);
        }

        [HttpPost, ActionName("EditFeed")]
        public ActionResult EditFeedPOST(int id, string feedurl, string websiteurl, string title, string author)
        {
            var feedPart = Services.ContentManager.Get<FeedPart>(id);

            dynamic model = Services.ContentManager.UpdateEditor(feedPart, this);

            if (!ModelState.IsValid)
            {
                Services.TransactionManager.Cancel();

                //var editor = Shape.EditorTemplate(TemplateName: "Parts/User.Create", Model: editModel, Prefix: null);
                // editor.Metadata.Position = "2";
                // model.Content.Add(editor);

                // Casting to avoid invalid (under medium trust) reflection over the protected View method and force a static invocation.
                return View((object)model);
            }

            feedPart.FeedUrl = feedurl;
            feedPart.WebsiteUrl = websiteurl;
            feedPart.Title = title;
            feedPart.Author = author;
           
            Services.Notifier.Information(T("Feed updated."));
            return RedirectToAction("Edit", "Admin", new { feedPart.FeedMixPartRecord.Id });
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
