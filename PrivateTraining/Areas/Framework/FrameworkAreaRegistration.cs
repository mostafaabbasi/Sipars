using System.Web.Mvc;

namespace PrivateTraining.Areas.Framework
{
    public class FrameworkAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Framework";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Framework_default",
                "Framework/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}