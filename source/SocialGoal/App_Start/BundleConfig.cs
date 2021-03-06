﻿using System.Web.Optimization;

namespace SocialGoal
{

    // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
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
            //ztree
            bundles.Add(new ScriptBundle("~/bundles/ztree/js").Include(
                       "~/Scripts/ztree/jquery.ztree.all-3.5.js"));

            bundles.Add(new StyleBundle("~/Content/ztrtee/css").Include(
                  "~/Content/css/zTreeStyle/zTreeStyle.css"
                    ));

            ////正式启用部分E:\GitHub\ManageSoftV1\source\SocialGoal\Scripts\artDialog\css\ui-dialog.css
            bundles.Add(new StyleBundle("~/content/smartadmin").Include(

                                 "~/Content/css/bootstrap.min.css",
                                 "~/Content/css/demo.min.css",
                                 "~/Content/css/font-awesome.min.css",
                                 "~/Content/css/invoice.min.css",
                                 "~/Content/css/lockscreen.min.css",
                                 "~/Content/css/smartadmin-production-plugins.min.css",
                                 "~/Content/css/smartadmin-production.css",
                                 "~/Content/css/smartadmin-rtl.backup.min.css",
                                 "~/Content/css/smartadmin-rtl.min.css",
                                 "~/Content/css/smartadmin-skins.min.css",
                                 "~/Content/css/your_style.css",
                                 "~/Scripts/artDialog/css/ui-dialog.css"
                                 ));

            bundles.Add(new ScriptBundle("~/scripts/smartadmin").Include(
                "~/scripts/app.config.js",
                "~/scripts/plugin/jquery-touch/jquery.ui.touch-punch.min.js",
                "~/scripts/bootstrap/bootstrap.min.js",
                "~/scripts/notification/SmartNotification.min.js",
                "~/scripts/smartwidgets/jarvis.widget.min.js",
                "~/scripts/plugin/jquery-validate/jquery.validate.min.js",
                "~/scripts/plugin/masked-input/jquery.maskedinput.min.js",
                "~/scripts/plugin/select2/select2.min.js",
                "~/scripts/plugin/bootstrap-slider/bootstrap-slider.min.js",
                "~/scripts/plugin/bootstrap-progressbar/bootstrap-progressbar.min.js",
                "~/scripts/plugin/msie-fix/jquery.mb.browser.min.js",
                "~/scripts/plugin/fastclick/fastclick.min.js",
                "~/scripts/app.js",
                 "~/Scripts/artDialog/dist/dialog-plus.js"));

            bundles.Add(new ScriptBundle("~/scripts/full-calendar").Include(
                "~/scripts/plugin/moment/moment.min.js",
                "~/scripts/plugin/fullcalendar/jquery.fullcalendar.min.js"
                ));

            bundles.Add(new ScriptBundle("~/scripts/charts").Include(
                "~/scripts/plugin/easy-pie-chart/jquery.easy-pie-chart.min.js",
                "~/scripts/plugin/sparkline/jquery.sparkline.min.js",
                "~/scripts/plugin/morris/morris.min.js",
                "~/scripts/plugin/morris/raphael.min.js",
                "~/scripts/plugin/flot/jquery.flot.cust.min.js",
                "~/scripts/plugin/flot/jquery.flot.resize.min.js",
                "~/scripts/plugin/flot/jquery.flot.time.min.js",
                "~/scripts/plugin/flot/jquery.flot.fillbetween.min.js",
                "~/scripts/plugin/flot/jquery.flot.orderBar.min.js",
                "~/scripts/plugin/flot/jquery.flot.pie.min.js",
                "~/scripts/plugin/flot/jquery.flot.tooltip.min.js",
                "~/scripts/plugin/dygraphs/dygraph-combined.min.js",
                "~/scripts/plugin/chartjs/chart.min.js"
                ));

            bundles.Add(new ScriptBundle("~/scripts/datatables").Include(
                "~/scripts/plugin/datatables/jquery.dataTables.min.js",
                "~/scripts/plugin/datatables/dataTables.colVis.min.js",
                "~/scripts/plugin/datatables/dataTables.tableTools.min.js",
                "~/scripts/plugin/datatables/dataTables.bootstrap.min.js",
                "~/scripts/plugin/datatable-responsive/datatables.responsive.min.js"
                ));

            bundles.Add(new ScriptBundle("~/scripts/jq-grid").Include(
                "~/scripts/plugin/jqgrid/jquery.jqGrid.min.js",
                "~/scripts/plugin/jqgrid/grid.locale-cn.js"
                ));

            bundles.Add(new ScriptBundle("~/scripts/forms").Include(
                "~/scripts/plugin/jquery-form/jquery-form.min.js"
                ));

            bundles.Add(new ScriptBundle("~/scripts/smart-chat").Include(
                "~/scripts/smart-chat-ui/smart.chat.ui.min.js",
                "~/scripts/smart-chat-ui/smart.chat.manager.min.js"
                ));

            bundles.Add(new ScriptBundle("~/scripts/vector-map").Include(
                "~/scripts/plugin/vectormap/jquery-jvectormap-1.2.2.min.js",
                "~/scripts/plugin/vectormap/jquery-jvectormap-world-mill-en.js"
                ));

            bundles.Add(new ScriptBundle("~/scripts/angular").Include(
                "~/Scripts/angular.js",
                 "~/Scripts/angular-strap.js",
                 "~/Scripts/angular-strap.tpl.js",
                 "~/Scripts/angular-ui/ui-bootstrap-tpls.js"
                ));

            BundleTable.EnableOptimizations = true;

        }
    }

}
