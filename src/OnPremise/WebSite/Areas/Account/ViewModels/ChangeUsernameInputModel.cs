using System.ComponentModel.DataAnnotations;

namespace Thinktecture.IdentityServer.Web.Areas.Account.ViewModels
{
    public class ChangeUsernameInputModel
    {
        [Required]
        [Display(Name = "New Username")]
        public string NewUsername { get; set; }
    }
}