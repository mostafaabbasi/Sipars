using PrivateTraining.BussinessLayer.Generic;
using PrivateTraining.DataLayer.Context;
using PrivateTraining.ServiceLayer.Interface.PrivateTraining;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PrivateTraining.DomainClasses.Entities;
using PrivateTraining.DomainClasses.Entities.PrivateTraining;
using PrivateTraining.ServiceLayer.BLL;

namespace PrivateTraining.Areas.PrivateTrain.Controllers
{
    public partial class ServiceLevelsController : BaseController
    {
        private readonly IServiceLevel _ServiceLevel;
        private readonly IUnitOfWork _uow;
        private readonly IService _service;

        public ServiceLevelsController(IUnitOfWork uow, IServiceLevel ServiceLevel ,IService service)
        {
            _uow = uow;
            _ServiceLevel = ServiceLevel;
            _service = service;
        }

        public virtual ActionResult Index()
        {
            ViewBag.ListService = _service.GetAllServiceParent();
            return View();
        }

        /// <summary>
        /// نمایش لیست  ثبت شده
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<JsonResult> ListServiceLevels()
        {
            try
            {
                datatable datatable = new datatable();
                datatable = datatableclasses.getdatatalbe(Request);

                var list = await _ServiceLevel.GetAllServiceLevel();

                if (!string.IsNullOrEmpty(datatable.searchValue))
                {
                    list = list.Where(c => c.Title.Contains(datatable.searchValue)  );
                }
                datatable.recordsTotal = list.Count();
                list = list.OrderBy(c => c.Id).Skip(datatable.skip).Take(datatable.pageSize);

                // Select Feild

                int Row = 1;
                datatable.data = list.ToList().Select(rec => new string[]
                {
                 "<div class=\"checkbox\" style=\"margin-right:-20px;padding-left:5px;\"><label><input type=\"checkbox\"  ng-checked=\"all\" ng-model=\"checkbox["+rec.Id+"]\" class=\"case\" value=\""+rec.Id +"\" ><span class=\"text\"></span></lable></div>"
                + ( Row++ ).ToString(),
                 rec.Title,
              //   rec.Percent.ToString(),
                 rec.Services.Title,
                }).ToList();

                datatable.draw = datatable.draw;
                datatable.recordsFiltered = datatable.recordsTotal;
                datatable.recordsTotal = datatable.recordsTotal;

                return Json(datatable);
            }
            catch (Exception ex)
            {
                return Json(null);
            }

        }

        /// <summary>
        /// افزودن 
        /// </summary>
        /// <param name="ServiceLevels"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<JsonResult> AddServiceLevel(ServiceLevel ServiceLevels)
        {
            try
            {
                await _ServiceLevel.AddServiceLevel(ServiceLevels);
                int Status = await _uow.SaveAllChangesAsync();

                return Json(new { Result = true, Messages = Status });
            }
            catch (Exception)
            {
                return Json(new { Result = false, Messages = "" });
            }

        }

        /// <summary>
        /// حذف 
        /// </summary>
        /// <param name="ServiceLevelId"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<JsonResult> DeleteServiceLevel(string[] ServiceLevelId)
        {
            string IdS = "0";
            try
            {
                for (int i = 0; i <= ServiceLevelId.Length - 1; i++)
                {
                    IdS = ServiceLevelId[i].ToString();
                    await _ServiceLevel.DeleteServiceLevel(Convert.ToInt32(ServiceLevelId[i]));
                }
                return Json(new { Result = true, Message = "حذف با موفقیت انجام شد" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Message = "امکان حذف برای کد " + IdS + " وجود ندارد" });
            }
        }

        /// <summary>
        /// ویرایش 
        /// </summary>
        /// <param name="param"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<JsonResult> EditServiceLevel(ServiceLevel param, int id)
        {
            try
            {
                var list = await _ServiceLevel.GetServiceLevel(id);
                if (list != null)
                {
                    list.Id = param.Id;
                    list.Title = param.Title;
                  //  list.Percent = param.Percent;
                    list.IsEnable = param.IsEnable;
                    list.ServiceId = param.ServiceId;
                }
                await _uow.SaveAllChangesAsync();
                return Json(new { Result = true, Messages = "" });

            }

            catch (Exception ex)
            {
                return Json(new { Result = false });
            }
        }

        /// <summary>
        /// لود اطلاعات برای ویرایش اطلاعات
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>         
        [HttpPost]
        public virtual async Task<JsonResult> LoadEdit(int Id)
        {

            ServiceLevel list = await _ServiceLevel.GetServiceLevel(Id);
            return Json(new
            {
                Id = list.Id,
                Title = list.Title,
                IsEnalable = list.IsEnable,
              //  Percent=list.Percent,
                ServiceId = list.ServiceId,

        });
        }
    }
}