using System.Web.Http;
using PrivateTraining.ServiceLayer.BLL;

namespace PrivateTraining.Api
{
    [AuthorizeUser]
    public class BaseApiController : ApiController
    {
    }
}