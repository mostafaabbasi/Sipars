using System;
using PrivateTraining.IocConfig;
using PrivateTraining.ServiceLayer.Interface;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.DataProtection;
using Owin;
using StructureMap.Web;
using Castle.Core.Logging;
using PrivateTraining.ServiceLayer.Interface.Security;
using System.Web.Mvc;
using System.Web.Routing;
using PrivateTraining.ServiceLayer.BLL;
using System.Web.Mvc.Async;
using PrivateTraining.ServiceLayer.Interface.PrivateTraining;
//using Swashbuckle.Application;

namespace PrivateTraining
{

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            configureAuth(app);
        }

        private static void configureAuth(IAppBuilder app)
        {
            System.Web.Http.HttpConfiguration httpConfiguration = new System.Web.Http.HttpConfiguration();
            
//            httpConfiguration
//                .EnableSwagger(c =>
//                {
//                     c.SingleApiVersion("v1", "Name.API");
//                })  
//                .EnableSwaggerUi();
                
            SmObjectFactory.Container.Configure(config =>
            {
                config.For<IDataProtectionProvider>()
                      .HybridHttpOrThreadLocalScoped()
                      .Use(() => app.GetDataProtectionProvider());


                config.For<IAsyncActionInvoker>().Use<InjectingActionInvoker>();
                config.For<IActionInvoker>().Use<InjectingActionInvoker>();
                config.For<ITempDataProvider>().Use<SessionStateTempDataProvider>();
                config.For<RouteCollection>().Use(RouteTable.Routes);

                config.Policies.SetAllProperties(c =>
                {
                    c.OfType<IActionInvoker>();
                    c.OfType<IAsyncActionInvoker>();
                    c.OfType<ITempDataProvider>();
                    c.WithAnyTypeFromNamespaceContainingType<IAccessLevel>();
                    c.WithAnyTypeFromNamespaceContainingType<IServiceReceiverServiceLocation>();
                    c.WithAnyTypeFromNamespaceContainingType<PrivateTraining.ServiceLayer.Interface.Framework.IAction>();
                });
            });
            //SmObjectFactory.Container.GetInstance<IApplicationUserManager>().SeedDatabase();

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                CookieName = "my-very-own-cookie-name",
                ExpireTimeSpan = TimeSpan.FromDays(30),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.
                    OnValidateIdentity = SmObjectFactory.Container.GetInstance<IApplicationUserManager>().OnValidateIdentity()
                }
            });
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            //app.UseFacebookAuthentication(
            //   appId: "",
            //   appSecret: "");

            //app.UseGoogleAuthentication(
            //    clientId: "",
            //    clientSecret: "");
            app.MapSignalR();
            
        }
    }
}
