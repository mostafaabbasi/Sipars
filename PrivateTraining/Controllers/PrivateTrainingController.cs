using Microsoft.AspNet.Identity;
using PrivateTraining.DataLayer.Context;
using PrivateTraining.DomainClasses.Entities.BaseTable;
using PrivateTraining.DomainClasses.Entities.PrivateTraining;
using PrivateTraining.DomainClasses.Entities.Security;
using PrivateTraining.DomainClasses.EntitiesView.PrivateTrainig;
using PrivateTraining.LocationLayer.Interface.PrivateTraining;
using PrivateTraining.ServiceLayer.Extention;
using PrivateTraining.ServiceLayer.Interface;
using PrivateTraining.ServiceLayer.Interface.PrivateTraining;
using PrivateTraining.ServiceLocationLayer.Interface.PrivateTraining;
using System;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Linq;
using System.Data;
using System.Web;
using System.Collections.Generic;
using PrivateTraining.DomainClasses.Security;
using PrivateTraining.BussinessLayer.Generic;
using PrivateTraining.DomainClasses.Entities;
using PrivateTraining.BussinessLayer.Enums;
using PrivateTraining.ServiceLayer.BLL;
using PrivateTraining.ServiceLayer.Interface.Security;

namespace PrivateTraining.Controllers
{
    /// <summary>
    /// Controller of 
    /// </summary>
    public partial class PrivateTrainingController : Controller
    {
        private IUnitOfWork _uow;
        private IDbSet<State> _state;
        private IDbSet<City> _city;
        private IService _service;
        private ILocation _location;
        private readonly IServiceLocation _servicelocation;
        private IServiceProviderInfo _serviceprovider;
        private IServiceReceiverInfo _serviceReceiver;
        private IServiceReceiverServiceLocation _servicereceiveservicelocation;
        private readonly IApplicationUserManager _userManager;
        private IDbSet<ServiceReceiverServiceLocation> _ServiceReceiverServiceLocations;
        private IDbSet<ServiceWorkUnit> _ServiceWorkUnit;
        private IDbSet<ServiceProperties> _ServiceProperties;
        private IDbSet<ServiceProviderInfo> _ServiceProviderInfo;
        private IDbSet<UserService> _UserService;
        private IDbSet<ServiceReceiverRequest> _ServiceReceiverRequest;
        private IDbSet<Form> _Form;
        private IDbSet<FormQuestion> _FormQuestion;
        private IDbSet<FormAnswer> _FormAnswers;
        private readonly IGroupPolicy _group;
        private IDbSet<PrivateSetting> _setting;

        public PrivateTrainingController(IUnitOfWork uow, IService service, ILocation location,
            IServiceProviderInfo serviceprovider, IApplicationUserManager userManager,
            IServiceLocation servicelocation, IServiceReceiverServiceLocation servicereceiveservicelocation,
            IServiceReceiverInfo serviceReceiver, IGroupPolicy group)
        {
            _uow = uow;
            _state = _uow.Set<State>();
            _city = _uow.Set<City>();
            _service = service;
            _location = location;
            _servicelocation = servicelocation;
            _serviceprovider = serviceprovider;
            _servicereceiveservicelocation = servicereceiveservicelocation;
            _userManager = userManager;
            _serviceReceiver = serviceReceiver;
            _ServiceReceiverServiceLocations = _uow.Set<ServiceReceiverServiceLocation>();
            _ServiceWorkUnit = _uow.Set<ServiceWorkUnit>();
            _ServiceProperties = _uow.Set<ServiceProperties>();
            _ServiceProviderInfo = _uow.Set<ServiceProviderInfo>();
            _UserService = _uow.Set<UserService>();
            _ServiceReceiverRequest = _uow.Set<ServiceReceiverRequest>();
            _Form = _uow.Set<Form>();
            _FormQuestion = _uow.Set<FormQuestion>();
            _FormAnswers = _uow.Set<FormAnswer>();
            _group = group;
            _setting = _uow.Set<PrivateSetting>();
        }


        ///region api new
        [HttpGet]
        public virtual async Task<JsonResult> MenuList()
        {
            var ListServiceProperties = _ServiceProperties.ToList();
            var AllListService = ListServiceProperties.Select(a =>
                new PrivateTraining_View_ServiceUsers
                {
                    Title = a.Title,
                    Id = a.Id,
                    ParentId = a.ParentId,
                    Level = a.Level,
                    IsEnable = a.IsEnable,
                }).ToList();
            var List = AllListService;
            return Json(new { result = "done", items = List }, JsonRequestBehavior.AllowGet);
        }

        ///endregion

        // [AllowAnonymous]
        public virtual ActionResult ListServiceMenus()
        {
            var ListServiceProperties = _ServiceProperties.ToList();
            var AllListService = ListServiceProperties.Select(a =>
                new PrivateTraining.DomainClasses.EntitiesView.PrivateTrainig.PrivateTraining_View_ServiceUsers
                {
                    Title = a.Title,
                    Id = a.Id,
                    ParentId = a.ParentId,
                    Level = a.Level,
                    IsEnable = a.IsEnable,
                }).ToList();
            var List = AllListService;

            return View(List);
        }

        public virtual ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// ثبت نام مشتری
        /// </summary>
        /// <param name="ServiceReceiverServiceLocationId"></param>
        /// <returns></returns>
        public virtual async Task<ActionResult> ApproveServices(int ServiceReceiverServiceLocationId)
        {
            return View();
        }

        public bool SelectedSRSL(int ServiceReceiverServiceLocationId, int ServiceId)
        {
            try
            {
                var temp = _ServiceReceiverServiceLocations.Where(c => c.Id == ServiceReceiverServiceLocationId)
                    .FirstOrDefault();
                if (temp.ServiceLocations.ServiceId == ServiceId)
                    return true;
                else return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //public virtual async Task<ActionResult> ApproveServicesPost()
        //{
        //    try
        //    {
        //        var CurrentUserId = Convert.ToInt32(User.Identity.GetUserId());

        //        return Json(new { Result = true, Messages = "success " });

        //    }
        //    catch (Exception ex)
        //    {

        //        return Json(new { Result = false, Messages = ex.Message });
        //    }
        //}

        //public virtual ActionResult ApproveServicesGetinformationServiceReceivers(int StateId = 0, int CityId = 0, int LocationId = 0, List<SelectServiceProviderForService> ServiceProviderAndServices = null)

        public virtual ActionResult ApproveServicesGetinformationServiceReceivers()
        {
            return View();
        }

        [HttpPost]
        public virtual async Task<JsonResult> GetApproveServiceInfo(int StateId = 0, int CityId = 0, int LocationId = 0,
            List<SelectServiceProviderForService> ServiceProviderAndServices = null,
            int ServiceReceiverServiceLocationId = 0)
        {
            try
            {
                var Userid = Convert.ToInt32(User.Identity.GetUserId());
                View_ApproveService Approve = new View_ApproveService();

                var State = await _state.FirstOrDefaultAsync(c => c.Id == StateId && c.IsEnable == true);
                Approve.StateId = State.Id;
                Approve.StateName = State.Name;

                var City = await _city.FirstOrDefaultAsync(c => c.Id == CityId && c.IsEnable == true);
                Approve.CityId = City.Id;
                Approve.CityName = City.Name;

                List<SelectServiceProviderForService> SList = new List<SelectServiceProviderForService>();
                foreach (var item in ServiceProviderAndServices)
                {
                    SelectServiceProviderForService S = new SelectServiceProviderForService();

                    var Service = await _service.GetService(item.ServiceId);
                    S.ServiceId = item.ServiceId;
                    S.ServiceName = Service.Title;

                    var ServiceProviderInfo = await _serviceprovider.GetServiceProviderInfo(item.ServiceProviderId);
                    S.ServiceProviderId = item.ServiceProviderId;
                    S.ServiceProviderName = ServiceProviderInfo.Name + " " + ServiceProviderInfo.Family;

                    //if(item.WorkUnitId == 0)
                    //{
                    //    var Q = _ServiceReceiverServiceLocations.Where(c => c.Id == ServiceReceiverServiceLocationId).FirstOrDefault();
                    //    if (Q != null)
                    //        item.WorkUnitId = (int)Q.WorkUnitId;
                    //}

                    S.WorkUnitId = item.WorkUnitId;
                    SList.Add(S);
                }

                Approve.SelectServiceProviderForServices = SList;

                var Location = await _location.GetLocation(LocationId);
                Approve.LocationId = Location.Id;
                Approve.LocationName = Location.Name;


                var ExitServiceReceiver = false;
                if (Userid != 0)
                {
                    var list = await _serviceReceiver.GetAllServiceReceiverInfo();
                    var exitUser = list.Where(c => c.Id == Userid).ToList();


                    if (exitUser.Count() != 0)
                    {
                        var TempUser = exitUser.FirstOrDefault();
                        Approve.Name = TempUser.Name;
                        Approve.Family = TempUser.Family;
                        Approve.Email = TempUser.Email;
                        Approve.Mobile = TempUser.Mobile;
                        Approve.HomeAddress = TempUser.HomeAddress;
                        Approve.HomeNumber = TempUser.HomeNumber;
                        Approve.UnitNumber = TempUser.UnitNumber;
                        Approve.HomePhone = TempUser.HomePhone;
                        ExitServiceReceiver = true;
                    }
                }

                return Json(new {Result = true, List = Approve, ExitServiceReceiver = ExitServiceReceiver});
            }
            catch (Exception ex)
            {
                return Json(new {Result = false, Messages = ex.Message});
            }
        }

        /// <summary>
        /// پر کردن فیلدهای جدول اطلاعات کاربر از مدل فرستاده شده
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        /// 
        private ServiceReceiverInfo SetUserInformation(ServiceReceiverInfo user, View_ApproveService model)
        {
            user.UserName = model.Mobile;
            user.Email = model.Email;
            user.Mobile = model.Mobile;
            user.Name = model.Name;
            user.Family = model.Family;
            user.Sex = model.Sex;
            user.HomeAddress = model.HomeAddress;
            user.CityId = model.CityId;
            user.StateId = model.StateId;
            user.LockoutEnabled = false;
            user.Deleted = (byte) DeleteUserRecord.Show;
            user.UserType = (byte) UserType.ServiceReceiver;
            user.YearBrithDay = "0";
            user.MonthBrithDay = "0";
            user.DayBrithDay = "0";
            return user;
        }

        /// <summary>
        /// ذخیره اطلاعات مشتری
        /// </summary>
        /// <param name="approve"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<JsonResult> AddApproveServices(View_ApproveService approve)
        {
            try
            {
                string Username = "";
                PersianDate PD = new PersianDate();
                int ServiceReciverId = 0;
                Random rand = new Random();
                var pass = rand.Next(100000, 999999);
                var CountMember = await _userManager.GetAllUsers();
                bool sw = false;
                PrivateTraining.ServiceLayer.BLL.SendSms Sms = new PrivateTraining.ServiceLayer.BLL.SendSms(_uow);
                approve.Mobile = PD.ConvertFaToEnNumber(approve.Mobile);

                var UserInfo = CountMember.OfType<ServiceReceiverInfo>()
                    .FirstOrDefault(c => c.UserName == approve.Mobile);
                try
                {
                    IdentityResult result;
                    IdentityResult result2;

                    if (UserInfo == null)
                    {
                        ServiceReceiverInfo user = new ServiceReceiverInfo();
                        SetUserInformation(user, approve);
                        //------------ تخصیص کد به مشتری
                        CreateUserCode(approve.LocationId, user);

                        //   user.UserName = user.ServiceReceiverCode;
                        user.UserName = user.Mobile;
                        Username = user.UserName;
                        result = await _userManager.CreateAsync(user, Convert.ToString(pass));
                        if (result.Succeeded)
                        {
                            sw = true;
                            var result3 = await _userManager.SetLockoutEnabledAsync(user.Id, false);
                            result2 = await _userManager.AddToRoleAsync(user.Id, "User");

                            DefineGroupUser("مشتری", user.Id);
                        }

                        ServiceReciverId = user.Id;
                    }
                    else
                    {
                        return Json(new
                        {
                            Result = false,
                            Messages =
                                "مشتری گرامی شماره همراه وارد شده در سیستم وجود دارد ، اگر قبلا ثبت نموده اید لاگین کنید و اگر رمز عبور را فراموش کرده اید ، بازیابی رمز عبور انجام دهید."
                        });
                    }
                    //else
                    //{
                    //    SetUserInformation(UserInfo, approve);
                    //    _uow.SaveAllChanges();
                    //    ServiceReciverId = UserInfo.Id;
                    //}

                    if (ServiceReciverId != 0)
                    {
                        var ServiceLocation = await _servicelocation.GetAllServiceLocation();
                        foreach (var item in approve.SelectServiceProviderForServices)
                        {
                            int ServiceLocationId = ServiceLocation
                                .Where(c => c.ServiceId == item.ServiceId && c.LocationId == approve.LocationId)
                                .FirstOrDefault().Id;
                            ServiceReceiverServiceLocation service = new ServiceReceiverServiceLocation();
                            service.ServiceLocationId = ServiceLocationId;
                            service.ServiceProviderId = item.ServiceProviderId;
                            service.ServiceReceiverId = ServiceReciverId;
                            service.DateRegister = PD.PersianDateLow();
                            service.WorkUnitId = item.WorkUnitId;
                            //service.WhoChangeStatus = UserInfo.Id;////
                            bool Status = await SaveServiceForServiceReciever(service);
                            if (!Status)
                                return Json(new {Result = false, Messages = "مشکلی در ثبت اطلاعات به وجود آمده است"});
                            else
                            {
                                //----------------------------- ارسال ایمیل و پیامک ثبت نام مشتری
                                try
                                {
                                    string Domain = Request.Url.Host;
                                    string Title = " خدمات آنلاین ";
                                    //if (user.Email != null && user.Email != "")
                                    //{
                                    //    PrivateTraining.ServiceLayer.EmailService email = new PrivateTraining.ServiceLayer.EmailService(_uow);
                                    //    email.SendEmailRegister(Domain, user.Name, user.Family, user.Mobile, pass.ToString(), user.Email, Title);

                                    //}
                                    if (approve.Mobile != null && approve.Mobile != "")
                                    {
                                        if (sw)
                                        {
                                            Sms.SensSmsRegisterReciver(Domain, approve.Name, approve.Family,
                                                approve.Mobile, pass.ToString(), approve.Mobile,
                                                Convert.ToInt32(User.Identity.GetUserId()), Title, service.Id,
                                                approve.Sex, "new");
                                        }
                                        else
                                        {
                                            Sms.SensSmsRegisterReciver(Domain, approve.Name, approve.Family,
                                                approve.Mobile, pass.ToString(), approve.Mobile,
                                                Convert.ToInt32(User.Identity.GetUserId()), Title, service.Id,
                                                approve.Sex, "");
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                }

                                //-----------------------  ارسال ایمیل و پیامک به خدمت دهنده ها
                                SendSmsAndEmail(item.ServiceId, service, approve, service.Id, item.ServiceLevelListId);
                            }
                        }
                    }
                    else
                        return Json(new
                            {Result = false, Messages = "مشکلی در ثبت و یا دریافت نام کاربری به وجود آمده است"});
                }
                catch (Exception ex)
                {
                    return Json(new {Resualt = false, Messages = ex.Message});
                }

                // bool Status = await SaveServiceForServiceReciever(service);
                //  if (Status)
                return Json(new
                {
                    Result = true, Messages = "اطلاعات با موفقیت ثبت گردید", pass = pass,
                    UserName = Username /*approve.Mobile*/
                });
                //  else
                //   return Json(new { Result = false, Messages = "" });
            }
            catch (Exception ex)
            {
                return Json(new {Result = false, Messages = ex.Message});
            }
        }

        public async void DefineGroupUser(string Name = "", int UserId = 0)
        {
            var GroupId = _group.GetAllIGroupPolicySearchName(Name);
            if (GroupId != 0)
            {
                PrivateTraining.DomainClasses.Entities.Security.GroupPolicyUser GroupPolicyUser =
                    new PrivateTraining.DomainClasses.Entities.Security.GroupPolicyUser();
                GroupPolicyUser.UserId = UserId;
                GroupPolicyUser.GroupPolicyId = GroupId;
                await _group.AddGroupPolicyUser(GroupPolicyUser);
                _uow.SaveAllChanges();
            }
        }


        /// <summary>
        /// ایجاد کد مشتری
        /// </summary>
        /// <param name="LocationId"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        private async void CreateUserCode(int LocationId, ServiceReceiverInfo user)
        {
            //----------- کد محل
            var tempLocationCode = await _location.GetLocation(LocationId);
            var AllServiceProvider = await _serviceReceiver.GetAllServiceReceiverInfo();
            //-------------   آخرین شماره مشتری ثبت شده در این  محل
            var maxusercodeServiceReceiver = AllServiceProvider
                .Where(c => c.LocationCode == tempLocationCode.LocationCode).Max(c => c.UserCode);

            if (maxusercodeServiceReceiver != null)
            {
                //--------------- ایجاد کد جدید  ---  
                var TempUserCodePlus1 = string.Format("{0:00000}", Convert.ToInt32(maxusercodeServiceReceiver) + 1);
                user.UserCode = TempUserCodePlus1;
                user.ServiceReceiverCode = tempLocationCode.LocationCode + TempUserCodePlus1;
            }
            else
            {
                //---------------  دفعه اول
                user.UserCode = "00001";
                user.ServiceReceiverCode = tempLocationCode.LocationCode + user.UserCode;
            }

            user.LocationCode = tempLocationCode.LocationCode;
        }

        /// <summary>
        /// ارسال ایمیل و پیامک به خدمت دهنده ها
        /// </summary>
        /// <param name="ServiceId"></param>
        /// <param name="service"></param>
        /// <param name="approve"></param>
        public async void SendSmsAndEmail(int ServiceId, ServiceReceiverServiceLocation service,
            View_ApproveService approve, decimal ServiceReceiverServiceLocationId,
            int ServiceLevelListId = 0)
        {
            try
            {
                // var Price = _ServiceWorkUnit.Where(c => c.ServicePropertiesId == ServiceId).FirstOrDefault().PriceWorkUnit;
                // var p = service.WorkUnits.ServiceLocationWorkUnits.FirstOrDefault();

                string Domain = Request.Url.Host;
                string Title = " خدمات آنلاین ";
                var Query = _userManager.FindById(service.ServiceProviderId);
                if (Query != null)
                {
                    //if (Query.Email != null && Query.Email != "")
                    //{
                    //    IdentityMessage Message = new IdentityMessage();
                    //    PrivateTraining.ServiceLayer.EmailService email = new PrivateTraining.ServiceLayer.EmailService(_uow);
                    //    string Body = "<div style='Direction:rtl;'>" +
                    //"<br /><div>کاربر گرامی  " + Query.Name + " " + Query.Family + " عزیز درخواست جدید برای شما در سایت " + Title
                    //+ " ثبت شده است. </div>";
                    //    Body += " آدرس : " + approve.HomeAddress + " - کد خدمت : " + ServiceId + " - نام خدمت : " + approve.ServiceName + " - مبلغ : " + Price + " تومان " + " - کد درخواست : " + ServiceReceiverServiceLocationId;
                    //    Body += "<br /><div><a href='http://" + Domain + "' target='_blank'>" + Domain + "</a></div>" + "</div>";
                    //    Message.Body = Body;
                    //    Message.Subject = " درخواست جدید در سایت " + Domain;
                    //    Message.Destination = Query.Email;
                    //    await email.SendAsync(Message);
                    //}
                    if (Query.Mobile != null && Query.Mobile != "")
                    {
                        PrivateTraining.ServiceLayer.BLL.PrivateTraining PT =
                            new PrivateTraining.ServiceLayer.BLL.PrivateTraining(_uow, _userManager);
                        PrivateTraining.ServiceLayer.BLL.SendSms Sms =
                            new PrivateTraining.ServiceLayer.BLL.SendSms(_uow);
                        ////string Text = "کاربر گرامی " + Query.Name + " " + Query.Family + " عزیز درخواست جدید برای شما در سایت " + Title 
                        ////  + "ثبت شده است." + "\n" +
                        //// - کد خدمت : " + ServiceId + "
                        //string Text = "درخواست جدید از سایت خدمات آنلاین " + "\n" +
                        //" آدرس : " + approve.HomeAddress + "  - نام خدمت : " + approve.ServiceName + " - مبلغ : " + Price + " تومان " + " - کد درخواست : " + ServiceReceiverServiceLocationId + "." +
                        //" جهت موافقت با درخواست عدد 2 و عدم موافقت عدد 12 را با فرمت زیر ارسال نمایید. " + "." +
                        //" کد درخواست * عدد - مثال" + "2*147" +
                        //" - یا به پنل خود در سایت مراجعه نمایید  ";
                        ////  "\n" + " http://" + Domain;

                        string Text = PT.SendSmsForProviderNewRequest(ServiceReceiverServiceLocationId, service, Domain,
                            ServiceLevelListId);
                        //   Sms.SendSmsClass(Query.Mobile, Text, Convert.ToInt32(User.Identity.GetUserId()));
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        [HttpPost]
        public virtual async Task<JsonResult> EditApproveServices(View_ApproveService param)
        {
            PersianDate PD = new PersianDate();
            var CurentUserId = Convert.ToInt32(User.Identity.GetUserId());
            var TempUserInfo = await _serviceReceiver.GetServiceReceiverInfo(CurentUserId);
            if (TempUserInfo != null)
            {
                TempUserInfo.HomeAddress = param.HomeAddress;
                TempUserInfo.UnitNumber = param.UnitNumber;
                TempUserInfo.Sex = param.Sex;
                TempUserInfo.HomePhone = param.HomePhone;
                TempUserInfo.Email = param.Email;
                TempUserInfo.HomeNumber = param.HomeNumber;
            }

            var serviceLocations = await _servicelocation.GetAllServiceLocation();
            ServiceReceiverServiceLocation service = new ServiceReceiverServiceLocation();

            foreach (var item in param.SelectServiceProviderForServices)
            {
                int serviceLocationId = serviceLocations
                    .Where(c => c.ServiceId == item.ServiceId && c.LocationId == param.LocationId).FirstOrDefault().Id;
                ////?????///
                // var ExitServiceReceiverServiceLocation = _ServiceReceiverServiceLocations.Where(c => c.ServiceLocationId == serviceLocationId && c.ServiceProviderId == item.ServiceProviderId && c.ServiceReceiverId == CurentUserId);

                //service.DateReceiver = param.DateReceiver;
                //service.TimeReceiver = param.TimeReceiver;
                service.ServiceLocationId = serviceLocationId;
                service.ServiceProviderId = item.ServiceProviderId;
                service.ServiceReceiverId = CurentUserId;
                service.DateRegister = PD.PersianDateLow();
                service.Status = 0;
                service.WorkUnitId = item.WorkUnitId;
                //service.WhoChangeStatus = CurentUserId;
                bool Status2 = await SaveServiceForServiceReciever(service);
                if (!Status2)
                    return Json(new {Result = false, Messages = "مشکلی در ثبت اطلاعات به وجود آمده است"});

                //----------------------------- ارسال ایمیل و پیامک ثبت نام مشتری
                try
                {
                    string Domain = Request.Url.Host;
                    string Title = " خدمات آنلاین ";
                    if (param.Mobile != null && param.Mobile != "")
                    {
                        PrivateTraining.ServiceLayer.BLL.SendSms Sms =
                            new PrivateTraining.ServiceLayer.BLL.SendSms(_uow);
                        Sms.SensSmsRegisterReciver(Domain, param.Name, param.Family, param.Mobile, "", param.Mobile,
                            Convert.ToInt32(User.Identity.GetUserId()), Title, service.Id, param.Sex, "");
                    }
                }
                catch (Exception ex)
                {
                }

                //-----------------------  ارسال ایمیل و پیامک به خدمت دهنده ها
                SendSmsAndEmail(item.ServiceId, service, param, service.Id, item.ServiceLevelListId);
            }

            await _uow.SaveAllChangesAsync();
            return Json(new {Result = true, Messages = "اطلاعات با موفقیت ثبت گردید"});
        }

        public async Task<bool> SaveServiceForServiceReciever(ServiceReceiverServiceLocation service)
        {
            try
            {
                //service.WhoChangeStatus = 1;
                await _servicereceiveservicelocation.AddServiceReceiverServiceLocation(service);
                int Status = await _uow.SaveAllChangesAsync();
                if (Status == 1)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        [HttpPost]
        public virtual JsonResult LoadForm(int FormId)
        {
            try
            {
                var ListQuestions = _FormQuestion.Where(c => c.FormId == FormId).ToList();
                var temp = ListQuestions.Select(a => new FormQuestion
                {
                    Id = a.Id,
                    FormId = a.FormId,
                    Title = a.Title,
                }).ToList();
                return Json(new {Result = true, ListQuestions = temp});
            }
            catch (Exception ex)
            {
                return Json(new {Result = false, Messages = "خطا در برقراری ارتباط"});
            }
        }

        [HttpPost]
        public virtual async Task<JsonResult> AnswerFormAssessment(List<FormAnswer> AnswerFormAssessment,
            int SRSLId = 0, int FormId = 0)
        {
            try
            {
                PersianDate Pd = new PersianDate();
                var CurrentUserId = Convert.ToInt32(User.Identity.GetUserId());
                var SPtemp = _ServiceReceiverServiceLocations.Where(c => c.Id == SRSLId).FirstOrDefault();
                var Score = 0;
                var countAnswer = 0;
                var SumScore = 0;

                foreach (var item in AnswerFormAssessment)
                {
                    switch (item.TypeScore)
                    {
                        case 1:
                            Score = _Form.Where(c => c.Id == FormId).FirstOrDefault().Score1;
                            break;
                        case 2:
                            Score = _Form.Where(c => c.Id == FormId).FirstOrDefault().Score2;
                            break;
                        case 3:
                            Score = _Form.Where(c => c.Id == FormId).FirstOrDefault().Score3;
                            break;
                        case 4:
                            Score = _Form.Where(c => c.Id == FormId).FirstOrDefault().Score4;
                            break;
                        case 5:
                            Score = _Form.Where(c => c.Id == FormId).FirstOrDefault().Score5;
                            break;
                    }

                    FormAnswer temp = new FormAnswer();
                    temp.FormQuestionId = item.FormQuestionId;
                    temp.TypeScore = item.TypeScore;
                    temp.ValueScore = Score;
                    temp.ServiceReceiverId = SPtemp.ServiceReceiverId;
                    temp.ServiceProviderId = SPtemp.ServiceProviderId;
                    temp.Date = Pd.PersianDateLow();
                    temp.Time = Pd.CurrentTime();
                    temp.ServiceId = SPtemp.ServiceLocations.ServiceId;
                    _FormAnswers.Add(temp);

                    countAnswer++;
                    SumScore += Score;
                }

                var TempUserservice = _UserService.Where(x =>
                        x.ServiceId == SPtemp.ServiceLocations.ServiceId && x.UserId == SPtemp.ServiceProviderId)
                    .FirstOrDefault();
                if (TempUserservice != null)
                {
                    TempUserservice.CalcScoreByServiceReciverAndSystem += (SumScore / countAnswer);
                    TempUserservice.CountScoreByServiceRecivers += 1;
                }

                await _uow.SaveAllChangesAsync();

                return Json(new {Result = true, Messages = "با موفقیت ثبت شد."});
            }
            catch (Exception ex)
            {
                return Json(new {Result = false, Messages = "خطا در برقراری ارتباط"});
            }
        }

        #region Setting

        /// <summary>
        /// تنظبمات
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Setting()
        {
            var Model = _setting.ToList().Select(a => new PrivateSetting
            {
                ShowPayOnline = a.ShowPayOnline,
            }).FirstOrDefault();
            return View(Model);
        }

        [HttpPost]
        [AllowAnonymous]
        public virtual JsonResult SaveSettings(bool ShowPayOnline = false)
        {
            try
            {
                var Query = _setting.FirstOrDefault();
                if (Query == null)
                {
                    PrivateSetting Table = new PrivateSetting();
                    Table.ShowPayOnline = ShowPayOnline;
                    Table.IsEnable = true;
                    var q1 = _setting.Add(Table);
                }
                else
                    Query.ShowPayOnline = ShowPayOnline;

                var Status = _uow.SaveAllChanges();
                return Json(new {Resualt = true, Messages = "عملیات با موفقیت انجام شد."});
            }
            catch (Exception Ex)
            {
                return Json(new {Resualt = false, Messages = Ex.Message});
            }
        }

        #endregion
    }
}