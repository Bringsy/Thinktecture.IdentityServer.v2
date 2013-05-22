using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Thinktecture.IdentityServer.Web.Areas.Account.ViewModels
{
    public class ChangeEmailFromKeyInputModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        public string NewEmail { get; set; }

        [HiddenInput]
        public string Key { get; set; }
    }
}