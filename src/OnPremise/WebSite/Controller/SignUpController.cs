using BrockAllen.MembershipReboot;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Thinktecture.IdentityServer.Web.ViewModels;

namespace Thinktecture.IdentityServer.Web.Controllers
{

    public class SignUpController : Controller
    {
        UserAccountService userAccountService;

        public SignUpController(UserAccountService userAccountService)
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
            // var profileClaims = ProfileClaimsConfiguration.GetProfileClaims().ToArray(); 

            return View(new SignUpModel());
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Index(SignUpModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userName = SecuritySettings.Instance.EmailIsUsername ? model.Email : model.UserName;
                    var user = this.userAccountService.CreateAccount(userName, model.Password, model.Email);

                    // TODO: Add ui culture claim ...
                    user.AddClaim(System.IdentityModel.Claims.ClaimTypes.GivenName, model.GivenName);
                    user.AddClaim(System.IdentityModel.Claims.ClaimTypes.Surname, model.Surname);

                    this.userAccountService.SaveChanges();

                    if (SecuritySettings.Instance.RequireAccountVerification)
                    {
                        return View("Success", model);
                    }
                    else
                    {
                        return View("Confirm", true);
                    }
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(model);
        }

        public ActionResult Confirm(string id)
        {
            var result = this.userAccountService.VerifyAccount(id);
            return View(result);
        }

        public ActionResult Cancel(string id)
        {
            var result = this.userAccountService.CancelNewAccount(id);
            return View(result);
        }
    }
}
