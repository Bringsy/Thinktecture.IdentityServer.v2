using System.ComponentModel.DataAnnotations;

namespace Thinktecture.IdentityServer.Web.ViewModels
{
    public class ChangeEmailRequestInputModel
    {
        [Required]
        [EmailAddress]
        public string NewEmail { get; set; }
    }
}