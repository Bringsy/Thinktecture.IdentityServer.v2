using BrockAllen.MembershipReboot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Thinktecture.IdentityServer.Web.ViewModels
{
    public abstract class BaseProfileClaimsModel
    {
        [ScaffoldColumn(false)]
        public IEnumerable<UserClaim> UserClaims { get; set; }

        internal void SetClaim(string type, string value)
        {
            this.UserClaims.Where(c => c.Type.Equals(type, StringComparison.InvariantCultureIgnoreCase)).Select(s => s.Value = value);
        }

        internal string GetClaimValue(string type)
        {
            return this.UserClaims.Where(c => c.Type.Equals(type, StringComparison.InvariantCultureIgnoreCase)).Select(s => s.Value).FirstOrDefault();
        }
    }

    public class FooBar : IListSource
    {
        public bool ContainsListCollection
        {
            get { throw new NotImplementedException(); }
        }

        public System.Collections.IList GetList()
        {
            throw new NotImplementedException();
        }
    }

    public class DropDownListAttribute : UIHintAttribute
    {
        string listSourceType; 

        public DropDownListAttribute(string listSourceType)
            : base("DropDownList")
        {
            this.listSourceType = listSourceType; 
        }

        System.Collections.IList GetDataSource()
        {
            return new string[] { "hallo", "welt" }; 
        }
    }

    public class ProfileClaimsModel : BaseProfileClaimsModel
    {
        [Required]
        [Display(Name = "Given Name")]
        public string GivenName
        {
            get { return base.GetClaimValue(System.IdentityModel.Claims.ClaimTypes.GivenName); }
            set { base.SetClaim(System.IdentityModel.Claims.ClaimTypes.GivenName, value); }
        }

        [Required]
        [Display(Name = "Surname")]
        public string Surname
        {
            get { return base.GetClaimValue(System.IdentityModel.Claims.ClaimTypes.Surname); }
            set { base.SetClaim(System.IdentityModel.Claims.ClaimTypes.Surname, value); }
        }

        [Required]
        [Display(Name = "Title")]
        public string Title
        {
            get { return base.GetClaimValue("title"); }
            set { base.SetClaim("Title", value); }
        }
    }
}