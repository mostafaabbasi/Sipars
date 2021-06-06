using System;
using System.ComponentModel;
using System.Data.Entity;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Web;
using PrivateTraining.DataLayer.Context;
using PrivateTraining.DomainClasses;
using PrivateTraining.ServiceLayer.Repository.IdentityRepository;
using PrivateTraining.ServiceLayer.Interface;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;
using PrivateTraining.DomainClasses.Security;
using PrivateTraining.ServiceLayer;
using PrivateTraining.ServiceLayer.Interface.Framework;
using PrivateTraining.ServiceLayer.Interface.Security;
using PrivateTraining.ServiceLayer.Repository.Security;
using StructureMap;
using StructureMap.Web;
using PrivateTraining.DomainClasses.Entities.FrameWork;
using PrivateTraining.ServiceLayer.Repository.Framework;

using PrivateTraining.ServiceLayer.BLL;
using PrivateTraining.ServiceLayer.Interface.PrivateTraining;
using PrivateTraining.ServiceLayer.Repository.PrivateTraining;
using PrivateTraining.ServicePropertiesLayer.Repository.PrivateTraining;
using PrivateTraining.DomainClasses.Entities.PrivateTraining;
using PrivateTraining.LocationLayer.Interface.PrivateTraining;
using PrivateTraining.LocationLayer.Repository.PrivateTraining;
using PrivateTraining.ServiceLocationLayer.Interface.PrivateTraining;


namespace PrivateTraining.IocConfig
{
    public static class SmObjectFactory
    {
        private static readonly Lazy<StructureMap.Container> _containerBuilder =
            new Lazy<StructureMap.Container>(defaultContainer, LazyThreadSafetyMode.ExecutionAndPublication);

        public static StructureMap.IContainer Container
        {
            get { return _containerBuilder.Value; }
        }

        //public static StructureMap.IContainer Initialize()
        //{
        //    SmObjectFactory.Initialize(x =>
        //    {
        //        x.Scan(scan =>
        //        {
        //            scan.TheCallingAssembly();
        //            scan.WithDefaultConventions();
        //        });
        //        x.SetAllProperties(pset =>
        //        {
        //            pset.WithAnyTypeFromNamespace("something.Infrastructure.ActionFilters");
        //            pset.OfType<IRepository<User>>();
        //        });
        //    });
        //    return SmObjectFactory.Container;
        //}

        private static StructureMap.Container defaultContainer()
        {

            return new StructureMap.Container(ioc =>
            {
                ioc.For<Microsoft.AspNet.SignalR.IDependencyResolver>().Singleton().Add<StructureMapSignalRDependencyResolver>();

                ioc.For<IIdentity>().Use(() => getIdentity());

                ioc.For<IUnitOfWork>()
                    .HybridHttpOrThreadLocalScoped()
                    .Use<ApplicationDbContext>();
                // Remove these 2 lines if you want to use a connection string named connectionString1, defined in the web.config file.
                //.Ctor<string>("connectionString")
                //.Is("Data Source=(local);Initial Catalog=TestDbIdentity;Integrated Security = true");

                ioc.For<ApplicationDbContext>().HybridHttpOrThreadLocalScoped()
                   .Use(context => (ApplicationDbContext)context.GetInstance<IUnitOfWork>());
                ioc.For<DbContext>().HybridHttpOrThreadLocalScoped()
                   .Use(context => (ApplicationDbContext)context.GetInstance<IUnitOfWork>());

                ioc.For<IUserStore<ApplicationUser, int>>()
                    .HybridHttpOrThreadLocalScoped()
                    .Use<CustomUserStoreRepository>();

                
                
                ioc.For<IRoleStore<CustomRole, int>>()
                    .HybridHttpOrThreadLocalScoped()
                    .Use<RoleStore<CustomRole, int, CustomUserRole>>();

                ioc.For<IAuthenticationManager>()
                      .Use(() => HttpContext.Current.GetOwinContext().Authentication);

                ioc.For<IApplicationSignInManager>()
                      .HybridHttpOrThreadLocalScoped()
                      .Use<ApplicationSignInManagerRepository>();

                ioc.For<IApplicationRoleManager>()
                      .HybridHttpOrThreadLocalScoped()
                      .Use<ApplicationRoleManagerRepository>();

                // map same interface to different concrete classes
                ioc.For<IIdentityMessageService>().Use<SmsService>();
                ioc.For<IIdentityMessageService>().Use<EmailService>();

                ioc.For<IApplicationUserManager>().HybridHttpOrThreadLocalScoped()
                    .Use<ApplicationUserManagerRepository>()
                   .Ctor<IIdentityMessageService>("smsService").Is<SmsService>()
                   .Ctor<IIdentityMessageService>("emailService").Is<EmailService>()
                   .Setter(userManager => userManager.SmsService).Is<SmsService>()
                   .Setter(userManager => userManager.EmailService).Is<EmailService>();

                ioc.For<ApplicationUserManagerRepository>().HybridHttpOrThreadLocalScoped()
                   .Use(context => (ApplicationUserManagerRepository)context.GetInstance<IApplicationUserManager>());

                ioc.For<ICustomRoleStore>()
                      .HybridHttpOrThreadLocalScoped()
                      .Use<CustomRoleStoreRepository>();

                ioc.For<ICustomUserStore>()
                      .HybridHttpOrThreadLocalScoped()
                      .Use<CustomUserStoreRepository>();

                #region Security
                ioc.For<IGroupPolicy>().Use<GroupPolicyRepository>();
                ioc.For<IAccessLevel>().Use<AccessLevelRepository>();

                #endregion

                #region Framework

                ioc.For<IMenu>().Use<MenuRepository>();
                ioc.For<IAction>().Use<ActionRepository>();
                ioc.For<IMessage>().Use<MessageRepository>();
                ioc.For<IFreind>().Use<FreindRepository>();
                ioc.For<IPayment>().Use<PaymentRepository>();
                ioc.For<IState>().Use<StateRepository>();
                ioc.For<ICity>().Use<CityRepository>();
                #endregion

                #region PrivateTraining
                ioc.For<IService>().Use<ServiceRepository>();
                ioc.For<IWorkUnit>().Use<WorkUnitRepository>();
                ioc.For<IServiceProperties>().Use<ServicePropertiesRepository>();
                ioc.For<IServiceProviderInfo>().Use<ServiceProviderInfoRepository>();
                ioc.For<IServiceReceiverInfo>().Use<ServiceReceiverInfoRepository>();
                ioc.For<ILocation>().Use<LocationRepository>();
                ioc.For<IServiceLocation>().Use<ServiceLocationRepository>();
                ioc.For<IServiceReceiverServiceLocation>().Use<ServiceReceiverServiceLocationRepository>();
                ioc.For<IServiceLevel>().Use<ServiceLevelRepository>();

                #endregion

                //config.For<IDataProtectionProvider>().Use(()=> app.GetDataProtectionProvider()); // In Startup class

                //ioc.For<ICategoryService>().Use<CategoryServiceRepository>();

                //ioc.For<IProductService>().Use<EfProductService>();
            });
        }

        private static IIdentity getIdentity()
        {
            if (HttpContext.Current != null && HttpContext.Current.User != null)
            {
                return HttpContext.Current.User.Identity;
            }

            return ClaimsPrincipal.Current != null ? ClaimsPrincipal.Current.Identity : null;
        }
    }
}
