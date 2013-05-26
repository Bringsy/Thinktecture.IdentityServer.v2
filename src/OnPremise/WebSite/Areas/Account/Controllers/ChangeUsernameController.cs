using BrockAllen.MembershipReboot;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens;
using System.Web.Mvc;
using Thinktecture.IdentityServer.Protocols;
using Thinktecture.IdentityServer.Repositories;
using Thinktecture.IdentityServer.Web.Areas.Account.ViewModels;

namespace Thinktecture.IdentityServer.Web.Areas.Account.Controllers
{
    [Authorize]
    public class ChangeUsernameController : Controller
    {
        [Import]
        private IConfigurationRepository configurationRepository { get; set; }
        private UserAccountService userAccountService;
        private ClaimsBasedAuthenticationService authService;        

        public ChangeUsernameController(UserAccountService userAccountService, ClaimsBasedAuthenticationService authService)
        {
            Container.Current.SatisfyImportsOnce(this);
            this.userAccountService = userAccountService;
            this.authService = authService;
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
            return View("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(ChangeUsernameInputModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    this.userAccountService.ChangeUsername(User.Identity.Name, model.NewUsername);
                    // this.authService.SignIn(model.NewUsername);

                    new AuthenticationHelper().SetSessionToken(
                         model.NewUsername,
                         AuthenticationMethods.Password,
                         false, /* TODO: get from current entity */
                         this.configurationRepository.Global.SsoCookieLifetime);

                    //this.TempData["Message"] = " Username changed successfully to " + User.Identity.Name;
                    //return View("Index", model);
                    return RedirectToAction("Success");
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            return View("Index", model);
        }

        public ActionResult Success()
        {
            this.TempData["Message"] = " Username changed successfully to " + User.Identity.Name;
            return View("Index");
        }
    }
}
