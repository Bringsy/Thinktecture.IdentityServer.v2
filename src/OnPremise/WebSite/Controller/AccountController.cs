using BrockAllen.MembershipReboot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Thinktecture.IdentityServer.Web.ViewModels;

namespace Thinktecture.IdentityServer.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        UserAccountService userAccountService;

        public AccountController(UserAccountService userAccountService)
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

            return View(model);
        }
    }
}
