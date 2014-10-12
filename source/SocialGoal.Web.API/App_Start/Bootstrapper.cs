using System.Web.Mvc;
using Autofac;
using Autofac.Integration.WebApi;
using System.Reflection;
using SocialGoal.CommandProcessor.Command;
using SocialGoal.CommandProcessor.Dispatcher;
using SocialGoal.Data.Infrastructure;
using SocialGoal.Data.Models;
using SocialGoal.Data.Repository;
using SocialGoal.Model.Models;
using SocialGoal.Service;
using SocialGoal.SocialGoal.Web.API.Mappings;
using SocialGoal.Web.Core.Authentication;
using System.Web.Http;

namespace SocialGoal.Web.API.App_Start
{
    public static class Bootstrapper
    {
        public static void Run()
        {
            SetAutofacWebAPIServices();
            //Configure AutoMapper
            AutoMapperConfiguration.Configure();
        }
        private static void SetAutofacWebAPIServices()
        {
            
            //容器建立者
            var builder = new ContainerBuilder();           
            //注册web api Controllers
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<DefaultCommandBus>().As<ICommandBus>().InstancePerRequest();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();
            builder.RegisterType<DatabaseFactory>().As<IDatabaseFactory>().InstancePerRequest();
            builder.RegisterAssemblyTypes(typeof(FocusRepository).Assembly)
            .Where(t => t.Name.EndsWith("Repository"))
            .AsImplementedInterfaces().InstancePerRequest();
            builder.RegisterAssemblyTypes(typeof(GoalService).Assembly)
           .Where(t => t.Name.EndsWith("Service"))
           .AsImplementedInterfaces().InstancePerRequest();

            builder.RegisterAssemblyTypes(typeof(DefaultFormsAuthentication).Assembly)
         .Where(t => t.Name.EndsWith("Authentication"))
         .AsImplementedInterfaces().InstancePerRequest();

            //builder.Register(c => new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new SocialGoalEntities())))
            //    .As<UserManager<ApplicationUser>>().InstancePerRequest();

            var services = Assembly.Load("SocialGoal.Domain");
            builder.RegisterAssemblyTypes(services)
            .AsClosedTypesOf(typeof(ICommandHandler<>)).InstancePerRequest();
            builder.RegisterAssemblyTypes(services)
            .AsClosedTypesOf(typeof(IValidationHandler<>)).InstancePerRequest();            

            //建立容器
            IContainer container = builder.Build();
            //建立相依解析器
            var resolver = new AutofacWebApiDependencyResolver(container);
            //组成web api 相依解析器
            GlobalConfiguration.Configuration.DependencyResolver = resolver;            
        }
    }
}