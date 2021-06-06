using PrivateTraining.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PrivateTraining.ServiceLayer.Interface;
using System.Collections.Generic;
using System.Data;
using Microsoft.Office.Interop.Excel;
using PrivateTraining.DomainClasses.Entities;
using PrivateTraining.BussinessLayer.Generic;
using PrivateTraining.DomainClasses.Security;
using PrivateTraining.DataLayer.Context;
using System.Data.Entity;
using PrivateTraining.ServiceLayer.Extention;
using PrivateTraining.ServiceLayer.BLL;
//using PrivateTraining.ServiceLayer.Interface.BusDriving;
using PrivateTraining.ServiceLayer.Interface.PrivateTraining;
using PrivateTraining.DomainClasses.Entities.Security;
using PrivateTraining.DomainClasses.Entities.BaseTable;
using PrivateTraining.DomainClasses.Entities.PrivateTraining;
using PrivateTraining.DomainClasses.EntitiesView;
using PrivateTraining.DomainClasses.EntitiesView.PrivateTrainig;
using PrivateTraining.ServiceLocationLayer.Interface.PrivateTraining;
using Newtonsoft.Json;
using System.IO;
using PrivateTraining.LocationLayer.Interface.PrivateTraining;
using PrivateTraining.ServiceLayer.Interface.Security;

namespace PrivateTraining.Controllers
{
    //aaaaaaaaa
    //aa
    //frvgrtsg
    //[Authorize]
    public partial class AccountController : BaseController
    {
        private readonly IAuthenticationManager _authenticationManager;
        private readonly IApplicationSignInManager _signInManager;
        private readonly IApplicationUserManager _userManager;
        private IUnitOfWork _uow;
        private IDbSet<SupplementaryInfoUser> _SupplementaryInfoUser;
        private IDbSet<SuspensionUser> _SuspensionUser;
        private IDbSet<CustomRole> _CustomRole;
        private IDbSet<DomainClasses.Security.ApplicationUser> _User;
      //  private readonly ILeaveRequest _LeaveRequest;
        private readonly IServiceProviderInfo _ServiceProviderInfo;
        private readonly IServiceReceiverInfo _ServiceReceiverInfo;
        private readonly IServiceLocation _servicelocation;
        private IDbSet<City> _City;
        private IDbSet<State> _State;
        private IDbSet<Location> _Location;
        private IDbSet<ServiceProperties> _ServiceProperties;
        //private IDbSet<ServiceLocation> _ServiceLocation;
        private IDbSet<UserServiceLocation> _UserServiceLocations;
        private IDbSet<ServiceWorkUnit> _ServiceWorkUnit;
        private IDbSet<UserService> _UserService;
        private IDbSet<UserLocation> _UserLocation;
        private IDbSet<ServiceLocationWorkUnit> _ServiceLocationWorkUnit;
        private IDbSet<DebtServiceProvider> _DebtServiceProvider;
        private IDbSet<Debt> _Debt;
        private IDbSet<UserFile> _UserFile;
        private IDbSet<UserServiceScore> _UserServiceScore;
        private readonly ILocation _RepLocation;
        private IDbSet<ServiceReceiverServiceLocation> _ServiceReceiverServiceLocations;
        private readonly IGroupPolicy _group;
        private IDbSet<PrivateSetting> _privatesetting;
        List<string> ListServiceChild = new List<string>();
        private readonly IService _Service;
        private IDbSet<ServiceLevelList> _ServiceLevelList;
        private readonly IServiceLevel _ServiceLevel;

        public AccountController(IApplicationUserManager userManager,
                                 IApplicationSignInManager signInManager,
                                 IAuthenticationManager authenticationManager,
                                  IUnitOfWork uow,
                                 // ILeaveRequest leaveRequest,
                                  IServiceProviderInfo serviceProviderInfo,
                                 IServiceReceiverInfo serviceReceiverInfo,
                                 IServiceLocation servicelocation,
                                  ILocation location,
                                  IGroupPolicy group, IService Service, IServiceLevel servicelevel
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authenticationManager = authenticationManager;
            _uow = uow;
            _SupplementaryInfoUser = _uow.Set<SupplementaryInfoUser>();
            _SuspensionUser = _uow.Set<SuspensionUser>();
            _CustomRole = _uow.Set<CustomRole>();
            _User = _uow.Set<DomainClasses.Security.ApplicationUser>();
         //   _LeaveRequest = leaveRequest;
            _ServiceProviderInfo = serviceProviderInfo;
            _ServiceReceiverInfo = serviceReceiverInfo;
            _City = _uow.Set<City>();
            _State = _uow.Set<State>();
            _Location = _uow.Set<Location>();
            _ServiceProperties = _uow.Set<ServiceProperties>();
            _UserServiceLocations = _uow.Set<UserServiceLocation>();
            _ServiceWorkUnit = _uow.Set<ServiceWorkUnit>();
            _UserService = _uow.Set<UserService>();
            _UserLocation = _uow.Set<UserLocation>();
            _ServiceLocationWorkUnit = _uow.Set<ServiceLocationWorkUnit>();
            _RepLocation = location;
            _UserServiceScore = _uow.Set<UserServiceScore>();
            _servicelocation = servicelocation;
            _ServiceReceiverServiceLocations = _uow.Set<ServiceReceiverServiceLocation>();
            _ServiceLocationWorkUnit = _uow.Set<ServiceLocationWorkUnit>();
            _DebtServiceProvider = _uow.Set<DebtServiceProvider>();
            _Debt = _uow.Set<Debt>();
            _UserFile = _uow.Set<UserFile>();
            _group = group;
            _privatesetting = _uow.Set<PrivateSetting>();
            _Service = Service;
            _ServiceLevel = servicelevel;
            _ServiceLevelList = _uow.Set<ServiceLevelList>();
        }

        #region PrivateTraining

        [HttpGet]
        public virtual async Task<ActionResult> ListServiceProvider()
        {
            ViewBag.ListStates = _State.Where(c => c.IsEnable == true).ToList();
            ViewBag.Services = _ServiceProperties.Where(c => c.IsEnable == true).ToList();
            return View();
        }


        public void RetrunListChild(int Id, List<Service> Service)
        {
            var TempService = Service.Where(c => c.ParentId == Id);
            foreach (var item in TempService)
            {
                if (ListServiceChild.IndexOf(item.Id.ToString()) == -1)
                    ListServiceChild.Add(item.Id.ToString());
            }
            foreach (var item in TempService)
            {
                RetrunListChild(item.Id, Service);
            }
        }

        //[HttpGet]
        //public virtual async Task<ActionResult> ListNewServiceProvider()
        //{
        //    ViewBag.ListStates = _State.Where(c => c.IsEnable == true).ToList();
        //    ViewBag.Services = _ServiceProperties.ToList();
        //    return View();
        //}

        [HttpPost]
        public virtual async Task<JsonResult> ListServiceProvider(int StateId = 0, int CityId = 0, int LocationId = 0, int ServiceId = 0, int StatusUserServiceLocationId = 5)
        {

            try
            {
                datatable datatable = new datatable();
                datatable = datatableclasses.getdatatalbe(Request);

                var list = await _userManager.GetAllUsers();

                list = list.OfType<ServiceProviderInfo>().Include("UserServices").Include("UserLocations").Include("UserServiceLocations");

                if (StateId != 0 && CityId == 0 && LocationId == 0)
                {
                    list = list.Where(x => x.StateId == StateId);
                }
                else if (StateId != 0 && CityId != 0 && LocationId == 0)
                {
                    list = list.Where(x => x.CityId == CityId);
                }
                else if (StateId != 0 && CityId != 0 && LocationId != 0)
                {
                    list = list.Where(x => x.UserServiceLocations.Any(y => y.LocationId == LocationId));
                }
                if (ServiceId != 0)
                {
                    var services = await _Service.GetAllService();
                    RetrunListChild(ServiceId, services.ToList());
                    ListServiceChild.Add(ServiceId.ToString());

                    //var dddd = ServiceId.ToString() + ",";
                    //foreach (var item2 in ListServiceChild)
                    //{
                    //    dddd += item2 + ",";
                    //}
                    //dddd = dddd.Substring(0, dddd.Length - 1);
                    ////   list = list.Where(x => x.UserServiceLocations.Any(y => dddd.Contains(y.ServiceId.ToString())));
                    //// list = list.Where(x => x.UserServiceLocations.Any(y => y.ServiceId.ToString().Contains(dddd)));

                    // list = list.Where(x => x.UserServiceLocations.Any(y => y.ServiceId == ServiceId));
                    list = list.Where(x => x.UserServiceLocations.Any(y => ListServiceChild.Contains(y.ServiceId.ToString())));
                }
                if (StatusUserServiceLocationId == 6) // خدمت دهنده های جدید
                {
                    list = list.Where(c => c.UserServices.Any(b => b.ActiveServiceForUser == 0 && b.IsEnable == true));
                }
                else if (StatusUserServiceLocationId != 5)
                {
                    list = list.Where(c => c.UserServiceLocations.Any(b => b.StatusServiceLocationUser == StatusUserServiceLocationId));
                }

                if (!string.IsNullOrEmpty(datatable.searchValue))
                {
                    list = list.Where(c => c.Name.Contains(datatable.searchValue) ||
                                           c.Family.Contains(datatable.searchValue) ||
                                           //   c.FatherName.Contains(datatable.searchValue) ||
                                           c.PersonnelId.ToString().Contains(datatable.searchValue)
                    //  ||  c.NationalCode.Contains(datatable.searchValue) ||
                    //   c.ShId.Contains(datatable.searchValue)
                    );
                }

                datatable.recordsTotal = 0;
                if (list != null)
                    datatable.recordsTotal = list.Count();
                list = list.OrderByDescending(c => c.Id).Skip(datatable.skip).Take(datatable.pageSize);

                //var ListUser = _ServiceProviderInfo.ListServiceLocation(0,0);
                //var h = ListUser.Select(a => new ServiceProviderInfo
                //{
                //    Id = a.Id,
                //    Name=a.Name,
                //    Family=a.Family,
                //}).ToList();

                // Select Feild
                datatable.data = list.ToList().Select(rec => new string[]
                {
                 //"<div class=\"checkbox\" style=\"margin-right:-20px;padding-left:5px;\"><label><input type=\"checkbox\" ng-model=\"checkbox["+rec.Id+"]\" class=\"case\" value=\""+rec.Id +"\" ><span class=\"text\"></span></lable></div>"
                 ""
                + rec.Id.ToString(),
              //  rec.Name +" "+ rec.Family + " " + FullInfo(rec.Id),
                 FullInfo(rec.Id,rec.Name +" "+ rec.Family),
                rec.Email,
                rec.Mobile,
                rec.States.Name,
                rec.Cities.Name,
                rec.UserServices.Any(c=>c.UserId==rec.Id).ToString(),
                rec.UserLocations.Any(c=>c.UserId==rec.Id).ToString(),
                ServiceProviderCode(rec.Id),
                SumScores(rec.Id),
                DisconnectProvider(rec.Id),
                rec.RegisterDate,
              //  "Status"
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
        /// قطع و بازگشت همکاری
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="Reason"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public virtual async Task<JsonResult> DisconnectProvideres(int UserId = 0, string Reason = "", bool Type = true)
        {
            try
            {
                var status = 0;
                string TextSms = "";
                var Query = await _ServiceProviderInfo.GetServiceProviderInfo(UserId);
                if (Query != null)
                {
                    PersianDate PD = new PersianDate();
                    if (Type)
                    { // قطع همکاری
                        Query.Disconnect = true;
                        Query.DisconnectDate = PD.PersianDateLow();
                        Query.DisconnectReason = Reason;
                        TextSms = "کاربر گرامی به دلیل " + Reason + " همکاری شما با مجموعه قطع شده است.";
                    }
                    else
                    { //  بازگشت همکاری
                        Query.Disconnect = false;
                        Query.DisconnectDate = "";
                        Query.DisconnectReason = "";
                        TextSms = "کاربر گرامی همکاری شما با مجموعه برگشت داده شد.";
                    }
                    status = await _uow.SaveAllChangesAsync();

                    if (status == 1 && Query.Mobile != null && Query.Mobile != "")
                    {
                        PrivateTraining.ServiceLayer.BLL.SendSms Sms = new PrivateTraining.ServiceLayer.BLL.SendSms(_uow);
                        Sms.SendSmsClass(Query.Mobile, TextSms, Convert.ToInt32(User.Identity.GetUserId()));
                    }
                }
                if (status == 1)
                    return Json(new { Result = true, Message = "عملیات با موفقیت انجام شد." });
                else
                    return Json(new { Result = false, Message = "عملیات با شکست مواجه شد!" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Message = "عملیات با شکست مواجه شد!" });
            }
        }

        /// <summary>
        /// نمایش دلیل قطع همکاری به مدیر
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public virtual async Task<JsonResult> ShowDisconnectReasons(int UserId = 0)
        {
            try
            {
                var status = "";
                var Query = await _ServiceProviderInfo.GetServiceProviderInfo(UserId);
                if (Query != null)
                    status = Query.DisconnectDate + " - " + Query.DisconnectReason;

                return Json(new { Result = true, Message = status });
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Message = "عملیات با شکست مواجه شد!" });
            }
        }

        /// <summary>
        /// نمایش وضعیت همکاری
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public string DisconnectProvider(int UserId = 0)
        {
            var Query = _ServiceProviderInfo.ServiceProviderInfo(UserId);
            if (Query.Disconnect)
                return "<a href=\"javascript:void(0)\" ng-click=\"ShowDisconnectReason(" + UserId + ")\">" + "قطع شده" + "</a>";
            else
                return "<a class=\"btn btn-danger shiny btn-circle btn-xs\" title=\"قطع همکاری\" href=\"javascript:void(0)\" ng-click=\"DisconnectProvider(" + UserId + ")\"><i class=\"fa fa-times\" style=\"color:#fff;\"></i></a>";
        }

        /// <summary>
        /// جمع امتیازات خدمت دهنده
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        private string SumScores(int UserId)
        {
            var S1 = _UserService.Where(c => c.UserId == UserId && c.IsEnable == true).Sum(c => c.ScoreByAdmin);
            var S2 = _UserService.Where(c => c.UserId == UserId && c.IsEnable == true).Sum(c => c.CountScoreByServiceRecivers);
            var S3 = _UserService.Where(c => c.UserId == UserId && c.IsEnable == true).Sum(c => c.CalcScoreByServiceReciverAndSystem);

            return (S1 + S2 + S3).ToString() + " &nbsp; <a href=\"javascript:void(0);\" ng-click=\"ShowAllScore(" + UserId + ")\" > جزئیات </a>";
        }

        [HttpGet]
        public virtual async Task<ActionResult> ListServiceReceiver()
        {
            ViewBag.ListStates = _State.Where(c => c.IsEnable == true).ToList();
            ViewBag.Services = _ServiceProperties.ToList();
            return View();
        }
        /// <summary>
        /// لیست مشتری ها برای مدیر
        /// </summary>
        /// <param name="StateId"></param>
        /// <param name="CityId"></param>
        /// <param name="LocationId"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<JsonResult> ListServiceReceiver(int StateId = 0, int CityId = 0, int LocationId = 0)
        {
            try
            {
                datatable datatable = new datatable();
                datatable = datatableclasses.getdatatalbe(Request);

                var list = await _userManager.GetAllUsers();
                list = list.OfType<ServiceReceiverInfo>();

                if (StateId != 0 && CityId == 0 && LocationId == 0)
                {
                    list = list.Where(x => x.StateId == StateId);
                }
                else if (StateId != 0 && CityId != 0 && LocationId == 0)
                {
                    list = list.Where(x => x.CityId == CityId);
                }
                else if (StateId != 0 && CityId != 0 && LocationId != 0)
                {
                    list = list.Where(x => x.UserServiceLocations.Any(y => y.LocationId == LocationId));
                }

                if (!string.IsNullOrEmpty(datatable.searchValue))
                {
                    list = list.Where(c => c.Name.Contains(datatable.searchValue) ||
                                           c.Family.Contains(datatable.searchValue) ||
                                           //   c.FatherName.Contains(datatable.searchValue) ||
                                           c.PersonnelId.ToString().Contains(datatable.searchValue)
                    //  ||  c.NationalCode.Contains(datatable.searchValue) ||
                    //   c.ShId.Contains(datatable.searchValue)
                    );
                }

                datatable.recordsTotal = list.Count();
                list = list.OrderBy(c => c.Id).Skip(datatable.skip).Take(datatable.pageSize);


                // Select Feild
                datatable.data = list.ToList().Select(rec => new string[]
                {
                 //"<div class=\"checkbox\" style=\"margin-right:-20px;padding-left:5px;\"><label><input type=\"checkbox\" ng-model=\"checkbox["+rec.Id+"]\" class=\"case\" value=\""+rec.Id +"\" ><span class=\"text\"></span></lable></div>"
                rec.Id.ToString(),
                rec.Name +" "+rec.Family,
                rec.Email,
                rec.Mobile,
                rec.States.Name,
                rec.Cities.Name,
                rec.Credit + ""
                //ExitServiceUser(rec.Id).ToString(),
               // ExitLocationUser(rec.Id).ToString(),
                //"Status"



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

        public string ServiceProviderCode(int Userid)
        {
            return _ServiceProviderInfo.GetServiceProviderInfo(Userid).Result.ServiceProviderCode;
        }
        public byte ExitServiceUser(int UserId)
        {
            var exit = _UserService.Where(c => c.UserId == UserId);
            if (exit == null) return 0;
            else return 1;
        }
        public byte ExitLocationUser(int UserId)
        {
            var exit = _UserServiceLocations.Where(c => c.UserId == UserId);
            if (exit == null) return 0;
            else return 1;
        }

        [AllowAnonymous]
        public virtual async Task<ActionResult> GetAddEditServiceProvider(string PmOfZarinPal = "", string PmerrorOfZarinPal = "", string OKZarinPal = "0")
        {

            ViewBag.ListStates = _State.Where(c => c.IsEnable == true).ToList();
            ViewBag.ListCitys = _City.Where(c => c.IsEnable == true).ToList();
            ViewBag.Services = _ServiceProperties.ToList();
            ViewBag.ServiceWorkUnits = _ServiceWorkUnit.ToList();
            ViewBag.UserService = _UserService.ToList();
            ViewBag.UserServiceLocation = _UserServiceLocations.ToList();
            //   ViewBag.ServiceLocation = _servicelocation.GetAllServiceLocation();

            ViewBag.PmOfZarinPals = PmOfZarinPal;
            ViewBag.PmerrorOfZarinPals = PmerrorOfZarinPal;
            ViewBag.OKZarinPals = OKZarinPal;

            return View();
        }


        public virtual async Task<ActionResult> GetAddEditServiceProviderInfo(int UserId=0)
        {
            ViewData["UserIdProvider"] =UserId;
            ViewBag.ListStates = _State.Where(c => c.IsEnable == true).ToList();
            ViewBag.ListCitys = _City.Where(c => c.IsEnable == true).ToList();
            ViewBag.Services = _ServiceProperties.ToList();
            ViewBag.ServiceWorkUnits = _ServiceWorkUnit.ToList();

            var UserIdNew = 0;

            if (User.IsInRole("Admin"))
                UserIdNew = UserId;

            if (UserId ==0)
                UserIdNew = Convert.ToInt32(User.Identity.GetUserId());

            var ListServiceProperties = _ServiceProperties.Include("UserServices").ToList();
            var AllListServiceUser = ListServiceProperties.Select(a => new PrivateTraining.DomainClasses.EntitiesView.PrivateTrainig.PrivateTraining_View_ServiceUsers
            {
                Title = a.Title,
                Id = a.Id,
                ParentId = a.ParentId,
                Level = a.Level,
                IsEnable = a.IsEnable,
                selected = a.UserServices.Any(b => b.ServiceId == a.Id && b.UserId == UserIdNew && b.IsEnable == true),
            }).ToList();
            ViewBag.UserServices = AllListServiceUser;
            var x = _DebtServiceProvider.Where(c => c.ServiceProviderId == UserIdNew);
            if (x.Count() != 0) { ViewBag.paycostRegister = _DebtServiceProvider.Where(c => c.ServiceProviderId == UserIdNew).FirstOrDefault().Status; }

            return View();
        }

        /// <summary>
        /// لود اطلاعات برای ویرایش اطلاعات
        /// </summary>       
        [HttpPost]
        public virtual async Task<JsonResult> LoadEditServiceProvider(int ServiceReceiverServiceLocationId = 0,int UserId=0)
        {
            try
            {
                var UserIdNew = 0;

                if (User.IsInRole("Admin"))
                    UserIdNew = UserId;

                if (UserId == 0)
                    UserIdNew = Convert.ToInt32(User.Identity.GetUserId());

                //       ServiceProviderInfo tempUser = await _ServiceProviderInfo.GetServiceProviderInfo(1);
                //    City tempCity = _City.ToList().Where(c => c.Id == Convert.ToInt32(tempUser.CityId)).FirstOrDefault();
                //   var ListServiceProvider = await _ServiceProviderInfo.GetAllServiceProviderInfo();
                var ListServiceProvider =  _ServiceProviderInfo.GetAllServiceProviderInfoById(UserIdNew);

                var TempUser = ListServiceProvider.ToList().Select(a => new ServiceProviderInfo
                {
                    Id = a.Id,
                    Name = a.Name,
                    Family = a.Family,
                    Email = a.Email,
                    Mobile = a.Mobile,
                    HomeAddress = a.HomeAddress,
                    StateId = a.StateId,
                    CityId = a.CityId,
                    Sex = a.Sex,
                    HomePhone = a.HomePhone,
                    BrithDay = a.BrithDay,
                    WorkPhone = a.WorkPhone,
                    WorkAddress = a.WorkAddress,
                    Picture = a.Picture,
                    Path = a.Path,
                    //NationalCard = a.NationalCard,
                    //DegreeEducation = a.DegreeEducation,
                    //Vocational = a.Vocational,
                    ServiceProviderCode = a.ServiceProviderCode,
                    ServiceCode = a.ServiceCode,
                    LocationCode = a.LocationCode,
                    UserCode = a.UserCode,
                    HowPerformServices = a.HowPerformServices,
                    BankCardNumber = a.BankCardNumber,
                    Resume = a.Resume,

                }).FirstOrDefault();

                var documents = _UserFile.Where(c => c.UserId == UserIdNew).ToList().Select(r => new UserFile
                {
                    Id = r.Id,
                    UserId = r.UserId,
                    FileName = r.FileName,
                    PathDocumentations = r.PathDocumentations,
                    FileType = r.FileType,
                });

                var ListServiceProperties = _ServiceProperties.ToList();
                var TempAllListServiceUser = ListServiceProperties.Select(a => new PrivateTraining.DomainClasses.EntitiesView.PrivateTrainig.PrivateTraining_View_ServiceUsers
                {
                    Title = a.Title,
                    Id = a.Id,
                    selected = a.UserServices.Any(b => b.ServiceId == a.Id && b.UserId == UserIdNew && b.ActiveServiceForUser == 1 && b.IsEnable == true),
                }).ToList();

                var ListServiceSelectUser = _UserService.ToList().Where(c => c.UserId == UserIdNew && c.IsEnable == true).Select(a => new UserService
                {
                    Id = a.Id,
                    ServiceId = a.ServiceId,
                    SpecialConditionsOfWork = a.SpecialConditionsOfWork,
                }).ToList();


                var TempListServiceUserLevel1 = ListServiceProperties.Where(c => c.Level == 1).Select(a => new PrivateTraining.DomainClasses.EntitiesView.PrivateTrainig.PrivateTraining_View_ServiceUsers
                {
                    Title = a.Title,
                    Id = a.Id,
                    selected = a.UserServices.Any(b => b.ServiceId == a.Id && b.UserId == UserIdNew && b.ActiveServiceForUser == 1 && b.IsEnable == true),
                }).ToList();

                var ListStates = _State.Where(c => c.IsEnable == true).ToList().Select(a => new State
                {
                    Id = a.Id,
                    Name = a.Name,
                }).ToList();

                var cityid = 0;
                if (TempUser != null)
                {
                    cityid = (int)TempUser.CityId;
                }

                var Listcities = _City.Where(c => c.IsEnable == true).ToList().Where(c => c.Id == cityid).Select(a => new City
                {
                    Id = a.Id,
                    Name = a.Name,
                }).ToList();

                View_ApproveService ServiceProviderTemp = new View_ApproveService();

                if (ServiceReceiverServiceLocationId != 0)
                {
                    var Temp = _ServiceReceiverServiceLocations.Where(c => c.Id == ServiceReceiverServiceLocationId).FirstOrDefault();
                    ServiceProviderTemp.ServiceId = Temp.ServiceLocations.ServiceId;
                    ServiceProviderTemp.ServiceProviderId = Temp.ServiceProviderId;
                    ServiceProviderTemp.ServiceProviderInfoFamily = Temp.ApplicationProviderUsers.Family;
                    ServiceProviderTemp.ServiceProviderInfoName = Temp.ApplicationProviderUsers.Name;

                }

                return Json(new { Result = true, TempUser = TempUser, TempAllListServiceUser = TempAllListServiceUser, TempListServiceUserLevel1 = TempListServiceUserLevel1, ListStates = ListStates, Listcities = Listcities, ServiceProviderTemp = ServiceProviderTemp, ListServiceSelectUser = ListServiceSelectUser, documents = documents });
            }
            catch (Exception ex)
            {
                return Json(new { Resualt = false, Messages = ex.Message });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public virtual JsonResult CheckCapacityServiceProviderForService(int ServiceId = 0, int LocationId = 0)
        {
            try
            {
                var resultcapacity = 1;//طرقیت خالی وجود ندارد
                var Message = "";
                bool ShowPayOnline = false;
                //  var TempService = _ServiceProperties.Where(c => c.Id == ServiceId).FirstOrDefault();
                var TempServiceLocation = _servicelocation.GetAllServiceLocationWirhServiceIds(ServiceId, LocationId);

                var priceRegister = _ServiceProperties.Where(c => c.Id == ServiceId).FirstOrDefault().PriceRegisterServiceProvider;

                // ----------------  تعداد دفعاتی که این خدمت توسط خدمت دهنده ها انتخاب شده
                var CountUserService = _UserService.Where(c => c.ServiceId == ServiceId).Count();
                //-------------- حداکثر ظرفیت تعریف شده برای خدمت
                //   var CountwithReserves = Math.Floor((TempService.PercentCountReservation / 100.0) * TempService.MaxCapacity) + TempService.MaxCapacity;

                //-------------- حداکثر ظرفیت تعریف شده برای خدمت محل
                var CountwithReserves = Math.Floor((TempServiceLocation.PercentCountReservationService / 100.0) * TempServiceLocation.MaxCapacityService) + TempServiceLocation.MaxCapacityService;


                //  if (CountUserService <= TempService.MaxCapacity)
                if (CountUserService <= TempServiceLocation.MaxCapacityService)
                {
                    resultcapacity = 2;//ظرفیت خالی وجود دارد.
                    Message = " ظرفیت خالی برای خدمت وجود دارد ، در صورت تمایل  کلید ثبت نام را بزنید و هزینه ثبت نام را پرداخت نمایید.";
                }
                else if (CountUserService <= CountwithReserves)
                {
                    resultcapacity = 3;//ظرفیت برای رزرو وجود دارد.
                    Message = " ظرفیت خالی برای خدمت وجود ندارد اما ظرفیت رزرو وجود دارد ، در صورت تمایل به ثبت در لیست رزرو ها کلید ثبت نام را بزنید و هزینه ثبت نام را پرداخت نمایید.";
                }
                else
                {
                    Message = "  برای خدمت انتخابی شما در حال حاضر ظرفیت خالی وجود ندارد، در صورت تمایل کلید ذخیره اطلاعات را بزنید تا در صورت ایجاد ظرفیت با شما ارتباط برقرار شود.";
                }

                //---------------------
                var setting = _privatesetting.Where(c => c.IsEnable == true).FirstOrDefault();
                if (setting != null)
                {
                    ShowPayOnline = setting.ShowPayOnline;
                }

                return Json(new { Result = true, Messages = Message, resultcapacity = resultcapacity, priceRegister = priceRegister, ShowPayOnline = ShowPayOnline });
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Messages = ex.Message });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public virtual JsonResult CountCapacity(int LocationId = 0)
        {
            try
            {
                var TempServiceLocation = _servicelocation.GetAllServiceLocationWithLocationId(LocationId);

 
                var P = (byte)StatusServiceLocationUser.PendingApproval;
            var A = (byte)StatusServiceLocationUser.Active;
            var R = (byte)StatusServiceLocationUser.Reservation;
            var U = (byte)StatusServiceLocationUser.SubmitInformation;

//            var CountUserService = UserServiceLocation.Where(c => c.ServiceId == ServiceId && c.LocationId== LocationId &&
//           (c.StatusServiceLocationUser == P || c.StatusServiceLocationUser == A || c.StatusServiceLocationUser == U )).Count();
//
//            //------------- اختلاف حداکثر ظرفیت از ثبت نام شده ها
//            string s = "ظرفیت باقیمانده : " + (MaxCapacity - CountUserService).ToString() + " &nbsp; - &nbsp; ";
//
//            //-------------- تعداد رزرو مجاز
//            var CountReserves = Math.Floor((PercentCountReservation / 100.0) * MaxCapacity);
//            //-------------- رزرو شده ها
//            var Reserves = UserServiceLocation.Where(c => c.ServiceId == ServiceId && c.StatusServiceLocationUser == R).Count();
//            //--------------  باقیمانده رزرو
//            s += "ظرفیت رزرو : " + (CountReserves - Reserves).ToString();
            
                if (TempServiceLocation != null)
                {
                    var f = TempServiceLocation.Count();
                    string[] cap = new string[f];
                    int i = 0;

                    var userServiceCountList = _UserServiceLocations.Where(usl => usl.LocationId == LocationId &&
                                                       (usl.StatusServiceLocationUser == P ||
                                                        usl.StatusServiceLocationUser == A ||
                                                        usl.StatusServiceLocationUser == U));
                                                        
                    
                    foreach (var item in TempServiceLocation)
                    {
                        string CountCapacity = "ظرفیت باقیمانده : " + (item.MaxCapacityService - userServiceCountList.Where(usl => usl.ServiceId == item.ServiceId).Count()).ToString() + " &nbsp; - &nbsp; ";
                        var CountReserves = Math.Floor((item.PercentCountReservationService / 100.0) * item.MaxCapacityService);
                        var Reserves = _UserServiceLocations.Where(c => c.ServiceId == item.ServiceId && c.StatusServiceLocationUser == R).Count();
                        CountCapacity += "ظرفیت رزرو : " + (CountReserves - Reserves).ToString();
                        
                        cap[i] = item.ServiceId.ToString() + "," + CountCapacity;
                        //   cap[i, 1] = CountCapacity;
                        i++;
                    }
                    return Json(new { Result = true, Messages = cap });
                }
                else
                    return Json(new { Result = false, Messages = "" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Messages = ex.Message });
            }
        }

        /// <summary>
        /// افزودن خدمت دهنده
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        public virtual async Task<JsonResult> AddServiceProvider(RegisterUserViewModel param, int ServiceId, int statusCapacity = 1, bool SendSms = false)
        {
            try
            {
                param.RoleId = 5;
                IdentityResult result;
                IdentityResult result2;
                var TempDebtId = 0;
                PersianDate PD = new PersianDate();
                param.NationalCode = PD.ConvertFaToEnNumber(param.NationalCode);
                param.Mobile = PD.ConvertFaToEnNumber(param.Mobile);

                ServiceProviderInfo user = new ServiceProviderInfo();
                //------------  ست شدن اطلاعات کاربر
                SetUserInformation(user, param);
                var list = await _userManager.GetAllUsers();
                //------------ تخصیص کد به خدمت دهنده
                CreateUserCode(ServiceId, user, param);
                //------------- ایجاد پسورد random
                Random rand = new Random();
                var pass = rand.Next(100000, 999999);

                //  user.UserName = user.ServiceProviderCode;
                user.UserName = user.NationalCode;
                result = await _userManager.CreateAsync(user, Convert.ToString(pass));

                if (result.Succeeded)
                {
                    var result3 = await _userManager.SetLockoutEnabledAsync(user.Id, false);
                    if (param.RoleId == (int)Roles.User)
                        result2 = await _userManager.AddToRoleAsync(user.Id, "User");
                    else if (param.RoleId == (int)Roles.Admin)
                        result2 = await _userManager.AddToRoleAsync(user.Id, "Admin");
                    else if (param.RoleId == (int)Roles.Modrator)
                        result2 = await _userManager.AddToRoleAsync(user.Id, "Modrator");
                    else if (param.RoleId == (int)Roles.ServiceProvider)
                        result2 = await _userManager.AddToRoleAsync(user.Id, "ServiceProvider");

                    DefineGroupUser("خدمت دهنده", user.Id);
                    //------------- تعریف خدمت های کاربر
                    var Price = DefineUserService(ServiceId, user, param, statusCapacity, TempDebtId, pass);
                    var Status = await _uow.SaveAllChangesAsync();

                    //----------------------------- ارسال ایمیل و پیامک ثبت نام خدمت دهنده
                    if (!SendSms) // اگر پرداخت آنلاین غیرفعال بود
                    {
                        try
                        {
                            string Domainn = Request.Url.Host;
                            string Title = " خدمات آنلاین ";
                            //if (user.Email != null && user.Email != "")
                            //{
                            //    PrivateTraining.ServiceLayer.EmailService email = new PrivateTraining.ServiceLayer.EmailService(_uow);
                            //    email.SendEmailRegister(Domain, user.Name, user.Family, user.Mobile, pass.ToString(), user.Email, Title);
                            //}
                            if (user.Mobile != null && user.Mobile != "")
                            {
                                PrivateTraining.ServiceLayer.BLL.SendSms Sms = new PrivateTraining.ServiceLayer.BLL.SendSms(_uow);
                                Sms.SensSmsRegisterProvider(Domainn, user.Name, user.Family, user.NationalCode, pass.ToString(), user.Mobile, Convert.ToInt32(User.Identity.GetUserId()), Title, user.Sex);
                            }
                        }
                        catch (Exception ex) { }
                    }
                    //------------------------------------------
                    string[] RE = Price.Result.Split('|');

                    Session["RegisterPassWord"] = pass;
                    Session["RegisterUserName"] = user.UserName;

                    if (statusCapacity != 1)
                        return Json(new { Resualt = true, Messages = "ثبت نام شما با موفقیت انجام شد .", Password = pass, UserName = user.UserName, UserId = user.Id, priceRegister = RE[0], TempDebtId = RE[1] });
                    else
                        return Json(new { Resualt = true, Messages = "اطلاعات شما با موفقیت در سامانه ثبت  شد ، در صورت نیاز با شما تماس گرفته  می شود.", Password = pass, UserId = user.Id, priceRegister = RE[0], TempDebtId = RE[1] });

                }
                else
                {
                    string Messages = "", PM = "";
                    foreach (var item in result.Errors)
                    {
                        Messages += item;
                    }

                    if (Messages.IndexOf("Passwords must have at least one lowercase ('a'-'z').") != -1 ||
                        Messages.IndexOf("Passwords must have at least one digit ('0'-'9').") != -1
                        || Messages.IndexOf("Passwords must have at least one uppercase ('A'-'Z').") != -1)
                        PM += "کلمه عبور باید شامل حروف بزرگ و کوچک و ترکیب اعداد باشد." + "<br/>";

                    if (Messages.IndexOf("Email") != -1)
                        PM += "ایمیل وارد شده نامعتبر است. " + "<br/>";

                    if (PM != "")
                        return Json(new { Resualt = false, Messages = PM });
                    else
                        return Json(new { Resualt = false, Messages = result.Errors.ToList()[0] });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Resualt = false, Messages = ex.Message });
            }
        }

        public async void DefineGroupUser(string Name = "", int UserId = 0)
        {
            var GroupId = _group.GetAllIGroupPolicySearchName(Name);
            if (GroupId != 0)
            {
                PrivateTraining.DomainClasses.Entities.Security.GroupPolicyUser GroupPolicyUser = new PrivateTraining.DomainClasses.Entities.Security.GroupPolicyUser();
                GroupPolicyUser.UserId = UserId;
                GroupPolicyUser.GroupPolicyId = GroupId;
                await _group.AddGroupPolicyUser(GroupPolicyUser);
                _uow.SaveAllChanges();
            }
        }

        /// <summary>
        ///  ایجاد کد خدمت دهنده
        /// </summary>
        /// <param name="ServiceId"></param>
        /// <param name="user"></param>
        /// <param name="param"></param>
        private async void CreateUserCode(int ServiceId, ServiceProviderInfo user, RegisterUserViewModel param)
        {
            //----------- کد سرویس
            var tempServiceCode = _ServiceProperties.Where(c => c.Id == ServiceId).FirstOrDefault().ServiceCode;
            //----------- کد محل
            var tempLocationCode = _Location.Where(c => c.Id == param.LocationId).FirstOrDefault().LocationCode;

            user.ServiceCode = tempServiceCode;
            user.LocationCode = tempLocationCode;

            //------------ لیست همه خدمت دهنده ها
            var AllServiceProvider = await _ServiceProviderInfo.GetAllServiceProviderInfo();
            //-------------   آخرین شماره خدمت دهنده ثبت شده در این خدمت و محل
            var maxusercodeServiceProvider = AllServiceProvider.Where(c => c.ServiceCode == tempServiceCode && c.LocationCode == tempLocationCode).Max(c => c.UserCode);

            if (maxusercodeServiceProvider != null)
            {
                //--------------- ایجاد کد جدید  ---  
                var TempUserCodePlus1 = string.Format("{0:00}", Convert.ToInt32(maxusercodeServiceProvider) + 1);
                user.UserCode = TempUserCodePlus1;
                user.ServiceProviderCode = tempLocationCode + "" + tempServiceCode + "" + TempUserCodePlus1;
            }
            else
            {
                //---------------  دفعه اول
                user.UserCode = "01";
                user.ServiceProviderCode = tempLocationCode + "" + tempServiceCode + "" + user.UserCode;
            }
        }

        /// <summary>
        /// تعریف خدمت
        /// </summary>
        /// <param name="ServiceId"></param>
        /// <param name="user"></param>
        /// <param name="param"></param>
        /// <param name="statusCapacity"></param>
        /// <param name="TempDebtId"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        public async Task<string> DefineUserService(int ServiceId, ServiceProviderInfo user, RegisterUserViewModel param, int statusCapacity = 1, int TempDebtId = 0, int pass = 0)
        {
            //--------------  خدمات کاربر
            UserService userService = new UserService();
            userService.ServiceId = ServiceId;
            userService.UserId = user.Id;
            var currentUserid = Convert.ToInt32(User.Identity.GetUserId());
            _UserService.Add(userService);

            //--------------  محل های کاربر
            UserLocation userlocation = new UserLocation();
            userlocation.LocationId = param.LocationId;
            userlocation.UserId = user.Id;
            _UserLocation.Add(userlocation);

            // var resultcapacity = 1;//طرقیت خالی وجود ندارد
            double priceRegister = 0;

            //-------------- چک کردن وجود خدمت محل 
            var exitServiceLocation = _servicelocation.GetAllServiceLocation().Result.FirstOrDefault(c => c.LocationId == param.LocationId && c.ServiceId == ServiceId);

            if (exitServiceLocation != null)
            {
                //------------- جدول خدمت محل های کاربر
                UserServiceLocation userservicelocation = new UserServiceLocation();
                userservicelocation.ServiceLocationId = exitServiceLocation.Id;
                userservicelocation.UserId = user.Id;
                userservicelocation.LocationId = param.LocationId;
                userservicelocation.ServiceId = ServiceId;
                priceRegister = _ServiceProperties.Where(c => c.Id == ServiceId).FirstOrDefault().PriceRegisterServiceProvider;

                if (statusCapacity == 2)//ظرفیت خالی وجود دارد.
                    userservicelocation.StatusServiceLocationUser = (byte)StatusServiceLocationUser.PendingApproval;
                else if (statusCapacity == 3)//ظرفیت برای رزرو وجود دارد
                    userservicelocation.StatusServiceLocationUser = (byte)StatusServiceLocationUser.Reservation;
                else //ظرفیت خالی وجود ندارد.
                    userservicelocation.StatusServiceLocationUser = (byte)StatusServiceLocationUser.SubmitInformation;

                _UserServiceLocations.Add(userservicelocation);
                if (statusCapacity == 2 || statusCapacity == 3)
                {
                    PersianDate pd = new PersianDate();
                    //---------------- ثبت بدهی برای کاربر
                    DebtServiceProvider TempDebtServiceProvider = new DebtServiceProvider();
                    TempDebtServiceProvider.TotalCost = priceRegister;
                    TempDebtServiceProvider.PercentOfShares = 100;
                    TempDebtServiceProvider.ServiceProviderId = user.Id;
                    TempDebtServiceProvider.CompanyCost = priceRegister;
                    TempDebtServiceProvider.Status = 0;
                    TempDebtServiceProvider.Date = pd.PersianDateLow();
                    _DebtServiceProvider.Add(TempDebtServiceProvider);
                    _uow.SaveAllChanges();

                    TempDebtId = TempDebtServiceProvider.Id;
                }
            }

            return priceRegister + "|" + TempDebtId;
        }

        /// <summary>
        /// ویرایش خدمت دهنده
        /// </summary>
        [HttpPost]
        public virtual async Task<JsonResult> EditServiceProvider(List<HttpPostedFileBase> Picture, List<HttpPostedFileBase> NationalCard, List<HttpPostedFileBase> DegreeEducation, List<HttpPostedFileBase> Vocational, List<HttpPostedFileBase> OtherDocuments, string model, string ServiceIdDescs,int UserId=0)
        {
            try
            {

                var priceRegister = 0;
                var CurentUserId = 0;

                if (UserId == 0)
                    CurentUserId = Convert.ToInt32(User.Identity.GetUserId());
                else if(User.IsInRole("Admin"))
                    CurentUserId = UserId;

                var PathImagesTemp = "/UserFiles/Documentations";
                string[] splitS = ServiceIdDescs.Split(',');
                var splitSlenght = splitS.Length;


                var param = JsonConvert.DeserializeObject<RegisterUserViewModel>(model);
                List<string> ListServiceLocationNew = new List<string>();


                var item = await _ServiceProviderInfo.GetServiceProviderInfo(CurentUserId);
                if (item != null)
                {
                    item.HomeAddress = param.HomeAddress;
                    item.HomePhone = param.HomePhone;
                    item.StateId = param.StateId;
                    item.CityId = param.CityId;
                    item.Sex = param.Sex;
                    item.BrithDay = param.BrithDay;
                    item.WorkPhone = param.WorkPhone;
                    item.WorkAddress = param.WorkAddress;
                    item.HowPerformServices = param.HowPerformServices;
                    item.Resume = param.Resume;
                    item.BankCardNumber = param.BankCardNumber;

                    await _uow.SaveAllChangesAsync();

                    var listUserServiceOld = _UserService.Where(c => c.UserId == CurentUserId).OrderBy(c => c.ServiceId).ToList();
                    int LastId = 0;

                    // آپدیت سرویس های کاربر در جدول یوزر سرویس
                    for (int i = 0; i < splitSlenght; i += 2)
                    {
                        if (param.ServiceId.IndexOf(Convert.ToInt32(splitS[i])) == -1)
                            param.ServiceId.Add(Convert.ToInt32(splitS[i]));

                        var listUserServiceOld2 = _UserService.Where(c => c.UserId == CurentUserId).ToList();
                        if (listUserServiceOld2.Count > 0)
                        {

                            var exitUserService = listUserServiceOld2.Where(c => c.ServiceId == Convert.ToInt32(splitS[i])).FirstOrDefault();
                            if (exitUserService == null)
                            {
                                UserService userService = new UserService();
                                userService.ServiceId = Convert.ToInt32(splitS[i]);
                                userService.UserId = CurentUserId;
                                userService.CountSTarScoreServiceUser = 0;
                                userService.ActiveServiceForUser = 0;
                                userService.SpecialConditionsOfWork = splitS[i + 1];
                                _UserService.Add(userService);
                                var Status = await _uow.SaveAllChangesAsync();
                            }
                            else
                            {
                                if (LastId == Convert.ToInt32(splitS[i])) // حذف سرویس تکراری
                                {
                                    var fd = Convert.ToInt32(splitS[i]);
                                    //var DelService333 = _UserService.Find(fd);
                                    var DelService333 = _UserService.Where(c => c.UserId == CurentUserId && c.ServiceId == fd).FirstOrDefault();
                                    if (DelService333 != null)
                                        _UserService.Remove(DelService333);
                                }
                                else {
                                    exitUserService.SpecialConditionsOfWork = splitS[i + 1];
                                    // exitUserService.ActiveServiceForUser = 0;
                                    exitUserService.IsEnable = true;

                                }
                            }
                            LastId = Convert.ToInt32(splitS[i]);

                        }
                        else
                        {
                            UserService userService = new UserService();
                            userService.ServiceId = Convert.ToInt32(splitS[i]);
                            userService.UserId = CurentUserId;

                            userService.SpecialConditionsOfWork = splitS[i + 1];
                            _UserService.Add(userService);
                            var Status = await _uow.SaveAllChangesAsync();
                        }
                    }

                    //   list = list.Where(x => x.UserServiceLocations.Any(y => ListServiceChild.Contains(y.ServiceId.ToString())));

                    //var DelService = _UserService.Where(c => c.UserId == CurentUserId && !ServiceIdDescs.Contains(c.ServiceId.ToString()));

                    //-------13970711

                    List<string> ServiceIdDescs2 = new List<string>();
                    var d2 = ServiceIdDescs.Split(',');
                    for (int i = 0; i <= d2.Length - 1; i++)
                    {
                        if (d2[i] != "" && d2[i] != null)
                            ServiceIdDescs2.Add(d2[i]);
                    }

                    var DelService = _UserService.Where(c => c.UserId == CurentUserId && !ServiceIdDescs2.Contains(c.ServiceId.ToString()));

                    if (DelService != null)
                    {
                        foreach (var itemss in DelService)
                        {
                            itemss.IsEnable = false;
                            // var s =  _uow.SaveAllChangesAsync();
                        }
                    }


                    //foreach (var item2 in param.ServiceId)
                    //{
                    //    var listUserServiceOld2 = _UserService.Where(c => c.UserId == CurentUserId).ToList();
                    //    if (listUserServiceOld2.Count > 0)
                    //    {

                    //        var exitUserService = listUserServiceOld2.Where(c => c.ServiceId == item2).FirstOrDefault();
                    //        if (exitUserService == null)
                    //        {
                    //            UserService userService = new UserService();
                    //            userService.ServiceId = item2;
                    //            userService.UserId = CurentUserId;
                    //            userService.ScoreServiceUser = 0;
                    //            userService.ActiveServiceForUser = 0;
                    //            userService.UpdateScoreByUserId = CurentUserId;
                    //            _UserService.Add(userService);
                    //            var Status = await _uow.SaveAllChangesAsync();
                    //        }
                    //        else { exitUserService.IsEnable = true; }
                    //    }
                    //    else
                    //    {
                    //        UserService userService = new UserService();
                    //        userService.ServiceId = item2;
                    //        userService.UserId = CurentUserId;
                    //        userService.ScoreServiceUser = 0;
                    //        userService.UpdateScoreByUserId = CurentUserId;
                    //        userService.ActiveServiceForUser = 0;
                    //        _UserService.Add(userService);
                    //        var Status = await _uow.SaveAllChangesAsync();
                    //    }
                    //}
                    /////////////////////////////////////////////////////
                    if (param.BankCardNumber != "")
                    {

                        //var ListDebt = _Debt.OfType<DebtServiceProvider>();
                        //ListDebt.Where(c=>c.ServiceProviderId==CurentUserId && );
                        /// هزینه پرداختی از کدام جدول باید خوانده شود ؟؟؟
                        priceRegister = 5;

                        ProblemReturnRegisterPrice Problemtemp = new ProblemReturnRegisterPrice();
                        PersianDate Pd = new PersianDate();
                        Problemtemp.ActiveByAdmin = false;
                        Problemtemp.Date = Pd.PersianDateLow();
                        Problemtemp.Time = Pd.CurrentTime();
                        Problemtemp.serviceProviderId = CurentUserId;
                        Problemtemp.StatusReturnPrice = 0;
                        Problemtemp.PriceRegister = priceRegister;
                    }

                    //-------13970711
                    List<string> DelServices = new List<string>();
                    foreach (var item3 in param.ServiceId)
                    {
                        DelServices.Add(item3.ToString());
                    }

                    var listDelUserService = listUserServiceOld.Where(c => !DelServices.Contains(c.ServiceId.ToString())).ToList();
                    foreach (var item3 in listDelUserService)
                    {
                        //_UserService.Remove(item3);
                        item3.IsEnable = false;
                        await _uow.SaveAllChangesAsync();
                    }


                    // آپدیت محل های کاربر در جدول یوزر لوکیشن
                    var listUserLocationOld = _UserLocation.Where(c => c.UserId == CurentUserId).ToList();
                    foreach (var itemlocationid in param.Locations)
                    {
                        var listUserLocationOld2 = _UserLocation.Where(c => c.UserId == CurentUserId).ToList();
                        if (listUserLocationOld2.Count > 0)
                        {

                            var exitUserService = listUserLocationOld2.Where(c => c.LocationId == itemlocationid).FirstOrDefault();

                            if (exitUserService == null)
                            {
                                UserLocation userlocation = new UserLocation();
                                userlocation.LocationId = itemlocationid;
                                userlocation.UserId = CurentUserId;
                                _UserLocation.Add(userlocation);
                                var Status = await _uow.SaveAllChangesAsync();
                            }
                            else { exitUserService.IsEnable = true; }
                        }
                        else
                        {
                            UserLocation userlocation = new UserLocation();
                            userlocation.LocationId = itemlocationid;
                            userlocation.UserId = CurentUserId;
                            _UserLocation.Add(userlocation);
                            var Status = await _uow.SaveAllChangesAsync();
                        }
                    }

                    //-------13970711
                    List<string> DelLocations = new List<string>();
                    foreach (var item2 in param.Locations)
                    {
                        DelLocations.Add(item2.ToString());
                    }

                    var listDelUserLocation = listUserLocationOld.Where(c => !DelLocations.Contains(c.LocationId.ToString())).ToList();
                    foreach (var itemlocationid2 in listDelUserLocation)
                    {
                        // _UserLocation.Remove(itemlocationid2);
                        itemlocationid2.IsEnable = false;
                        await _uow.SaveAllChangesAsync();
                    }



                    // آپدیت سرویس ها و محل های کاربر در جدول یوزر سرویس لوکیشن 
                    var listUserServiceLocationOld = _UserServiceLocations.Where(c => c.UserId == CurentUserId).ToList();
                    int LastIdL = 0;
                    foreach (var itemLocationId in param.Locations)
                    {
                      //  Array.Sort(param.ServiceId);
                        foreach (var itemServiceId in param.ServiceId)
                        {

                            var AllServiceLocation = await _servicelocation.GetAllServiceLocation();
                            var TempUserServiceLocationId = AllServiceLocation.Where(c => c.LocationId == itemLocationId && c.ServiceId == itemServiceId);
                            if (TempUserServiceLocationId.Count() != 0)
                            {
                                var servicelocationId = TempUserServiceLocationId.FirstOrDefault().Id;
                                ListServiceLocationNew.Add(servicelocationId.ToString());
                                var listUserServiceLocationOld2 = _UserServiceLocations.Where(c => c.UserId == CurentUserId).ToList();

                                if (listUserServiceLocationOld2.Count > 0)
                                {
                                    var yExit = listUserServiceLocationOld2.Where(c => c.ServiceLocationId == servicelocationId).FirstOrDefault();

                                    if (yExit == null)
                                    {
                                        UserServiceLocation userService = new UserServiceLocation();
                                        userService.LocationId = itemLocationId;
                                        userService.UserId = CurentUserId;
                                        userService.ServiceLocationId = servicelocationId;
                                        userService.StatusServiceLocationUser = Convert.ToByte(StatusServiceLocationUser.SubmitInformation);
                                        userService.ServiceId = itemServiceId;
                                        userService.IsEnable = true;
                                        _UserServiceLocations.Add(userService);

                                        var Status = await _uow.SaveAllChangesAsync();
                                    }
                                    else {

                                        //if (LastIdL == Convert.ToInt32(itemServiceId)) // حذف سرویس تکراری
                                        //{
                                        //    var DelServiceLll = _UserServiceLocations.Where(c => c.UserId == CurentUserId && c.ServiceId == itemServiceId 
                                        //    && c.LocationId== itemLocationId).FirstOrDefault();
                                        //    if (DelServiceLll != null)
                                        //        _UserServiceLocations.Remove(DelServiceLll);
                                        //}
                                        //else {
                                            yExit.IsEnable = true;
                                      //  }
                                    }
                                    LastIdL = Convert.ToInt32(itemServiceId);

                                }
                                else
                                {
                                    UserServiceLocation userService = new UserServiceLocation();
                                    userService.LocationId = itemLocationId;
                                    userService.UserId = CurentUserId;
                                    userService.ServiceLocationId = servicelocationId;
                                    userService.StatusServiceLocationUser = Convert.ToByte(StatusServiceLocationUser.SubmitInformation);
                                    userService.ServiceId = itemServiceId;
                                    userService.IsEnable = true;

                                    _UserServiceLocations.Add(userService);
                                    var Status = await _uow.SaveAllChangesAsync();
                                }
                            }
                        }
                    }


                    var DelListUserServiceLocation = listUserServiceLocationOld.Where(c => !ListServiceLocationNew.Contains(c.ServiceLocationId.ToString())).ToList();
                    foreach (var item5 in DelListUserServiceLocation)
                    {
                        //_UserServiceLocations.Remove(item5);
                        item5.IsEnable = false;
                        await _uow.SaveAllChangesAsync();
                    }

                    var DelServiceL = _UserServiceLocations.Where(c => c.UserId == CurentUserId && !ServiceIdDescs2.Contains(c.ServiceId.ToString()));
                    if (DelServiceL != null)
                    {
                        foreach (var itemss in DelServiceL)
                        {
                            itemss.IsEnable = false;
                            //   itemss.StatusServiceLocationUser = 2;
                            //   var s = _uow.SaveAllChangesAsync();
                        }
                    }



                    //// save upload files
                    var tempUserFile = _UserFile.Where(z => z.UserId == CurentUserId).ToList();

                    if (Picture.Count() != 0)
                    {
                        foreach (var itemPicture in Picture)
                        {
                            if (itemPicture != null && itemPicture.ContentLength > 0)
                            {
                                var PathProfilePictureTemp = "/UserFiles/ProfilePicture";

                                //  var PathProfilePictureTemp = "~/UserFiles/ProfilePicture";
                                string FileType = itemPicture.FileName.Substring(itemPicture.FileName.LastIndexOf("."), itemPicture.FileName.Length - itemPicture.FileName.LastIndexOf("."));
                                var fileName = "Picture_" + item.ServiceProviderCode + Path.GetFileName(FileType);
                                var path = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath(PathProfilePictureTemp.ToString()), fileName);
                                if (item.Picture != null && item.Picture != fileName)
                                {
                                    var pathOldFileName = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath(PathProfilePictureTemp.ToString()), item.Picture);
                                    System.IO.File.Delete(pathOldFileName);
                                }
                                itemPicture.SaveAs(path);
                                item.Picture = fileName;
                            }
                        }
                    }

                    if (NationalCard.Count() != 0)
                    {
                        var i = 1;
                        foreach (var itemNationalCard in NationalCard)
                        {
                            if (itemNationalCard != null && itemNationalCard.ContentLength > 0)
                            {
                                string FileType = itemNationalCard.FileName.Substring(itemNationalCard.FileName.LastIndexOf("."), itemNationalCard.FileName.Length - itemNationalCard.FileName.LastIndexOf("."));
                                var fileName = "NationalCard" + i + "_" + item.ServiceProviderCode + Path.GetFileName(FileType);
                                var path = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath(PathImagesTemp.ToString()), fileName);
                                // پاک کردن عکس 
                                var pathOldFileName = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath(PathImagesTemp.ToString()), fileName);
                                System.IO.File.Delete(pathOldFileName);
                                foreach (var x in tempUserFile.Where(c => c.FileType == 0).ToList())
                                {//  پاک کردن رکوردهای دیتابیس                              
                                    _UserFile.Remove(x);
                                }

                                //if (item.NationalCard != null && item.NationalCard != fileName)
                                //{
                                //    var pathOldFileName = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath(PathImagesTemp.ToString()), item.NationalCard);
                                //    System.IO.File.Delete(pathOldFileName);
                                //}

                                itemNationalCard.SaveAs(path);
                                UserFile userfile2 = new UserFile();
                                userfile2.FileType = 0;
                                userfile2.PathDocumentations = PathImagesTemp;
                                userfile2.UserId = CurentUserId;
                                userfile2.FileName = fileName;
                                _UserFile.Add(userfile2);
                            }
                            i++;
                        }
                    }

                    if (DegreeEducation != null)
                    {
                        var i = 1;
                        foreach (var itemDegreeEducation in DegreeEducation)
                        {
                            if (itemDegreeEducation != null && itemDegreeEducation.ContentLength > 0)
                            {
                                string FileType = itemDegreeEducation.FileName.Substring(itemDegreeEducation.FileName.LastIndexOf("."), itemDegreeEducation.FileName.Length - itemDegreeEducation.FileName.LastIndexOf("."));
                                var fileName = "DegreeEducation" + i + "_" + item.ServiceProviderCode + Path.GetFileName(FileType);
                                var path = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath(PathImagesTemp.ToString()), fileName);
                                // پاک کردن عکس 
                                var pathOldFileName = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath(PathImagesTemp.ToString()), fileName);
                                System.IO.File.Delete(pathOldFileName);
                                foreach (var x in tempUserFile.Where(c => c.FileType == 1).ToList())
                                {//  پاک کردن رکوردهای دیتابیس                              
                                    _UserFile.Remove(x);
                                }
                                itemDegreeEducation.SaveAs(path);
                                UserFile userfile2 = new UserFile();
                                userfile2.FileType = 1;
                                userfile2.PathDocumentations = PathImagesTemp;
                                userfile2.UserId = CurentUserId;
                                userfile2.FileName = fileName;
                                _UserFile.Add(userfile2);
                            }
                            i++;
                        }
                    }
                    if (Vocational != null)
                    {
                        var i = 1;

                        foreach (var itemVocational in Vocational)
                        {
                            if (itemVocational != null && itemVocational.ContentLength > 0)
                            {
                                string FileType = itemVocational.FileName.Substring(itemVocational.FileName.LastIndexOf("."), itemVocational.FileName.Length - itemVocational.FileName.LastIndexOf("."));
                                var fileName = "Vocational" + i + "_" + item.ServiceProviderCode + Path.GetFileName(FileType);
                                var path = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath(PathImagesTemp.ToString()), fileName);
                                // پاک کردن عکس 
                                var pathOldFileName = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath(PathImagesTemp.ToString()), fileName);
                                System.IO.File.Delete(pathOldFileName);
                                foreach (var x in tempUserFile.Where(c => c.FileType == 2).ToList())
                                {//  پاک کردن رکوردهای دیتابیس                              
                                    _UserFile.Remove(x);
                                }
                                itemVocational.SaveAs(path);
                                UserFile userfile2 = new UserFile();
                                userfile2.FileType = 2;
                                userfile2.PathDocumentations = PathImagesTemp;
                                userfile2.UserId = CurentUserId;
                                userfile2.FileName = fileName;
                                _UserFile.Add(userfile2);
                            }
                            i++;
                        }
                    }

                    if (OtherDocuments != null)
                    {
                        var i = 1;
                        foreach (var itemOtherDocument in OtherDocuments)
                        {
                            if (itemOtherDocument != null && itemOtherDocument.ContentLength > 0)
                            {
                                string FileType = itemOtherDocument.FileName.Substring(itemOtherDocument.FileName.LastIndexOf("."), itemOtherDocument.FileName.Length - itemOtherDocument.FileName.LastIndexOf("."));
                                var fileName = "OtherDocuments" + i + "_" + item.ServiceProviderCode + Path.GetFileName(FileType);
                                var path = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath(PathImagesTemp.ToString()), fileName);
                                // پاک کردن عکس 
                                var pathOldFileName = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath(PathImagesTemp.ToString()), fileName);
                                System.IO.File.Delete(pathOldFileName);
                                foreach (var x in tempUserFile.Where(c => c.FileType == 3).ToList())
                                {//  پاک کردن رکوردهای دیتابیس                              
                                    _UserFile.Remove(x);
                                }
                                itemOtherDocument.SaveAs(path);
                                UserFile userfile2 = new UserFile();
                                userfile2.FileType = 3;
                                userfile2.PathDocumentations = PathImagesTemp;
                                userfile2.UserId = CurentUserId;
                                userfile2.FileName = fileName;
                                _UserFile.Add(userfile2);
                            }
                            i++;
                        }
                    }

                    var d = await _uow.SaveAllChangesAsync();
                    return Json(new { Result = true, Messages = "با موفقیت ثبت شد" });
                }
                else
                {
                    return Json(new { Result = false, Messages = "ثبت نشد" });
                }

            }

            catch (Exception ex)
            {
                return Json(new
                {
                    Result = false,
                    Messages = ex.Message
                });
            }
        }


        /// <summary>
        /// پر کردن فیلدهای جدول اطلاعات کاربر از مدل فرستاده شده
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        private ServiceProviderInfo SetUserInformation(ServiceProviderInfo user, RegisterUserViewModel model)
        {
            PersianDate PD = new PersianDate();

            user.UserName = model.Mobile;
            user.Email = model.Email;
            user.Mobile = model.Mobile;
            user.Name = model.Name;
            user.Family = model.Family;
            user.Sex = model.Sex;
            user.NationalCode = model.NationalCode;
            //user.HomeAddress = model.HomeAddress;
            //user.HomePhone = model.HomePhone;
            user.CityId = model.CityId;
            user.StateId = model.StateId;
            user.LockoutEnabled = false;
            user.Deleted = (byte)DeleteUserRecord.Show;
            user.UserType = (byte)UserType.ServiceProvider;
            user.Path = "/UserFiles/ProfilePicture";
            user.HowPerformServices = model.HowPerformServices;
            user.YearBrithDay = "0";
            user.MonthBrithDay = "0";
            user.DayBrithDay = "0";
            user.RegisterDate = PD.PersianDateLow();
            return user;
        }


        ///// <summary>
        ///// نمایش شهر ها
        ///// </summary>
        ///// <param name="StateId"></param>
        ///// <param name="DefaultCityId"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public virtual async Task<JsonResult> ListCity(int StateId, int DefaultCityId = 0)
        //{
        //    try
        //    {
        //        var ListCity = _City.ToList().Where(c => c.StateId == StateId).ToList().Select(a => new City
        //        {
        //            Id = a.Id,
        //            Name = a.Name,
        //        }).ToList();

        //        return Json(new { list = ListCity, Resualt = true });
        //    }
        //    catch (Exception)
        //    {
        //        return Json(new { Resualt = false, JsonRequestBehavior.AllowGet });
        //    }
        //}

        /// <summary>
        /// لیست خدمت دهنده ها بر اساس خدمت و محل زمان درخواست
        /// </summary>
        /// <param name="param"></param>
        /// <param name="Sex"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public virtual async Task<JsonResult> ListServiceProviderLocation(RegisterUserViewModel param, int Sex = 0, int ServiceLevelListId = 0)
        {
            try
            {
                //var list = await _userManager.GetAllUsers();
                //list = list.OfType<ServiceProviderInfo>();
                //var serviceLocationId = param.ServiceLocationId[0];
                int ServiceId = param.ServiceId[0];
                int LocationId = param.LocationId;

                //var ServiceLocationId = 0;
                // var exitServiceLocationId = _servicelocation.GetAllServiceLocation().Result.Where(x => x.ServiceId == ServiceId && x.LocationId == param.LocationId);
                // if (exitServiceLocationId.Count() != 0)
                // {
                //     ServiceLocationId = exitServiceLocationId.FirstOrDefault().Id;

                // }
                //var ListServiceProvider = _UserServiceLocations.Include("Users").Where(x => x.ServiceLocationId == ServiceLocationId && x.IsEnable == true && x.StatusServiceLocationUser == 1).ToList();
                //if (Sex == 1)
                //{
                //    var ListServiceLocation = ListServiceProvider.Where(x => x.Users.Sex == false).ToList();
                //}
                //if (Sex == 2)
                //{
                //    var ListServiceLocation = ListServiceProvider.Where(x => x.Users.Sex == true).ToList();
                //}
                //var ListUser = ListServiceProvider.ToList().Select(a => new ServiceProviderInfo
                //{
                //    Id = a.Users.Id,
                //    Name = a.Users.Name,
                //    Family = a.Users.Family,
                //    Sex = a.Users.Sex,
                //    Picture = a.Users.Picture,
                //    Path = a.Users.Path,

                //}).ToList();
                var ListUser = _ServiceProviderInfo.ListServiceLocation(ServiceId, LocationId);
                //var PriceServiceLocation = _ServiceLocationWorkUnit.Where(c => c.ServiceLocationId == serviceLocationId).FirstOrDefault().PriceWorkUnit;

                if (ServiceLevelListId != 0)
                    ListUser = ListUser.Where(c => c.ServiceLevelListId == ServiceLevelListId).ToList();

                return Json(new { Result = true, list = ListUser, PriceServiceLocation = 0, Messages = "true " });

            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Messages = "false" });
            }

        }

        /// <summary>
        /// چک کردن موبایل تکراری
        /// </summary>
        [HttpPost]
        public virtual async Task<JsonResult> CheckMobileUser(string Mobile)
        {
            try
            {
                bool SW = await _userManager.CheckMobileNumber(Mobile);
                if (SW)
                    return Json(new { Resualt = true, Userexist = false, Messages = "" });
                else
                    return Json(new { Resualt = true, Userexist = true, Messages = "" });

            }
            catch (Exception)
            {
                return Json(new { Resualt = false, Messages = "" });
            }

        }


        /// <summary>
        /// لود اطلاعات مشتری برای ویرایش
        /// </summary>
        [AllowAnonymous]
        [HttpPost]
        public virtual async Task<JsonResult> LoadEditServiceReciever(int ServiceProviderId = 0, int serviceReceiverId = 0)
        {
            try
            {


                var CurrentUserId = Convert.ToInt32(User.Identity.GetUserId());
                var ListStates = _State.Where(c => c.IsEnable == true).ToList().Select(a => new State
                {
                    Id = a.Id,
                    Name = a.Name,
                }).ToList();

                var Listcities = _City.Where(c => c.IsEnable == true).ToList().Select(a => new City
                {
                    Id = a.Id,
                    Name = a.Name,
                }).ToList();

                dynamic TempUser;

                if (CurrentUserId != 0)
                {
                    if (serviceReceiverId == 0)
                    {
                        serviceReceiverId = CurrentUserId;
                    }
                    else if (ServiceProviderId == 0)
                    {
                        ServiceProviderId = CurrentUserId;
                    }
                    //var ServiceProviderInfo = await _ServiceReceiverInfo.GetServiceReceiverInfo(ServiceProviderId);
                    var ServiceProviderInfo = await _ServiceProviderInfo.GetServiceProviderInfo(ServiceProviderId);
                    if (ServiceProviderInfo != null)
                    {
                        TempUser = _ServiceReceiverInfo.GetAllServiceReceiverInfo().Result.Where(c => c.Id == serviceReceiverId).ToList().Select(a => new View_ApproveService
                        {
                            Name = a.Name,
                            Family = a.Family,
                            Email = a.Email,
                            Mobile = a.Mobile,
                            HomeAddress = a.HomeAddress,
                            HomeNumber = a.HomeNumber,
                            UnitNumber = a.UnitNumber,
                            CityName = a.Cities.Name,
                            StateName = a.States.Name,
                            HomePhone = a.HomePhone,

                            Sex = a.Sex,
                            CityId = (int)a.CityId,

                            StateId = a.StateId,
                            ServiceProviderId = ServiceProviderInfo.Id,
                            ServiceProviderInfoName = ServiceProviderInfo.Name,
                            ServiceProviderInfoFamily = ServiceProviderInfo.Family,
                        }).FirstOrDefault();
                    }
                    else
                    {
                        TempUser = _ServiceReceiverInfo.GetAllServiceReceiverInfo().Result.Where(c => c.Id == serviceReceiverId).ToList().Select(a => new View_ApproveService
                        {
                            Name = a.Name,
                            Family = a.Family,
                            Email = a.Email,
                            Mobile = a.Mobile,
                            HomeAddress = a.HomeAddress,
                            HomeNumber = a.HomeNumber,
                            UnitNumber = a.UnitNumber,
                            CityName = a.Cities.Name,
                            StateName = a.States.Name,
                            HomePhone = a.HomePhone,

                            Sex = a.Sex,
                            CityId = (int)a.CityId,

                            StateId = a.StateId,

                        }).FirstOrDefault();
                    }

                    if (TempUser != null)
                    {
                        int TempCityId = TempUser.CityId;
                        Listcities = _City.ToList().Where(c => c.Id == TempCityId).Select(a => new City
                        {
                            Id = a.Id,
                            Name = a.Name,
                        }).ToList();

                        var ListLocation = _RepLocation.GetAllLocation().Result.Where(c => c.CityId == TempCityId).ToList().Select(a => new PrivateTraining.DomainClasses.EntitiesView.PrivateTrainig.PrivateTraining_View_LocationUsers
                        {
                            Id = a.Id,
                            Name = a.Name,
                            selected = a.UserLocations.Any(b => b.LocationId == a.Id && b.UserId == serviceReceiverId),
                        }).ToList();

                        return Json(new { Result = true, TempUser = TempUser, ListStates = ListStates, Listcities = Listcities, ListLocation = ListLocation });
                    }
                    else
                        return Json(new { Result = false, Message = "کاربری یافت  نشد.", ListStates = ListStates });

                }
                return Json(new { Result = true, ListStates = ListStates });

            }
            catch (Exception ex)
            {

                return Json(new { Result = false, Message = ex.Message });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public virtual async Task<JsonResult> LoadServiceProviderMaxScore(int ServiceId = 0, int LocationId = 0, int ServiceLocationId = 0, int Sex = 0
            , int ServiceLevelListId = 0)
        {
            try
            {
                var boolSex = true;
                if (Sex == 1)
                {//مرد 
                    boolSex = false;
                }
                else
                {//زن
                    boolSex = true;
                }

                if (ServiceLocationId == 0)
                {
                    var exitServiceLocationId = _servicelocation.GetAllServiceLocation().Result.Where(x => x.ServiceId == ServiceId && x.LocationId == LocationId);
                    ServiceLocationId = exitServiceLocationId.FirstOrDefault().Id;
                }
                else
                {
                    double MaxScore = 0;
                    var UserMaxScore1 = 0;

                    var userserviceLocation = _UserServiceLocations.Where(a => a.ServiceLocationId == ServiceLocationId && a.IsEnable == true && a.StatusServiceLocationUser == 1).ToList();
                    if (userserviceLocation.Count() != 0)
                    {
                        foreach (var item in userserviceLocation)
                        {
                            //13960605
                            var h = 0;
                            var temp = _ServiceReceiverServiceLocations.Where(x => x.ServiceProviderId == item.UserId && (x.Status == 0 || x.Status == 1 || x.Status == 2 || x.Status == 3)).ToList();
                            if (temp.Count() > 0)
                                h = temp.Count();

                            var exitUserService = _UserService.Include("Users").Where(b => b.UserId == item.UserId && b.ServiceId == ServiceId && b.CapacityServiceUser >= h && b.IsEnable == true && b.ActiveServiceForUser == 1 && b.Users.Sex == boolSex && b.ServiceLevelListId == ServiceLevelListId);
                            if (exitUserService.ToList().Count() != 0)
                            {
                                var UserService = exitUserService.OrderByDescending(c => c.CountSTarScoreServiceUser).FirstOrDefault();
                                if (MaxScore <= UserService.CountSTarScoreServiceUser)
                                {
                                    MaxScore = UserService.CountSTarScoreServiceUser;
                                    UserMaxScore1 = UserService.UserId;
                                }
                            }
                        }
                        if (UserMaxScore1 != 0)
                        {
                            var ListServiceProvider = await _ServiceProviderInfo.GetAllServiceProviderInfo();
                            var TempUser = ListServiceProvider.Where(c => c.Id == UserMaxScore1).ToList().Select(a => new ServiceProviderInfo
                            {
                                Id = a.Id,
                                Name = a.Name,
                                Family = a.Family,
                                Email = a.Email,
                                Mobile = a.Mobile,
                                HomeAddress = a.HomeAddress,
                                StateId = a.StateId,
                                CityId = a.CityId,
                                Sex = a.Sex,
                                HomePhone = a.HomePhone,
                                BrithDay = a.BrithDay,
                                WorkPhone = a.WorkPhone,
                                WorkAddress = a.WorkAddress,
                                Picture = a.Picture,
                                //NationalCard = a.NationalCard,
                                //DegreeEducation = a.DegreeEducation,
                                //Vocational = a.Vocational,
                            }).FirstOrDefault();

                            return Json(new { Result = true, TempUser = TempUser });
                        }
                        
                    }
                }
                
                return Json(new { Result = false, Message = "با عرض پوزش خدمتیار برای سفارش شما وجود ندارد. اًمید که خدمتگذار خوبی برای سفارشات بعدی شما باشیم." });
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Message = "مشکل در برقراری ارتباط" });
            }
        }

        [HttpPost]
        public virtual async Task<JsonResult> CheckHowPerformService(int ServiceId = 0, int HowPerformServices = 1)
        {
            try
            {
                var tempHowPerform = "";
                var ExitService = _ServiceProperties.Where(c => c.Id == ServiceId);

                if (ExitService.Count() != 0)
                {
                    var HowPerformService = ExitService.FirstOrDefault().HowPerform;
                    var TitleService = ExitService.FirstOrDefault().Title;
                    if (HowPerformService == 0)
                    {
                        tempHowPerform = "شرکتی";
                    }
                    else if (HowPerformService == 1)
                    {
                        tempHowPerform = "شخصی";
                    }
                    if (HowPerformService != HowPerformServices && HowPerformService != 2)
                        return Json(new { Result = false, Message = TitleService + " " + tempHowPerform + " قابل انجام است" });
                    else return Json(new { Result = true });
                }
                else
                {
                    return Json(new { Result = true });

                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = false });
            }
        }

        /// <summary>
        /// برگرداندن سوابق و مدارک خدمت دهنده
        /// </summary>
        [HttpPost]
        public virtual async Task<JsonResult> GetResume(int ServiceProviderId)
        {
            try
            {
                var temp = await _ServiceProviderInfo.GetServiceProviderInfo(ServiceProviderId);
                var serviceProvider = temp.ToString().Select(a => new ServiceProviderInfo
                {
                    Resume = temp.Resume,
                    Path = temp.Path,
                    Picture = temp.Picture,

                }).FirstOrDefault();

                var documents = _UserFile.Where(c => c.UserId == ServiceProviderId).ToList().Select(r => new UserFile
                {
                    Id = r.Id,
                    UserId = r.UserId,
                    FileName = r.FileName,
                    PathDocumentations = r.PathDocumentations,
                    FileType = r.FileType,
                });




                return Json(new { Result = true, serviceProvider = serviceProvider, documents = documents, Message = "" });

            }
            catch (Exception ex)
            {

                return Json(new { Result = false, Message = "مشکل در برقراری ارتباط" });
            }
        }

        [HttpPost]
        public virtual async Task<JsonResult> UpdateUserServiceLocations(int UserServiceLocationId = 0, byte IsActive = 0)
        {
            try
            {
                var temp = _UserServiceLocations.Where(c => c.Id == UserServiceLocationId).FirstOrDefault();
                temp.StatusServiceLocationUser = IsActive;
                temp.IsEnable = true;
                if (IsActive == 1)// اگر فعال بود
                {
                    _UserService.Where(c => c.ServiceId == temp.ServiceId && c.UserId == temp.UserId).FirstOrDefault().ActiveServiceForUser = 1;
                    _UserService.Where(c => c.ServiceId == temp.ServiceId && c.UserId == temp.UserId).FirstOrDefault().IsEnable = true;
                }
                await _uow.SaveAllChangesAsync();
                return Json(new { Result = true, Message = "با موفقیت ثبت شد" });
            }
            catch (Exception ex)
            {

                return Json(new { Result = false, Message = "مشکل در برقراری ارتباط" });
            }
        }


        /// <summary>
        /// چک کردن لاگین بودن مشتری
        /// </summary>
        [HttpPost]
        public virtual async Task<JsonResult> CheckLoginServiceReceiver()
        {
            try
            {
                int currentuserid = Convert.ToInt32(User.Identity.GetUserId());
                var List = await _userManager.GetAllUsersAsync();
                var ListServiceReceivers = List.OfType<ServiceReceiverInfo>();
                var temp = ListServiceReceivers.Where(c => c.Id == currentuserid).FirstOrDefault();
                if (temp != null)
                    return Json(new { Result = true });
                else
                    return Json(new { Result = false });

            }
            catch (Exception ex)
            {

                return Json(new { Result = false });
            }
        }

        public List<PrivateTraining_View_ServiceLevelList> ListServiceLevels(int Id = 0)
        {
            try
            {
                ShowPlusMenuAnnuncement SP = new ShowPlusMenuAnnuncement();
                var NewId = SP.GetOneLevelServicePropertiesId(_ServiceProperties.ToList(), Id);

                var listServiceLevel = _ServiceLevel.GetAllServiceLevel2();
                var listServiceLevels = listServiceLevel.Where(m => m.ServiceId == Id || m.ServiceId == NewId).ToList().Select(c => new PrivateTraining_View_ServiceLevelList
                {
                    ServiceLevelListId = c.ServiceLevelLists.Any(b => b.ServiceLevelId == c.Id && b.ServicePropertiesId == Id) ? c.ServiceLevelLists.FirstOrDefault(b => b.ServiceLevelId == c.Id && b.ServicePropertiesId == Id).Id : 0,
                    ServiceLevelTitle = c.Title,
                    selected = c.ServiceLevelLists.Any(b => b.ServiceLevelId == c.Id && b.ServicePropertiesId == Id),
                    PercentServiceLevel = c.ServiceLevelLists.Any(b => b.ServiceLevelId == c.Id && b.ServicePropertiesId == Id) ? c.ServiceLevelLists.FirstOrDefault(b => b.ServiceLevelId == c.Id && b.ServicePropertiesId == Id).PercentServiceLevel : 0,
                }).ToList();

                return listServiceLevels.Where(c => c.selected == true).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public virtual JsonResult ListServiceLevelPost(int Id = 0)
        {
            try
            {
                return Json(new { Result = true, Message = ListServiceLevels(Id) });
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Message = "" });
            }
        }

        #region Score Page

        /// <summary>
        /// نمایش لیست امتیازات کاربر
        /// </summary>
        /// <returns></returns>
        public virtual async Task<ActionResult> ShowAllScores(int UId = 0)
        {
            ViewData["UserIdViewbag"] = UId;
            return View();
        }

        [HttpPost]
        public virtual async Task<JsonResult> ShowAllScoreJson(int UserId = 0)
        {
            try
            {

                //  var UserId = Convert.ToInt32(Request.QueryString["UId"]);
                datatable datatable = new datatable();
                datatable = datatableclasses.getdatatalbe(Request);

                var list = _UserService.Where(c => c.IsEnable == true).ToList();

                if (UserId != 0)
                {
                    list = list.Where(x => x.UserId == UserId).ToList();
                }

                if (!string.IsNullOrEmpty(datatable.searchValue))
                {
                    list = list.Where(c => c.Users.Name.Contains(datatable.searchValue) ||
                                           c.Users.Family.Contains(datatable.searchValue) ||
                                           c.ServiceProperties.Title.Contains(datatable.searchValue)
                    //  ||  c.NationalCode.Contains(datatable.searchValue) ||
                    //   c.ShId.Contains(datatable.searchValue)
                    ).ToList();
                }

                datatable.recordsTotal = list.Count();
                list = list.OrderByDescending(c => c.Id).Skip(datatable.skip).Take(datatable.pageSize).ToList();

                //var ListUser = _ServiceProviderInfo.ListServiceLocation(0,0);
                //var h = ListUser.Select(a => new ServiceProviderInfo
                //{
                //    Id = a.Id,
                //    Name=a.Name,
                //    Family=a.Family,
                //}).ToList();

                // Select Feild
                datatable.data = list.ToList().Select(rec => new string[]
                {
                rec.Id.ToString(),
                rec.Users.Name +" "+ rec.Users.Family,
                rec.ServiceProperties.Title,
                rec.ScoreByAdmin.ToString(),
                rec.CountScoreByServiceRecivers.ToString(),
                rec.CalcScoreByServiceReciverAndSystem.ToString(),
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

        #endregion

        #endregion PrivateTraining

        #region Other
        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public virtual async Task<ActionResult> ConfirmEmail(int? userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await _userManager.ConfirmEmailAsync(userId.Value, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // POST: /Account/ExternalLogin
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public ActionResult ExternalLogin(string provider, string returnUrl)
        //{
        //    // Request a redirect to the external login provider
        //    return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        //}

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public virtual async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await _authenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await _signInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return redirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl });
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        //{
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        return RedirectToAction("Index", "Manage");
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        // Get the information about the user from the external login provider
        //        var info = await _authenticationManager.GetExternalLoginInfoAsync();
        //        if (info == null)
        //        {
        //            return View("ExternalLoginFailure");
        //        }
        //        var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
        //        var result = await _userManager.CreateAsync(user);
        //        if (result.Succeeded)
        //        {
        //            result = await _userManager.AddLoginAsync(user.Id, info.Login);
        //            if (result.Succeeded)
        //            {
        //                await _signInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
        //                return redirectToLocal(returnUrl);
        //            }
        //        }
        //        addErrors(result);
        //    }

        //    ViewBag.ReturnUrl = returnUrl;
        //    return View(model);
        //}



        private async Task signInAsync(DomainClasses.Security.ApplicationUser user, bool isPersistent)
        {
            _authenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie, DefaultAuthenticationTypes.TwoFactorCookie);
            _authenticationManager.SignIn(new AuthenticationProperties { IsPersistent = isPersistent },
                await _userManager.GenerateUserIdentityAsync(user));
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public virtual ActionResult ExternalLoginFailure()
        {
            return View();
        }



        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                var code = await _userManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account",
                    new { userId = user.Id, code }, protocol: Request.Url.Scheme);
                await _userManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking here: <a href=\"" + callbackUrl + "\">link</a>");
                ViewBag.Link = callbackUrl;
                return View("ForgotPasswordConfirmation");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public virtual ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public virtual ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {

            PersianDate PD = new PersianDate();
           model.UserName = PD.ConvertFaToEnNumber(model.UserName);
            model.Password = PD.ConvertFaToEnNumber(model.Password);

            
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            //غیرفعال کردن تعلیق در صورتی که از بازه تعلیق خارج شده باشد 
            await activateSuspension();
            await DeactivateSuspension();

            // This doesn't count login failures towards lockout only two factor authentication
            // To enable password failures to trigger lockout, change to shouldLockout: true
            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);

            /*var userName = User.Identity.GetUserName();
            if (string.IsNullOrWhiteSpace(userName))
            {

            }*/

            switch (result)
            {
                case SignInStatus.Success:
                    return redirectToLocal(returnUrl, model.UserName.Length >= 11);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl });
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public virtual async Task<ActionResult> LoginPost(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { Result = false, Messages = "لطفا نام کاربری و کلمه عبور خود را وارد نمایید" });
            }

            // This doesn't count login failures towards lockout only two factor authentication
            // To enable password failures to trigger lockout, change to shouldLockout: true
            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);

            /*var userName = User.Identity.GetUserName();
            if (string.IsNullOrWhiteSpace(userName))
            {

            }*/

            switch (result)
            {
                case SignInStatus.Success:
                    //return redirectToLocal(returnUrl);
                    return Json(new { Result = true });
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl });
                default:
                    return Json(new { Result = false, Messages = "نام کاربری یا کلمه عبور اشتباه می باشد" });
                    //ModelState.AddModelError("", "Invalid login attempt.");
                    //return View(model);
            }
        }

        //
        // POST: /Account/LogOff
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        [AllowAnonymous]
        public virtual async Task<ActionResult> LogOff()
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                _authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                await _userManager.UpdateSecurityStampAsync(user.Id);
            }
            catch (Exception ex)
            {
            }
            return RedirectToAction("Index", "Home");
        }


        //
        // POST: /Account/Register
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public virtual async Task<ActionResult> Register(RegisterViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = new PrivateTraining.DomainClasses.Security.ApplicationUser { UserName = model.PersonnelId, Email = model.Email, NationalCode = model.NationalCode, Mobile = model.Mobile, Name = model.Name, Family = model.Family , PersonnelId =model.PersonnelId };
        //        var result = await _userManager.CreateAsync(user, model.Password);
        //        if (result.Succeeded)
        //        {
        //            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user.Id);
        //            var callbackUrl = Url.Action("ConfirmEmail", "Account",
        //                new { userId = user.Id, code }, protocol: Request.Url.Scheme);
        //            await _userManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking this link: <a href=\"" + callbackUrl + "\">link</a>");
        //            ViewBag.Link = callbackUrl;
        //            return View(global::MVC.Account.Views.DisplayEmail);
        //        }
        //        addErrors(result);
        //    }

        //    // If we got this far, something failed, redisplay form
        //    return View(model);
        //}

        //



        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public virtual ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public virtual async Task<ActionResult> SendCode(string returnUrl)
        {
            var userId = await _signInManager.GetVerifiedUserIdAsync();
            /*if (userId == null)
            {
                return View("Error");
            }*/
            var userFactors = await _userManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await _signInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, model.ReturnUrl });
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public virtual async Task<ActionResult> VerifyCode(string provider, string returnUrl)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await _signInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            var user = await _userManager.FindByIdAsync(await _signInManager.GetVerifiedUserIdAsync());
            if (user != null)
            {
                ViewBag.Status = "For DEMO purposes the current " + provider + " code is: " + await _userManager.GenerateTwoFactorTokenAsync(user.Id, provider);
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _signInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: false, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return redirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        private void addErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult redirectToLocal(string returnUrl, bool isUser = false)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            
            //return RedirectToAction("Index", "Home");
            
            var url = "/panel/service/list/1";
            // if (isUser)
            // {
            //     url = "/PrivateTrain/ServiceReceiverServiceLocation/ServicesServiceReceiver";
            // }
                
            return Redirect(url);
        }

        #endregion

        #region  Password

        /// <summary>
        /// تغییر کلمه عبور
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public virtual async Task<JsonResult> ChangePasswords(ChangePasswordViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Resualt = false, Messages = "اطلاعات نامعتبر است" });
                }
                var result = await _userManager.ChangePasswordAsync(_userManager.GetCurrentUserId(), model.OldPassword, model.NewPassword);
                if (!result.Succeeded)
                {
                    return Json(new { Resualt = false, Messages = "رمز عبور جاری اشتباه است" });
                }
                else if (result.Succeeded)
                {
                    var user = await _userManager.GetCurrentUserAsync();
                    if (user != null)
                    {
                        await signInAsync(user, isPersistent: false);
                    }
                }
                addErrors(result);
                return Json(new { Resualt = true, Messages = "عملیات با موفقیت انجام شد." });
            }
            catch (Exception Ex)
            {
                return Json(new { Resualt = false, Messages = Ex.Message });
            }
        }

        /// <summary>
        /// فراموشی پسورد
        /// </summary>
        /// <returns></returns>
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public virtual ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public virtual async Task<JsonResult> ForgotPasswords(ForgotPasswordViewModel model)
        {
            try
            {
                PersianDate PD = new PersianDate();
                model.UserName = PD.ConvertFaToEnNumber(model.UserName);
                model.Mobile = PD.ConvertFaToEnNumber(model.Mobile);

                if (ModelState.IsValid)
                {
                    //   var user = await _userManager.FindByUserNameAndEmail(model.UserName, model.Email);
                    var user = await _userManager.FindByUserNameAndMobile(model.UserName, model.Mobile);


                    if (user == null)
                    {
                        return Json(new { Resualt = false, Messages = "اطلاعات وارد شده معتبر نمی باشد." });
                    }
                    else
                    {
                        var code = await _userManager.GeneratePasswordResetTokenAsync(user.Id);
                        //var callbackUrl = Url.Action("ResetPassword", "Account",
                        //    new { userId = user.Id, code }, protocol: Request.Url.Scheme);
                        //await _userManager.SendEmailAsync(user.Id, "Reset Password", "لطفا جهت بازیابی پسورد جدید روی لینک زیر کلیک نمایید: <a href=\"" + callbackUrl + "\">link</a>");
                        //// ViewBag.Link = callbackUrl;


                        Random rand = new Random();
                        var pass = rand.Next(100000, 999999);
                        var result = await _userManager.ResetPasswordAsync(user.Id, code, pass.ToString());
                        if (user.Mobile != null && user.Mobile != "")
                        {
                            PrivateTraining.ServiceLayer.BLL.SendSms Sms = new PrivateTraining.ServiceLayer.BLL.SendSms(_uow);
                            Sms.SendSmsClass(user.Mobile, "رمز عبور جدید شما در سایت : " + pass.ToString(), user.Id);
                        }

                        return Json(new { Resualt = true, Messages = "عملیات با موفقیت انجام شد." });
                    }
                }
                else
                    return Json(new { Resualt = false, Messages = "اطلاعات وارد شده معتبر نمی باشد." });
                    }
            catch (Exception Ex)
            {
                return Json(new { Resualt = false, Messages = Ex.Message });
            }
        }

        /// <summary>
        /// Reset Password
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public virtual ActionResult ResetPassword(string code)
        {
            var UserName = _userManager.GetUserName(Convert.ToInt32(Request.QueryString["userId"]));
            ViewData["UserName"] = UserName;

            return code == null ? View("Error") : View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            //  var user = await _userManager.FindByNameAsync(model.Email);
            var user = await _userManager.FindByNameAsync(model.UserName);
            // var user = await _userManager.FindByUserNameAndEmail(model.UserName, model.Email);

            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await _userManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            addErrors(result);
            return View();
        }

        /// <summary>
        /// تغییر کلمه عبور توسط کاربر
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public virtual async Task<ActionResult> ChangePasswordAdmin(int UserId = 0)
        {

            var code = await _userManager.GeneratePasswordResetTokenAsync(UserId);
            var callbackUrl = Url.Action("ResetPassword", "Account",
                new { userId = UserId, code }, protocol: Request.Url.Scheme);
            ViewBag.Link = callbackUrl;
            return View();
        }

        #endregion

        #region Register

        //
        // GET: /Account/Register
        //[AllowAnonymous]
        public virtual ActionResult Register(int Id = 0)
        {
            ViewBag.listUsers = _CustomRole.Where(c => c.Id != (int)Roles.Administrator).ToList();
            RegisterViewModel ViewModel = new RegisterViewModel();
            ViewModel.SupplementaryInfoUsers = _SupplementaryInfoUser.Where(c => c.Id == Id).FirstOrDefault();

            return View(ViewModel);
        }

        /// <summary>
        ///  ثبت نام توسط مدیر
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        //[AllowAnonymous]
        public virtual async Task<JsonResult> Register(RegisterViewModel model, string Type = "Enter")
        {
            try
            {
                // IdentityResult identityResult = _userManager.CreateAsync(user, model.Password).Result;
                bool SW = await _userManager.CheckPersonnelId(model.PersonnelId);
                IdentityResult result;
                IdentityResult result2;

                if (SW)
                {
                    // ثبت اولیه در صورت یک شماره پرسنلی وجود نداشت

                    SupplementaryInfoUser user = new SupplementaryInfoUser();
                    SetUserInfo(user, model);

                    result = await _userManager.CreateAsync(user, model.Password);
                    var result3 = await _userManager.SetLockoutEnabledAsync(user.Id, false);
                    if (Type == "Excel" || model.RoleId == (int)Roles.User)
                        result2 = await _userManager.AddToRoleAsync(user.Id, "User");
                    else
                    {
                        if (model.RoleId == (int)Roles.Admin)
                            result2 = await _userManager.AddToRoleAsync(user.Id, "Admin");
                        else if (model.RoleId == (int)Roles.Modrator)
                            result2 = await _userManager.AddToRoleAsync(user.Id, "Modrator");
                    }
                }
                else
                {
                    // ویرایش
                    SupplementaryInfoUser user = FindUserId(model.PersonnelId);
                    SetUserInfo(user, model);
                    result = await _userManager.UpdateAsync(user);
                }

                if (result.Succeeded)
                {
                    return Json(new { Resualt = true, Messages = "ثبت نام با موفقیت انجام شد" });
                }
                else
                {
                    string Messages = "", PM = "";
                    foreach (var item in result.Errors)
                    {
                        Messages += item;
                    }

                    if (Messages.IndexOf("Passwords must be at least 6 characters") != -1)
                        PM += "کلمه عبور باید حداقل 6 حرف باشد. " + "<br/>";

                    if (Messages.IndexOf("Passwords must have at least one lowercase ('a'-'z').") != -1 ||
                        Messages.IndexOf("Passwords must have at least one digit ('0'-'9').") != -1
                        || Messages.IndexOf("Passwords must have at least one uppercase ('A'-'Z').") != -1)
                        PM += "کلمه عبور باید شامل حروف بزرگ و کوچک و ترکیب اعداد باشد." + "<br/>";

                    if (Messages.IndexOf("Email") != -1)
                        PM += "ایمیل وارد شده نامعتبر است. " + "<br/>";

                    if (Messages.IndexOf("Name") != -1)
                        PM += "شماره پرسنلی وارد شده نامعتبر است. " + "<br/>";

                    // Email 'm_sadeghi200n@yahoo.com' is already taken.
                    return Json(new { Resualt = false, Messages = PM });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Resualt = false, Messages = ex.Message });
            }
        }

        private SupplementaryInfoUser FindUserId(int PersonnelId)
        {
            var UserId = _SupplementaryInfoUser.Where(c => c.PersonnelId == PersonnelId).FirstOrDefault();
            return _SupplementaryInfoUser.Find(UserId.Id);
        }

        /// <summary>
        /// پر کردن فیلدهای جدول اطلاعات کاربر از مدل فرستاده شده
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        private SupplementaryInfoUser SetUserInfo(SupplementaryInfoUser user, RegisterViewModel model)
        {
            PersianDate pd = new PersianDate();
            user.UserName = model.PersonnelId.ToString();
            user.Email = model.Email;
            user.NationalCode = model.NationalCode;
            user.Mobile = model.Mobile;
            user.Name = model.Name;
            user.Family = model.Family;
            user.PersonnelId = model.PersonnelId;

            user.Sex = model.Sex;
            user.HomeAddress = model.Address;
            user.HomePhone = model.Phone;
            user.RegisterDate = pd.PersianDateLow();

            user.CertificateId = model.CertificateId;
            user.CertificateType = model.CertificateType;
            user.CertificationDate = model.CertificationDate;
            user.CertificateCredit = model.CertificateCredit;
            user.Status = model.Status;
            user.BusId = model.BusId;
            user.YearEmployment = model.YearEmployment;
            user.EducationComers = model.EducationComers;
            user.OtherCourses = model.OtherCourses;
            user.NumberChildren = model.NumberChildren;
            user.Degree = model.Degree;
            user.FieldOfStudy = model.FieldOfStudy;
            user.IssuedOnHealthCards = model.IssuedOnHealthCards;
            user.ValidityDuration = model.ValidityDuration;
            user.TheValidityPeriodOfTheYear = model.TheValidityPeriodOfTheYear;
            user.ExpirationDate = model.ExpirationDate;
            user.LockoutEnabled = false;
            user.Deleted = (byte)DeleteUserRecord.Show;
            //  user.Suspension = false;
            return user;

        }

        /// <summary>
        /// چک کردن شماره پرسنلی تکراری
        /// </summary>
        /// <param name="PersonnelId"></param>
        /// <returns></returns>
        [HttpPost]
        //[AllowAnonymous]
        public virtual async Task<JsonResult> CheckPersonnelId(int PersonnelId)
        {
            try
            {
                bool SW = await _userManager.CheckPersonnelId(PersonnelId);

                if (SW)
                    return Json(new { Resualt = true, Userexist = false, Messages = "" });
                else
                    return Json(new { Resualt = true, Userexist = true, Messages = "" });

            }
            catch (Exception)
            {
                return Json(new { Resualt = false, Messages = "" });
            }

        }

        /// <summary>
        /// چک کردن  کد ملی تکراری
        /// </summary>
        /// <param name="PersonnelId"></param>
        /// <returns></returns>
        //[HttpPost]
        ////[AllowAnonymous]
        //public virtual async Task<JsonResult> CheckNationalCode(string NationalCode)
        //{
        //    try
        //    {
        //        bool SW = await _userManager.CheckNationalCode(NationalCode);

        //        if (SW)
        //            return Json(new { Resualt = true, Userexist = false, Messages = "" });
        //        else
        //            return Json(new { Resualt = true, Userexist = true, Messages = "" });

        //    }
        //    catch (Exception)
        //    {
        //        return Json(new { Resualt = false, Messages = "" });
        //    }

        //}




        #endregion

        #region List Users

        /// <summary>
        /// لیست کاربران
        /// </summary>
        /// <returns></returns>
        //[AllowAnonymous]
        public virtual ActionResult ListUsers()
        {
            return View();
        }

        [HttpPost]
        //[AllowAnonymous]
        public virtual async Task<JsonResult> ListJsonUsers(byte UserType = 0, int RoleId = 0)
        {
            try
            {
                datatable datatable = new datatable();
                datatable = datatableclasses.getdatatalbe(Request);

                var list = await _userManager.GetAlTypelUsers();


                //var SuspensionUsers = (from s in _SuspensionUser
                //                       join u in _User on s.UserId equals u.Id
                //                       select new
                //                       {
                //                           s.UserId
                //                       }).ToList();
                //List<int> ListUsers = SuspensionUsers.Select(c => c.UserId).ToList();

                //if (SuspensionFilter == 1)
                //  list = list.Where(c => c.Suspension == true || ListUsers.Contains(c.Id));

                if (UserType == 1)
                    list = list.OfType<ServiceProviderInfo>();
                if (UserType == 2)
                    list = list.OfType<ServiceReceiverInfo>();


                if (RoleId != 0)
                    list = list.Where(x => x.Roles.Any(a => a.RoleId == RoleId));

                if (!string.IsNullOrEmpty(datatable.searchValue))
                {
                    list = list.Where(c => c.Name.Contains(datatable.searchValue) ||
                                           c.Family.Contains(datatable.searchValue) ||
                                           //   c.FatherName.Contains(datatable.searchValue) ||
                                           c.PersonnelId.ToString().Contains(datatable.searchValue)
                    //  ||  c.NationalCode.Contains(datatable.searchValue) ||
                    //   c.ShId.Contains(datatable.searchValue)
                    );
                }

                //sort
                //if (!(string.IsNullOrEmpty(datatable.sortColumn) && string.IsNullOrEmpty(datatable.sortColumnDir)))
                //{
                //    //for make sort simpler we will add Syste.Linq.Dynamic reference
                //    list = list.OrderBy(datatable.sortColumn + " " + datatable.sortColumnDir);
                //}
                datatable.recordsTotal = list.Count();
                list = list.OrderBy(c => c.Id).Skip(datatable.skip).Take(datatable.pageSize);


                // Select Feild
                datatable.data = list.ToList().Select(rec => new string[]
                {
                 "<div class=\"checkbox\" style=\"margin-right:-20px;padding-left:5px;\"><label><input type=\"checkbox\" ng-model=\"checkbox["+rec.Id+"]\" ng-checked=all class=\"case\" value=\""+rec.Id +"\" ><span class=\"text\"></span></lable></div>"
                ,
                rec.Name + " "+rec.Family ,
                rec.UserName,
                rec.Mobile,
                rec.Cities.Name,
                rec.NationalCode,
                ChangePasswordUser(rec.Id),
                rec.RegisterDate,
                rec.UserType.ToString(),
                rec.Id.ToString()
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
        public string ChangePasswordUser(int UserId)
        {
            return "<a class=\"col-lg-3 btn btn-magenta shiny btn-circle btn-xs\" title=\"تغییر رمز عبور\" target=\"_blank\" href=\"/Account/ChangePasswordAdmin?UserId=" + UserId + "\" >" +
                "<i class=\"fa fa-edit\" style=\"color:#fff;\"></i></a>";
        }

        [HttpPost]
        public virtual async Task<JsonResult> GetServiceUsers(int UserId = 0)
        {
            try
            {
                // var listUser = await _userManager.GetAllUsers();
                //  var List = _UserService.Where(c => c.UserId == UserId);
                var List = _UserService.Where(c => c.UserId == UserId && c.IsEnable == true).ToList();

                //var Temp = from o in List
                //           select new
                //           {
                //               o.Id,
                //               o.ServiceId,
                //               o.UserId,
                //               o.ScoreByAdmin,
                //               o.ActiveServiceForUser,
                //               o.CapacityServiceUser,
                //           };

                var ListServiceuser = List.Select(a => new PrivateTraining_View_ServiceUsers
                {
                    Id = a.Id,
                    Serviceid = a.ServiceId,
                    // Title = a.ServiceProperties.Title,
                    Title = FunShowGroup(a.ServiceId),
                    UserId = a.UserId,
                    Score = a.ScoreByAdmin,
                    ActiveServiceForUser = a.ActiveServiceForUser,
                    CapacityServiceUser = a.CapacityServiceUser,
                    ListServiceLevel = ListServiceLevels(a.ServiceId),
                    ServiceLevelListId = a.ServiceLevelListId,
                }).ToList();

                return Json(new { Result = true, List = ListServiceuser });
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, List = "خطا" });
            }
        }
        [HttpPost]
        public virtual async Task<JsonResult> GetUserServiceLocations(int UserId = 0)
        {
            try
            {
                var Users = await _userManager.GetAllUsers();
                var TempUser = Users.Where(c => c.Id == UserId).FirstOrDefault();
                if (TempUser != null)
                {
                    //  var ListUserServiceLocations = TempUser.UserServiceLocations.Select(a => new PrivateTraining_View_UserServiceLocations

                    var ListUserServiceLocations = TempUser.UserServiceLocations.Where(c => c.IsEnable == true && c.Locations.IsEnable == true).Select(a => new PrivateTraining_View_UserServiceLocations
                    {
                        Id = a.Id,
                        ServiceId = a.ServiceLocations.ServiceId,
                        //  TitleService = a.ServiceLocations.Services.Title,
                        TitleService = FunShowGroup(a.ServiceLocations.Services.Id),
                        UserId = a.UserId,
                        LocationId = a.ServiceLocations.LocationId,
                        LocationName = a.Locations.Name,
                        StatusServiceLocationUser = a.StatusServiceLocationUser,
                    }).ToList().OrderBy(c => c.LocationName);
                    return Json(new { Result = true, List = ListUserServiceLocations });
                }
                else
                {
                    return Json(new { Result = false, Message = "وجود ندارد" });
                }

            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Message = "خطا" });
            }
        }

        public string FunShowGroup(int Id)
        {
            PrivateTraining.ServiceLayer.BLL.PrivateTraining PT = new PrivateTraining.ServiceLayer.BLL.PrivateTraining(_uow, _userManager);
            return PT.FunGroupName(Id);
        }

        //[HttpPost]
        ////[AllowAnonymous]
        //public virtual async Task<JsonResult> SaveScoreServiceUsers(int Id, int Score)
        //{
        //    try
        //    {
        //        PersianDate PD = new PersianDate();
        //        if (Score > 100)
        //        {
        //            return Json(new { Result = false, Message = "امتیاز باید بین 0 تا 100 باشد" });
        //        }
        //        UserService usersservice = new UserService();
        //        usersservice = _UserService.Where(c => c.Id == Id).FirstOrDefault();
        //        usersservice.ScoreServiceUser = Score;
        //        usersservice.UpdateScoreByUserId = Convert.ToInt32(User.Identity.GetUserId());
        //        usersservice.DateUpdateScore = PD.PersianDateLow();
        //        usersservice.TimeUpdateScore = PD.CurrentTime();

        //        if (Score == 0)
        //            usersservice.ActiveServiceForUser = false;
        //        else
        //            usersservice.ActiveServiceForUser = true;

        //        await _uow.SaveAllChangesAsync();

        //        return Json(new { Result = true });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Result = false, Message = "خطا" });
        //    }
        //}
        [HttpPost]
        public virtual async Task<JsonResult> UpdateServiceUsers(int UserServiceId, int Score, int CapacityServiceUser, int StatusUserService = 1, int Level = 0, int ServiceLevelListId = 0,int UserId=0)
        {
            try
            {

                PersianDate PD = new PersianDate();

                if (Score > 100)
                {
                    return Json(new { Result = false, Message = "امتیاز باید بین 0 تا 100 باشد" });
                }
                UserService usersservice = new UserService();
                usersservice = _UserService.Where(c => c.Id == UserServiceId).FirstOrDefault();
                usersservice.ScoreByAdmin = Score;
                //usersservice.UpdateScoreByUserId = Convert.ToInt32(User.Identity.GetUserId());
                //usersservice.DateUpdateScore = PD.PersianDateLow();
                //usersservice.TimeUpdateScore = PD.CurrentTime();
                usersservice.CapacityServiceUser = CapacityServiceUser;
                usersservice.ServiceLevelListId = ServiceLevelListId;

                var temp = _UserServiceLocations.Where(c => c.ServiceId == usersservice.ServiceId && c.UserId == usersservice.UserId).FirstOrDefault();
                /// مقدار StatusServiceLocationUser کی باید عوض شه؟                

                if (StatusUserService == 1)
                {
                    usersservice.ActiveServiceForUser = 1;
                    // temp.StatusServiceLocationUser = Convert.ToByte(StatusServiceLocationUser.Active);
                }
                else if (StatusUserService == 2)
                {
                    usersservice.ActiveServiceForUser = 2;
                    //  temp.StatusServiceLocationUser = Convert.ToByte(StatusServiceLocationUser.DeActive);
                }

                var currentuserId = Convert.ToInt32(User.Identity.GetUserId());
                var ExitUserServiceScore = _UserServiceScore.Where(c => c.UserId == usersservice.UserId && c.ServiceId == usersservice.ServiceId && c.ScoreByUserId == currentuserId).FirstOrDefault();
                if (ExitUserServiceScore != null)
                {
                    ExitUserServiceScore.ScoreByUserId = Convert.ToInt32(User.Identity.GetUserId());
                    ExitUserServiceScore.Score = Score;
                    ExitUserServiceScore.DateUpdateScore = PD.PersianDateLow();
                    ExitUserServiceScore.TimeUpdateScore = PD.CurrentTime();
                }
                else if (ExitUserServiceScore == null)
                {
                    UserServiceScore temp2 = new UserServiceScore();
                    temp2.ServiceId = usersservice.ServiceId;
                    temp2.UserId = usersservice.UserId;
                    temp2.ScoreByUserId = Convert.ToInt32(User.Identity.GetUserId());
                    temp2.Score = Score;
                    temp2.DateUpdateScore = PD.PersianDateLow();
                    temp2.TimeUpdateScore = PD.CurrentTime();
                }
                await _uow.SaveAllChangesAsync();

                //------------------- فعال شدن خدمت محل های این خدمت
                var temp3 = _UserServiceLocations.Where(c => c.UserId== UserId && c.ServiceId== usersservice.ServiceId).ToList();
                foreach(var item in temp3)
                {
                    item.StatusServiceLocationUser = (byte)StatusUserService;
                    item.IsEnable = true;
                    await _uow.SaveAllChangesAsync();
                }
                //--------------------

                return Json(new { Result = true, Message = "با موفقیت ثبت شد." });
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Message = "خطا" });
            }
        }

        //[HttpPost]
        //public virtual async Task<JsonResult> UpdateServiceUsers(int Id, int CapacityServiceUser)
        //{
        //    try
        //    {
        //        PersianDate PD = new PersianDate();

        //        UserService usersservice = new UserService();
        //        usersservice = _UserService.Where(c => c.Id == Id).FirstOrDefault();
        //        usersservice.CapacityServiceUser = CapacityServiceUser;

        //        await _uow.SaveAllChangesAsync();

        //        return Json(new { Result = true });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Result = false, Message = "خطا" });
        //    }
        //}

        public string StatusName(bool LockoutEnabled, int UserId)
        {
            string Status = "";
            if (!LockoutEnabled)
                Status = "<a class=\"col-lg-3 btn btn-success shiny btn-circle btn-xs\" title=\"غیرفعال کردن\" ng-click=\"InactiveUser(" + UserId + ")\" id=\"" + UserId + "\">" +
                         "<i class=\"fa fa-check\" style=\"color:#fff;\"></i></a>";
            else
                Status = "<a class=\"col-lg-3 btn btn-warning shiny btn-circle btn-xs\" title=\"فعال کردن\" ng-click=\"InactiveUser(" + UserId + ")\" id=\"" + UserId + "\">" +
                         "<i class=\"fa fa-ban\" style=\"color:#fff;\"></i></a>";

            return Status;
        }

        public string FunSuspension(bool Suspension, int UserId)
        {
            string Status = "";
            //if (!Suspension)
            Status = "<a class=\"col-lg-3 btn btn-success shiny btn-circle btn-xs\" title=\"فعال کردن تعلیق\" ng-click=\"SuspensionUser(" + UserId + ")\" id=\"" + UserId + "\">" +
         "<i class=\"fa fa-check\" style=\"color:#fff;\"></i></a>";
            //else
            //    Status = "<a class=\"col-lg-3 btn btn-warning shiny btn-circle btn-xs\" title=\"غیر فعال کردن تعلیق\" ng-click=\"SuspensionUser(" + UserId + ")\" id=\"" + UserId + "\">" +
            //             "<i class=\"fa fa-ban\" style=\"color:#fff;\"></i></a>";

            var SuspensionList = _SuspensionUser.Where(c => c.UserId == UserId);
            if (SuspensionList.Count() > 0 || Suspension == true)
                Status += "<div style=\"font-size:10px;cursor:pointer;float:left;\" ><a ng-click=\"ShowSuspension(" + UserId + ")\">  سابقه تعلیق  </a></div>";

            return Status;
        }

        [HttpPost]
        public virtual async Task<JsonResult> DeleteSuspension(int SuspensionId)
        {
            try
            {
                SuspensionUser Suspensionuser = _SuspensionUser.Find(SuspensionId);
                _SuspensionUser.Remove(Suspensionuser);
                await _uow.SaveAllChangesAsync();
                return Json(new { Result = true, Messages = "حذف شد" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Messages = "خطا" });
            }
        }

        public string FullInfo(int UserId,string Name="")
        {
            string str = "<a target=\"_blank\" href=\"/Account/GetAddEditServiceProviderInfo?UserId=" + UserId + "\" >" + Name+"<a>";
            str+= " <a class=\"col-lg-3 btn btn-azure shiny btn-circle btn-xs\" title=\"نمایش\" target=\"_blank\" href=\"/Home/IndexPanel?UserId=" + UserId + "\" >" +
                         "<i class=\"fa fa-info\" style=\"color:#fff;\"></i></a>";
            return str;
        }

        public string ListLeave(int UserId)
        {
            return "<a class=\"col-lg-3 btn btn-yellow shiny btn-circle btn-xs\" title=\"نمایش\" href=\"/BusDriving/Leave/Index/?UserId=" + UserId + "\">" +
                         "<i class=\"fa fa-list-ul\" style=\"color:#fff;\"></i></a>";
            //return "<a class=\"col-lg-3 btn btn-yellow shiny btn-circle btn-xs\" title=\"نمایش\" ng-click=\"ListLeave(" + UserId + ")\" id=\"" + UserId + "\">" +
            //             "<i class=\"fa fa-list-ul\" style=\"color:#fff;\"></i></a>";
        }

        public string ChangePasswordm(int UserId)
        {
            return "<a class=\"col-lg-3 btn btn-magenta shiny btn-circle btn-xs\" title=\"تغییر رمز عبور\" target=\"_blank\" href=\"/Account/ChangePasswordAdmin?UserId=" + UserId + "\" >" +
                "<i class=\"fa fa-edit\" style=\"color:#fff;\"></i></a>";
        }

        /// <summary>
        /// حذف کاربر
        /// </summary>
        /// <param name="UsersId"></param>
        /// <returns></returns>
        [HttpPost]
        //[AllowAnonymous]
        public virtual async Task<JsonResult> DeleteUsers(string[] UsersId)
        {
            string IdS = "0";
            try
            {
                for (int i = 0; i <= UsersId.Length - 1; i++)
                {
                    IdS = UsersId[i].ToString();
                    await _userManager.DeleteUser(Convert.ToInt32(UsersId[i]));
                }
                return Json(new { Result = true, Message = "عملیات با موفقیت انجام شد" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Message = "امکان غیرفعال کردن برای کد " + IdS + " وجود ندارد" });
            }
        }

        /// <summary>
        /// غیر فعال کردن کاربر
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [HttpPost]
        //[AllowAnonymous]
        public virtual async Task<JsonResult> InactiveUsers(int UserId)
        {
            try
            {
                await _userManager.InactiveUser(UserId);

                return Json(new { Result = true, Message = "عملیات با موفقیت انجام شد" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Message = "امکان غیرفعال کردن برای کد " + UserId + " وجود ندارد" });
            }
        }

        /// <summary>
        /// افزودن بازه تعلیق کاربر
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="SuspensionDesc"></param>
        /// <returns></returns>
        [HttpPost]
        //[AllowAnonymous]
        public virtual async Task<JsonResult> SuspensionUsers(int UserId, string SuspensionDesc, string FromSuspensionDate = "", string ToSuspensionDate = "")
        {
            try
            {
                if (FromSuspensionDate == "" || ToSuspensionDate == "")
                {
                    return Json(new { Result = false, Message = "لطفا تاریخ شروع و اتمام تاریخ را وارد نمایید" });
                }
                else
                {
                    return Json(new { Result = true, Message = "عملیات با موفقیت انجام شد" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Message = "انجام نشد." });
            }
        }

        /// <summary>
        /// فعال کردن تعلیق کاربر
        /// </summary>
        [HttpPost]
        //[AllowAnonymous]
        public virtual async Task<JsonResult> activateSuspension()
        {
            try
            {
                PersianDate PD = new PersianDate();
                // PD.ConvertPersianNember();
                var y = PD.PersianDateLow();
                var List = _SuspensionUser.Where(c => c.ToSuspensionDate.CompareTo(y) >= 0 && c.FromSuspensionDate.CompareTo(y) <= 0).ToList();
                if (List.Count() > 0)
                {
                    foreach (var item in List)
                    {
                        var User = _User.Where(c => c.Id == item.UserId).FirstOrDefault();
                        //       User.Suspension =true;
                    }
                }
                await _uow.SaveAllChangesAsync();
                return Json(new { Result = true, Message = "عملیات با موفقیت انجام شد" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Message = "" });
            }
        }
        /// <summary>
        /// غیرفعال کردن تعلیق کاربر
        /// </summary>
        [HttpPost]
        //[AllowAnonymous]
        public virtual async Task<JsonResult> DeactivateSuspension()
        {
            try
            {
                PersianDate PD = new PersianDate();
                var y = PD.PersianDateLow();
                var List = _SuspensionUser.Where(c => c.ToSuspensionDate.CompareTo(y) < 0).ToList();
                if (List.Count() > 0)
                {
                    //foreach (var item in List)
                    //{
                    //    var User = _User.Where(c => c.Id == item.UserId).FirstOrDefault();
                    //    User.Suspension = false;
                    //}
                }
                await _uow.SaveAllChangesAsync();
                return Json(new { Result = true, Message = "عملیات با موفقیت انجام شد" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Message = "" });
            }
        }


        /// <summary>
        ///نمایش لیست تعلیقی های  کاربر
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [HttpPost]
        //[AllowAnonymous]
        public virtual JsonResult SuspensionList(int UserId)
        {
            try
            {
                var List = _SuspensionUser.Where(c => c.UserId == UserId).ToList().Select(a => new SuspensionUser
                {
                    Id = a.Id,
                    SuspensionDate = a.SuspensionDate,
                    FromSuspensionDate = a.FromSuspensionDate,
                    ToSuspensionDate = a.ToSuspensionDate,
                    SuspensionDesc = a.SuspensionDesc,
                }).ToList();

                //var List = "";
                //var SuspensionList = _SuspensionUser.Where(c => c.UserId == UserId);
                //if (SuspensionList != null)
                //    foreach (var item in SuspensionList.ToList())
                //    {
                //        List += item.SuspensionDate + "<br />" + item.SuspensionDesc;
                //    }

                return Json(new { Result = true, Message = List });
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Message = "مشکل در دریافت اطلاعات" });
            }
        }


        #endregion

        #region Import Excel


        /// <summary>
        /// آپلود فایل اکسل لیست کاربران
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public virtual ActionResult ImportExcelUsers()
        {
            return View();
        }

        /// <summary>
        /// آپلود فایل اکسل
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        //[AllowAnonymous]
        public virtual async Task<JsonResult> UploadEcxel(List<HttpPostedFileBase> file)
        {
            try
            {
                var Result = await Upload3(file);
                if (Result == "OK")
                    return Json(new { Resualt = true, message = "آپلود با موفقیت انجام شد" });
                else
                    return Json(new { Resualt = false, message = Result });
            }
            catch (Exception Ex)
            {
                return Json(new { Resualt = false });
            }
        }

        public async Task<string> Upload3(List<HttpPostedFileBase> file)
        {
            try
            {
                //string fileExtension = System.IO.Path.GetExtension(Request.Files["file"].FileName);
                //' string fileLocation = "D:\\Mega-tech Project\\ToosSeir\\PR.MVC\\UserFiles\\list55.xlsx";

                string fileLocation = System.Web.Hosting.HostingEnvironment.MapPath("~/UserFiles/ExcelUser/") + Request.Files["file"].FileName;
                if (System.IO.File.Exists(fileLocation))
                {
                    System.IO.File.Delete(fileLocation);
                }
                Request.Files["file"].SaveAs(fileLocation);

                Application app = new Application();

                Workbook wb = app.Workbooks.Open(fileLocation, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                         Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                         Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                         Type.Missing, Type.Missing);

                Worksheet sheet = (Worksheet)wb.Sheets["اتوبوسران"];

                object[,] cellValues;
                object[,] cellFormulas;
                int iTotalColumns = sheet.UsedRange.Columns.Count;
                int iTotalRows = sheet.UsedRange.Rows.Count;
                //These two lines do the magic.
                sheet.Columns.ClearFormats();
                sheet.Rows.ClearFormats();

                Microsoft.Office.Interop.Excel.Range range = (Microsoft.Office.Interop.Excel.Range)sheet.get_Range("A6:AH9", Type.Missing);
                cellValues = range.Value2 as object[,];
                cellFormulas = range.Formula as object[,];
                var Count = cellValues.Length / 34;

                RegisterViewModel model = new RegisterViewModel();
                for (int i = 1; i <= Count; i++)
                {
                    for (int j = 1; j <= 32; j++)
                    {
                        if (cellValues[i, j] == null || cellValues[i, j] == "")
                        {
                            cellValues[i, j] = "0";
                            cellValues[i, j] = cellValues[i, j].ToString().Replace("'", "");
                        }
                    }

                    model.PersonnelId = Convert.ToInt32(cellValues[i, 1]);
                    model.Name = cellValues[i, 2].ToString();
                    model.Family = cellValues[i, 3].ToString();

                    bool Sex = true;
                    if (cellValues[i, 4].ToString() == "مرد")
                        Sex = false;
                    model.Sex = Sex;

                    model.FatherName = cellValues[i, 5].ToString();
                    model.ShId = cellValues[i, 6].ToString();
                    model.NationalCode = cellValues[i, 7].ToString();
                    model.BrithDay = cellValues[i, 8].ToString();
                    model.PlaceOfBirth = cellValues[i, 9].ToString();
                    model.Phone = cellValues[i, 10].ToString();
                    model.Mobile = cellValues[i, 11].ToString();
                    model.Address = cellValues[i, 12].ToString();
                    model.CertificateId = cellValues[i, 13].ToString();
                    model.CertificateType = cellValues[i, 14].ToString();
                    model.CertificationDate = cellValues[i, 15].ToString();
                    model.CertificateCredit = cellValues[i, 16].ToString();
                    model.Status = cellValues[i, 17].ToString();
                    model.BusId = Convert.ToInt32(cellValues[i, 18]);
                    model.YearEmployment = cellValues[i, 19].ToString();
                    model.EducationComers = cellValues[i, 20].ToString();
                    model.OtherCourses = cellValues[i, 21].ToString();
                    model.NumberChildren = Convert.ToByte(cellValues[i, 22]);
                    model.Degree = cellValues[i, 25].ToString();
                    model.FieldOfStudy = cellValues[i, 26].ToString();
                    model.IssuedOnHealthCards = cellValues[i, 29].ToString() + "/" + cellValues[i, 28].ToString() + "/" + cellValues[i, 27].ToString();
                    model.ValidityDuration = cellValues[i, 30].ToString();
                    model.TheValidityPeriodOfTheYear = Convert.ToByte(cellValues[i, 31]);
                    model.ExpirationDate = cellValues[i, 34].ToString() + "/" + cellValues[i, 33].ToString() + "/" + cellValues[i, 32].ToString();

                    // model.Email = cellValues[i, 35].ToString();
                    model.Email = "";
                    //model.Password = "Ts" + model.NationalCode;
                    model.Password = model.NationalCode;
                    //model.ConfirmPassword = "Ts" + model.NationalCode;
                    model.ConfirmPassword = model.NationalCode;
                    var Result = await Register(model, "Excel");
                }

                wb.Close(false, Type.Missing, Type.Missing);

                return "OK";
            }
            catch (Exception Ex)
            {
                return Ex.Message;
            }
        }

        #endregion

    }
}