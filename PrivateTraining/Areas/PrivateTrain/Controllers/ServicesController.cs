using PrivateTraining.DataLayer.Context;
using PrivateTraining.DomainClasses.Entities.PrivateTraining;
using PrivateTraining.DomainClasses.EntitiesView;
using PrivateTraining.ServiceLayer.Interface.PrivateTraining;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PrivateTraining.ServiceLayer.BLL;
using Microsoft.AspNet.Identity;


namespace PrivateTraining.Areas.PrivateTrain.Controllers
{
    public partial class ServicesController : Controller
    {
        // GET: PrivateTraining/Services
        private readonly IService _Service;
        private readonly IUnitOfWork _uow;
        private IDbSet<Service> _Servicedb;
        private IDbSet<UserService> _UserService;



        public ServicesController(IUnitOfWork uow, IService Service)
        {
            _uow = uow;
            _Service = Service;
            _Servicedb = _uow.Set<Service>();
            _UserService = _uow.Set<UserService>();

        }

        // GET: PrivateTraining/Services
        public virtual ActionResult Index()
        {

            var list = _Servicedb.Where(c => c.IsEnable == true).OrderBy(c => c.Title).ToList();
            if (list.Count() > 0)
                return View(list);
            else
                return View();
        }
        [HttpPost]
        public virtual async Task<JsonResult> GetServiceList(Service param)
        {
            try
            {
                var list = await _Service.GetAllService();
                if (list.Count() > 0)
                {
                    return Json(new { Result = true, list = list, Message = "" });
                }
                else
                    return Json(new { Result = false, Message = "وجود ندارد" });
            }
            catch
            {
                return Json(new { Result = false, Message = "خطا" });
            }

        }

        /// <summary>
        /// افزودن خدمت
        /// </summary>
        /// <param name="Services"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<JsonResult> AddService(Service param)
        {
            try
            {
                int Level = 0;
                if (param.ParentId != 0)
                {
                    var x = await _Service.GetService(param.ParentId);
                    Level = x.Level;
                }
                param.IsEnable = true;
                param.Level = Level + 1;
                await _Service.AddService(param);
                int Status = await _uow.SaveAllChangesAsync();

                return Json(new { Result = true, Messages = Status });
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Messages = "" });
            }

        }


        /// <summary>
        /// حذف خدمت
        /// </summary>
        /// <param name="ServiceId"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<JsonResult> DeleteService(string Id)
        {

            try
            {
                var y = await _Service.GetService(Convert.ToInt32(Id));
                var listT = _Servicedb.Where(c => c.ParentId == y.Id);
                if (listT.Count() > 0)
                {
                    return Json(new { Result = false, Message = "بدلیل داشتن زیر گروه امکان حذف وجود ندارد." });
                }
                await _Service.DeleteService(Convert.ToInt32(y.Id));
                return Json(new { Result = true, Message = "حذف با موفقیت انجام شد" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = true, Message = "حذف انجام نشد." });
            }
        }

        public virtual async Task<JsonResult> EditService(Service param, int id)
        {
            try
            {
                var list = await _Service.GetService(id);
                if (list != null)
                {
                    list.Id = param.Id;
                    list.Title = param.Title;
                }
                int Status = await _uow.SaveAllChangesAsync();
                return Json(new { Result = true, Messages = Status });

            }

            catch (Exception ex)
            {
                return Json(new { Result = false });
            }
        }

        [HttpPost]
        public virtual async Task<JsonResult> ListSubService(int ServiceId)
        {
            try
            {
                var ListSubService = await _Service.GetAllService();
                var Listresult = ListSubService.Where(c => c.ParentId == ServiceId).ToList().Select(a => new Service()
                {
                    Id = a.Id,
                    Title = a.Title
                    //   ServiceId = a.ServiceId
                }).ToList();

                return Json(new { list = Listresult, Resualt = true });
            }
            catch (Exception)
            {
                return Json(new { Resualt = false });
            }
        }

        [HttpPost]
        public virtual async Task<JsonResult> LoadServiceUser(int ServiceId) {
            try
            {
                var CurentUserId = Convert.ToInt32(User.Identity.GetUserId());

                var ServiceUser = _UserService.ToList().Where(c => c.UserId == CurentUserId && c.ServiceId==ServiceId).Select(a => new UserService
                {
                    Id = a.Id,
                    ServiceId = a.ServiceId,
                    SpecialConditionsOfWork = a.SpecialConditionsOfWork,
                }).FirstOrDefault();
                return Json(new { Result = true, ServiceUser = ServiceUser });

            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Messages = ex.Message });
            }
        }
    }
}