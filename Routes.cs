using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Orchard.Mvc.Routes;

namespace Orchard.Core.Feeds.Rss {
    public class Routes : IRouteProvider {
        public IEnumerable<RouteDescriptor> GetRoutes() {
            return new[] {
                             new RouteDescriptor {
                                                     Priority = -5,
                                                     Route = new Route(
                                                         "DeftIndustries.feedmix/feed/{friendlyFeedName}",
                                                         new RouteValueDictionary {
                                                                                      {"area", "DeftIndustries.feedmix"},
                                                                                      {"controller", "Feed"},
                                                                                      {"action", "Index"}
                                                                                  },
                                                         new RouteValueDictionary (),
                                                         new RouteValueDictionary {
                                                                                      {"area", "DeftIndustries.feedmix"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 }
                         };
        }

        public void GetRoutes(ICollection<RouteDescriptor> routes) {
            foreach (RouteDescriptor routeDescriptor in GetRoutes()) {
                routes.Add(routeDescriptor);
            }
        }
    }
}