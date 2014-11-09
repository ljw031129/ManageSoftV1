using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using System.Reflection;
using SocialGoal.Data.Repository;
using SocialGoal.Data.Infrastructure;
using SocialGoal.Service;
using SocialGoal.Mappings;
using SocialGoal.Web.Core.Authentication;
using Microsoft.AspNet.Identity.EntityFramework;
using SocialGoal.Model.Models;
using SocialGoal.Data.Models;
using Microsoft.AspNet.Identity;

namespace SocialGoal
{
    public static class Bootstrapper
    {
        public static void Run()
        {
            SetAutofacContainer();
            //Configure AutoMapper
            AutoMapperConfiguration.Configure();
        }
        private static void SetAutofacContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());     
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();
            builder.RegisterType<DatabaseFactory>().As<IDatabaseFactory>().InstancePerRequest();
            builder.RegisterAssemblyTypes(typeof(EquipmentRepository).Assembly)
            .Where(t => t.Name.EndsWith("Repository"))
            .AsImplementedInterfaces().InstancePerRequest();
            builder.RegisterAssemblyTypes(typeof(EquipmentService).Assembly)
           .Where(t => t.Name.EndsWith("Service"))
           .AsImplementedInterfaces().InstancePerRequest();

            builder.RegisterAssemblyTypes(typeof(DefaultFormsAuthentication).Assembly)
         .Where(t => t.Name.EndsWith("Authentication"))
         .AsImplementedInterfaces().InstancePerRequest();

            //注册UserManager
            builder.Register(c => new UserManager<ApplicationUser>(new UserStore<ApplicationUser>( new SocialGoalEntities())))
                .As<UserManager<ApplicationUser>>().InstancePerRequest();
            builder.Register(c => new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new SocialGoalEntities())))
               .As<RoleManager<IdentityRole>>().InstancePerRequest();

          
            //要使用的筛选器属性的属性注入，只需调用 RegisterFilterProvider 方法提供的 Autofac.MVC5
            builder.RegisterFilterProvider();
            IContainer container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}