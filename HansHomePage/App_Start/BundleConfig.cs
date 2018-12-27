using System.Web;
using System.Web.Optimization;

namespace HansHomePage
{
    public class BundleConfig
    {
        // 묶음에 대한 자세한 내용은 https://go.microsoft.com/fwlink/?LinkId=301862를 참조하세요.
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/hanshome/css").Include(
                   "~/Template/assets/vendor/bootstrap/css/bootstrap.min.css",
                   "~/Template/assets/vendor/fontawesome/css/font-awesome.min.css",
                   "~/Template/assets/vendor/flaticons/flaticon.css",
                   "~/Template/assets/vendor/hover/css/hover-min.css",
                   "~/Template/assets/vendor/wow/animate.css",
                   "~/Template/assets/vendor/mfp/css/magnific-popup.css",
                   "~/Template/assets/vendor/footable/footable.core.css",
                   "~/Template/assets/custom/css/style.css"));

            bundles.Add(new ScriptBundle("~/hanshome/js").Include(
                     "~/Template/assets/vendor/jquery/js/jquery-2.2.0.min.js",
                     "~/Template/assets/vendor/bootstrap/js/bootstrap.min.js",
                     "~/Template/assets/vendor/imagesloaded/js/imagesloaded.pkgd.min.js",
                     "~/Template/assets/vendor/isotope/js/isotope.pkgd.min.js",
                     "~/Template/assets/vendor/mfp/js/jquery.magnific-popup.min.js",
                     "~/Template/assets/vendor/circle-progress/circle-progress.js",
                     "~/Template/assets/vendor/waypoints/waypoints.min.js",
                     "~/Template/assets/vendor/anicounter/jquery.counterup.min.js",
                     "~/Template/assets/vendor/wow/wow.min.js",
                     "~/Template/assets/vendor/pjax/jquery.pjax.js",
                     "~/Template/assets/vendor/footable/footable.all.min.js",
                     "~/Template/assets/custom/js/custom.js"
                 ));
        }
    }
}
