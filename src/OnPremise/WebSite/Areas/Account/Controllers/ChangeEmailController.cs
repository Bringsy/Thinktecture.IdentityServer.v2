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
    public class ChangeEmailController : Controller
    {
        [Import]
        IConfigurationRepository configurationRepository { get; set; }
        UserAccountService userAccountService;
        ClaimsBasedAuthenticationService authService;

        public ChangeEmailController(UserAccountService userAccountService,
            ClaimsBasedAuthenticationService authService)
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
        public ActionResult Index(ChangeEmailRequestInputModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (this.userAccountService.ChangeEmailRequest(User.Identity.Name, model.NewEmail))
                    {
                        TempData["Message"] = "Update Successful, You should recieve an email shortly at " + model.NewEmail;
                        return View(); 
                        //return View("ChangeRequestSuccess", (object)model.NewEmail);
                    }

                    ModelState.AddModelError("", "Error requesting email change.");
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            return View("Index", model);
        }

        public ActionResult Confirm(string id)
        {
            var vm = new ChangeEmailFromKeyInputModel();
            vm.Key = id;
            return View("Confirm", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Confirm(ChangeEmailFromKeyInputModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (this.userAccountService.ChangeEmailFromKey(model.Password, model.Key, model.NewEmail))
                    {
                        // since we've changed the email, we need to re-issue the cookie that contains the claims.
                        var account = this.userAccountService.GetByEmail(model.NewEmail);

                        // authService.SignIn(account.Username);
                        new AuthenticationHelper().SetSessionToken(
                            account.Username,
                            AuthenticationMethods.Password,
                            true,
                            this.configurationRepository.Global.SsoCookieLifetime,
                            null);

                        TempData["Message"] = "You email was successfully changed.";
                        return View("Confirm"); 
                        // return View("Success");
                    }

                    ModelState.AddModelError("", "Error changing email.");
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            return View("Confirm", model);
        }
    }
}
