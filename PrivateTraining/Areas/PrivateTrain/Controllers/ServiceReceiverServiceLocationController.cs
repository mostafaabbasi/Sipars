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
using Newtonsoft.Json.Linq;
using PrivateTraining.ServiceLayer.Interface.PrivateTraining;
using PrivateTraining.DomainClasses.EntitiesView.PrivateTrainig;
using PrivateTraining.DomainClasses.Entities.FrameWork;
using PrivateTraining.ServiceLayer.BLL;
using PrivateTraining.ServiceLayer.Interface;
using PrivateTraining.ServiceLayer.Extention;

namespace PrivateTraining.Areas.PrivateTrain.Controllers
{
    public partial class ServiceReceiverServiceLocationController : BaseController
    {
        private IUnitOfWork _uow;
        private IDbSet<ServiceReceiverServiceLocation> _ServiceReceiverServiceLocations;
        private IDbSet<FormAnswer> _FormAnswer;
        //   private IDbSet<Message> _Message;
        private IDbSet<CommentPrivate> _Comment;
        private readonly IApplicationUserManager _userManager;


        public ServiceReceiverServiceLocationController(IUnitOfWork uow, IApplicationUserManager userManager)
        {
            _uow = uow;
            _ServiceReceiverServiceLocations = _uow.Set<ServiceReceiverServiceLocation>();
            _FormAnswer = _uow.Set<FormAnswer>();
            //      _Message = _uow.Set<Message>();
            _userManager = userManager;

            _Comment = _uow.Set<CommentPrivate>();
        }

        /// <summary>
        /// لیست خدمات درخواست شده با خدمتیار
        /// </summary>
        public virtual async Task<ActionResult> ServicesServiceProvider()
        {
            return View();
        }

        /// <summary>
        /// لیست خدمات جدید درخواست شده با خدمتیار
        /// </summary>
        public virtual async Task<ActionResult> NewServicesServiceProvider()
        {
            return View();
        }

        /// <summary>
        /// لیست خدمات قطعی درخواست شده با خدمتیار
        /// </summary>
        public virtual async Task<ActionResult> CertainServiceServiceProvider()
        {
            return View();
        }

        /// <summary>
        /// لیست خدمات اتمام یافته درخواست شده با خدمتیار
        /// </summary>
        public virtual async Task<ActionResult> FinishedServicesServiceProvider()
        {
            return View();
        }

        /// <summary>
        /// لیست خدمات ارائه شده توسط خدمتیار
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        public virtual async Task<JsonResult> ListServicesServiceProvider(int Status = 9)
        {
            try
            {
                // غیرقطعی؟؟؟؟؟status=6 
                datatable datatable = new datatable();
                datatable = datatableclasses.getdatatalbe(Request);

                var list = _ServiceReceiverServiceLocations.Where(c => 1 != 1).ToList();

                var CurrentUserId = Convert.ToInt32(User.Identity.GetUserId());

                if (User.IsInRole("Admin") || User.IsInRole("Administrator") || User.IsInRole("Modrator"))
                    list = _ServiceReceiverServiceLocations.ToList();
                else if (User.IsInRole("ServiceProvider"))
                    list = _ServiceReceiverServiceLocations.Where(c => c.ServiceProviderId == CurrentUserId).ToList();
                else
                    list = _ServiceReceiverServiceLocations.Where(c => c.ServiceReceiverId == CurrentUserId).ToList();

                //
                if (Status == 1 || Status == 6 || Status == 0 || Status == 8)
                {
                    list = list.Where(c => c.ServiceLocations.Services.automation == true).ToList();
                }
                //status=8  در حال بررسی یا موافق یا غیرقطعی 
                if (Status == 8)
                {
                    list = list.Where(c => c.Status == 1 || c.Status == 0 || c.Status == 6).ToList();
                }
                //status=7 قطعی شده یا ناتمام 
                else if (Status == 7)
                {
                    list = list.Where(c => c.Status == 2 || c.Status == 3).ToList();
                }

                //status=9 تمام 
                else if (Status != 9)
                {
                    list = list.Where(c => c.Status == Status).ToList();
                }


                if (!string.IsNullOrEmpty(datatable.searchValue))
                {
                    list = list.Where(c => c.ServiceLocations.Services.Title.Contains(datatable.searchValue)
                    || c.ServiceLocations.Locations.Name.Contains(datatable.searchValue)
                    || (c.ApplicationReceiverUsers.Name + " " + c.ApplicationReceiverUsers.Family).Contains(datatable.searchValue)
                    ).ToList();
                }

                datatable.recordsTotal = list.Count();
                list = list.OrderByDescending(c => c.Id).Skip(datatable.skip).Take(datatable.pageSize).ToList();


                datatable.data = list.ToList().Select(rec => new string[]
              {
                rec.Id.ToString(),
                //rec.ServiceLocations.Services.Title ,
                FunShowGroup(rec.ServiceLocations.Services.Id),
                rec.ServiceLocations.Locations.Name,
                rec.ServiceReceiverId.ToString(),
                rec.Status.ToString(),
                rec.DateRegister,
                ShowStatusName(rec.Status),
                FunShowReciverName(rec.ServiceReceiverId,rec.Status),
                rec.ServiceLocations.Services.Id.ToString(),
                JObject.FromObject(rec).ToString()
              }).ToList();

                //datatable.list = JArray.FromObject(list);
                
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

        public string FunShowReciverName(int Id, int Status)
        {
            var Name = "";
            if (Status != 0)
            {
                var s = _userManager.GetAllUsersWithId(Id).FirstOrDefault();
                if (s != null)
                {
                    Name = s.Name + " " + s.Family;
                }
            }
            return Name;
        }

        public string FunShowGroup(int Id)
        {
            PrivateTraining.ServiceLayer.BLL.PrivateTraining PT = new PrivateTraining.ServiceLayer.BLL.PrivateTraining(_uow, _userManager);
            return PT.FunGroupName(Id);
        }

        public string ShowStatusName(int Status)
        {
            string Name = "";
            if (Status == (int)StatusServiceLocationRequest.Accept)
                Name = StatusServiceLocationRequest.Accept.GetDescription();

            if (Status == (int)StatusServiceLocationRequest.certain)
                Name = StatusServiceLocationRequest.certain.GetDescription();

            if (Status == (int)StatusServiceLocationRequest.Unfinished)
                Name = StatusServiceLocationRequest.Unfinished.GetDescription();

            if (Status == (int)StatusServiceLocationRequest.final)
                Name = StatusServiceLocationRequest.final.GetDescription();

            if (Status == (int)StatusServiceLocationRequest.checking)
                Name = StatusServiceLocationRequest.checking.GetDescription();

            if (Status == (int)StatusServiceLocationRequest.UnCertain)
                Name = StatusServiceLocationRequest.UnCertain.GetDescription();

            return Name;
        }

        /// <summary>
        /// تغییر وضعیت خدمت در خواست شده
        /// </summary>
        [HttpPost]
        public virtual async Task<ActionResult> ChangeStatusRequest(byte Status, int ServiceReceiverServiceLocationId = 0, int RequestId = 0, int WorkUnitId = 0, string ReasonCancel = "")
        {
            var CurrentUserId = Convert.ToInt32(User.Identity.GetUserId());

            PrivateTraining.ServiceLayer.BLL.PrivateTraining Private = new PrivateTraining.ServiceLayer.BLL.PrivateTraining(_uow, _userManager);
            var save = Private.ChangeStatusRequestService(ServiceReceiverServiceLocationId, Status, CurrentUserId, WorkUnitId, ReasonCancel);
            if (save == 1)
                return Json(new { Resualt = true, Messages = "با موفقیت ثبت شد" });
            else
                return Json(new { Resualt = false, Messages = "ثبت نشد" });
        }

        /// <summary>
        /// ارجاع خدمت درخواست شده به خدمتیار با بالاترین امتیاز
        /// </summary>
        [HttpPost]
        public virtual async Task<ActionResult> RefrenceServiceReceiverServiceLocations(int Id)
        {
            try
            {
                PrivateTraining.ServiceLayer.BLL.PrivateTraining Private = new PrivateTraining.ServiceLayer.BLL.PrivateTraining(_uow, _userManager);
                var save = await Private.RefrenceServiceReceiverServiceLocations(Id);
                if (save == 1)
                    return Json(new { Resualt = true, Messages = "با موفقیت ثبت شد" });
                else
                    return Json(new { Resualt = false, Messages = "ثبت نشد" });
            }
            catch (Exception)
            {

                return Json(new { Resualt = false, Messages = "خطا" });
            }

        }

        /// <summary>
        /// لیست خدمات ارائه شده به مشتری
        /// </summary>
        public virtual async Task<ActionResult> ServicesServiceReceiver()
        {
            return View();
        }

        /// <summary>
        /// ليست خدمات ارائه شده به خدمت گيرنده برای مشتری
        /// </summary>
        [HttpPost]
        public virtual async Task<JsonResult> ListServicesServiceReceiver(int Status = 9)
        {
            try
            {
                datatable datatable = new datatable();
                datatable = datatableclasses.getdatatalbe(Request);

                var CurrentUserId = Convert.ToInt32(User.Identity.GetUserId());
                // CurrentUserId = 7;

                var list = _ServiceReceiverServiceLocations.Where(c => c.ServiceReceiverId == CurrentUserId).ToList();

                if (Status != 9)
                {
                    list = list.Where(c => c.Status == Status).ToList();
                }

                if (!string.IsNullOrEmpty(datatable.searchValue))
                {
                    list = list.Where(c => c.ServiceLocations.Services.Title.Contains(datatable.searchValue)
                    || c.ServiceLocations.Locations.Name.Contains(datatable.searchValue)
                    || (c.ApplicationProviderUsers.Name + " " + c.ApplicationProviderUsers.Family).Contains(datatable.searchValue)
                    ).ToList();
                }

                datatable.recordsTotal = list.Count();
                list = list.OrderByDescending(c => c.Id).Skip(datatable.skip).Take(datatable.pageSize).ToList();


                datatable.data = list.ToList().Select(rec => new string[]
              {
                rec.Id.ToString(),
               // rec.ServiceLocations.Services.Title ,
                FunShowGroup(rec.ServiceLocations.Services.Id),
                rec.ServiceLocations.Locations.Name,
                rec.ApplicationProviderUsers.Name+" "+rec.ApplicationProviderUsers.Family,

                rec.Status.ToString(),
                //قبلا فرم ارزشیابی پر شده یا نه
               CheckFillFormAssessment( rec.Id,CurrentUserId).ToString(),
               rec.DateRegister,
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

        [HttpPost]
        public int CheckFillFormAssessment(decimal SRSL = 0, int ServiceReciverId = 0)
        {
            try
            {
                var tempSRSL = _ServiceReceiverServiceLocations.Where(c => c.Id == SRSL).FirstOrDefault();
                var temp = _FormAnswer.Where(c => c.ServiceId == tempSRSL.ServiceLocations.ServiceId && c.ServiceProviderId == tempSRSL.ServiceProviderId && c.ServiceReceiverId == tempSRSL.ServiceReceiverId).FirstOrDefault();
                if (temp != null)
                {
                    return 1;
                }
                else
                    return 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        [HttpPost]
        public virtual async Task<JsonResult> ListSRSLserviceReceiver(int UserId = 0)
        {
            try
            {
                var list = _ServiceReceiverServiceLocations.Where(c => c.ServiceReceiverId == UserId);
                var temp = list.ToList().Select(c => new View_ServicesRendered
                {
                    Service = c.ServiceLocations.Services.Title,
                    Location = c.ServiceLocations.Locations.Name,
                    ServiceProvider = c.ApplicationProviderUsers.Name + " " + c.ApplicationProviderUsers.Family,
                    ServiceReceiver = c.ApplicationReceiverUsers.Name + " " + c.ApplicationReceiverUsers.Family,
                    Date = c.DateRegister,
                    Status = Convert.ToByte(c.Status),

                });
                return Json(new { Result = true, SRSL = temp });

            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Messages = ex.Message });
            }
        }

        #region Admin

        /// <summary>
        /// لیست خدمات ارائه شده غیر اتوماسیون 
        /// </summary>
        public virtual async Task<ActionResult> ServicesNonAutomation()
        {
            return View();
        }

        /// <summary>
        /// لیست خدمات درخواست شده غیر اتوماسیون و قطعی 
        /// </summary>
        public virtual async Task<ActionResult> ServicesNonAutomationAndCertain()
        {
            return View();
        }

        /// <summary>
        /// لیست خدمات درخواست شده اتوماسیون  
        /// </summary>
        public virtual async Task<ActionResult> ServicesAutomation()
        {
            return View();
        }

        /// <summary>
        /// لیست  خدمات درخواست  شده 
        /// </summary>
        [HttpPost]
        public virtual async Task<JsonResult> ListServiceReceiverServiceLocations(int Status = 9, bool Automation = true)
        {
            try
            {
                datatable datatable = new datatable();
                datatable = datatableclasses.getdatatalbe(Request);

                var list = _ServiceReceiverServiceLocations.Where(c => c.ServiceLocations.Services.automation == Automation).ToList();
                //status=8 غیراتوماسیون و قطعی
                if (Status == 8)
                {
                    list = list.Where(c => c.Status == 2 || c.Status == 3 || c.Status == 4).ToList();
                }

                //status=9 همه
                else if (Status != 9)
                {
                    list = list.Where(c => c.Status == Status).ToList();
                }

                if (!string.IsNullOrEmpty(datatable.searchValue))
                {
                    list = list.Where(c => c.ServiceLocations.Services.Title.Contains(datatable.searchValue)
                    || c.ServiceLocations.Locations.Name.Contains(datatable.searchValue)
                    || (c.ApplicationReceiverUsers.Name + " " + c.ApplicationReceiverUsers.Family).Contains(datatable.searchValue)
                    || (c.ApplicationProviderUsers.Name + " " + c.ApplicationProviderUsers.Family).Contains(datatable.searchValue)
                    || c.DateRegister.Contains(datatable.searchValue)
                    || c.Id.ToString().Contains(datatable.searchValue)
                    ).ToList();
                }

                datatable.recordsTotal = list.Count();
                list = list.OrderByDescending(c => c.Id).Skip(datatable.skip).Take(datatable.pageSize).ToList();

                datatable.data = list.ToList().Select(rec => new string[]
              {
                rec.Id.ToString(),
              //  rec.ServiceLocations.Services.Title ,
                FunShowGroup(rec.ServiceLocations.Services.Id),
                rec.ServiceLocations.Locations.Name,
                rec.ServiceReceiverId.ToString(),
                rec.Status.ToString(),
                rec.ApplicationProviderUsers.Name +" "+rec.ApplicationProviderUsers.Family,
                rec.ApplicationReceiverUsers.Name+" "+rec.ApplicationReceiverUsers.Family,
                rec.DateRegister,
               FunShowProblem( rec.TypeProblem,rec.Id),
               FunDesc(rec.Id,rec.DateAcceptStatus,rec.DateCertainStatus)
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

        public string FunDesc(decimal Id = 0,string DateAcceptStatus="",string DateCertainStatus="")
        {
            return "<a class=\"btn btn-warning\" href=\"javascript:void(0);\" ng-click=\"FunDesc(" + Id + ",'"+ DateAcceptStatus + "','"+ DateCertainStatus + "')\" ><i class=\"fa fa-info\"></i></a>";
        }

        /// <summary>
        /// وضعیت مشکل
        /// </summary>
        /// <param name="TypeProblem"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public string FunShowProblem(byte TypeProblem = 0, decimal Id = 0)
        {
            string Text = "";
            if (TypeProblem == 0)
                Text = "<span class=\"text-success\" > <i class=\"fa fa-check-circle\" ></i> بدون مشکل  </span>";
            else
                Text = "<a ng-click=\"ShowProblemBox(" + Id + ")\" style=\"cursor:pointer;\" > <span class=\"text-danger\" > <i class=\"fa fa-minus-circle\" ></i>  نمایش مشکل  </span></a>";

            return Text;
        }

        /// <summary>
        /// نمایش دلیل مشکل برا ی مدیر 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<ActionResult> ShowProblem(int Id)
        {
            try
            {
                var Query = _ServiceReceiverServiceLocations.Where(c => c.Id == Id).FirstOrDefault();
                return Json(new { Result = true, Date = Query.DateProblem + " " + Query.TimeProblem, Reason = Query.ReasonProblem });
            }
            catch (Exception)
            {
                return Json(new { Result = false, Messages = "خطا" });
            }
        }

        #endregion Admin


        [HttpPost]
        public virtual async Task<JsonResult> LoadSRSL(int ServiceReceiverServiceLocationId = 0)
        {
            try
            {
                var temp = _ServiceReceiverServiceLocations.Where(c => c.Id == ServiceReceiverServiceLocationId);
                var SRSL = temp.ToList().Select(a => new PrivateTraining_View_ServiceReceiverServiceLocation
                {
                    Id = a.Id,
                    ServiceLocationId = a.ServiceLocationId,
                    ServiceId = a.ServiceLocations.ServiceId,
                    LocationId = a.ServiceLocations.LocationId,
                    ServiceProviderId = a.ServiceProviderId,
                    ServiceProviderFullName = a.ApplicationProviderUsers.Name + " " + a.ApplicationProviderUsers.Family,
                    WorkUnitName = a.WorkUnits.Title,
                    WorkUnitId = (int)a.WorkUnitId,
                }).FirstOrDefault();
                return Json(new { Result = true, SRSl = SRSL });
            }
            catch (Exception ex)
            {
                return Json(new { Result = false });
            }
        }

        // افزودن کامنت برای خدمتیار
        [HttpPost]
        public virtual async Task<JsonResult> AddComment(CommentPrivate Comment, int ServiceReceiverServiceLocationId = 0)
        {
            try
            {
                PersianDate PD = new PersianDate();
                var s = _Comment.Where(c => c.Id == Comment.Id).FirstOrDefault();
                if (s == null)
                {
                    var temp = _ServiceReceiverServiceLocations.Where(c => c.Id == ServiceReceiverServiceLocationId).FirstOrDefault();
                    CommentPrivate TempComment = new CommentPrivate();
                    TempComment.Subject = Comment.Subject;
                    TempComment.Desc = Comment.Desc;
                    TempComment.ReciverUserId = temp.ServiceProviderId;
                    TempComment.SenderUserId = temp.ServiceReceiverId;
                    TempComment.ServiceId = temp.ServiceLocations.ServiceId;
                    TempComment.Date = PD.PersianDateLow();
                    TempComment.Time = PD.CurrentTime();

                    _Comment.Add(TempComment);
                }
                else
                {
                    s.Subject = Comment.Subject;
                    s.Desc = Comment.Desc;
                    s.Date = PD.PersianDateLow();
                    s.Time = PD.CurrentTime();

                }
                await _uow.SaveAllChangesAsync();
                return Json(new { Result = true, Messages = "با موفقیت ثبت شد." });
            }
            catch (Exception ex)
            {

                return Json(new { Result = false, Messages = "خطا در برقراری ارتباط" });
            }
        }
    
        // لود کامنت ذخیره شده
        [HttpPost]
        public virtual async Task<JsonResult> loadComment(int ServiceReceiverServiceLocationId = 0)
        {
            try
            {
                var temp = _ServiceReceiverServiceLocations.Where(c => c.Id == ServiceReceiverServiceLocationId).FirstOrDefault();
                var g = _Comment.Where(c => c.SenderUserId == temp.ServiceReceiverId && c.ReciverUserId == temp.ServiceProviderId && c.ServiceId == temp.ServiceLocations.ServiceId);
                var comment = g.ToList().Select(a => new PrivateTraining.DomainClasses.Entities.PrivateTraining.CommentPrivate
                {
                    Id = a.Id,
                    Subject = a.Subject,
                    Desc = a.Desc,
                    ReciverUserId = a.ReciverUserId,
                    SenderUserId = a.SenderUserId,
                    ServiceId = a.ServiceId,
                }).FirstOrDefault();
                return Json(new { Result = true, comment = comment });

            }
            catch (Exception ex)
            {

                return Json(new { Result = false });
            }
        }

        // لود کامنت ذخیره شده
        [HttpPost]
        public virtual async Task<JsonResult> loadCommentByUserIdANDServiceId(int UserId = 0, int ServiceId = 0)
        {
            try
            {
                var g = _Comment.Where(c => c.ReciverUserId == UserId && c.ServiceId == ServiceId && c.IsEnable == true);
                var comments = g.ToList().Select(a => new PrivateTraining.DomainClasses.Entities.PrivateTraining.CommentPrivate
                {
                    Id = a.Id,
                    Subject = a.Subject,
                    Desc = a.Desc,
                    ReciverUserId = a.ReciverUserId,
                    SenderUserId = a.SenderUserId,
                    ServiceId = a.ServiceId,
                    Date = a.Date,
                    Time = a.Time,
                }).ToList();
                if (comments.Count() != 0)
                    return Json(new { Result = true, comments = comments });
                else
                    return Json(new { Result = false });

            }
            catch (Exception ex)
            {

                return Json(new { Result = false });
            }
        }


    }
}