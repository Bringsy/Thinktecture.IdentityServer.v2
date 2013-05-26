using BrockAllen.MembershipReboot;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Thinktecture.IdentityServer.Web.Areas.Account.ViewModels
{
    public class AccountModel : IValidatableObject
    {
        [Display(Name = "E-Mail")]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "User Name")]
        [ScaffoldColumn(false)]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Given Name")]
        public string GivenName { get; set; }

        [Required]
        [Display(Name = "Surname")]
        public string Surname { get; set; }

        [Display(Name = "Roles")]
        public string[] Roles { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!SecuritySettings.Instance.EmailIsUsername &&
                String.IsNullOrWhiteSpace(this.UserName))
            {
                yield return new ValidationResult("Username is required", new string[] { "UserName" });
            }
        }
    }
}