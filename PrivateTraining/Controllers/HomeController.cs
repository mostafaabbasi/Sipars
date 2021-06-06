using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using PrivateTraining.DomainClasses.Entities;
using PrivateTraining.ServiceLayer.Interface;
using PrivateTraining.ServiceLayer.BLL;
using PrivateTraining.Models;
using PrivateTraining.DomainClasses.Security;
using System.Data.Entity;
using PrivateTraining.DataLayer.Context;
using System;

using PrivateTraining.DomainClasses.Entities.BusDriving;
using PrivateTraining.ServiceLayer.Extention;
using PrivateTraining.DomainClasses.Entities.Security;
using PrivateTraining.DomainClasses.Entities.PrivateTraining;
using PrivateTraining.ServiceLayer.Interface.Framework;

namespace PrivateTraining.Controllers
{
    public partial class HomeController : BaseController
    {
        private IUnitOfWork _uow;
        private IDbSet<SupplementaryInfoUser> _SupplementaryInfoUser;
        private IDbSet<ServiceReceiverInfo> _ServiceReceiverInfo;
        private IDbSet<ServiceProperties> _ServiceProperties;
        private readonly IApplicationUserManager _userManager;
        private readonly IMessage _message;
        private IDbSet<ServiceReceiverServiceLocation> _ServiceReceiverServiceLocations;

        //  private IDbSet<LeaveRequest> _Leave;
        //private IDbSet<UserRequest> _UserRequests;

        public HomeController(IUnitOfWork uow, IApplicationUserManager userManager, IMessage message)
        {
            _uow = uow;
            _SupplementaryInfoUser = _uow.Set<SupplementaryInfoUser>();
            _ServiceReceiverInfo = _uow.Set<ServiceReceiverInfo>();
            _ServiceProperties = _uow.Set<ServiceProperties>();
            _userManager = userManager;
            _message = message;
            _ServiceReceiverServiceLocations = _uow.Set<ServiceReceiverServiceLocation>();

            // _Leave = _uow.Set<LeaveRequest>();
            //_UserRequests= _uow.Set<UserRequest>();
        }

        [AllowAnonymous]
        public virtual async Task<ActionResult> Index(int UserId = 0)
        {
            // MachineInfo A = new MachineInfo();
            // ViewData["a"] = A.GetInfo();


            // if (UserId == 0 || User.IsInRole("User") || User.IsInRole("Modrator") || User.IsInRole("Admin"))
            // {
            //     UserId = Convert.ToInt32(User.Identity.GetUserId());
            // }
            // //RegisterUserViewModel ViewModel = new RegisterUserViewModel();
            // //ViewModel.ServiceReceiverInfo = _ServiceReceiverInfo.Where(c =>c.Id == UserId).FirstOrDefault();


            //RegisterViewModel ViewModel = new RegisterViewModel();
            //ViewModel.SupplementaryInfoUsers = _SupplementaryInfoUser.Where(c =>c.Id == UserId).FirstOrDefault();

            // // var countLeave = _Leave.Where(c => c.StatusLeave == (byte)StatusLeave.NotChecked).Count();
            // //ViewBag.countleaves = countLeave;

            // //var countRequest = _UserRequests.Where(c => c.StatusRequest == (byte)StatusRequest.Send).Count();
            // //ViewBag.countrequest = countRequest;



            return View();
        }

        //  [AllowAnonymous]
        //[OverrideAuthorization]
        //[Authorize(Roles = "User,Modrator,Administrator,Admin")]
        public virtual async Task<ActionResult> IndexPanel(int UserId = 0)
        {
            //return RedirectToRoute("/PrivateTrain/ServiceReceiverServiceLocation/ServicesServiceReceiver");
            //return RedirectToAction("Index");
            
            RegisterUserViewModel ViewModel = new RegisterUserViewModel();
            if (UserId == 0)
            {
                if (User.IsInRole("User") || User.IsInRole("Modrator") || User.IsInRole("Admin") || User.IsInRole("ServiceProvider"))
                    UserId = Convert.ToInt32(User.Identity.GetUserId());
            }

            //RegisterViewModel ViewModel = new RegisterViewModel();
            //ViewModel.SupplementaryInfoUsers = _SupplementaryInfoUser.Where(c => c.Id == UserId).FirstOrDefault();

            ViewModel = FunUserInfo(ViewModel, UserId);
            ViewData["CountMessage"] = CountMessages();
            ViewData["CountTransaction"] = CountTransactions();

            return View(ViewModel);
        }

        public RegisterUserViewModel FunUserInfo(RegisterUserViewModel ViewModel, int UserId = 0)
        {
            // ---------------  انتخاب کاربر
            var Query = _userManager.GetAllUsersWithId(UserId);

            //--------------------  اگر کاربر خدمتیار بود
            ViewModel = (from x in Query.OfType<ServiceProviderInfo>()
                         select new RegisterUserViewModel
                         {
                             Id = x.Id,
                             Name = x.Name,
                             Family = x.Family,
                             Sex = x.Sex,
                             HomeAddress = x.HomeAddress,
                             HomePhone = x.HomePhone,
                             Mobile = x.Mobile,
                             NationalCode = x.NationalCode,
                             Picture = x.Picture,
                             Path = x.Path,
                             Email = x.Email,
                             UserName = x.UserName,
                             BrithDay = x.BrithDay,
                             RoleId = x.Roles.Select(r => r.RoleId).FirstOrDefault(),
                             CityId = (int)x.CityId,
                             StateId = x.StateId,
                             StateName = x.States.Name,
                             CityName = x.Cities.Name,
                             RegisterDate = x.RegisterDate,
                             ServiceProviderCode = x.ServiceProviderCode,
                             WorkAddress = x.WorkAddress,
                             WorkPhone = x.WorkPhone,
                             Resume = x.Resume,
                             HowPerformServices = x.HowPerformServices,
                             BankCardNumber = x.BankCardNumber,
                             // Level=x.Level,
                             Disconnect = x.Disconnect,
                             DisconnectDate = x.DisconnectDate,
                             DisconnectReason = x.DisconnectReason,
                         }).FirstOrDefault();

            //--------------------  اگر کاربر مشتری بود
            if (ViewModel == null)
                ViewModel = (from x in Query.OfType<ServiceReceiverInfo>()
                             select new RegisterUserViewModel
                             {
                                 Id = x.Id,
                                 Name = x.Name,
                                 Family = x.Family,
                                 Sex = x.Sex,
                                 HomeAddress = x.HomeAddress,
                                 HomePhone = x.HomePhone,
                                 Mobile = x.Mobile,
                                 NationalCode = x.NationalCode,
                                 Picture = x.Picture,
                                 Path = x.Path,
                                 Email = x.Email,
                                 UserName = x.UserName,
                                 BrithDay = x.BrithDay,
                                 RoleId = x.Roles.Select(r => r.RoleId).FirstOrDefault(),
                                 CityId = (int)x.CityId,
                                 StateId = x.StateId,
                                 StateName = x.States.Name,
                                 CityName = x.Cities.Name,
                                 RegisterDate = x.RegisterDate,
                                 ServiceReceiverCode = x.ServiceReceiverCode,
                                 HomeNumber = x.HomeNumber,
                                 UnitNumber = x.UnitNumber,
                             }).FirstOrDefault();

            //--------------------  اگر کاربری مثل مدیر سایت بود  
            if (ViewModel == null)
                ViewModel = (from x in Query
                             select new RegisterUserViewModel
                             {
                                 Id = x.Id,
                                 Name = x.Name,
                                 Family = x.Family,
                                 Sex = x.Sex,
                                 HomeAddress = x.HomeAddress,
                                 HomePhone = x.HomePhone,
                                 Mobile = x.Mobile,
                                 NationalCode = x.NationalCode,
                                 Picture = x.Picture,
                                 Path = x.Path,
                                 Email = x.Email,
                                 UserName = x.UserName,
                                 BrithDay = x.BrithDay,
                                 RoleId = x.Roles.Select(r => r.RoleId).FirstOrDefault(),
                                 CityId = (int)x.CityId,
                                 StateId = x.StateId,
                                 StateName = x.States.Name,
                                 CityName = x.Cities.Name,
                                 RegisterDate = x.RegisterDate,
                             }).FirstOrDefault();

            return ViewModel;
        }

        [AllowAnonymous]
        public virtual async Task<ActionResult> About()
        {
            return View();
        }

        [AllowAnonymous]
        public virtual ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public virtual async Task<JsonResult> SaveContactUs(string name = "", string email = "", string subject = "", string desc = "")
        {
            try
            {
                IdentityMessage message = new IdentityMessage();
                message.Body = name + "<br />" + email + "<br />" + desc;
                message.Subject = subject;
                message.Destination = "info@sipars.ir";

                PrivateTraining.ServiceLayer.EmailService emails = new PrivateTraining.ServiceLayer.EmailService(_uow);
                await emails.SendAsync(message);

                return Json(new { Resualt = true, Messages = "عملیات با موفقیت انجام شد." });
            }
            catch (Exception Ex)
            {
                return Json(new { Resualt = false, Messages = Ex.Message });
            }
        }

        [AllowAnonymous]
        public virtual ActionResult AcceptableConditions()
        {
            return View();
        }

        [AllowAnonymous]
        public virtual ActionResult Procedures()
        {
            return View();
        }

        [AllowAnonymous]
        public virtual async Task<ActionResult> ShowUserName()
        {
            var UserId = Convert.ToInt32(User.Identity.GetUserId());
            ViewData["UserNameTitle"] = "";
            string RoleName = "";

            var Query = _userManager.GetAllUsersWithId(UserId).FirstOrDefault();
            if (Query != null)
            {
                ViewData["UserNameTitle"] = Query.Name + " " + Query.Family;

                if (User.IsInRole("User"))
                {
                    if (Query.UserType == 2)
                        RoleName = "مشتری";
                    //if (Query.UserType == 1)
                    //    RoleName = "خدمتیار";
                }
                else if (User.IsInRole("ServiceProvider"))
                    RoleName = "خدمتیار";
                else if (User.IsInRole("Admin") || User.IsInRole("Administrator"))
                    RoleName = "مدیرسایت";

            }

            // ViewData["UserNameTitle"] = " کاربر عزیز " + ViewData["UserNameTitle"] + " خوش آمدید. " + "( " + RoleName + " )";

            ViewData["UserNameTitle"] = " " + RoleName + " عزیز " + " <span class=\"text-success\" style=\"font-weight:bold;\" >" + ViewData["UserNameTitle"] + "</span>" + " خوش آمدید. ";
            return View();
        }

        /// <summary>
        /// نمایش تعداد رویدادهای جدید
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public virtual async Task<ActionResult> CountTasks(byte Type = 0)
        {
            string Role = "";
            if (User.IsInRole("Admin") || User.IsInRole("Adminstrator"))
                Role = "Admin";
            else if (User.IsInRole("Modrator"))
                Role = "Modrator";

            // var CountUser = _userManager.GetCountUserNewRegister();
            var CountUser = 0;

            var CountMessage = CountMessages();
            var CountTransaction = CountTransactions();

            var Count = 0;
            if (User.IsInRole("Admin") || User.IsInRole("Administrator"))
                Count = CountUser + CountMessage + CountTransaction;

            else if (User.IsInRole("User"))
                Count = CountMessage + CountTransaction;

            ViewData["Count"] = Count;
            ViewData["CountUser"] = CountUser;
            ViewData["CountMessage"] = CountMessage;
            ViewData["CountTransaction"] = CountTransaction;


            return View();
        }

        public int CountTransactions()
        {
            var checking = (int)StatusServiceLocationRequest.checking;
            var Accept = (int)StatusServiceLocationRequest.Accept;
            var UserId = Convert.ToInt32(User.Identity.GetUserId());

            var Query = _userManager.GetAllUsersWithId(UserId).FirstOrDefault();

            if (User.IsInRole("User"))
            {
                if (Query.UserType == 2)
                    return _ServiceReceiverServiceLocations.Where(c => c.ServiceLocations.Services.automation == true &&
                    c.Status == checking && c.ServiceReceiverId == UserId).Count();
                else if (Query.UserType == 1)
                    return _ServiceReceiverServiceLocations.Where(c => c.ServiceLocations.Services.automation == true &&
                    c.Status == checking && c.ServiceProviderId == UserId).Count();
                else
                    return 0;
            }
            else if (User.IsInRole("Admin") || User.IsInRole("Administrator"))
                return _ServiceReceiverServiceLocations.Where(c => c.ServiceLocations.Services.automation == true && (
                c.Status == checking)).Count();
            else
                return 0;
        }

        public int CountMessages()
        {
            return _message.GetCountUserMessage();
        }

        /// <summary>
        /// نمایش عکس کاربر لاگین شده
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public virtual ActionResult GetPicUser()
        {
            ViewBag.UserPicProfile = "/assets/Alien/contents/default-user.png";
            try
            {
                var UserId = Convert.ToInt32(User.Identity.GetUserId());
                var FileTypes = Convert.ToInt32(FileType.UserPic);
                var pic = _userManager.GetAllUsersWithId(UserId).FirstOrDefault();
                if (pic != null && !string.IsNullOrEmpty(pic.Picture))
                {
                    ViewBag.UserPicProfile = pic.Path + "/" + pic.Picture;
                }
            }
            catch (Exception ex)
            {
            }
            return View();
        }

        [AllowAnonymous]
        public virtual async Task<ActionResult> Rule()
        {
            return View();
        }
    }
}
