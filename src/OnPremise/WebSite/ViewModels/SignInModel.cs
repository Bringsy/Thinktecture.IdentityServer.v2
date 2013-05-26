/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * see license.txt
 */

using BrockAllen.MembershipReboot;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Thinktecture.IdentityServer.Web.ViewModels
{
    public class SignInModel : IValidatableObject
    {
        [Display(Name = "Email")]
        [EmailAddress]
        [ScaffoldColumn(false)]
        public string Email { get; set; }

        [Display(Name = "Username")]
        [ScaffoldColumn(false)]
        public string UserName { get; set; }

        /*
        [Display(Name = "Username or E-Mail")]
        [ScaffoldColumn(false)]
        public string Credential { get; set; }
        */

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(Resources.SignInModel))]
        public string Password { get; set; }

        [Display(Name = "EnableSSO", ResourceType = typeof(Resources.SignInModel))]
        public bool EnableSSO { get; set; }

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
        public string ReturnUrl { get; set; }
        public bool ShowClientCertificateLink { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!SecuritySettings.Instance.EmailIsUsername &&
                String.IsNullOrWhiteSpace(this.UserName))
            {
                yield return new ValidationResult("Username is required", new string[] { "UserName" });
            }
            else
            {

            }
        }
    }
}