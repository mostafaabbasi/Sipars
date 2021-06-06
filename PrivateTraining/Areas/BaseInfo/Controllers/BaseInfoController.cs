using PrivateTraining.DataLayer.Context;
using PrivateTraining.DomainClasses.Entities.BaseTable;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Description;
using System.Web.Mvc;
using PrivateTraining.DomainClasses.Entities;
using PrivateTraining.BussinessLayer.Generic;
using PrivateTraining.ServiceLayer.Interface.Security;

namespace PrivateTraining.Areas.BaseInfo.Controllers
{
   
    public partial class BaseInfoController : Controller
    {
        private readonly IUnitOfWork _uow;
        private readonly ICity _RCity;
        private readonly IState _RState;

        private IDbSet<City> _City;
        private IDbSet<State> _State;

        public BaseInfoController(IUnitOfWork uow, IState state, ICity City)
        {
            _uow = uow;
            _City = _uow.Set<City>();
            _State = _uow.Set<State>();
            _RState = state;
            _RCity = City;
        }

        // GET: BaseInfo/BaseInfo
        public virtual ActionResult GetCity()
        {
            return View();
        }

        /// <summary>
        /// نمایش شهر ها
        /// </summary>
        /// <param name="StateId"></param>
        /// <param name="DefaultCityId"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public virtual async Task<JsonResult> ListCity(int StateId, int DefaultCityId = 0)
        {
            try
            {
                var ListCity = _City.ToList().Where(c => c.StateId == StateId).ToList().Select(a => new City
                {
                    Id = a.Id,
                    Name = a.Name,
                    //برگرداندن شهر انتخاب شده هنگام ویرایش محل 
                    selected = a.ServiceLocations.Any(t => t.CityId == DefaultCityId),
                }).ToList();

                return Json(new {list = ListCity, Resualt = true});
            }
            catch (Exception)
            {
                return Json(new {Resualt = false, JsonRequestBehavior.AllowGet});
            }
        }


        #region State

        public virtual ActionResult GetStates()
        {
            return View();
        }

        /// <summary>
        /// نمایش لیست استان های  ثبت شده
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<JsonResult> ListStates()
        {
            try
            {
                datatable datatable = new datatable();
                datatable = datatableclasses.getdatatalbe(Request);

                var list = await _RState.GetAllState();

                if (!string.IsNullOrEmpty(datatable.searchValue))
                {
                    list = list.Where(c => c.Name.Contains(datatable.searchValue));
                }

                datatable.recordsTotal = list.Count();
                list = list.OrderByDescending(c => c.Id).Skip(datatable.skip).Take(datatable.pageSize);

                // Select Feild
                datatable.data = list.ToList().Select(rec => new string[]
                {
                    "<div class=\"checkbox\" style=\"margin-right:-20px;padding-left:5px;\"><label><input type=\"checkbox\" ng-model=\"checkbox[" +
                    rec.Id + "]\" ng-checked=\"all\" class=\"case\" value=\"" + rec.Id +
                    "\" ><span class=\"text\"></span></lable></div>",
                    //+rec.Id.ToString(),
                    rec.Name
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
        /// افزودن استان  
        /// </summary>
        /// <param name="States"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<JsonResult> AddState(State States)
        {
            try
            {
                await _RState.AddState(States);
                int Status = await _uow.SaveAllChangesAsync();

                return Json(new {Result = true, Messages = Status});
            }
            catch (Exception)
            {
                return Json(new {Result = false, Messages = ""});
            }
        }

        /// <summary>
        /// حذف استان  
        /// </summary>
        /// <param name="StateId"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<JsonResult> DeleteState(string[] StateId)
        {
            string IdS = "0";
            try
            {
                for (int i = 0; i <= StateId.Length - 1; i++)
                {
                    IdS = StateId[i].ToString();
                    await _RState.DeleteState(Convert.ToInt32(StateId[i]));
                }

                return Json(new {Result = true, Message = "حذف با موفقیت انجام شد"});
            }
            catch (Exception ex)
            {
                return Json(new {Result = false, Message = "امکان حذف برای کد " + IdS + " وجود ندارد"});
            }
        }

        /// <summary>
        /// ویرایش استان
        /// </summary>
        /// <param name="param"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<JsonResult> EditState(State param, int id)
        {
            try
            {
                var list = await _RState.GetState(id);
                if (list != null)
                {
                    list.Id = param.Id;
                    list.Name = param.Name;
                    list.IsEnable = param.IsEnable;
                }

                await _uow.SaveAllChangesAsync();
                return Json(new {Result = true, Messages = ""});
            }

            catch (Exception ex)
            {
                return Json(new {Result = false});
            }
        }

        /// <summary>
        /// لود اطلاعات برای ویرایش اطلاعات
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>         
        [HttpPost]
        public virtual async Task<JsonResult> LoadEditState(int Id)
        {
            State list = await _RState.GetState(Id);
            return Json(new
            {
                Id = list.Id,
                Name = list.Name,
                IsEnalable = list.IsEnable,
            });
        }

        #endregion State

        #region City

        public virtual ActionResult GetCitys()
        {
            ViewBag.ListStates = _State.Where(c => c.IsEnable == true).ToList();
            return View();
        }

        /// <summary>
        /// نمایش لیست شهرهای ثبت شده
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<JsonResult> ListCitys()
        {
            try
            {
                datatable datatable = new datatable();
                datatable = datatableclasses.getdatatalbe(Request);

                var list = await _RCity.GetAllCity();

                if (!string.IsNullOrEmpty(datatable.searchValue))
                {
                    list = list.Where(c =>
                        c.Name.Contains(datatable.searchValue) || c.States.Name.Contains(datatable.searchValue));
                }

                datatable.recordsTotal = list.Count();
                list = list.OrderByDescending(c => c.Id).Skip(datatable.skip).Take(datatable.pageSize);

                // Select Feild
                datatable.data = list.ToList().Select(rec => new string[]
                {
                    "<div class=\"checkbox\" style=\"margin-right:-20px;padding-left:5px;\"><label><input type=\"checkbox\"  ng-checked=\"all\" ng-model=\"checkbox[" +
                    rec.Id + "]\" class=\"case\" value=\"" + rec.Id + "\" ><span class=\"text\"></span></lable></div>",
                    //+rec.Id.ToString(),
                    rec.Name,
                    rec.States.Name
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
        /// افزودن شهر
        /// </summary>
        /// <param name="Citys"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<JsonResult> AddCity(City Citys)
        {
            try
            {
                await _RCity.AddCity(Citys);
                int Status = await _uow.SaveAllChangesAsync();

                return Json(new {Result = true, Messages = Status});
            }
            catch (Exception)
            {
                return Json(new {Result = false, Messages = ""});
            }
        }

        /// <summary>
        /// حذف شهر
        /// </summary>
        /// <param name="CityId"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<JsonResult> DeleteCity(string[] CityId)
        {
            string IdS = "0";
            try
            {
                for (int i = 0; i <= CityId.Length - 1; i++)
                {
                    IdS = CityId[i].ToString();
                    await _RCity.DeleteCity(Convert.ToInt32(CityId[i]));
                }

                return Json(new {Result = true, Message = "حذف با موفقیت انجام شد"});
            }
            catch (Exception ex)
            {
                return Json(new {Result = false, Message = "امکان حذف برای کد " + IdS + " وجود ندارد"});
            }
        }

        /// <summary>
        /// ویرایش شهر
        /// </summary>
        /// <param name="param"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<JsonResult> EditCity(City param, int id)
        {
            try
            {
                var list = await _RCity.GetCity(id);
                if (list != null)
                {
                    list.Id = param.Id;
                    list.Name = param.Name;
                    list.StateId = param.StateId;
                    list.IsEnable = param.IsEnable;
                }

                await _uow.SaveAllChangesAsync();
                return Json(new {Result = true, Messages = ""});
            }

            catch (Exception ex)
            {
                return Json(new {Result = false});
            }
        }

        /// <summary>
        /// لود اطلاعات برای ویرایش اطلاعات
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>         
        [HttpPost]
        public virtual async Task<JsonResult> LoadEditCity(int Id)
        {
            City list = await _RCity.GetCity(Id);
            return Json(new
            {
                Id = list.Id,
                Name = list.Name,
                StateId = list.StateId,
                IsEnalable = list.IsEnable,
            });
        }

        #endregion City
    }
}