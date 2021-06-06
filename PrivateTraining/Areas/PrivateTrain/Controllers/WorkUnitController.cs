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

namespace PrivateTraining.Areas.PrivateTrain.Controllers
{
    public partial class WorkUnitController : Controller
    {
        // GET: PrivateTrain/WorkUnit
        private readonly IWorkUnit _WorkUnit;

        private readonly IUnitOfWork _uow;
        public WorkUnitController(IUnitOfWork uow, IWorkUnit WorkUnit)
        {
            _uow = uow;
            _WorkUnit = WorkUnit;
        }

        public virtual ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// نمایش لیست واحد کار ثبت شده
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<JsonResult> ListWorkUnits()
        {
            try
            {
                datatable datatable = new datatable();
                datatable = datatableclasses.getdatatalbe(Request);

                var list = await _WorkUnit.GetAllWorkUnit();

                if (!string.IsNullOrEmpty(datatable.searchValue))
                {
                    list = list.Where(c => c.Title.Contains(datatable.searchValue));
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
        /// افزودن واحد کار
        /// </summary>
        /// <param name="WorkUnits"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<JsonResult> AddWorkUnit(WorkUnit WorkUnits)
        {
            try
            {
                await _WorkUnit.AddWorkUnit(WorkUnits);
                int Status = await _uow.SaveAllChangesAsync();

                return Json(new { Result = true, Messages = Status });
            }
            catch (Exception)
            {
                return Json(new { Result = false, Messages = "" });
            }

        }

        /// <summary>
        /// حذف واحد کار
        /// </summary>
        /// <param name="WorkUnitId"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<JsonResult> DeleteWorkUnit(string[] WorkUnitId)
        {
            string IdS = "0";
            try
            {
                for (int i = 0; i <= WorkUnitId.Length - 1; i++)
                {
                    IdS = WorkUnitId[i].ToString();
                    await _WorkUnit.DeleteWorkUnit(Convert.ToInt32(WorkUnitId[i]));
                }
                return Json(new { Result = true, Message = "حذف با موفقیت انجام شد" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Message = "امکان حذف برای کد " + IdS + " وجود ندارد" });
            }
        }

        /// <summary>
        /// ویرایش واحد کار
        /// </summary>
        /// <param name="param"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<JsonResult> EditWorkUnit(WorkUnit param, int id)
        {
            try
            {
                var list = await _WorkUnit.GetWorkUnit(id);
                if (list != null)
                {
                    list.Id = param.Id;
                    list.Title = param.Title;
                    list.IsEnable = param.IsEnable;
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

            WorkUnit list = await _WorkUnit.GetWorkUnit(Id);
            return Json(new
            {
                Id = list.Id,
                Title = list.Title,
                IsEnalable = list.IsEnable,
            });
        }
    }
}