using Orchard.Localization;
using Orchard.UI.Navigation;

namespace DeftIndustries.FeedMix
{
    public class AdminMenu : INavigationProvider
    {
        public string MenuName
        {
            get { return "admin"; }
        }

        public AdminMenu()
        {
            T = NullLocalizer.Instance;
        }

        private Localizer T { get; set; }

        public void GetNavigation(NavigationBuilder builder)
        {
            builder

                // Image set
                .AddImageSet("feeds")

                // "Webshop"
                .Add(item => item

                    .Caption(T("Feeds"))
                    .Position("2")
                    .Action("Index", "Admin", new { area = "DeftIndustries.FeedMix" })
                );
        }
    }
}