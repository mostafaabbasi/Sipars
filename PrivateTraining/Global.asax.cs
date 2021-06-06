using System;
using System.Data.Entity;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Newtonsoft.Json;
using PrivateTraining.IocConfig;
using StructureMap.Web.Pipeline;
using PrivateTraining.DataLayer.Context;
using System.Web;
using System.Web.Http;

namespace PrivateTraining
{
    // Note: For instructions on enabling IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=301868
    //public class MvcApplication : System.Web.HttpApplication
    //{
    //    protected void Application_Start()
    //    {
    //        AreaRegistration.RegisterAllAreas();
    //        FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
    //        RouteConfig.RegisterRoutes(RouteTable.Routes);
    //        BundleConfig.RegisterBundles(BundleTable.Bundles);
    //    }
    //}


    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
          //  JobScheduler.Start();


            // WebApiConfig.Register(GlobalConfiguration.Configuration);

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
           
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            GlobalConfiguration.Configure(WebApiConfig.Register);
            
             RouteConfig.RegisterRoutes(RouteTable.Routes);
             
            setDbInitializer();
            //Set current Controller factory as StructureMapControllerFactory
            ControllerBuilder.Current.SetControllerFactory(new StructureMapControllerFactory());
            Microsoft.AspNet.SignalR.GlobalHost.DependencyResolver = SmObjectFactory.Container.GetInstance<Microsoft.AspNet.SignalR.IDependencyResolver>();

            //------------------------------------------ Json ------------------------------------------------
            //var jsonSerializerSettings = new JsonSerializerSettings
            //{
            //    PreserveReferencesHandling = PreserveReferencesHandling.Objects
            //};

            //GlobalConfiguration.Configuration.Formatters.Clear();
            //GlobalConfiguration.Configuration.Formatters.Add(new JsonNetFormatter(jsonSerializerSettings));

            //------------------------------------------ Json ------------------------------------------------
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            HttpContextLifecycle.DisposeAndClearAll();
        }

        public class StructureMapControllerFactory : DefaultControllerFactory
        {
            protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
            {
                if (controllerType == null)
                {
                    if ((int)System.Net.HttpStatusCode.NotFound == 404)
                        HttpContext.Current.Response.Redirect("~/Error/NotFound");
                    else if ((int)System.Net.HttpStatusCode.NotFound == 500)
                        HttpContext.Current.Response.Redirect("~/Error/Error");
                    else
                        throw new InvalidOperationException(string.Format("Page not found: {0}", requestContext.HttpContext.Request.RawUrl));
                }
                return SmObjectFactory.Container.GetInstance(controllerType) as Controller;
            }
        }

        private static void setDbInitializer()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>());
            SmObjectFactory.Container.GetInstance<IUnitOfWork>().ForceDatabaseInitialize();
        }
    }

}
