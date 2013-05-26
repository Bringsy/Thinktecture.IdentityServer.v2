using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Thinktecture.IdentityServer.Web.Areas.Account.ViewModels
{
    public class ChangeEmailFromKeyInputModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "New E-Mail")]
        public string NewEmail { get; set; }

        [HiddenInput]
        public string Key { get; set; }
    }
}