using System.IdentityModel.Services;
using System.Web.Mvc;

namespace Thinktecture.IdentityServer.Web.Controllers
{
    public class SignOutController : Controller
    {
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                FederatedAuthentication.SessionAuthenticationModule.DeleteSessionTokenCookie();
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
