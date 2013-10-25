/*
 * Copyright (c) Russlan Akiev.  All rights reserved.
 */

using System;
using System.Web;
using System.Web.Mvc;

namespace Thinktecture.IdentityServer.Web.GlobalFilter
{
    public class DetectRpFilter : ActionFilterAttribute
    {
        //[Import]
        //public IRelyingPartyRepository RelyingPartyRepository { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //Container.Current.SatisfyImportsOnce(this);
            var wtrealm = HttpContext.Current.Request.QueryString["wtrealm"];

            if (!string.IsNullOrEmpty(wtrealm))
                HttpContext.Current.Response.Cookies.Add(new HttpCookie("idsrvlr", wtrealm)
                {
                    Expires = DateTime.Now.AddDays(1)
                });

            base.OnActionExecuting(filterContext);
        }
    }
}