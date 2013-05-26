using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Thinktecture.IdentityServer.Web.Areas.Account.ViewModels
{
    public class ChangePasswordFromResetKeyInputModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        
        [Required]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Password confirmation must match password.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string ConfirmPassword { get; set; }
        
        [HiddenInput]
        public string Key { get; set; }
    }
}