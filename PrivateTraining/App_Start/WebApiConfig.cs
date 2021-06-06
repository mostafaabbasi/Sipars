using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using log4net.Repository.Hierarchy;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PrivateTraining.IocConfig;

namespace PrivateTraining
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            
            GlobalConfiguration.Configuration.Services.Replace(
                typeof(IHttpControllerActivator), new StructureMapHttpControllerActivator(SmObjectFactory.Container));

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "v1/{controller}/{action}/{id}",
                defaults: new {id = RouteParameter.Optional}
            );
            
            config.MessageHandlers.Add(new LogDelegatingHandler());

            SetSerializerSettings();
        }
        
        private static void SetSerializerSettings()
        {
            var formatters = GlobalConfiguration.Configuration.Formatters;
            var jsonFormatter = formatters.JsonFormatter;
            var settings = jsonFormatter.SerializerSettings;
            settings.Formatting = Formatting.None;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
        
        public class LogDelegatingHandler : DelegatingHandler
        {
            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage messageRequest,
                CancellationToken cancellationToken)
            {
                var startDateTime = DateTime.Now;
                var response = await base.SendAsync(messageRequest, cancellationToken);

                if (!messageRequest.Method.Method.ToUpper().Equals("OPTION"))
                {
                    var req = HttpContext.Current?.Request;
                    var res = HttpContext.Current?.Response;
                    var items = HttpContext.Current?.Items;
                    Task.Run(async () => new PrivateTraining.Utils.Logger().Test(messageRequest, req, response, res, startDateTime, "", "", items));
                }

                return response;
            }
        }

    }
}