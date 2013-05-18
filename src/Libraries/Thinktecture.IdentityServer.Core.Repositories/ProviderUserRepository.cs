using BrockAllen.MembershipReboot;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Security.Claims;

namespace Thinktecture.IdentityServer.Repositories
{
    public class ProviderUserRepository : IUserRepository
    {
        UserAccountService userService;

        [Import]
        public IClientCertificatesRepository Repository { get; set; }

        public ProviderUserRepository()
        {
            Container.Current.SatisfyImportsOnce(this);
            this.userService = new UserAccountService(new EFUserAccountRepository(), null, null);
        }

        public bool ValidateUser(string userName, string password)
        {
            return this.userService.Authenticate(userName, password); 
            // TODO: 
            //return userName == password;
        }

        public bool ValidateUser(System.Security.Cryptography.X509Certificates.X509Certificate2 clientCertificate, 
            out string userName)
        {
            return Repository.TryGetUserNameFromThumbprint(clientCertificate, out userName);
        }

        public IEnumerable<string> GetRoles(string userName)
        {
            var user = this.userService.GetByUsername(userName);
            return user.Claims.Where(c => c.Type.Equals(ClaimTypes.Role)).Select(s => s.Value);
        }
    }
}