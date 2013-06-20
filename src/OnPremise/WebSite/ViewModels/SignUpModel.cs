using BrockAllen.MembershipReboot;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Thinktecture.IdentityServer.Web.ViewModels
{
    public class SignUpModel : IValidatableObject
    {
        [Required]
        [Display(Name = "E-Mail")]
        [EmailAddress]
        public string Email { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password Repeat")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Password confirmation must match password.")]
        public string PasswordRepeat { get; set; }

        public string ReturnUrl { get; set; }

        //[Required]
        [Display(Name = "Given Name")]
        public string GivenName { get; set; }

        // [Required]
        [Display(Name = "Surname")]
        public string Surname { get; set; }

        bool? isSigninRequest;
        public bool IsSigninRequest
        {
            get
            {

                if (isSigninRequest == null)
                {
                    isSigninRequest = !String.IsNullOrWhiteSpace(ReturnUrl);
                }
                return isSigninRequest.Value;
            }
            set
            {
                isSigninRequest = value;
            }
        }

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