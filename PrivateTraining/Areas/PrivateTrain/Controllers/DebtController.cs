using PrivateTraining.DataLayer.Context;
using PrivateTraining.DomainClasses.Entities.PrivateTraining;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PrivateTraining.DomainClasses.Entities;
using PrivateTraining.BussinessLayer.Generic;
using Microsoft.AspNet.Identity;
using PrivateTraining.DomainClasses.Entities.Security;
using PrivateTraining.ServiceLayer.Interface.PrivateTraining;
using PrivateTraining.DomainClasses.EntitiesView.PrivateTrainig;
using PrivateTraining.ServiceLayer.BLL;
using PrivateTraining.DomainClasses.Entities.BaseTable;
using PrivateTraining.DomainClasses.Entities.FrameWork;
using PrivateTraining.ServiceLayer.Interface;
using System.Web.Routing;
using PrivateTraining.ServiceLayer.Interface.Framework;
using PrivateTraining.Areas.PrivateTrain.Models;

namespace PrivateTraining.Areas.PrivateTrain.Controllers
{
    public partial class DebtController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private IDbSet<Debt> _DebtDb;
        private IDbSet<DebtServiceProvider> _DebtServiceProvider;
        private IDbSet<DebtServiceReceiverServiceLocation> _DebtServiceReceiverServiceLocation;
        private readonly IServiceProviderInfo _ServicesProviderInfo;
        private readonly IServiceReceiverInfo _ServiceReceiverInfo;
        private IDbSet<State> _State;
        private IDbSet<ServiceProperties> _ServiceProperties;
        private IDbSet<ServiceReceiverServiceLocation> _ServiceReceiverServiceLocation;
        private readonly IApplicationUserManager _userManager;
        private IDbSet<Setting> _Setting;
        private readonly IService _Service;
        List<int> ListServiceChild = new List<int>();

        public DebtController(IUnitOfWork uow, IServiceProviderInfo ServicesProviderInfo, IServiceReceiverInfo ServiceReceiverInfo, 
            IApplicationUserManager userManager, IService Service)
        {
            _uow = uow;
            _DebtDb = _uow.Set<Debt>();
            _DebtServiceProvider = _uow.Set<DebtServiceProvider>();
            _DebtServiceReceiverServiceLocation = _uow.Set<DebtServiceReceiverServiceLocation>();
            _ServicesProviderInfo = ServicesProviderInfo;
            _ServiceReceiverInfo = ServiceReceiverInfo;
            _State = _uow.Set<State>();
            _ServiceProperties = _uow.Set<ServiceProperties>();
            _ServiceReceiverServiceLocation = _uow.Set<ServiceReceiverServiceLocation>();
            _userManager = userManager;
            _Setting = _uow.Set<Setting>();
            _Service = Service;
        }

        #region بدهی

        // GET: PrivateTrain/Debt
        public virtual ActionResult GetListDebts()
        {
            ViewBag.ListStates = _State.Where(c => c.IsEnable == true).ToList();
            ViewBag.Services = _ServiceProperties.Where(c => c.IsEnable == true).ToList();

            return View();
        }

        [HttpPost]
        public virtual async Task<JsonResult> ListDetailDebts(int StateId = 0, int CityId = 0, int LocationId = 0, int ServiceId = 0, int TypeDebt = 0, string Name = "", int PriceDebtMoreThan = 0, int CountDebtMoreThan = 0, string DateDebtMoreThan = "")
        {
            try
            {
                datatable datatable = new datatable();
                datatable = datatableclasses.getdatatalbe(Request);
                PersianDate PD = new PersianDate();

                var CurrentUserId = Convert.ToInt32(User.Identity.GetUserId());
                var UserType = Convert.ToByte(ServiceLayer.Extention.UserType.NotUser);

                if (!User.IsInRole("User") && !User.IsInRole("ServiceProvider"))
                {
                    string str = StateId + "," + CityId + "," + LocationId + "," + ServiceId + "," + CurrentUserId + "," + UserType + "," + 0 + "," + TypeDebt + ",'" + Name + "'" + "," + PriceDebtMoreThan + "," + CountDebtMoreThan + ",'" + PD.ConvertFaToEnNumber(DateDebtMoreThan) + "'";
                    var h = "exec [PrivateTraining].[SP_Debts] " + str;

                    var list = _uow.GetRowsWithoutParam<View_Debts>("exec [PrivateTraining].[SP_Debts] " + str).ToList();
                    if (!string.IsNullOrEmpty(datatable.searchValue))
                    {

                    }

                    if (ServiceId != 0)
                    {
                        var services = await _Service.GetAllService();
                        RetrunListChild(ServiceId, services.ToList());
                        ListServiceChild.Add(ServiceId);
                        list = list.Where(x => ListServiceChild.Contains(x.ServiceId)).ToList();
                    }

                    datatable.recordsTotal = list.Count();
                    list = list.OrderByDescending(c => c.Id).Skip(datatable.skip).Take(datatable.pageSize).ToList();

                    datatable.data = list.Select(rec => new string[] {
                            "<div class=\"checkbox\" style=\"margin-right:-20px;padding-left:5px;\"><label><input type=\"checkbox\"  ng-checked=\"all\"  ng-model=\"checkbox[" +rec.Id+"]\" class=\"case\" value=\""+rec.Id +"\" ><span class=\"text\"></span></lable></div>"
                            /*+rec.Id.ToString()*/,
                            rec.TotalCost.ToString("N0"),
                            rec.PercentOfShares.ToString(),
                            rec.CompanyCost.ToString("N0"),
                            rec.Status.ToString(),
                            rec.StatusServiceReceiverServiceLocation.ToString(),
                            rec.ReasonDebt,
                            rec.TotalCostReceived.ToString("N0"),
                            rec.ServiceLocationName,
                            "<span class=\"hideCol \">"+rec.ProviderFullName+"</span>",
                            rec.RecevierFullName,
                        }).OrderByDescending(x => x[1]).ToList();
                }


                else
                {
                    var IsUserServiceProvider = await _ServicesProviderInfo.GetServiceProviderInfo(CurrentUserId);
                    /////////////////////// اگر کاربر خدمتیار بود 
                    if (IsUserServiceProvider != null)
                    {

                        UserType = Convert.ToByte(ServiceLayer.Extention.UserType.ServiceProvider);
                        string str = StateId + "," + CityId + "," + LocationId + "," + ServiceId + "," + CurrentUserId + "," + UserType + "," + 0 + "," + TypeDebt + ",'" + Name + "'" + "," + PriceDebtMoreThan + "," + CountDebtMoreThan + ",'" + PD.ConvertFaToEnNumber(DateDebtMoreThan) + "'";
                        var h = "exec [PrivateTraining].[SP_Debts] " + str;

                        var list = _uow.GetRowsWithoutParam<View_Debts>("exec [PrivateTraining].[SP_Debts] " + str).ToList();
                        if (!string.IsNullOrEmpty(datatable.searchValue))
                        {

                        }

                        if (ServiceId != 0)
                        {
                            var services = await _Service.GetAllService();
                            RetrunListChild(ServiceId, services.ToList());
                            ListServiceChild.Add(ServiceId);
                            list = list.Where(x => ListServiceChild.Contains(x.ServiceId)).ToList();
                        }

                        datatable.recordsTotal = list.Count();
                        list = list.OrderByDescending(c => c.Id).Skip(datatable.skip).Take(datatable.pageSize).ToList();

                        datatable.data = list.Select(rec => new string[] {
                            "<div class=\"checkbox\" style=\"margin-right:-20px;padding-left:5px;\"><label><input type=\"checkbox\"  ng-checked=\"all\"  ng-model=\"checkbox[" +rec.Id+"]\" class=\"case\" value=\""+rec.Id +"\" ><span class=\"text\"></span></lable></div>"
                            /*+rec.Id.ToString()*/,
                            rec.TotalCost.ToString("N0"),
                            rec.PercentOfShares.ToString(),
                            rec.CompanyCost.ToString("N0"),
                            rec.Status.ToString(),
                            rec.StatusServiceReceiverServiceLocation.ToString(),
                            rec.ReasonDebt,
                            rec.TotalCostReceived.ToString("N0"),
                            rec.ServiceLocationName,
                            "<span class=\"hideCol \">"+rec.ProviderFullName+"</span>",
                            rec.RecevierFullName,
                        }).ToList();
                    }
                    ////////////////////////// اگر کاربر خدمتیار نبود
                    // لازم نیست
                    //else
                    //{
                    //    var IsUserServiceReceiver = await _ServiceReceiverInfo.GetServiceReceiverInfo(CurrentUserId);
                    //    ////////////////////////// اگر کاربر مشتری بود
                    //    if (IsUserServiceReceiver != null)
                    //    {
                    //        UserType = Convert.ToByte(ServiceLayer.Extention.UserType.ServiceReceiver);
                    //        var list = _uow.GetRowsWithoutParam<View_Debts>("exec [PrivateTraining].[SP_Debts] " + StateId + "," + CityId + "," + LocationId + "," + ServiceId + "," + CurrentUserId + "," + UserType + "," + 0).ToList();
                    //        if (!string.IsNullOrEmpty(datatable.searchValue))
                    //        {

                    //        }

                    //        datatable.recordsTotal = list.Count();
                    //        list = list.OrderByDescending(c => c.Id).Skip(datatable.skip).Take(datatable.pageSize).ToList();

                    //        datatable.data = list.Select(rec => new string[] {
                    //        "<div class=\"checkbox\" style=\"margin-right:-20px;padding-left:5px;\"><label><input type=\"checkbox\"  ng-checked=\"all\"  ng-model=\"checkbox[" +rec.Id+"]\" class=\"case\" value=\""+rec.Id +"\" ><span class=\"text\"></span></lable></div>"
                    //      /* +rec.Id.ToString()*/,
                    //        rec.TotalCost +" تومان ",
                    //        rec.PercentOfShares+" درصد ",
                    //        rec.CompanyCost+" تومان ",
                    //        rec.Status.ToString(),
                    //        rec.StatusServiceReceiverServiceLocation.ToString(),
                    //        rec.ReasonDebt,
                    //        rec.TotalCostReceived+" تومان ",
                    //        rec.ServiceLocationName,
                    //        rec.ProviderFullName,
                    //       "<span class=\"hideCol \">"+rec.RecevierFullName+"</span>",
                    //        rec.RecevierFullName,
                    //    }).ToList();
                    //    }
                    //}
                }
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

        public void RetrunListChild(int Id, List<Service> Service)
        {
            var TempService = Service.Where(c => c.ParentId == Id);
            foreach (var item in TempService)
            {
                if (ListServiceChild.IndexOf(item.Id) == -1)
                    ListServiceChild.Add(item.Id);
            }
            foreach (var item in TempService)
            {
                RetrunListChild(item.Id, Service);
            }
            // return List;
        }


        [HttpPost]
        public virtual async Task<JsonResult> ListDebtUsers(int StateId = 0, int CityId = 0, int LocationId = 0, int ServiceId = 0)
        {
            try
            {
                datatable datatable = new datatable();
                datatable = datatableclasses.getdatatalbe(Request);
                if (!User.IsInRole("User"))
                {

                }

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

        public virtual ActionResult AddPayment(int[] ListId)
        {
            try
            {

                if (ListId != null)
                {
                    string ListIdSend = "";
                    var sum = 0.0;
                    //var TransactionNumber = 0;
                    //var StatusPayment = 0;
                    PersianDate PD = new PersianDate();
                    var Date = PD.PersianDateLow();
                    var Time = PD.CurrentTime();
                    foreach (var item in ListId)
                    {
                        var t = _DebtDb.Where(c => c.Id == item);
                        sum = sum + t.FirstOrDefault().CompanyCost;
                        ListIdSend += "," + item.ToString();
                    }
                    ListIdSend = ListIdSend.Substring(1, ListIdSend.Count() - 1);
                    var MemberId = Convert.ToInt32(User.Identity.GetUserId());
                    //var ModratorId = MemberId;
                    //DataLayer.Context.ApplicationDbContext context = new DataLayer.Context.ApplicationDbContext();
                    //string Query = "exec PrivateTraining.SP_CalculationPayment '" + MemberId + "','" + ModratorId + "','" + TransactionNumber + "','" + StatusPayment + "','" + ListIdSend + "'";
                    //var List = context.Database.ExecuteSqlCommand(Query);

                    ///// اس ام اس
                    //var ReceiverNumber = _userManager.FindById(MemberId).Mobile;
                    //var ContentSms = _Setting.Where(c => c.Subject == "ConfirmPaymentDebt").FirstOrDefault().Value1;
                    //SendSms SendSms = new SendSms(_uow, _userManager);
                    //SendSms.ContentSmsAndSendSms("", ReceiverNumber, ContentSms, MemberId);

                    //_uow.SaveAllChangesAsync();
                    //return Json(new { result = true, SumPrice = sum, Message = "عملیات با موفقیت انجام شد." });

                 //   return RedirectToAction("CallBackUrlMellats", "Payment", new { Area = "Framework", Price = 0, OrderId = 10 });

                    return RedirectToAction("LoadPageMellat", "Payment", new { Area = "Framework", DebtId = ListIdSend, Price = sum, OrderId = 0, TransactionType = 1, CurrentUserId = MemberId });
                }
                else
                {
                    return Json(new { result = false, Message = "لطفا بدهی خود را انتخاب نمایید" });

                }
            }
            catch (Exception ex)
            {
                return Json(new { result = false, Message = "مشکل در برقراری ارتباط" });
            }
        }

        [HttpPost]
        public virtual async Task<JsonResult> LoadUserforsms(int[] DebtId)
        {
            try
            {
                List<int> List = new List<int>();
                foreach (var item in DebtId)
                {
                    var temp1 = _DebtServiceReceiverServiceLocation.Where(c => c.Id == item).FirstOrDefault();
                    if (temp1 != null)
                    {
                        var SRSL = _ServiceReceiverServiceLocation.Where(c => c.Id == temp1.ServiceReceiverServiceLocationId).FirstOrDefault();
                        if (SRSL != null)
                        {
                            var h = await _ServicesProviderInfo.GetServiceProviderInfo(SRSL.ServiceProviderId);

                            if (!List.Contains(h.Id))
                            {
                                List.Add(h.Id);
                            }

                        }
                    }
                    else
                    {
                        var temp2 = _DebtServiceProvider.Where(c => c.Id == item).FirstOrDefault();
                        if (temp2 != null)
                        {
                            var v = await _ServicesProviderInfo.GetServiceProviderInfo(temp2.ServiceProviderId);

                            if (!List.Contains(v.Id))
                            {
                                List.Add(v.Id);
                            }

                        }
                    }
                }

                return Json(new { Result = true, ListUserId = List });


            }
            catch (Exception ex)
            {

                return Json(new { Result = false, Message = "مشکل در برقراری ارتباط" });
            }
        }

        #endregion

        #region پرداختی

        /// <summary>
        /// تابع عمومی برای همه چاپ ها شامل نمایش . چاپ و اکسپورت
        /// </summary>
        /// <returns></returns>
        #region Public  

      

      
       
        #endregion

        public virtual async Task<ActionResult> GetListPayments(byte PaymentType = 3, int UserId = 0, string FromTime = "", string ToTime = "")
        {
            //PrivateTraining.ServiceLayer.BLL.PrivateTraining p = new PrivateTraining.ServiceLayer.BLL.PrivateTraining(_uow, _userManager);
            //var n = p.AddFromTimeAndToTimeRequest(42, 4, "18:20", "1396/10/18");
            // var f = p.AddPriceReceivedByServiceProvider(23, 15000);

            Session["PaymentType"] = PaymentType;
            Session["UserId"] = UserId;
            Session["FromTime"] = FromTime;
            Session["ToTime"] = ToTime;
            return View();
        }

        [AllowAnonymous]
        public virtual ActionResult StiPrintPayment(byte PaymentType = 3, int UserId = 0, string FromTime = "", string ToTime = "")
        {
            try
            {
                Session["PaymentType"] = "";
                Session["UserId"] = "";
                Session["FromTime"] = "";
                Session["ToTime"] = "";

                PersianDate Pd = new PersianDate();
                FromTime = Pd.ConvertFaToEnNumber(FromTime);
                ToTime = Pd.ConvertFaToEnNumber(ToTime);

                //var Temp = from o in _payment.GetAllPayment(0).Where(c => c.OrderId == UserId)
                //           select new { o.AllPrice, o.SaveDatePayment, o.Users.Name, o.Users.Family, o.OrderId, o.PaymentType };

                //  var list = Temp.ToList().Select(a => new PaymentViewModel
                //{
                //    AllPrice = a.AllPrice,
                //    SaveDatePayment = a.SaveDatePayment,
                //    AllPrice2 = PD.ConvertToCurrencyDecimal(a.AllPrice),
                //    PaymentName = a.Name + " " + a.Family,
                //    OrderId = (int)a.OrderId,
                //    PaymentType = a.PaymentType,
                //}).OrderBy(c => c.SaveDatePayment);

                var list = _uow.GetRowsWithoutParam<PaymentListViewModel>(" select * from View_ListPayment ").ToList();

                var UserIdLogin = Convert.ToInt32(User.Identity.GetUserId());

                //if (UserId != 0)
                //    list = list.Where(c => c.OrderId == UserId).ToList();

                if (User.IsInRole("User") || User.IsInRole("ServiceProvider"))
                    list = list.Where(c => c.MemberId == UserIdLogin).ToList();

                //if (PaymentType != 3)
                //    list = list.Where(c => c.PaymentType == PaymentType).ToList();

                if (FromTime != "")
                    list = list.Where(c => c.Date.CompareTo(FromTime) >= 0).ToList();

                if (ToTime != "")
                    list = list.Where(c => c.Date.CompareTo(ToTime) <= 0).ToList();

                //        ایجاد شی جدید
                return null;
            }
            catch (Exception ex)
            {
                return View();
            }
        }


        #endregion

    }
}