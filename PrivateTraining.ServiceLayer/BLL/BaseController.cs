using System.Web.Mvc;

namespace PrivateTraining.ServiceLayer.BLL
{
    [AuthorizeUser]
    public class BaseController : Controller
    {
    }
}
