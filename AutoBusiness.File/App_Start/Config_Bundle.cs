using System.Web;
using System.Web.Optimization;

namespace AutoBusiness.File
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery.fileupload").Include(
                                         "~/Scripts/jquery.fileupload/vendor/jquery.ui.widget.js",
                                         "~/Scripts/jquery.fileupload/load-image.js",
                                         "~/Scripts/jquery.fileupload/load-image-orientation.js",
                                         "~/Scripts/jquery.fileupload/load-image-meta.js",
                                         "~/Scripts/jquery.fileupload/load-image-exif.js",
                                         "~/Scripts/jquery.fileupload/load-image-exif-map.js",
                                         "~/Scripts/jquery.fileupload/canvas-to-blob.js",
                                         "~/Scripts/jquery.fileupload/jquery.iframe-transport.js",
                                         "~/Scripts/jquery.fileupload/jquery.fileupload.js",
                                         "~/Scripts/jquery.fileupload/jquery.fileupload-process.js",
                                         "~/Scripts/jquery.fileupload/jquery.fileupload-image.js",
                                         "~/Scripts/jquery.fileupload/jquery.fileupload-validate.js"
                                         ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                                        "~/Content/bootstrap.css",
                                        "~/Content/site.css",
                                        "~/Content/jquery.fileupload.css"
                                       ));
        }
    }
}
