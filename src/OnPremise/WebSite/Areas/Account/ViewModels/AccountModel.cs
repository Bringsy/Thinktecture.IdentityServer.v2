using BrockAllen.MembershipReboot;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Thinktecture.IdentityServer.Web.Areas.Account.ViewModels
{
    public class AccountModel 
    {
        [Display(Name = "E-Mail")]
        [EmailAddress]
        [ScaffoldColumn(false)]
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

       
    }
}