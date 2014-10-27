using System.Web;
using System.Web.Optimization;

namespace SocialGoal
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                       "~/Scripts/jquery-2.1.1.js"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate.js",
                        "~/Scripts/jquery.validate.unobtrusive.js",
                         "~/Scripts/jquery.unobtrusive-ajax.js"));


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
               "~/Scripts/jquery-ui.min-1.11.1.js",
               "~/Content/assets/javascripts/plugins/jquery_ui_touch_punch/jquery.ui.touch-punch.min.js",
               "~/Content/assets/javascripts/bootstrap/bootstrap.js",
               "~/Content/assets/javascripts/plugins/modernizr/modernizr.min.js",
               "~/Content/assets/javascripts/plugins/retina/retina.js",
               "~/Scripts/jquery.nicescroll.min.js",//Head和left滚动
               "~/Content/assets/javascripts/plugins/slimscroll/jquery.slimscroll.min.js",//局部滚动，可能与angular冲突
               "~/Scripts/angular.js",
               "~/Content/assets/javascripts/theme.js"
               ));

            //select2
            bundles.Add(new ScriptBundle("~/bundles/select2/js").Include(
                      "~/Scripts/plugins/select2-3.5.1/select2.js",
                      "~/Scripts/plugins/select2-3.5.1/select2_locale_zh-CN.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/select2/css").Include(
                   "~/Scripts/plugins/select2-3.5.1/select2.css",
                    "~/Scripts/plugins/select2-3.5.1/select2-bootstrap.css"
                    ));

            //ztree

            bundles.Add(new ScriptBundle("~/bundles/ztree/js").Include(
                       "~/Content/assets/javascripts/plugins/ztree/jquery.ztree.all-3.5.js"));

            bundles.Add(new StyleBundle("~/Content/ztrtee/css").Include(
                   "~/Content/assets/stylesheets/plugins/zTreeStyle/zTreeStyle.css"
                    ));

            //SmartAdmin
            bundles.Add(new StyleBundle("~/Content/SmartAdmin/css").Include(
                      "~/Content/SmartAdmin/css/bootstrap.min.css",
                      "~/Content/SmartAdmin/css/font-awesome.min.css",
                      "~/Content/SmartAdmin/css/smartadmin-production.min.css",
                      "~/Content/SmartAdmin/css/smartadmin-skins.min.css",
                      "~/Content/SmartAdmin/css/your_style.css",
                      "~/Content/SmartAdmin/css/demo.min.css"));
            bundles.Add(new ScriptBundle("~/bundles/SmartAdmin/jquery").Include(
                  "~/Content/SmartAdmin/js/libs/jquery-2.0.2.min.js",
                  "~/Content/SmartAdmin/js/libs/jquery-ui-1.10.3.min.js"
                   ));

            bundles.Add(new ScriptBundle("~/bundles/SmartAdmin/js").Include(
                //IMPORTANT: APP CONFIG
                  "~/Content/SmartAdmin/js/app.config.js",
                //JS TOUCH : include this plugin for mobile drag / drop touch events
                  "~/Content/SmartAdmin/js/plugin/jquery-touch/jquery.ui.touch-punch.min.js",
                   "~/Content/SmartAdmin/js/bootstrap/bootstrap.min.js",
                //CUSTOM NOTIFICATION
                  "~/Content/SmartAdmin/js/notification/SmartNotification.min.js",
                //JARVIS WIDGETS
                  "~/Content/SmartAdmin/js/smartwidgets/jarvis.widget.min.js",
                //EASY PIE CHARTS 
                  "~/Content/SmartAdmin/js/plugin/easy-pie-chart/jquery.easy-pie-chart.min.js",
                //SPARKLINES
                  "~/Content/SmartAdmin/js/plugin/sparkline/jquery.sparkline.min.js",
                //JQUERY MASKED INPUT
                  "~/Content/SmartAdmin/js/plugin/masked-input/jquery.maskedinput.min.js",
                  "~/Content/SmartAdmin/js/plugin/select2/select2.min.js",
                //JQUERY UI + Bootstrap Slider
                  "~/Content/SmartAdmin/js/plugin/bootstrap-slider/bootstrap-slider.min.js",
                // browser msie issue fix 
                  "~/Content/SmartAdmin/js/plugin/msie-fix/jquery.mb.browser.min.js",
                //FastClick: For mobile devices: you can disable this in app.js
                  "~/Content/SmartAdmin/js/plugin/fastclick/fastclick.min.js",
                  "~/Content/SmartAdmin/js/demo.min.js",
                  "~/Content/SmartAdmin/js/appDes.js",
                  "~/Content/SmartAdmin/js/speech/voicecommand.min.js"
                    ));
        }
    }
}
