using System.Web;
using System.Web.Optimization;

namespace StretchGarageWeb
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new Bundle("~/bundles/garage").Include(
                        "~/Scripts/router.js",
                        "~/Scripts/services.js",
                        "~/Scripts/garage.js"));

            bundles.Add(new Bundle("~/bundles/angular").Include(
                        "~/Scripts/angular-min.js",
                        "~/Scripts/ui-bootstrap-tpls-0.13.0.min.js",
                        "~/Scripts/angular-route.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/Site.css"));
        }

    }
}
