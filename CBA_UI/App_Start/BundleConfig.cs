using System.Web;
using System.Web.Optimization;

namespace CBA
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/tree.jquery.js",
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            //bundles.Add(new ScriptBundle("~/bundles/adminlteCSS").Include(
            //            "~/Scripts/AdminLTE/AdminLTE.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/adminlteJS").Include(
                "~/Scripts/AdminLTE/jQuery-2.1.4.min.js",
                "~/Scripts/AdminLTE/bootstrap.min.js",
                "~/Scripts/AdminLTE/demo.js",
                "~/Scripts/AdminLTE/fastclick.min.js",
                "~/Scripts/AdminLTE/jquery.slimscroll.min.js",
                "~/Scripts/AdminLTE/app.min.js"
                ));

            //bundles.Add(new ScriptBundle("~/bundles/adminlteJS").IncludeDirectory("~/Scripts/AdminLTE",
            //            "*.js"));

            //bundles.Add(new StyleBundle("~/bundles/adminlteCSS").IncludeDirectory("~/Scripts/AdminLTE",
            //            "*.css"));

            bundles.Add(new StyleBundle("~/bundles/adminlteCSS").Include(
                "~/Scripts/AdminLTE/bootstrap.min.css",
                "~/Scripts/AdminLTE/_all-skins.min.css",
                "~/Scripts/AdminLTE/AdminLTE.min.css"
                ));


            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      //"~/Content/bootstrap.css",
                      "~/Content/site.css"));
        }
    }
}
