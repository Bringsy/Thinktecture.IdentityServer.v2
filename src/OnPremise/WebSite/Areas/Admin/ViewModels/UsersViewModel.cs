using BrockAllen.MembershipReboot;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Thinktecture.IdentityServer.Repositories;

namespace Thinktecture.IdentityServer.Web.Areas.Admin.ViewModels
{
    public class UsersViewModel
    {
        UserAccountService userAccountService;
        public IEnumerable<UserAccount> Users { get; set; }
        public int Total { get; set; }
        public string Filter { get; set; }
        public int Page { get; set; }
        public int Size { get; set; }
        public UserDeleteModel[] UsersDeleteList { get; set; }

        public bool EmailIsUsername
        {
            get
            {
                return BrockAllen.MembershipReboot.SecuritySettings.Instance.EmailIsUsername;
            }
        }

        public UsersViewModel(UserAccountService userAccountService, int page = 0, int size = 20, string filter = null)
        {
            this.userAccountService = userAccountService;
            this.Filter = filter;
            this.Page = page * size;
            this.Size = size;

            // load users 
            var query = this.userAccountService.GetAll();

            if (!string.IsNullOrWhiteSpace(this.Filter))
                query = query.Where(c => c.Email.Contains(filter) || c.Username.Contains(filter));

            this.Total = query.Count();
            this.Users = query.OrderBy(c => c.Username).Skip(this.Page).Take(this.Size).ToList();

            this.UsersDeleteList = this.Users.Select(c => new UserDeleteModel { Username = c.Username }).ToArray();
        }
    }

    public class UserDeleteModel
    {
        public string Username { get; set; }
        public bool Delete { get; set; }
    }




    public class UsersViewModel2
    {
        [Import]
        public IConfigurationRepository ConfigurationRepository { get; set; }

        private Repositories.IUserManagementRepository UserManagementRepository;

        public UsersViewModel2(Repositories.IUserManagementRepository UserManagementRepository, string filter)
        {
            Container.Current.SatisfyImportsOnce(this);

            this.UserManagementRepository = UserManagementRepository;
            this.Filter = filter;

            if (String.IsNullOrEmpty(filter))
            {
                Users = UserManagementRepository.GetUsers();
                Total = Showing = Users.Count();
            }
            else
            {
                Users = UserManagementRepository.GetUsers(filter);
                Total = UserManagementRepository.GetUsers().Count();
                Showing = Users.Count();
            }

            UsersDeleteList = Users.Select(x => new UserDeleteModel { Username = x }).ToArray();
        }

        public IEnumerable<string> Users { get; set; }
        public UserDeleteModel[] UsersDeleteList { get; set; }
        public string Filter { get; set; }
        public int Total { get; set; }
        public int Showing { get; set; }

        public bool IsProfileEnabled
        {
            get
            {
                return false; // TODO 
            }
        }

        public bool IsOAuthRefreshTokenEnabled
        {
            get
            {
                return ConfigurationRepository.OAuth2.Enabled &&
                    (ConfigurationRepository.OAuth2.EnableCodeFlow || ConfigurationRepository.OAuth2.EnableResourceOwnerFlow);
            }
        }
    }

}