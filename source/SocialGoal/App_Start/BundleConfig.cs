using System.Web;
using System.Web.Optimization;

namespace SocialGoal
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/Scripts/HomeLayout").Include(
                    "~/Scripts/jquery-1.7.2.min.js",
                    "~/Scripts/jquery-ui-1.8.21.custom.min.js",
                    "~/Scripts/jquery.validate.min.js",
                    "~/Scripts/jquery.unobtrusive-ajax.js",
                    "~/Scripts/jquery.pjax.js",
                    "~/Scripts/bootstrap-transition.js",
                    "~/Scripts/bootstrap-popover.js",
                    "~/Scripts/bootstrap-alert.js",
                    "~/Scripts/bootstrap-modal.js",
                    "~/Scripts/bootstrap-dropdown.js",
                    "~/Scripts/bootstrap-scrollspy.js",
                    "~/Scripts/bootstrap-collapse.js",
                    "~/Scripts/jquery.autocomplete.js",
                    "~/Scripts/jquery-ui-1.8.11.min.js",
                    "~/Scripts/jqDnR.js",
                    "~/Scripts/jqModal.js",
                    "~/Scripts/jqplot.barRenderer.min.js",
                    "~/Scripts/jqplot.dateAxisRenderer.min.js",
                    "~/Scripts/jqplot.categoryAxisRenderer.min.js",
                    "~/Scripts/jqplot.pointLabels.min.js",
                    "~/Scripts/jqplot.canvasTextRenderer.min.js",
                    "~/Scripts/jqplot.canvasAxisTickRenderer.min.js",
                    "~/Scripts/jqplot.canvasAxisTickRenderer.min.js",
                    "~/Scripts/jqplot.ohlcRenderer.min.js",
                    "~/Scripts/jqplot.highlighter.min.js",
                    "~/Scripts/handlebars-1.0.rc.1.js",
                    "~/Scripts/jquery.nicescroll.min.js"
                    ));

            bundles.Add(new ScriptBundle("~/Scripts/Layout").Include(
                   "~/Scripts/jquery-1.7.2.min.js",
                   "~/Scripts/jquery-ui-1.8.21.custom.min.js",
                   "~/Scripts/jquery.validate.min.js",
                   "~/Scripts/jquery.unobtrusive-ajax.js",
                   "~/Scripts/jquery.pjax.js",
                   "~/Scripts/bootstrap-transition.js",
                   "~/Scripts/bootstrap-alert.js",
                   "~/Scripts/bootstrap-modal.js",
                   "~/Scripts/bootstrap-dropdown.js",
                   "~/Scripts/bootstrap-scrollspy.js",
                   "~/Scripts/bootstrap-collapse.js",
                   "~/Scripts/jquery.autocomplete.js",
                   "~/Scripts/jquery-ui-1.8.11.min.js",
                   "~/Scripts/jqDnR.js",
                   "~/Scripts/jqModal.js",
                   "~/Scripts/handlebars-1.0.rc.1.js",
                   "~/Scripts/jquery.nicescroll.min.js"
                   ));



            bundles.Add(new ScriptBundle("~/Scripts/PageLayout").Include(
                      "~/Scripts/jquery-1.7.2.min.js",
                      "~/Scripts/bootstrap-popover.js",
                      "~/Scripts/bootstrap-transition.js",
                      "~/Scripts/bootstrap-alert.js",
                      "~/Scripts/bootstrap-modal.js",
                      "~/Scripts/bootstrap-dropdown.js",
                      "~/Scripts/bootstrap-scrollspy.js",
                      "~/Scripts/bootstrap-collapse.js",
                      "~/Scripts/jquery.autocomplete.js",
                      "~/Scripts/jquery-ui-1.8.21.custom.min.js",
                      "~/Scripts/jquery.unobtrusive-ajax.js",
                      "~/Scripts/jquery.pjax.js",
                      "~/Scripts/jquery-ui-1.8.11.min.js",
                      "~/Scripts/jqDnR.js",
                      "~/Scripts/jqModal.js",
                      "~/Scripts/jquery.jqplot.min.js",
                      "~/Scripts/jqplot.barRenderer.min.js",
                      "~/Scripts/jqplot.dateAxisRenderer.min.js",
                      "~/Scripts/jqplot.categoryAxisRenderer.min.js",
                      "~/Scripts/jqplot.pointLabels.min.js",
                      "~/Scripts/jqplot.canvasTextRenderer.min.js",
                      "~/Scripts/jqplot.canvasAxisTickRenderer.min.js",
                      "~/Scripts/jqplot.ohlcRenderer.min.js",
                      "~/Scripts/jqplot.highlighter.min.js",
                      "~/Scripts/jquery.nicescroll.min.js",
                      "~/Scripts/handlebars-1.0.rc.1.js"
                      ));

            bundles.Add(new ScriptBundle("~/Scripts/GoalLayout").Include(
                     "~/Scripts/jquery-1.7.2.min.js",
                     "~/Scripts/bootstrap-transition.js",
                     "~/Scripts/bootstrap-popover.js",
                     "~/Scripts/bootstrap-alert.js",
                     "~/Scripts/bootstrap-modal.js",
                     "~/Scripts/bootstrap-dropdown.js",
                     "~/Scripts/bootstrap-scrollspy.js",
                     "~/Scripts/bootstrap-collapse.js",
                     "~/Scripts/jquery.autocomplete.js",
                     "~/Scripts/jquery-ui-1.8.21.custom.min.js",
                     "~/Scripts/jquery.unobtrusive-ajax.js",
                     "~/Scripts/jquery.pjax.js",
                     "~/Scripts/jquery-ui-1.8.11.min.js",
                     "~/Scripts/jqDnR.js",
                     "~/Scripts/jqModal.js",
                     "~/Scripts/jquery.jqplot.min.js",
                     "~/Scripts/jqplot.barRenderer.min.js",
                    "~/Scripts/jqplot.dateAxisRenderer.min.js",
                    "~/Scripts/jqplot.categoryAxisRenderer.min.js",
                    "~/Scripts/jqplot.pointLabels.min.js",
                    "~/Scripts/jqplot.canvasTextRenderer.min.js",
                    "~/Scripts/jqplot.canvasAxisTickRenderer.min.js",
                    "~/Scripts/jqplot.ohlcRenderer.min.js",
                    "~/Scripts/jqplot.highlighter.min.js",
                    "~/Scripts/jquery.nicescroll.min.js",
                    "~/Scripts/handlebars-1.0.rc.1.js"
                ));


            bundles.Add(new StyleBundle("~/Content/CSS").Include(
                    "~/Content/bootstrap.css",
                     "~/Content/bootstrap-responsive.css",
                     "~/Content/HomePage.css",
                     "~/Content/jquery-ui-1.8.21.custom.css",
                     "~/Content/jqModal.css",
                     "~/Content/jquery.jqplot.min.css"
                     ));


            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-2.1.1.js"));
            //datatables
            bundles.Add(new ScriptBundle("~/bundles/datatables").Include(
                "~/Scripts/DataTables-1.10.2/jquery.dataTables.js"));

            bundles.Add(new StyleBundle("~/Content/datatables/css").Include("~/Content/DataTables-1.10.2/css/jquery.dataTables.css"));

            bundles.Add(new StyleBundle("~/Content/Flatty/Css").Include(
                "~/Content/assets/stylesheets/bootstrap/bootstrap.css",
                "~/Content/assets/stylesheets/light-theme.css",
                "~/Content/assets/stylesheets/theme-colors.css",
                "~/Content/assets/stylesheets/demo.css"
                ));

            bundles.Add(new ScriptBundle("~/Content/Flatty/Js").Include(
               "~/Scripts/jquery-2.1.1.js",
               "~/Content/assets/javascripts/jquery/jquery.mobile.custom.min.js",
               "~/Content/assets/javascripts/jquery/jquery-migrate.min.js",
               "~/Scripts/jquery-ui-1.8.20.js",
               "~/Content/assets/javascripts/plugins/jquery_ui_touch_punch/jquery.ui.touch-punch.min.js",
               "~/Content/assets/javascripts/bootstrap/bootstrap.js",
               "~/Content/assets/javascripts/plugins/modernizr/modernizr.min.js",
               "~/Content/assets/javascripts/plugins/retina/retina.js",
                "~/Content/assets/javascripts/theme.js"
               ));
        }
    }
}
