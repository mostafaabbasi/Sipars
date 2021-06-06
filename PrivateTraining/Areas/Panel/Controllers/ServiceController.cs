using System.Web.Mvc;
using System.Threading.Tasks;
using PrivateTraining.ServiceLayer.BLL;

namespace PrivateTraining.Areas.Panel.Controllers
{
    [AuthorizeUser]
    public partial class ServiceController : Controller
    {
        public ServiceController()
        {
        }

        public virtual ActionResult List()
        {
            return View();
        }
    }
}