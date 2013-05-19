using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Web.Mvc;
using Thinktecture.IdentityServer.Protocols;
using Thinktecture.IdentityServer.Repositories;
using Thinktecture.IdentityServer.Web.ViewModels;

namespace Thinktecture.IdentityServer.Web.Controllers
{
    public class SignInController : Controller
    {
        [Import]
        public IUserRepository UserRepository { get; set; }

        [Import]
        public IConfigurationRepository ConfigurationRepository { get; set; }

        public SignInController()
        {
            Container.Current.SatisfyImportsOnce(this);
        }

        public SignInController(IUserRepository userRepository, IConfigurationRepository configurationRepository)
        {
            UserRepository = userRepository;
            ConfigurationRepository = configurationRepository;
        }
        
        public ActionResult Index(string returnUrl, bool mobile = false)
        {
            // you can call AuthenticationHelper.GetRelyingPartyDetailsFromReturnUrl to get more information about the requested relying party

            var vm = new SignInModel()
            {
                ReturnUrl = returnUrl,
                ShowClientCertificateLink = ConfigurationRepository.Global.EnableClientCertificateAuthentication
            };

            if (mobile) vm.IsSigninRequest = true;
            return View(vm);
        }

        [HttpPost]
        public ActionResult Index(SignInModel model)
        {
            if (ModelState.IsValid)
            {
                if (UserRepository.ValidateUser(model.Email, model.Password))
                {
                    // establishes a principal, set the session cookie and redirects
                    // you can also pass additional claims to signin, which will be embedded in the session token

                    return SignIn(
                        model.Email,
                        AuthenticationMethods.Password,
                        model.ReturnUrl,
                        model.EnableSSO,
                        ConfigurationRepository.Global.SsoCookieLifetime);
                }
            }

            ModelState.AddModelError("", Resources.AccountController.IncorrectCredentialsNoAuthorization);

            model.ShowClientCertificateLink = ConfigurationRepository.Global.EnableClientCertificateAuthentication;
            return View(model);
        }

        // handles client certificate based signin
        public ActionResult Certificate(string returnUrl)
        {
            if (!ConfigurationRepository.Global.EnableClientCertificateAuthentication)
            {
                return new HttpNotFoundResult();
            }

            var clientCert = HttpContext.Request.ClientCertificate;

            if (clientCert != null && clientCert.IsPresent && clientCert.IsValid)
            {
                string userName;
                if (UserRepository.ValidateUser(new X509Certificate2(clientCert.Certificate), out userName))
                {
                    // establishes a principal, set the session cookie and redirects
                    // you can also pass additional claims to signin, which will be embedded in the session token

                    return SignIn(
                        userName,
                        AuthenticationMethods.X509,
                        returnUrl,
                        false,
                        ConfigurationRepository.Global.SsoCookieLifetime);
                }
            }

            return View("Error");
        }

        #region helpers 

        protected virtual ActionResult SignIn(string userName, string authenticationMethod, 
            string returnUrl, bool isPersistent, int ttl, IEnumerable<Claim> additionalClaims = null)
        {
            new AuthenticationHelper().SetSessionToken(
                userName,
                authenticationMethod,
                isPersistent,
                ttl,
                additionalClaims);

            if (!string.IsNullOrWhiteSpace(returnUrl))
            {
                return RedirectToLocal(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        #endregion
    }
}
