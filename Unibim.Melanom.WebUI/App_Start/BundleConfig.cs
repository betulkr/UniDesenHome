using System.Web;
using System.Web.Optimization;// Manage Nuget kısmından Microsoft.AspNet.Web.Optimization eklenmelidir.

// global.asax içerisine -- BundleConfig.RegisterBundles(BundleTable.Bundles); -- eklenmelidir.

namespace Unibim.Melanom.WebUI
{
    public class BundleConfig
    {

        public static void RegisterBundles(BundleCollection bundles)
        {

            #region Ayar
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/jquery.loadvalidate.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));
            #endregion

            #region Css
            // farklı klasörlerde olduğundan dolayı include yaptık
            bundles.Add(new StyleBundle("~/Content/Assets/global/plugins/globalMandatory").
                Include("~/Content/Assets/global/plugins/font-awesome/css/font-awesome.min.css", new CssRewriteUrlTransform()).
                Include("~/Content/Assets/global/plugins/simple-line-icons/simple-line-icons.min.css", new CssRewriteUrlTransform()).
                Include("~/Content/Assets/global/plugins/bootstrap/css/bootstrap.min.css", new CssRewriteUrlTransform()).
                Include("~/Content/themes/base/jquery.ui.dialog.css", new CssRewriteUrlTransform()).
                Include("~/Content/Assets/global/plugins/bootstrap-fileinput/bootstrap-fileinput.css", new CssRewriteUrlTransform()).
                Include("~/Content/Assets/global/css/Karisik.css", new CssRewriteUrlTransform()).
                Include("~/Content/Assets/global/plugins/bootstrap-switch/css/bootstrap-switch.min.css", new CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/content/Assets/global/css/themeGlobal").
                Include("~/Content/Assets/global/plugins/select2/css/select2.min.css", new CssRewriteUrlTransform()).
                Include("~/Content/Asssets/global/plugins/select2/css/select2-bootstrap.min.css", new CssRewriteUrlTransform()).
                Include("~/Content/Assets/global/css/components.min.css", new CssRewriteUrlTransform()).
                Include("~/Content/themes/metronic/assets/global/css/plugins.min.css", new CssRewriteUrlTransform()).
                Include("~/Content/Assets/global/plugins/jstree/dist/themes/default/style.min.css", new CssRewriteUrlTransform()).
                Include("~/Content/themes/metronic/assets/global/css/components-rounded.min.css", new CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/content/Assets/global/plugins/themeLayout")
                .Include("~/Content/themes/aquincum/aquincum.ui_custom.css", new CssRewriteUrlTransform())
                .Include("~/Content/themes/aquincum/aquincum.plugins.css", new CssRewriteUrlTransform())
                .Include("~/Content/themes/metronic/assets/layouts/layout4/css/layout.min.css", new CssRewriteUrlTransform())
                .Include("~/Content/themes/metronic/assets/layouts/layout4/css/themes/default.min.css", new CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/Content/Assets/global/plugins/toaster")
               .Include("~/Content/toastr.min.css", new CssRewriteUrlTransform()));
            #endregion

            #region js
            bundles.Add(new ScriptBundle("~/bundles/strategia-aquincum")
                        .Include("~/Scripts/jquery-1.8.2.js")

                        .Include("~/Content/themes/metronic/assets/global/plugins/jquery-migrate.min.js")
                        .Include("~/Scripts/jquery-ui-1.8.24.js")
                        .Include("~/Scripts/jquery.ui.spinner.js")


                        .Include("~/Scripts/plugins/forms/autogrowtextarea.js")
                        .Include("~/Scripts/plugins/forms/jquery.uniform.js")
                        .Include("~/Scripts/plugins/forms/jquery.inputlimiter.min.js")
                        .Include("~/Scripts/plugins/forms/jquery.tagsinput.min.js")
                        .Include("~/Scripts/plugins/forms/jquery.maskedinput.min.js")
                        .Include("~/Scripts/plugins/forms/jquery.autotab.js")
                        .Include("~/Scripts/plugins/forms/jquery.chosen.min.js")
                        .Include("~/Scripts/plugins/forms/jquery.dualListBox.js")
                        .Include("~/Scripts/plugins/forms/jquery.cleditor.js")
                        .Include("~/Scripts/plugins/forms/jquery.ibutton.js")

                        .Include("~/Scripts/plugins/tables/jquery.dataTables.js")

                        .Include("~/Scripts/plugins/ui/jquery.collapsible.min.js")
                        .Include("~/Scripts/plugins/ui/jquery.breadcrumbs.js")
                        .Include("~/Scripts/plugins/ui/jquery.tipsy.js")
                        .Include("~/Scripts/plugins/ui/jquery.progress.js")
                        .Include("~/Scripts/plugins/ui/jquery.timeentry.min.js")
                        .Include("~/Scripts/plugins/ui/jquery.colorpicker.js")
                        .Include("~/Scripts/plugins/ui/jquery.jgrowl.js")
                        .Include("~/Scripts/plugins/ui/jquery.fancybox.js")
                        .Include("~/Scripts/plugins/ui/jquery.fileTree.js")
                        .Include("~/Scripts/plugins/ui/jquery.easytabs.min.js")

                        .Include("~/Scripts/plugins/others/jquery.elfinder.js")

                        //.Include("~/Scripts/files/functions.js")
                        .Include("~/Scripts/jquery.qtip.js")
                        );


            bundles.Add(new ScriptBundle("~/bundles/strategia-cims")
                       .Include("~/Scripts/strategia/strategia.cims.js")
                       .Include("~/Scripts/strategia/strategia.cims.datatable.js")
                       .Include("~/Scripts/strategia/strategia.cims.dialog.js")
                       );

            bundles.Add(new ScriptBundle("~/bundles/metronic")
                      .Include("~/Content/themes/metronic/assets/global/plugins/bootstrap/js/bootstrap.min.js")
                       .Include("~/Content/Assets/global/plugins/select2/js/select2.full.min.js")
                      .Include("~/Content/assets/global/plugins/bootstrap-fileinput/bootstrap-fileinput.js")
                      .Include("~/Content/themes/metronic/assets/global/scripts/app.min.js")
                      .Include("~/Content/Assets/pages/scripts/components-select2.min.js")
                      .Include("~/Content/themes/metronic/assets/layouts/layout4/scripts/layout.min.js")
                      );

            bundles.Add(new ScriptBundle("~/bundles/toaster")
                      .Include("~/Scripts/toastr.min.js")
                      .Include("~/Scripts/toastr.config.js")
                      );

            bundles.Add(new ScriptBundle("~/bundles/beginCorePlugins")
                //.Include("~/Content/themes/metronic/assets/global/plugins/jquery.min.js")
                .Include("~/Content/themes/metronic/assets/global/plugins/jquery-migrate.min.js")
                //.Include("~/Content/assets/global/plugins/jquery.min.js")
                //.Include("~/Scripts/jquery-ui-1.8.24.js")
                .Include("~/Scripts/jquery.ui.spinner.js")

                //.Include("~/Content/assets/global/plugins/bootstrap/js/bootstrap.min.js")
                .Include("~/Content/assets/global/plugins/js.cookie.min.js")
                .Include("~/Content/assets/global/plugins/jquery-slimscroll/jquery.slimscroll.min.js")
                .Include("~/Content/assets/global/plugins/jquery.blockui.min.js")


                .Include("~/Content/assets/global/plugins/bootstrap-switch/js/bootstrap-switch.min.js")
                           .Include("~/Content/Assets/global/plugins/select2/js/select2.full.min.js")
                .Include("~/Content/assets/global/plugins/bootstrap-tabdrop/js/bootstrap-tabdrop.js"));

            bundles.Add(new ScriptBundle("~/bundles/beginThemeGlobal")

                      .Include("~/Content/assets/global/scripts/app.min.js")
                      );

            bundles.Add(new ScriptBundle("~/bundles/beginThemeLayout")
                      .Include("~/Content/assets/layouts/layout/scripts/layout.min.js")
                      .Include("~/Content/assets/layouts/layout/scripts/demo.min.js")
                      .Include("~/Content/assets/layouts/global/scripts/quick-sidebar.min.js")
                      .Include("~/Content/assets/layouts/global/scripts/quick-nav.min.js")
            );



            bundles.Add(new ScriptBundle("~/bundles/strategia-cims")
                    .Include("~/Scripts/strategia/strategia.cims.js")
                    .Include("~/Scripts/strategia/strategia.cims.datatable.js")
                    .Include("~/Scripts/strategia/strategia.cims.dialog.js")
                    );


            #endregion

            BundleTable.EnableOptimizations = false;
        }
    }
}