using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using PrivateTraining.DataLayer.Context;
using PrivateTraining.DomainClasses.Entities.FrameWork;
using PrivateTraining.ServiceLayer.Interface.Framework;
using PrivateTraining.BussinessLayer.Framework;
using Microsoft.AspNet.Identity;
using PrivateTraining.ServiceLayer.Extention;
using PrivateTraining.DomainClasses.Entities.BusDriving;
using System.Data.Entity;
using PrivateTraining.ServiceLayer.BLL;
using System.Linq;

namespace PrivateTraining.Areas.Framework.Controllers
{
    public partial class MenuController : BaseController
    {

        private readonly IMenu _menu;
        private readonly IUnitOfWork _uow;
        private IDbSet<LeaveRequest> _Leave;
        private IDbSet<UserRequest> _UserRequests;

        public MenuController(IUnitOfWork uow, IMenu menu)
        {
            _uow = uow;
            _menu = menu;
            _Leave = _uow.Set<LeaveRequest>();
            _UserRequests = _uow.Set<UserRequest>();
        }

        // GET: Framework/Menu

        public virtual ActionResult Index(bool Sw = false)
        {
            var countLeave = _Leave.Where(c => c.StatusLeave == (byte)StatusLeave.NotChecked).Count();
            ViewBag.countleaves = countLeave;

            var countRequest = _UserRequests.Where(c => c.StatusRequest == (byte)StatusRequest.Send).Count();
            ViewBag.countrequest = countRequest;

            ViewBag.Sw = Sw;

            return View();
        }

        public virtual ActionResult ListMenus()
        {
            string Role = "User";
            if (User.IsInRole("Admin"))
                Role = "Admin";
            else if (User.IsInRole("User"))
                Role = "User";
            else if (User.IsInRole("Modrator"))
                Role = "Modrator";
            else if (User.IsInRole("Administrator"))
                Role = "Administrator";
            else if (User.IsInRole("ServiceProvider"))
                Role = "ServiceProvider";

            var List = _menu.ListMenu(Role).OrderBy(c=>c.Sort);

            return View(List);
        }



        //[HttpPost]
        //public virtual async Task<JsonResult> ListMenus()
        //{
        //    try
        //    {
        //        //var menu = await _menu.ListMenu();
        //        // string Menu = "{\"label\": \"Overview\",\"iconClasses\": \"\",\"separator\": true}";
        //        string menus = Menus.LayoutMenus(await _menu.ListMenu());

        //        //menus += "," + Menu;

        //        return Json(new { Resualt = true, Listmenu = JsonConvert.SerializeObject(menus), Messages = "" });
        //    }
        //    catch (Exception)
        //    {
        //        return Json(new { Resualt = false, Messages = "" });
        //    }

        //}

    }
}