using Autofac;
using Autofac.Integration.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using TMS.BusinessRule.Concretes;
using TMS.BusinessRule.Interfaces;
using TMS.DAL.Repositories.Concretes;
using TMS.DAL.Repositories.Interfaces;
using TMS.Model;
using TMS.Web.Controllers;

namespace TMS.Web.App_Start
{
    public class Bootstrapper
    {
        public static void Run()
        {
            SetAutofacContainer();
        }

        private static void SetAutofacContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
            builder.RegisterType<DbFactory>().As<IDbFactory>().InstancePerLifetimeScope();

            // Repositories
            builder.RegisterType<UserRepository<User>>().As<IUserRepository<User>>().InstancePerRequest();
            //Then we register the controller itself resolving/indicating the target type to use on the constructor.


            builder.RegisterAssemblyTypes(typeof(UserRepository<User>).Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces().InstancePerRequest();
            builder.RegisterAssemblyTypes(typeof(AdminRepository).Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces().InstancePerRequest();
            builder.RegisterAssemblyTypes(typeof(DistributorRepository).Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces().InstancePerRequest();
            builder.RegisterAssemblyTypes(typeof(DealerRepository).Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces().InstancePerRequest();
            builder.RegisterAssemblyTypes(typeof(CustomerRepository).Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces().InstancePerRequest();
            builder.RegisterAssemblyTypes(typeof(DeviceInfoRepository).Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces().InstancePerRequest();
            builder.RegisterAssemblyTypes(typeof(DeviceModelRepository).Assembly)
               .Where(t => t.Name.EndsWith("Repository"))
               .AsImplementedInterfaces().InstancePerRequest();
            builder.RegisterAssemblyTypes(typeof(DeviceTypeRepository).Assembly)
                           .Where(t => t.Name.EndsWith("Repository"))
                           .AsImplementedInterfaces().InstancePerRequest();

            // Services
            //builder.Register(c => new UserService<User>(c.Resolve<IUserRepository<User>>(), c.Resolve<IUnitOfWork>())).InstancePerRequest();
            builder.RegisterType<UserService<User>>().As<IUserService<User>>().InstancePerRequest();


            builder.RegisterAssemblyTypes(typeof(UserService<User>).Assembly)
               .Where(t => t.Name.EndsWith("Service"))
               .AsImplementedInterfaces().InstancePerRequest();
            builder.RegisterAssemblyTypes(typeof(AdminService).Assembly)
               .Where(t => t.Name.EndsWith("Service"))
               .AsImplementedInterfaces().InstancePerRequest();
            builder.RegisterAssemblyTypes(typeof(DistributorService).Assembly)
               .Where(t => t.Name.EndsWith("Service"))
               .AsImplementedInterfaces().InstancePerRequest();
            builder.RegisterAssemblyTypes(typeof(DealerService).Assembly)
               .Where(t => t.Name.EndsWith("Service"))
               .AsImplementedInterfaces().InstancePerRequest();
            builder.RegisterAssemblyTypes(typeof(CustomerService).Assembly)
               .Where(t => t.Name.EndsWith("Service"))
               .AsImplementedInterfaces().InstancePerRequest();
            builder.RegisterAssemblyTypes(typeof(DeviceInfoService).Assembly)
               .Where(t => t.Name.EndsWith("Service"))
               .AsImplementedInterfaces().InstancePerRequest();
            builder.RegisterAssemblyTypes(typeof(DeviceDataModelService).Assembly)
               .Where(t => t.Name.EndsWith("Service"))
               .AsImplementedInterfaces().InstancePerRequest();
            builder.RegisterAssemblyTypes(typeof(DeviceTypeService).Assembly)
                           .Where(t => t.Name.EndsWith("Service"))
                           .AsImplementedInterfaces().InstancePerRequest();

            // builder.RegisterType<DeviceModelService<DeviceModel>>().As<IDeviceModelService<DeviceModel>>().InstancePerRequest();

            builder.Register(c => new UserAccountController(c.Resolve<IUserService<User>>())).InstancePerRequest();

            IContainer container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}
