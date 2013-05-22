using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web.Mvc;
using Thinktecture.IdentityModel.Authorization.Mvc;
using Thinktecture.IdentityServer.Repositories;
using Thinktecture.IdentityServer.Web.Areas.Admin.ViewModels;

namespace Thinktecture.IdentityServer.Web.Areas.Account.Controllers
{
    public class MenuController : Controller
    {
        [ChildActionOnly]
        public ActionResult Index()
        {
            return PartialView("Index");
        }

        bool IsController(string controller)
        {
            ControllerContext ctx = this.ControllerContext;
            while (ctx.ParentActionViewContext != null)
            {
                ctx = ctx.ParentActionViewContext;
            }
            var val = (string)ctx.RouteData.Values["controller"];
            return controller.Equals(val, StringComparison.OrdinalIgnoreCase);
        }
        
        bool IsAction(params string[] actions)
        {
            ControllerContext ctx = this.ControllerContext;
            while (ctx.ParentActionViewContext != null)
            {
                ctx = ctx.ParentActionViewContext;
            }
            var action = (string)ctx.RouteData.Values["action"];
            return actions.Contains(action, StringComparer.OrdinalIgnoreCase);
        }

        [ChildActionOnly]
        public ActionResult ChildMenu(string controllerName)
        {
            if (IsController(controllerName))
            {
                return PartialView("LoadChildMenu", controllerName);
            }

            return new EmptyResult();
        }
    }
}
