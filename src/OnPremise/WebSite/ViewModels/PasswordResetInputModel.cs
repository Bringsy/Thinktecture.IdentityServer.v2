using System.ComponentModel.DataAnnotations;

namespace Thinktecture.IdentityServer.Web.ViewModels
{
    public class PasswordResetInputModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}