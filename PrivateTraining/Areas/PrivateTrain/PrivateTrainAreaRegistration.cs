using System.Web.Mvc;

namespace PrivateTraining.Areas.PrivateTrain
{
    public class PrivateTrainAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "PrivateTrain";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "PrivateTrain_default",
                "PrivateTrain/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}