using PrivateTraining.BussinessLayer.Generic;
using PrivateTraining.DataLayer.Context;
using PrivateTraining.DomainClasses.Entities.PrivateTraining;
using PrivateTraining.ServiceLayer.Extention;
using PrivateTraining.ServiceLayer.Interface.PrivateTraining;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using PrivateTraining.DomainClasses.Entities;
using System.Web.Mvc;
using PrivateTraining.DomainClasses.EntitiesView.PrivateTrainig;
using PrivateTraining.ServiceLayer.BLL;
using PrivateTraining.ServiceLayer.Interface;

namespace PrivateTraining.Areas.PrivateTrain.Controllers
{
    public partial class ServiceReceiverRequestController : Controller
    {
        private IUnitOfWork _uow;
        private IDbSet<ServiceReceiverServiceLocation> _ServiceReceiverServiceLocations;
        private IServiceReceiverServiceLocation _servicereceiveservicelocation;
        private IDbSet<ServiceReceiverRequest> _ServiceReceiverRequest;
        private IDbSet<Debt> _Debt;
        private IDbSet<ServiceLocationWorkUnit> _ServiceLocationWorkUnit;
        private readonly IApplicationUserManager _userManager;
        private IDbSet<ServiceLevelList> _ServiceLevelList;
        private IDbSet<UserService> _UserService;

        public ServiceReceiverRequestController(IUnitOfWork uow, IServiceReceiverServiceLocation servicereceiveservicelocation, IApplicationUserManager userManager)
        {
            _uow = uow;
            _ServiceReceiverServiceLocations = _uow.Set<ServiceReceiverServiceLocation>();
            _servicereceiveservicelocation = servicereceiveservicelocation;
            _ServiceReceiverRequest = _uow.Set<ServiceReceiverRequest>();
            _Debt = _uow.Set<Debt>();
            _ServiceLocationWorkUnit = _uow.Set<ServiceLocationWorkUnit>();
            _userManager = userManager;
            _ServiceLevelList = _uow.Set<ServiceLevelList>();
            _UserService = _uow.Set<UserService>();
        }

        /// <summary>
        /// تایید خدمت دریافت شده توسط مشتری
        /// </summary>
        [HttpPost]
        public virtual async Task<ActionResult> ConfirmServiceReceiverRequest(int RequestId, byte StatusConfirm)
        {

            PrivateTraining.ServiceLayer.BLL.PrivateTraining Private = new PrivateTraining.ServiceLayer.BLL.PrivateTraining(_uow,_userManager);
            var save = await Private.ConfirmServiceReceiverRequest(RequestId, StatusConfirm);
            if (save == 1)
                return Json(new { Resualt = true, Messages = "با موفقیت ثبت شد" });
            else
                return Json(new { Resualt = false, Messages = "ثبت نشد" });
        }

        [HttpPost]
        public virtual async Task<JsonResult> AddServiceReceiverRequest(ServiceReceiverRequest Param)
        {
            try
            {
                PersianDate Pd = new PersianDate();

                // اگر تمام شده بود  
                if (Param.StatusRequest == 0)
                {
                    Param.StatusRequest = Convert.ToInt32(StatusServiceLocationRequest.final);
                    var temp1 = _Debt.OfType<DebtServiceReceiverServiceLocation>().Where(c => c.ServiceReceiverServiceLocationId == Param.ServiceReceiverServiceLocationId).FirstOrDefault();

                    if (temp1 != null)
                    {
                        temp1.CompanyCost = (temp1.CompanyCost / (float)temp1.TotalCost) * Param.PriceCalcBySystem;
                        temp1.TotalCost = Param.PriceCalcBySystem + temp1.TotalCost;
                        temp1.TotalCostReceived = Param.PriceReceived + temp1.TotalCostReceived;
                    }
                }

                else if (Param.StatusRequest == 1) // اگر ناتمام بود
                {
                    Param.StatusRequest = Convert.ToInt32(StatusServiceLocationRequest.Unfinished);
                    var temp2 = _Debt.OfType<DebtServiceReceiverServiceLocation>().Where(c => c.ServiceReceiverServiceLocationId == Param.ServiceReceiverServiceLocationId).FirstOrDefault();

                    if (temp2 != null)
                    {
                        temp2.CompanyCost = (temp2.CompanyCost / temp2.TotalCost) * Param.PriceCalcBySystem + temp2.CompanyCost;
                        temp2.TotalCost = Param.PriceCalcBySystem + temp2.TotalCost;
                        temp2.TotalCostReceived = Param.PriceReceived + temp2.TotalCostReceived;

                    }
                }
                Param.ConfirmServiceReceiver = Convert.ToByte(StatusConfirmServiceReciverRequest.NotChecked);
                Param.PriceReceived = Param.PriceReceived;
                Param.Date = Pd.ConvertFaToEnNumber(Param.Date);
                Param.NextMeeting = Pd.ConvertFaToEnNumber(Param.NextMeeting);
                await _servicereceiveservicelocation.AddServiceReceiverRequest(Param);

                var temp = _ServiceReceiverServiceLocations.Where(c => c.Id == Param.ServiceReceiverServiceLocationId).FirstOrDefault();
                if (temp != null)
                {
                    temp.CalcPrice = temp.CalcPrice + Param.PriceCalcBySystem;
                    temp.Status = Param.StatusRequest;
                    temp.CalcPriceReceived = temp.CalcPriceReceived + Param.PriceReceived;

                    if (Param.StatusRequest == 4)
                    {
                        //------------ ثبت بدهی در صورت اتمام
                        PrivateTraining.ServiceLayer.BLL.PrivateTraining Private = new PrivateTraining.ServiceLayer.BLL.PrivateTraining(_uow,_userManager);
                        Private.setDebtAfterEnd((int)Param.ServiceReceiverServiceLocationId, temp. WorkUnits.Id, temp.ServiceLocationId, temp.ServiceLocations.PercentOfShares, temp.CalcPrice, temp.CalcPriceReceived);
                    }
                }

                int save = await _uow.SaveAllChangesAsync();
                if (save != 0)
                    return Json(new { Resualt = true, Messages = "با موفقیت ثبت شد" });
                else
                    return Json(new { Resualt = false, Messages = "ثبت نشد" });

            }
            catch (Exception ex)
            {

                return Json(new { Resualt = false, Messages = "ثبت نشد" });
            }
        }

        [HttpPost]
        public virtual async Task<JsonResult> loadPrice(ServiceReceiverRequest Param,int ServiceId = 0)
        {
            try
            {
                PersianDate Pd = new PersianDate();
                var FromDateMiladi = Pd.shamsiToMiladi(Param.Date + " " + Param.FromTime);
                var ToDateMiladi = Pd.shamsiToMiladi(Param.Date + " " + Param.ToTime);
                DateTime first = DateTime.Parse(Convert.ToString(FromDateMiladi));
                DateTime second = DateTime.Parse(Convert.ToString(ToDateMiladi));
                TimeSpan ts = second - first;


                var priceCalcBySystem = 0;
                var y = _ServiceReceiverServiceLocations.Where(c => c.Id == Param.ServiceReceiverServiceLocationId);
                var x = _ServiceLocationWorkUnit.Where(c => c.WorkUnitId == y.FirstOrDefault().WorkUnitId && c.ServiceLocationId == y.FirstOrDefault().ServiceLocationId);

                var ServiceLevelId = 0;
                var UserSer = _UserService.Where(c => c.ServiceId == ServiceId && c.UserId == y.FirstOrDefault().ServiceProviderId).FirstOrDefault();
                if (UserSer != null)
                    ServiceLevelId = UserSer.ServiceLevelListId;

                var SL = _ServiceLevelList.Where(c => c.ServicePropertiesId == ServiceId &&  c.Id== ServiceLevelId).FirstOrDefault();
                float Percent = 0;
                if (SL != null)
                {
                    Percent = SL.PercentServiceLevel;
                    Percent = Percent / 100;
                }

                if (x.Count() > 0)
                {
                    if(Percent==0)
                     priceCalcBySystem = Convert.ToInt32((x.FirstOrDefault().PriceWorkUnit / 60) * ts.TotalMinutes);
                    else
                    {
                        var d = (x.FirstOrDefault().PriceWorkUnit) + (Percent * x.FirstOrDefault().PriceWorkUnit);
                        priceCalcBySystem = Convert.ToInt32((d / 60) * ts.TotalMinutes);
                    }
                }

                return Json(new { Result = true, Price = priceCalcBySystem });
            }
            catch (Exception ex)
            {

                return Json(new { Result = false, Message = "خطا" });
            }
        }

        /// <summary>
        /// لود اطلاعات سرویس ارائه شده
        /// </summary>
        [HttpPost]
        public virtual async Task<JsonResult> LoadServiceRequests(int ServiceReceiverServiceLocationId = 0)
        {
            try
            {
                var Temp = _ServiceReceiverRequest.Where(c => c.ServiceReceiverServiceLocationId == ServiceReceiverServiceLocationId).ToList().Select(a => new privatetraining_ServiceReceiverRequests
                {
                    Id = a.Id,
                    Date = a.Date,
                    FromTime = a.FromTime,
                    ToTime = a.ToTime,
                    PriceReceived = a.PriceReceived,
                    PriceCalcBySystem = a.PriceCalcBySystem,
                    ServiceReceiverServiceLocationId = a.ServiceReceiverServiceLocationId,
                    ConfirmServiceReceiver = a.ConfirmServiceReceiver,
                    FullNameServiceReceiver = a.ServiceReceiverServiceLocations.ApplicationReceiverUsers.Name + " " + a.ServiceReceiverServiceLocations.ApplicationReceiverUsers.Family,
                    FullNameServicProvider = a.ServiceReceiverServiceLocations.ApplicationProviderUsers.Name + " " + a.ServiceReceiverServiceLocations.ApplicationProviderUsers.Family,

                }).ToList();
                if (Temp != null)
                {
                    return Json(new { Result = true, Temp = Temp });

                }
                else
                    return Json(new { Result = false, Message = "کاربری یافت  نشد." });
            }
            catch (Exception ex)
            {

                return Json(new { Result = false, Message = "خطا" });
            }
        }


        /// <summary>
        /// لیست خدمات گرفته شده توسط مشتری
        /// </summary>
        [HttpPost]
        public virtual async Task<JsonResult> ListServicesProvidedForServiceReceiver()
        {
            try
            {
                datatable datatable = new datatable();
                datatable = datatableclasses.getdatatalbe(Request);

                var CurrentUserId = Convert.ToInt32(User.Identity.GetUserId());

                var list = _ServiceReceiverRequest.Where(c => c.ServiceReceiverServiceLocations.ServiceReceiverId == 2);

                if (!string.IsNullOrEmpty(datatable.searchValue))
                {

                }

                datatable.recordsTotal = list.Count();
                list = list.OrderBy(c => c.Id).Skip(datatable.skip).Take(datatable.pageSize);


                datatable.data = list.ToList().Select(rec => new string[]
              {
                rec.Id.ToString(),
                rec.Date,
                rec.FromTime,
                rec.ToTime,
                rec.PriceReceived.ToString(),
                rec.PriceCalcBySystem.ToString(),
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
    }
}
