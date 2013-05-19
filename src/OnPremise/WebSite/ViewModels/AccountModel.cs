using BrockAllen.MembershipReboot;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Thinktecture.IdentityServer.Web.ViewModels
{
    public class AccountModel : IValidatableObject
    {
        [Required]
        [Display(Name = "E-Mail")]
        [EmailAddress]
        public string Email { get; set; }

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

        // TODO: add more here 

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