using System.ComponentModel.DataAnnotations;

namespace Thinktecture.IdentityServer.Web.Areas.Account.ViewModels
{
    public class ChangeEmailRequestInputModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "New E-Mail")]
        public string NewEmail { get; set; }
    }
}