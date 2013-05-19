using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Thinktecture.IdentityServer.Web.ViewModels
{
    public class ResetPasswordModel
    {
        [Required]
        [Display(Name = "E-Mail")]
        [EmailAddress]
        public string Email { get; set; }

    }
}