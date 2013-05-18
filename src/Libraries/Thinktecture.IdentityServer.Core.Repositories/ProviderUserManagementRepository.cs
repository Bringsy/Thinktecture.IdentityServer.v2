using BrockAllen.MembershipReboot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Thinktecture.IdentityServer.Repositories.Sql;
using Thinktecture.IdentityServer.Repositories.Sql.Configuration;

namespace Thinktecture.IdentityServer.Repositories
{
    public class ProviderUserManagementRepository : IUserManagementRepository
    {
        UserAccountService userService;

        public ProviderUserManagementRepository()
        {
            this.userService = new UserAccountService(new EFUserAccountRepository(), null, null);
        }

        public void CreateUser(string userName, string password, string email = null)
        {
            var user = userService.CreateAccount(userName, password, email == null ? userName : email);
            this.userService.VerifyAccount(user.VerificationKey);
        }

        public void DeleteUser(string userName)
        {
            userService.DeleteAccount(userName);
        }

        public IEnumerable<string> GetUsers()
        {
            return userService.GetAll().Select(c => c.Username);
        }

        public IEnumerable<string> GetUsers(string filter)
        {
            var items = userService.GetAll();
            var query =
                from user in items
                where user.Username.Contains(filter) ||
                      (user.Email != null && user.Email.Contains(filter))
                select user.Username;
            return query;
        }

        public void SetPassword(string userName, string password)
        {
            this.userService.SetPassword(userName, password);
        }

        #region Roles

        public void SetRolesForUser(string userName, IEnumerable<string> roles)
        {
            var user = this.userService.GetByUsername(userName);

            foreach (var role in roles)
            {
                user.AddClaim(ClaimTypes.Role, role);
            }

            this.userService.SaveChanges(); 
        }

        public IEnumerable<string> GetRolesForUser(string userName)
        {
            var user = this.userService.GetByUsername(userName);
            
            if (user == null)
                return new string[] { }; 

            return user.Claims.Where(c => c.Type.Equals(ClaimTypes.Role)).Select(s => s.Value);
        }

        public IEnumerable<string> GetRoles()
        {
            using (var db = IdentityServerConfigurationContext.Get())
            {
                return db.ClaimInfos.Where(c => c.Type.Equals(ClaimTypes.Role)).Select(s => s.Value).ToList();
            }
        }

        public void CreateRole(string roleName)
        {
            using (var db = IdentityServerConfigurationContext.Get())
            {
                var claimInfo = new ClaimInfo
                {
                    Type = ClaimTypes.Role,
                    Value = roleName
                };
                db.ClaimInfos.Add(claimInfo);
                db.SaveChanges();
            }
        }

        public void DeleteRole(string roleName)
        {
            using (var db = IdentityServerConfigurationContext.Get())
            {
                var claimInfo = db.ClaimInfos.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Role)
                    && c.Value.Equals(roleName, StringComparison.InvariantCultureIgnoreCase));

                if (claimInfo != null)
                {
                    db.ClaimInfos.Remove(claimInfo);
                    db.SaveChanges();
                }
            }
        }

        #endregion
    }
}
