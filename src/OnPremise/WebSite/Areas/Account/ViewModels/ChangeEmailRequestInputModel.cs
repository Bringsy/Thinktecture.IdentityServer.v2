using System.ComponentModel.DataAnnotations;

namespace Thinktecture.IdentityServer.Web.Areas.Account.ViewModels
{
    public class ChangeEmailRequestInputModel
    {
        [Required]
        [EmailAddress]
        public string NewEmail { get; set; }
    }
}