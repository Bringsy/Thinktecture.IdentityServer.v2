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
                        "~/Scripts/foundation/foundation.js",

                        "~/Scripts/foundation/foundation.alerts.js",
                        "~/Scripts/foundation/foundation.clearing.js",
                        "~/Scripts/foundation/foundation.cookie.js",
                //"~/Scripts/foundation/foundation.dropdown.js",
                        "~/Scripts/foundation/foundation.forms.js",
                //"~/Scripts/foundation/foundation.joyride.js",
                //"~/Scripts/foundation/foundation.magellan.js",
                //"~/Scripts/foundation/foundation.orbit.js",
                //"~/Scripts/foundation/foundation.placeholder.js",
                //"~/Scripts/foundation/foundation.reveal.js",
                //"~/Scripts/foundation/foundation.section.js",
                //"~/Scripts/foundation/foundation.tooltips.js",
                        "~/Scripts/foundation/foundation.topbar.js",

                        "~/Scripts/jquery.validate.js",
                        "~/Scripts/jquery.validate.unobtrusive.js",

                        "~/Scripts/Bringsy/bringsy.js",
                        "~/Scripts/Bringsy/bingRepository.js",
                        "~/Scripts/lazyload.js"));

            bundles.Add(new YuiStyleBundle("~/content/styles").Include(
                   "~/Content/main.css",
                   "~/Content/font-awesome.min.css"));
        }
    }
}