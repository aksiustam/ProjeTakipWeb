using System.Web;
using System.Web.Optimization;

namespace ProjeTakip
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
           
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));
            /*
           // Use the development version of Modernizr to develop with and learn from. Then, when you're
           // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
           bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                       "~/Scripts/modernizr-*"));

           bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                     "~/Scripts/bootstrap.js",
                     "~/Scripts/respond.js"));
           */
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/vendor/fontawesome-free/css/all.min.css",
                      "~/Content/sb-admin-2.min.css",
                      "~/Content/config.css"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/vendor/jquery/jquery.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/vendor").Include(
                      "~/vendor/bootstrap/js/bootstrap.bundle.min.js",
                      "~/vendor/jquery-easing/jquery.easing.min.js",
                      "~/js/sb-admin-2.min.js",
                      "~/vendor/sweetalert/sweetalert2.all.min.js"));

            bundles.Add(new StyleBundle("~/Content/datatable").Include(
                      "~/vendor/datatables/dataTables.bootstrap4.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/datatable").Include(
                     "~/vendor/datatables/jquery.dataTables.min.js",
                     "~/vendor/datatables/dataTables.bootstrap4.min.js",
                     "~/vendor/datatables/dataTables.buttons.min.js",
                     "~/vendor/datatables/buttons.flash.min.js",
                     "~/vendor/datatables/buttons.html5.min.js",
                     "~/vendor/datatables/buttons.print.min.js",
                     "~/vendor/datatables/jszip.min.js",
                     "~/vendor/datatables/pdfmake.min.js",
                     "~/vendor/datatables/vfs_fonts.js"));

        }
    }
}
