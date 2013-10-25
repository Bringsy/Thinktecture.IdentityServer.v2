using System.Web.Optimization;
using Web.Optimization.Bundles.YUI;

namespace Thinktecture.IdentityServer.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.IgnoreList.Clear();

            // bringsy main package 
            bundles.Add(new YuiScriptBundle("~/content/scripts").Include(
                        "~/Scripts/foundation.js",
                        "~/Scripts/foundation.alerts.js",
                        "~/Scripts/foundation.clearing.js",
                        "~/Scripts/foundation.cookie.js",
                        //"~/Scripts/foundation/foundation.dropdown.js",
                        "~/Scripts/foundation.forms.js",
                        //"~/Scripts/foundation/foundation.joyride.js",
                        //"~/Scripts/foundation/foundation.magellan.js",
                        //"~/Scripts/foundation/foundation.orbit.js",
                        //"~/Scripts/foundation/foundation.placeholder.js",
                        //"~/Scripts/foundation/foundation.reveal.js",
                        //"~/Scripts/foundation/foundation.section.js",
                        //"~/Scripts/foundation/foundation.tooltips.js",
                        "~/Scripts/foundation.topbar.js",
                        "~/Scripts/jquery.validate.js",
                        "~/Scripts/jquery.validate.unobtrusive.js"));

            bundles.Add(new YuiStyleBundle("~/content/styles").Include(
                       "~/Content/main.css",
                       "~/Content/font-awesome.min.css"));
        }
    }
}