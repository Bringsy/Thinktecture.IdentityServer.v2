using BrockAllen.MembershipReboot;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Thinktecture.IdentityServer.Repositories;

namespace Thinktecture.IdentityServer.Web.Areas.Admin.ViewModels
{
    public class UserViewModel
    {
        [Import]
        public IConfigurationRepository ConfigurationRepository { get; set; }

        public UserViewModel()
        {
            Container.Current.SatisfyImportsOnce(this);
        }

        public bool EmailIsUsername
        {
            get
            {
                return BrockAllen.MembershipReboot.SecuritySettings.Instance.EmailIsUsername;
            }
        }

        public bool IsOAuthRefreshTokenEnabled
        {
            get
            {
                return this.ConfigurationRepository.OAuth2.Enabled &&
                    (ConfigurationRepository.OAuth2.EnableCodeFlow || ConfigurationRepository.OAuth2.EnableResourceOwnerFlow);
            }
        }

        public bool IsProfileEnabled
        {
            get
            {
                return true; // TODO 
            }
        }

        public UserAccount User { get; set; }
    }
}