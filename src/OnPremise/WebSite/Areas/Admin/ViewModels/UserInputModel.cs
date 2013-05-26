using BrockAllen.MembershipReboot;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Thinktecture.IdentityServer.Web.Areas.Admin.ViewModels
{
    public class UserInputModel : IValidatableObject
    {
        [Display(Name = "E-Mail")]
        [EmailAddress]
        [ScaffoldColumn(false)]
        public string Email { get; set; }

        [Display(Name = "Username")]
        [ScaffoldColumn(false)]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [ScaffoldColumn(false)]
        public UserRoleAssignment[] Roles { get; set; }

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