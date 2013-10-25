using BrockAllen.MembershipReboot;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Thinktecture.IdentityServer.TokenService;

namespace Thinktecture.IdentityServer.Repositories
{
    public class ProviderClaimsRepository : IClaimsRepository
    {
        private const string ProfileClaimPrefix = "http://identityserver.thinktecture.com/claims/profileclaims/";
        UserAccountService userService;

        public ProviderClaimsRepository()
        {
            this.userService = new UserAccountService(new EFUserAccountRepository(), null, null);
        }

        public IEnumerable<Claim> GetClaims(ClaimsPrincipal principal, RequestDetails requestDetails)
        {
            var userName = principal.Identity.Name;
            var claims = new List<Claim>(from c in principal.Claims select c);
            var user = this.userService.GetByUsername(userName);

            if (user != null)
            {
                // replace name claim by email claim if email is user name 
                if (SecuritySettings.Instance.EmailIsUsername)
                {
                    var nameClaim = claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Name));
                    if (nameClaim != null)
                    {
                        claims.Add(new Claim(ClaimTypes.Email, nameClaim.Value));
                        claims.Remove(nameClaim);
                    }
                }
                else
                {
                    // TODO: validate if email is already there if email is not user name 
                    claims.Add(new Claim(ClaimTypes.Email, user.Email));
                }

                // add name id 
                var nameIdentifierClaim = claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier));
                if (nameIdentifierClaim != null)
                {
                    claims.Remove(nameIdentifierClaim); 
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, user.NameID.ToString()));
                }

                // add other user claims 
                // TODO: filter claims 
                foreach (var userClaim in user.Claims)
                    claims.Add(new Claim(userClaim.Type, userClaim.Value));
            }

            return claims;
        }

        public IEnumerable<string> GetSupportedClaimTypes()
        {
            var claimTypes = new List<string>
            {
                ClaimTypes.NameIdentifier,
                ClaimTypes.Name,
                ClaimTypes.Email,
                ClaimTypes.Role
            };

            // if (ProfileManager.Enabled)
            // {
            //     foreach (SettingsProperty prop in ProfileBase.Properties)
            //     {
            //         claimTypes.Add(GetProfileClaimType(prop.Name.ToLowerInvariant()));
            //     }
            // }

            return claimTypes;
        }

        /*
        protected virtual IEnumerable<Claim> GetProfileClaims(string userName)
        {
            var claims = new List<Claim>();

            if (ProfileManager.Enabled)
            {
                var profile = ProfileBase.Create(userName, true);
                if (profile != null)
                {
                    foreach (SettingsProperty prop in ProfileBase.Properties)
                    {
                        object value = profile.GetPropertyValue(prop.Name);
                        if (value != null)
                        {
                            if (!string.IsNullOrWhiteSpace(value.ToString()))
                            {
                                claims.Add(new Claim(GetProfileClaimType(prop.Name.ToLowerInvariant()), value.ToString()));
                            }
                        }
                    }
                }
            }

            return claims;
        }

        protected virtual string GetProfileClaimType(string propertyName)
        {
            if (StandardClaimTypes.Mappings.ContainsKey(propertyName))
            {
                return StandardClaimTypes.Mappings[propertyName];
            }
            else
            {
                return string.Format("{0}{1}", ProfileClaimPrefix, propertyName);
            }
        }

        protected virtual IEnumerable<string> GetRolesForToken(string userName)
        {
            var returnedRoles = new List<string>();

            if (Roles.Enabled)
            {
                var roles = Roles.GetRolesForUser(userName);
                returnedRoles = roles.Where(role => !(role.StartsWith(Constants.Roles.InternalRolesPrefix))).ToList();
            }

            return returnedRoles;
        }
         * */
    }
}