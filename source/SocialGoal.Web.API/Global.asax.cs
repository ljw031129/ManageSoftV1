using SocialGoal.Web.API.App_Start;
using SocialGoal.Web.API.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SocialGoal.Web.API
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            // WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Bootstrapper.Run();
            //解决跨域问题，自定义Handlers的方式
            GlobalConfiguration.Configuration.MessageHandlers.Add(new CorsHandler());
            //使用自带的功能，开启跨域功能
            // GlobalConfiguration.Configuration.EnableCors();
        }
    }
}
