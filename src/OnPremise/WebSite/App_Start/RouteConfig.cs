using System.Web.Mvc;
using System.Web.Routing;
using Thinktecture.IdentityServer.Repositories;

namespace Thinktecture.IdentityServer.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes, IConfigurationRepository configuration, IUserRepository userRepository)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
               "InitialConfiguration",
               "initialconfiguration",
               new { controller = "InitialConfiguration", action = "Index" }
           );

            #region Main UI

            /*
            routes.MapRoute(
                "Account",
                "account/{action}",
                new { controller = "Account", action = "Index", id = UrlParameter.Optional }
            );
           
            routes.MapRoute(
               "SignUp",
               "signup/{action}/{id}",
               new { controller = "SignUp", action = "Index", id = UrlParameter.Optional }
            );
           

            routes.MapRoute(
                "Home",
                "{action}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new string[] { "Thinktecture.IdentityServer.Web.Controllers" }
            );
 */

            routes.MapRoute(
              name: "Default",
              url: "{controller}/{action}/{id}",
              defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
              namespaces: new string[] { "Thinktecture.IdentityServer.Web.Controllers" }
          );

            #endregion
        }

    }
}