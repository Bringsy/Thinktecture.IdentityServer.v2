using BrockAllen.MembershipReboot;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Configuration;
using System.Data.Entity;
using System.Security.Claims;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Thinktecture.IdentityServer.Repositories;
using Thinktecture.IdentityServer.Repositories.Sql;

namespace Thinktecture.IdentityServer.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        [Import]
        public IConfigurationRepository ConfigurationRepository { get; set; }

        [Import]
        public IUserRepository UserRepository { get; set; }

        [Import]
        public IRelyingPartyRepository RelyingPartyRepository { get; set; }

        protected void Application_Start()
        {
            // create empty config database if it not exists
            Database.SetInitializer(new ConfigurationDatabaseInitializer());
            Database.SetInitializer(new MembershipRebootDatabaseInitializer());

            // set the anti CSRF for name (that's a unqiue claim in our system)
            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.Name;

            // setup MEF
            SetupCompositionContainer();
            Container.Current.SatisfyImportsOnce(this);

            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters, ConfigurationRepository);
            RouteConfig.RegisterRoutes(RouteTable.Routes, ConfigurationRepository, UserRepository);
            ProtocolConfig.RegisterProtocols(GlobalConfiguration.Configuration, RouteTable.Routes, ConfigurationRepository, UserRepository, RelyingPartyRepository);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        private void SetupCompositionContainer()
        {
            Container.Current = new CompositionContainer(new RepositoryExportProvider());
        }
    }

    // TODO: move to some propper location
    // need to set requireAccountVerification to false so membershipReboot don't send emails 
    public class MembershipRebootDatabaseInitializer : CreateDatabaseIfNotExists<EFMembershipRebootDatabase>
    {
        public static void SeedContext(EFMembershipRebootDatabase context)
        {
            var entry = ConfigurationManager.AppSettings["idsrv:CreateTestDataOnInitialization"];

            if (entry != null)
            {
                bool createData = false;
                if (bool.TryParse(entry, out createData))
                {
                    if (createData)
                    {
                        CreateTestUserAccounts();
                    }
                }
            }
        }

        protected override void Seed(EFMembershipRebootDatabase context)
        {
            SeedContext(context);
            base.Seed(context);
        }

        private static void CreateUserAccount(string name, UserAccountService accountService)
        {
            var user = accountService.CreateAccount(name, "test", name.ToLower() + "@domain.com");
            accountService.VerifyAccount(user.VerificationKey);
        }

        private static void CreateTestUserAccounts()
        {
            var accountService = DependencyResolver.Current.GetService<UserAccountService>();

            CreateUserAccount("Kristina", accountService);
            CreateUserAccount("Toni", accountService);
            CreateUserAccount("Misty", accountService);
            CreateUserAccount("Mae", accountService);
            CreateUserAccount("Shelly", accountService);
            CreateUserAccount("Daisy", accountService);
            CreateUserAccount("Ramona", accountService);
            CreateUserAccount("Sherri", accountService);
            CreateUserAccount("Erika", accountService);
            CreateUserAccount("Katrina", accountService);
            CreateUserAccount("Claire", accountService);
            CreateUserAccount("Lindsey", accountService);
            CreateUserAccount("Lindsay", accountService);
            CreateUserAccount("Geneva", accountService);
            CreateUserAccount("Guadalupe", accountService);
            CreateUserAccount("Belinda", accountService);
            CreateUserAccount("Margarita", accountService);
            CreateUserAccount("Sheryl", accountService);
            CreateUserAccount("Cora", accountService);
            CreateUserAccount("Faye", accountService);
            CreateUserAccount("Ada", accountService);
            CreateUserAccount("Natasha", accountService);
            CreateUserAccount("Sabrina", accountService);
            CreateUserAccount("Isabel", accountService);
            CreateUserAccount("Marguerite", accountService);
            CreateUserAccount("Hattie", accountService);
            CreateUserAccount("Harriet", accountService);
            CreateUserAccount("Molly", accountService);
            CreateUserAccount("Cecilia", accountService);
            CreateUserAccount("Kristi", accountService);
            CreateUserAccount("Brandi", accountService);
            CreateUserAccount("Blanche", accountService);
            CreateUserAccount("Sandy", accountService);
            CreateUserAccount("Rosie", accountService);
            CreateUserAccount("Joanna", accountService);
            CreateUserAccount("Iris", accountService);
            CreateUserAccount("Eunice", accountService);
            CreateUserAccount("Angie", accountService);
            CreateUserAccount("Inez", accountService);
            CreateUserAccount("Lynda", accountService);
            CreateUserAccount("Madeline", accountService);
            CreateUserAccount("Amelia", accountService);
            CreateUserAccount("Alberta", accountService);
            CreateUserAccount("Genevieve", accountService);
            CreateUserAccount("Monique", accountService);
            CreateUserAccount("Jodi", accountService);
            CreateUserAccount("Janie", accountService);
            CreateUserAccount("Maggie", accountService);
            CreateUserAccount("Kayla", accountService);
            CreateUserAccount("Sonya", accountService);
            CreateUserAccount("Jan", accountService);
            CreateUserAccount("Lee", accountService);


        }
    }
}