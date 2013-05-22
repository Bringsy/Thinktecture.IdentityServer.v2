using System.Web.Mvc;

namespace Thinktecture.IdentityServer.Web.Areas.Account
{
    public class AccountAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Account";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Account_default",
                "Account/{controller}/{action}/{id}",
                new { area="Account", action = "Index", id = UrlParameter.Optional }
            );

            AutofacConfig.Register(); 
        }
    }
}
