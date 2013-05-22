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

            routes.MapRoute("ChangeImail", "changeemail/{action}/{id}", new { controller = "ChangeEmail", action = "Index", id = UrlParameter.Optional });
            routes.MapRoute("ChangePassword", "changepassword/{action}/{id}", new { controller = "ChangePassword", action = "Index", id = UrlParameter.Optional });
            routes.MapRoute("CloseAccount", "closeaccount/{action}/{id}", new { controller = "CloseAccount", action = "Index", id = UrlParameter.Optional });
            routes.MapRoute("PasswordReset", "passwordreset/{action}/{id}", new { controller = "PasswordReset", action = "Index", id = UrlParameter.Optional });
            routes.MapRoute("SignIn", "signin/{action}/{id}", new { controller = "SignIn", action = "Index", id = UrlParameter.Optional });
            routes.MapRoute("SignOut", "signout/{action}/{id}", new { controller = "SignOut", action = "Index", id = UrlParameter.Optional });
            routes.MapRoute("SignUp", "signup/{action}/{id}", new { controller = "SignUp", action = "Index", id = UrlParameter.Optional });

            #region Main UI

            routes.MapRoute(
                "Home",
                "{action}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new string[] { "Thinktecture.IdentityServer.Web.Controllers" }
            );
            #endregion

            #endregion
        }

    }
}