using System.Configuration;
using Autofac;
using Autofac.Integration.Mvc;
using BrockAllen.MembershipReboot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Thinktecture.IdentityServer.Web.Areas.Account
{
    public static class HttpContextExtensions
    {
        public static string GetApplicationUrl(this HttpContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            var request = context.Request;
            var baseUrl =
                request.Url.Scheme +
                "://" +
                request.Url.Host + (
                    ((request.Url.Scheme == "http" && request.Url.Port == 80) ||
                    (request.Url.Scheme == "https" && request.Url.Port == 443)) ? "" : ":" + request.Url.Port) +
                request.ApplicationPath;
            if (!baseUrl.EndsWith("/")) baseUrl += "/";

            return baseUrl;
        }
    }

    public static class AutofacConfig
    {
        internal static void Register()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<UserAccountService>();
            builder.RegisterType<ClaimsBasedAuthenticationService>();

            builder
                .RegisterType<EFUserAccountRepository>()
                .As<IUserAccountRepository>()
                .InstancePerHttpRequest();

            //builder.RegisterType<NopMessageDelivery>().As<IMessageDelivery>();
            builder.RegisterType<SmtpHtmlMessageDelivery>().As<IMessageDelivery>();

            builder.RegisterType<NopPasswordPolicy>().As<IPasswordPolicy>();
            //builder.Register<IPasswordPolicy>(x=>new BasicPasswordPolicy { MinLength = 4 });

            builder.RegisterType<TemplateNotificationService>().As<INotificationService>();

            builder.Register<ApplicationInformation>(
                x =>
                {
                    // build URL
                    var baseUrl = HttpContext.Current.GetApplicationUrl();

                    return new ApplicationInformation
                    {
                        EmailSignature = ConfigurationManager.AppSettings["EmailSignature"],
                        ApplicationName = ConfigurationManager.AppSettings["ApplicationName"],
                        LoginUrl = baseUrl + "SignIn",
                        VerifyAccountUrl = baseUrl + "SignUp/Confirm/",
                        CancelNewAccountUrl = baseUrl + "SignUp/Cancel/",
                        ConfirmPasswordResetUrl = baseUrl + "PasswordReset/Confirm/",
                        ConfirmChangeEmailUrl = baseUrl + "Account/ChangeEmail/Confirm/"
                    };
                });

            builder.RegisterControllers(typeof(AutofacConfig).Assembly);
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}