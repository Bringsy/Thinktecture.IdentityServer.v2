using BrockAllen.MembershipReboot;
using System.IdentityModel.Services;
using System.Linq;
using System.Security.Claims;
using System.Web.Mvc;
using Thinktecture.IdentityServer.Web.Areas.Account.ViewModels;

namespace Thinktecture.IdentityServer.Web.Areas.Account.Controllers
{
    public static class UserAccountExtensions
    {
        public static void EditClaim(this UserAccount user, string type, string value)
        {
            if (user.HasClaim(type))
                user.RemoveClaim(type);

            user.AddClaim(type, value);
        }
    }

    [Authorize]
    public class GeneralController : Controller
    {
        UserAccountService userAccountService;

        public GeneralController(UserAccountService userAccountService)
        {
            this.userAccountService = userAccountService;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.userAccountService != null)
                {
                    this.userAccountService.Dispose();
                    this.userAccountService = null;
                }
            }
            base.Dispose(disposing);
        }

        public ActionResult Index()
        {
            var user = this.userAccountService.GetByUsername(ClaimsPrincipal.Current.Identity.Name);

            if (user == null)
                FederatedAuthentication.SessionAuthenticationModule.DeleteSessionTokenCookie();

            var vm = new AccountModel
            {
                UserName = user.Username,
                Email = user.Email
            };

            // Map claims 
            vm.Surname = user.GetClaimValue(System.IdentityModel.Claims.ClaimTypes.Surname);
            vm.GivenName = user.GetClaimValue(System.IdentityModel.Claims.ClaimTypes.GivenName);
            vm.Roles = user.GetClaimValues(System.Security.Claims.ClaimTypes.Role).ToArray();

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(AccountModel model)
        {
            var user = this.userAccountService.GetByUsername(ClaimsPrincipal.Current.Identity.Name);

            user.EditClaim(System.IdentityModel.Claims.ClaimTypes.Surname, model.Surname);
            user.EditClaim(System.IdentityModel.Claims.ClaimTypes.GivenName, model.GivenName);

            this.userAccountService.SaveChanges();

            TempData["Message"] = "Update Successful";

            return RedirectToAction("Index"); 
        }
    }
}
