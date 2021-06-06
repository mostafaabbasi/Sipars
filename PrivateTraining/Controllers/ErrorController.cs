using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PrivateTraining.Controllers
{
    [AllowAnonymous]
    public partial class ErrorController : Controller
    {
        // GET: Error
        public virtual ActionResult NotFound()
        {
            //var statusCode = (int)System.Net.HttpStatusCode.NotFound;
            //Response.StatusCode = statusCode;
            //Response.TrySkipIisCustomErrors = true;
            //HttpContext.Response.StatusCode = statusCode;
            //HttpContext.Response.TrySkipIisCustomErrors = true;
            return View();
        }

        public virtual ActionResult Error()
        {
            //Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
            //Response.TrySkipIisCustomErrors = true;
            ViewData["ErrorMessage"] = "";
            ViewData["ErrorNumber"] = (int)System.Net.HttpStatusCode.InternalServerError;
            if ((int)System.Net.HttpStatusCode.InternalServerError == 403)
                ViewData["ErrorMessage"] = " کاربر گرامی، شما از دسترسی به صفحه موردنظر خارج شده اید. لطفا مجدد وارد سایت شوید.";
            else if ((int)System.Net.HttpStatusCode.InternalServerError == 500)
                ViewData["ErrorMessage"] = " خطا در نمایش صفحه موردنظر";

            return View();
        }
    }
}