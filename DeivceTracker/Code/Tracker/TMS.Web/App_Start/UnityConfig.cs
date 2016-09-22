using System.Web.Mvc;
using Microsoft.Practices.Unity;
using TMS.BusinessRule.Concretes;
using TMS.BusinessRule.Interfaces;
using TMS.DAL.Repositories.Concretes;
using TMS.DAL.Repositories.Interfaces;
using Unity.Mvc5;
using TMS.Model;

namespace TMS.Web
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            // The Microsoft.Practices.Unity.HierarchialLifetimeManager provides:
            // Dispose() call after each request
            // The same DbContext instance per request
            container.RegisterType<IUnitOfWork, UnitOfWork>(new HierarchicalLifetimeManager());
            container.RegisterType<IDbFactory, DbFactory>(new HierarchicalLifetimeManager());
            
            // Repositories
            container.RegisterType<IAdminRepository, AdminRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IDealerRepository, DealerRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IDeviceInfoRepository, DeviceInfoRepository>(new HierarchicalLifetimeManager());

            // Business Services
            container.RegisterType<IAdminService, AdminService>(new HierarchicalLifetimeManager());
            container.RegisterType<IDealerService, DealerService>(new HierarchicalLifetimeManager());
            container.RegisterType<IDeviceModelRepository<DeviceModels>, DeviceModelRepository>(new HierarchicalLifetimeManager());

            

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}