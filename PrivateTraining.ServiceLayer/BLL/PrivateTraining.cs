using PrivateTraining.DataLayer.Context;
using PrivateTraining.LocationLayer.Interface.PrivateTraining;
using PrivateTraining.DomainClasses.Entities.PrivateTraining;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using PrivateTraining.ServiceLayer.Interface.PrivateTraining;
using PrivateTraining.ServiceLayer.Interface.Security;
using PrivateTraining.ServiceLayer.Extention;
using PrivateTraining.DomainClasses.Entities;
using Microsoft.AspNet.Identity;
using System;
using System.Web.Mvc;
using System.Web;
using PrivateTraining.ServiceLocationLayer.Interface.PrivateTraining;
using PrivateTraining.ServiceLayer.Interface;
using PrivateTraining.DomainClasses.Security;

namespace PrivateTraining.ServiceLayer.BLL
{
    public class PrivateTraining
    {
        private IUnitOfWork _uow;
        public IDbSet<ServiceReceiverRequest> _servicerequest;
        private IDbSet<ServiceReceiverServiceLocation> _ServiceReceiverServiceLocations;
        private IDbSet<UserServiceLocation> _UserServiceLocation;
        private IDbSet<Debt> _DebtDb;
        private IDbSet<UserService> _UserService;
        private IDbSet<DebtServiceReceiverServiceLocation> _DebtServiceReceiverServiceLocationDb;
        private IDbSet<ServiceWorkUnit> _ServiceWorkUnit;
        private IDbSet<ServiceLocationWorkUnit> _ServiceLocationWorkUnit;
        private IDbSet<payment> _payment;

        private IDbSet<paymentDetail> _paymentDetail;
        //   private IDbSet<ApplicationUser> _user;

        // private IServiceReceiverServiceLocation _servicereceiveservicelocation;
        private readonly IApplicationUserManager _userManager;
        private IDbSet<Service> _Service;
        private IDbSet<ServiceLevelList> _ServiceLevelList;

        public PrivateTraining(IUnitOfWork uow, IApplicationUserManager userManager)
        {
            _uow = uow;
            _servicerequest = _uow.Set<ServiceReceiverRequest>();
            _ServiceReceiverServiceLocations = _uow.Set<ServiceReceiverServiceLocation>();
            _UserService = _uow.Set<UserService>();
            _UserServiceLocation = _uow.Set<UserServiceLocation>();
            _DebtDb = _uow.Set<Debt>();
            _ServiceWorkUnit = _uow.Set<ServiceWorkUnit>();
            _DebtServiceReceiverServiceLocationDb = _uow.Set<DebtServiceReceiverServiceLocation>();
            _ServiceLocationWorkUnit = _uow.Set<ServiceLocationWorkUnit>();
            _payment = _uow.Set<payment>();
            _paymentDetail = _uow.Set<paymentDetail>();
            _Service = _uow.Set<Service>();
            _ServiceLevelList = _uow.Set<ServiceLevelList>();

            //_servicereceiveservicelocation = servicereceiveservicelocation;
            _userManager = userManager;
        }

        /// <summary>
        /// تعیین وضعیت درخواست مشتری
        /// </summary>
        /// <param name="ServiceReceiverServiceLocationId">کد درخواست</param>
        /// <param name="Status">وضعیت
        /// 1=  موافقت کرده
        /// </param>
        /// <param name="CurrentUserId">کد کاربر</param>
        /// <param name="WorkUnitId">کد واحد</param>
        /// <param name="ReasonCancel">دلیل انصراف</param>
        /// <returns></returns>
        public int ChangeStatusRequestService(int ServiceReceiverServiceLocationId, int Status,
            int CurrentUserId = 0, int WorkUnitId = 0, string ReasonCancel = "", string Domain = "")
        {
            try
            {
                PersianDate PD = new PersianDate();
                SendSms Sms = new SendSms(_uow);
                var TempSRSL = _ServiceReceiverServiceLocations.Where(c => c.Id == ServiceReceiverServiceLocationId)
                    .FirstOrDefault();

                //  TempSRSL.Status = Enum StatusServiceLocationRequest

                if (TempSRSL != null)
                {
                    string SexProvider = "آقای ";
                    if (TempSRSL.ApplicationProviderUsers.Sex)
                        SexProvider = "خانم ";

                    string SexReceiver = "آقای ";
                    if (TempSRSL.ApplicationReceiverUsers.Sex)
                        SexReceiver = "خانم ";

                    SexProvider = "";
                    SexReceiver = "";
                    if (Status == (int) StatusServiceLocationRequest.Accept &&
                        TempSRSL.Status == (int) StatusServiceLocationRequest.checking) // اگر موافقت کرد
                    {
                        TempSRSL.DateAcceptStatus = PD.PersianDateLow();
                        TempSRSL.TimeAcceptStatus = PD.CurrentTime();
                        TempSRSL.Status = Status;
                        TempSRSL.WhoChangeStatus = CurrentUserId;

                        if (TempSRSL.ApplicationProviderUsers.Mobile != null &&
                            TempSRSL.ApplicationProviderUsers.Mobile != "")
                        {
                            // ------------------ به خدمت دهنده در صورت موافقت مرحله قبل پیامک 
                            //string Text = "درخواست از سایت خدمات آنلاین " + "\n" +
                            //" مشتری :" + TempSRSL.ApplicationReceiverUsers.Name + " " + TempSRSL.ApplicationReceiverUsers.Family + " - " +
                            // " آدرس : " + TempSRSL.ApplicationReceiverUsers.HomeAddress /*+ TempSRSL.ApplicationReceiverUsers.HomeNumber*/ + " - " + TempSRSL.ApplicationReceiverUsers.HomePhone + " - " + TempSRSL.ApplicationReceiverUsers.Mobile +
                            //" - کد درخواست : " + ServiceReceiverServiceLocationId + "\n" +
                            ////   " - کد خدمت : " + TempSRSL.ServiceLocations.ServiceId + "\n" +
                            ////" - نام خدمت : " + TempSRSL.ServiceLocations.Services.Title +
                            ////" - مبلغ : " + TempSRSL.WorkUnits.ServiceWorkUnits.Where(c => c.ServicePropertiesId == TempSRSL.ServiceLocations.ServiceId) + " تومان "
                            //" جهت قطعی شدن درخواست عدد 3 و غیر قطعی عدد 13 را با فرمت زیر ارسال نمایید. " + "\n" +
                            //" کد درخواست * عدد - مثال" + "3*147" +
                            //" - لطفا حداکثر تا نیم ساعت با متقاضی تماس حاصل نمایید  ";

                            string Text = "کد: " + ServiceReceiverServiceLocationId + " نام : " +
                                          TempSRSL.ApplicationReceiverUsers.Name + " " +
                                          TempSRSL.ApplicationReceiverUsers.Family + " تلفن: " +
                                          TempSRSL.ApplicationReceiverUsers.Mobile +
                                          "،حداکثر در 20 دقیقه با مشتری تماس گرفته و در صورت " + "\n" +
                                          //  "قطعی شدن: " + ServiceReceiverServiceLocationId + "*3" + "\n" +
                                          "قطعی شدن: 3*" + ServiceReceiverServiceLocationId + "\n" +
                                          //  "و قطعی نشدن: " + ServiceReceiverServiceLocationId + "*13" + "\n" +
                                          "و قطعی نشدن: 13*" + ServiceReceiverServiceLocationId + "\n" +
                                          "را پیامک و یا در پنل اقدام کنید.";
                            Sms.SendSmsClass(TempSRSL.ApplicationProviderUsers.Mobile, Text, CurrentUserId);

                            Text =
                                "خدمت دهنده محترم ضمن تشکر از جنابعالی، تاکید می گردد، هر گونه تقاضای مشتریان، باید از مجاری تعریف شده شرکت باشد و ارتباط خارج از مقررات، تخلف و پیگرد خواهد داشت. ";
                            Sms.SendSmsClass(TempSRSL.ApplicationProviderUsers.Mobile, Text, CurrentUserId);
                        }
                    }
                    else if (Status == (int) StatusServiceLocationRequest.certain &&
                             TempSRSL.Status == (int) StatusServiceLocationRequest.Accept) // اگر قطعی کرد
                    {
                        TempSRSL.DateCertainStatus = PD.PersianDateLow();
                        TempSRSL.TimeCertainStatus = PD.CurrentTime();
                        //RequestTemp.WorkUnitId = WorkUnitId;                     
                        TempSRSL.Status = Status;
                        TempSRSL.WhoChangeStatus = CurrentUserId;

                        //DebtServiceReceiverServiceLocation Temp1 = new DebtServiceReceiverServiceLocation();
                        //Temp1.ServiceReceiverServiceLocationId = ServiceReceiverServiceLocationId;
                        //if (WorkUnitId != 0)
                        //{
                        //    var x = _ServiceLocationWorkUnit.Where(c => c.WorkUnitId == WorkUnitId && c.ServiceLocationId == TempSRSL.ServiceLocationId).FirstOrDefault().PriceWorkUnit;

                        //    Temp1.TotalCost = x;
                        //    Temp1.PercentOfShares = TempSRSL.ServiceLocations.PercentOfShares;
                        //    float percent = Temp1.PercentOfShares / 100f;
                        //    Temp1.CompanyCost = Math.Round(percent * x);
                        //    Temp1.Status = 0; // پرداخت نشده؟؟؟؟
                        //    Temp1.StatusServiceReceiverServiceLocation = Convert.ToByte(StatusServiceLocationRequest.certain);
                        //    _DebtServiceReceiverServiceLocationDb.Add(Temp1);

                        //    TempSRSL.WorkUnitId = WorkUnitId;
                        //}
                        //------------------------------- پیام به خدمت دهنده
                        if (TempSRSL.ApplicationProviderUsers.Mobile != null &&
                            TempSRSL.ApplicationProviderUsers.Mobile != "")
                        {
                            //string Text = TempSRSL.ApplicationProviderUsers.Name + " " + TempSRSL.ApplicationProviderUsers.Family +
                            //               " عزیز درصورت بروز مشکل در مورد خدمت " + TempSRSL.ServiceLocations.Services.Title + " در سایت خدمات آنلاین عدد 10را ارسال نمایید. در غیراینصورت پیام را با فرمت زیر وارد نمایید " +
                            //" دقیقه ورود*ساعت ورود*دقیقه خروج*ساعت خروج*کد درخواست*11 - مثال" + "30*10*30*12*147*15" + "\n" +
                            //" - کد درخواست : " + ServiceReceiverServiceLocationId + "\n";
                            // "  کد خدمت : " + TempSRSL.ServiceLocations.ServiceId + "\n";

                            string Text = "شما خدمت " + ServiceReceiverServiceLocationId +
                                          "را قطعی نمودید. جهت اقدامات بعدی به پنل خود مراجعه و یا کدهای زیر را پیامک کنید" +
                                          "\n" +
                                          "زمان ورود" + "\n" +
                                          //    "دقیقه*ساعت ورود*" + ServiceReceiverServiceLocationId + "*5" + "\n" +
                                          "دقیقه*ساعت ورود*5*" + ServiceReceiverServiceLocationId + "\n" +
                                          " خروج بااتمام خدمت" + "\n" +
                                          // "دقیقه*ساعت خروج*" + ServiceReceiverServiceLocationId + "*4" + "\n" +
                                          "دقیقه*ساعت خروج*4*" + ServiceReceiverServiceLocationId + "\n" +
                                          "خروج با خدمت ناتمام" + "\n" +
                                          //   "دقیقه*ساعت خروج*" + ServiceReceiverServiceLocationId + "*14" + "\n" +
                                          "دقیقه*ساعت خروج*14*" + ServiceReceiverServiceLocationId + "\n" +
                                          "مبالغ دریافتی" + "\n" +
                                          // "مبلغ به تومان*" + ServiceReceiverServiceLocationId + "*7" + "\n" +
                                          "مبلغ به تومان*7*" + ServiceReceiverServiceLocationId + "\n" +
                                          "بروز مشکل" + "\n" +
                                          ServiceReceiverServiceLocationId + "*10";
                            Sms.SendSmsClass(TempSRSL.ApplicationProviderUsers.Mobile, Text, CurrentUserId);
                        }

                        //------------------------------- پیام به مشتری
                        if (TempSRSL.ApplicationReceiverUsers.Mobile != null &&
                            TempSRSL.ApplicationReceiverUsers.Mobile != "")
                        {
                            //string Text = TempSRSL.ApplicationReceiverUsers.Name + " " + TempSRSL.ApplicationReceiverUsers.Family +
                            //    " عزیز درخواست شما برای خدمت " + TempSRSL.ServiceLocations.Services.Title + " در سایت خدمات آنلاین قطعی شده است. در صورت بروزمشکل عدد 20 را ارسال نمایید.";

                            string Text = "خدمت درخواستی شما با کد " + ServiceReceiverServiceLocationId + "قطعی و " +
                                          SexProvider + TempSRSL.ApplicationProviderUsers.Name + " " +
                                          TempSRSL.ApplicationProviderUsers.Family + " جهت ارائه خدمت تعیین گردید. " +
                                          "\n" + Domain;
                            Sms.SendSmsClass(TempSRSL.ApplicationReceiverUsers.Mobile, Text, CurrentUserId);
                        }
                    }
                    else if (Status == (int) StatusServiceLocationRequest.certain &&
                             TempSRSL.Status == (int) StatusServiceLocationRequest.checking)
                    {
                        // اگر خدمت دهنده با پیامک قصد قطعی کردین داشته باشه و هنوز در سایت در حال بررسی باشد 
                        //------------------------------- پیام به خدمت دهنده
                        if (TempSRSL.ApplicationProviderUsers.Mobile != null &&
                            TempSRSL.ApplicationProviderUsers.Mobile != "")
                        {
                            string Text = TempSRSL.ApplicationProviderUsers.Name + " " +
                                          TempSRSL.ApplicationProviderUsers.Family +
                                          "  خدمت " + TempSRSL.ServiceLocations.Services.Title +
                                          " هنوز در انتظار موافقت و بررسی می باشد. ";
                            Sms.SendSmsClass(TempSRSL.ApplicationProviderUsers.Mobile, Text, CurrentUserId);
                        }
                    }
                    else if (Status == (int) StatusServiceLocationRequest.Unfinished &&
                             TempSRSL.Status == (int) StatusServiceLocationRequest.certain) // اگر ناتمام بود
                    {
                        TempSRSL.Status = Status;
                        TempSRSL.WhoChangeStatus = CurrentUserId;

                        //// ارسال پیامک در صورتی که خدمت دهنده میخواهد وضعیت اتمام درخواست را معین کند
                        //if (TempSRSL.ApplicationProviderUsers.Mobile != null && TempSRSL.ApplicationProviderUsers.Mobile != "")
                        //{
                        //    string Text = TempSRSL.ApplicationProviderUsers.Name + " " + TempSRSL.ApplicationProviderUsers.Family +
                        //                   " عزیز درصورتی که خدمت " + TempSRSL.ServiceLocations.Services.Title + " در سایت خدمات آنلاین به پایان رسیده است عدد 4 و در غیراینصورت عدد 14 را با فرمت زیر وارد نمایید " +
                        //    " مبلغ(درصورتی که دریافت شده است)*کد خدمت*15 - مثال" + "4*147*20000" + "\n" +
                        //   " - کد درخواست : " + ServiceReceiverServiceLocationId + "\n";
                        //    //"  کد خدمت : " + TempSRSL.ServiceLocations.ServiceId + "\n";
                        //    Sms.SendSmsClass(TempSRSL.ApplicationProviderUsers.Mobile, Text, CurrentUserId);
                        //}
                    }
                    else if (Status == (int) StatusServiceLocationRequest.final &&
                             TempSRSL.Status == (int) StatusServiceLocationRequest.Unfinished) // اگر تمام شده بود
                    {
                        TempSRSL.Status = Status;
                        TempSRSL.WhoChangeStatus = CurrentUserId;

                        //------------------------------- پیام به مشتری
                        if (TempSRSL.ApplicationReceiverUsers.Mobile != null &&
                            TempSRSL.ApplicationReceiverUsers.Mobile != "")
                        {
                            //string Text = TempSRSL.ApplicationReceiverUsers.Name + " " + TempSRSL.ApplicationReceiverUsers.Family +
                            //    " عزیز خدمت " + TempSRSL.ServiceLocations.Services.Title + " درسایت خدمات آنلاین به پایان رسیده است.جهت تایید و ارزشیابی به سایت مراجعه نمایید. ";
                            string Text =
                                "ضمن قدردانی، از آنجا که رضایت شما، تنها مسیر توفیق  ماست و نظرات ارزنده شما درانتخاب و امتیاز خدمت دهندگان موثرترین است،لطفا فرم ارزشیابی را در پنل خودتکمیل و زمان و مبالغ پرداختی را تایید فرمایید. "
                                + "\n" + Domain + "/Account/Login";
                            Sms.SendSmsClass(TempSRSL.ApplicationReceiverUsers.Mobile, Text, CurrentUserId);
                        }

                        //------------ثبت بدهی
                        setDebtAfterEnd(ServiceReceiverServiceLocationId, WorkUnitId, TempSRSL.ServiceLocationId,
                            TempSRSL.ServiceLocations.PercentOfShares);
                    }
                    else if (Status == (int) StatusServiceLocationRequest.UnCertain &&
                             TempSRSL.Status == (int) StatusServiceLocationRequest.Accept) // اگر غیر قطعی بود
                    {
                        TempSRSL.Status = Status;
                        TempSRSL.WhoChangeStatus = CurrentUserId;

                        //TempSRSL.TypeProblem = Convert.ToByte(Problem.AfterCertainByServiceProvider);
                        //TempSRSL.ReasonProblem = ReasonCancel;
                        //TempSRSL.ReasonProblemByUserId = CurrentUserId;
                        //TempSRSL.DateProblem = PD.PersianDateLow();
                        //TempSRSL.TimeProblem = PD.CurrentTime();
                    }
                    else if (Status == 12) // در صورتی که خدمت دهنده مخالفت کرده و. مشتری درخواست خدمت دهنده دیگر دارد
                    {
                        //------------------------------- پیام به مشتری
                        if (TempSRSL.ApplicationReceiverUsers.Mobile != null &&
                            TempSRSL.ApplicationReceiverUsers.Mobile != "")
                        {
                            string Text = SexReceiver + TempSRSL.ApplicationReceiverUsers.Name + " " +
                                          TempSRSL.ApplicationReceiverUsers.Family +
                                          " عزیز، پاسخ خدمت دهنده انتخابی شما به خدمت " +
                                          ServiceReceiverServiceLocationId +
                                          "منفی بود. لطفا شخص دیگری انتخاب و یا در سایت و یا پیامک کد " + //"\n" + ServiceReceiverServiceLocationId + "*9"+
                                          "\n" + "9*" + ServiceReceiverServiceLocationId +
                                          "\n" + "انتخاب را به شرکت واگذار نمایید.";
                            Sms.SendSmsClass(TempSRSL.ApplicationReceiverUsers.Mobile, Text, CurrentUserId);
                        }
                    }

                    var s = _uow.SaveAllChanges();
                }

                return 1;
            }
            catch (System.Exception ex)
            {
                string Message = ex.Message;
                return 0;
            }
        }

        public string SendSmsForProviderNewRequest(decimal ServiceReceiverServiceLocationId,
            ServiceReceiverServiceLocation service, string Domain = "", int ServiceLevelListId = 0)
        {
            //  var p = service.WorkUnits.ServiceLocationWorkUnits.FirstOrDefault();
            var p = _ServiceLocationWorkUnit.Where(c => c.ServiceLocationId == service.ServiceLocationId)
                .FirstOrDefault();
            var SL = _ServiceLevelList.Where(c =>
                    c.ServicePropertiesId == service.ServiceLocations.ServiceId && c.Id == ServiceLevelListId)
                .FirstOrDefault();
            float Percent = 0;
            if (SL != null)
            {
                Percent = SL.PercentServiceLevel;
                Percent = Percent / 100;
            }

            string Text = "خدمت جدید از سی پارس با کدخواست " + ServiceReceiverServiceLocationId + " موضوع  " +
                          service.ServiceLocations.Services.Title +
                          " در " + service.ApplicationReceiverUsers.HomeAddress +
                          " به مبلغ " + (p.PriceWorkUnit + (Percent * p.PriceWorkUnit)) + " تومان (" +
                          p.WorkUnits.Title + ") برای" + service.DateRegister +
                          "درخواست شد. در حداکثر 10 دقیقه جهت" + "\n" +
                          //   "موافقت:  " + ServiceReceiverServiceLocationId + "*2 " + "\n" +
                          "موافقت: 2*" + ServiceReceiverServiceLocationId + "\n" +
                          //"و مخالفت: " + ServiceReceiverServiceLocationId + "*12 " + "\n" +
                          "مخالفت: 12*" + ServiceReceiverServiceLocationId + "\n" +
                          "را پیامک و یا در پنل خوداقدام نمایید." + "\n" + Domain;
            return Text;
        }

        public void setDebtAfterEnd(int ServiceReceiverServiceLocationId, int WorkUnitId, int ServiceLocationId,
            double PercentOfShares, int CalcPrice = 0, int CalcPriceReceived = 0)
        {
            DebtServiceReceiverServiceLocation Temp1 = new DebtServiceReceiverServiceLocation();
            Temp1.ServiceReceiverServiceLocationId = ServiceReceiverServiceLocationId;
            if (WorkUnitId != 0)
            {
                int x = 0;
                var q = _ServiceLocationWorkUnit
                    .Where(c => c.WorkUnitId == WorkUnitId && c.ServiceLocationId == ServiceLocationId)
                    .FirstOrDefault();
                if (q != null)
                    x = q.PriceWorkUnit;

                // Temp1.TotalCost = x;
                Temp1.TotalCostUnit = x;
                Temp1.TotalCost = CalcPrice;
                Temp1.TotalCostReceived = CalcPriceReceived;

                Temp1.PercentOfShares = (int) PercentOfShares;
                float percent = Temp1.PercentOfShares / 100f;
                //Temp1.CompanyCost = Math.Round(percent * x);
                Temp1.CompanyCost = Math.Round(percent * CalcPriceReceived);

                Temp1.Status = 0; // پرداخت نشده؟؟؟؟
                Temp1.StatusServiceReceiverServiceLocation = Convert.ToByte(StatusServiceLocationRequest.certain);
                _DebtServiceReceiverServiceLocationDb.Add(Temp1);
            }
        }

        public async Task<int> RefrenceServiceReceiverServiceLocations(int Id, int SendByUserId = 0, string Domain = "")
        {
            try
            {
                PersianDate PD = new PersianDate();
                int savestatus = 0;
                string Mobile = "";
                var temp = _ServiceReceiverServiceLocations.Where(c => c.Id == Id).FirstOrDefault();

                if (temp != null)
                {
                    double MaxScore = 0;
                    var userserviceLocation = _UserServiceLocation.Where(a =>
                        a.ServiceLocationId == temp.ServiceLocationId &&
                        a.UserId != temp.ServiceProviderId).ToList();
                    if (userserviceLocation.Count() != 0)
                    {
                        foreach (var item in userserviceLocation)
                        {
                            var temp1 = _ServiceReceiverServiceLocations.Where(x =>
                                x.ServiceProviderId == item.UserId &&
                                (x.Status == 0 || x.Status == 1 || x.Status == 2 || x.Status == 3));

                            var exitUserService = _UserService.Where(b =>
                                b.UserId == item.UserId && b.ServiceId == item.ServiceId &&
                                b.CapacityServiceUser > temp1.Count());
                            if (exitUserService.Count() != 0)
                            {
                                var Score = exitUserService.OrderByDescending(c => c.CountSTarScoreServiceUser)
                                    .FirstOrDefault().CountSTarScoreServiceUser;
                                if (MaxScore < Score)
                                    MaxScore = Score;
                            }
                        }

                        var UserMaxScore = _UserService.Where(c => c.CountSTarScoreServiceUser == MaxScore)
                            .FirstOrDefault().UserId;
                        temp.ServiceProviderId = UserMaxScore;
                    }
                    else
                    {
                        //*********** خدمت دهنده ی دیگری وجود ندارد
                        temp.TypeProblem = Convert.ToByte(Problem.NotFindAnotherServiceProvider);
                        temp.ReasonProblem = "خدمت دهنده ی دیگری وجود ندارد";
                        //  temp.ReasonProblemByUserId = ;
                        temp.DateProblem = PD.PersianDateLow();
                        temp.TimeProblem = PD.CurrentTime();
                    }

                    savestatus = await _uow.SaveAllChangesAsync();

                    if (savestatus != 0)
                    {
                        SendSms Sms = new SendSms(_uow);
                        var alluser = await _userManager.GetAlTypelUsers();
                        var u = alluser.Where(c => c.Id == temp.ServiceProviderId).FirstOrDefault().Mobile;
                        string Text = SendSmsForProviderNewRequest(Id, temp);
                        Sms.SendSmsClass(u, Text, SendByUserId);
                    }
                }

                return savestatus;
            }
            catch (System.Exception ex)
            {
                string Message = ex.Message;
                return 0;
            }
        }

        public async Task<int> ConfirmServiceReceiverRequest(int RequestId, byte StatusConfirm)
        {
            try
            {
                int savestatus = 0;
                var RequestTemp = _servicerequest.Where(c => c.Id == RequestId).FirstOrDefault();
                if (RequestTemp != null)
                {
                    RequestTemp.ConfirmServiceReceiver = StatusConfirm;
                    savestatus = await _uow.SaveAllChangesAsync();
                }

                return savestatus;
            }
            catch (System.Exception ex)
            {
                string Message = ex.Message;
                return 0;
            }
        }

        public async Task<int> AddPriceReceivedByServiceProvider(int ServiceReceiverServiceLocationId, int Price)
        {
            try
            {
                PersianDate Pd = new PersianDate();
                var Temp = _ServiceReceiverServiceLocations.Where(c => c.Id == ServiceReceiverServiceLocationId)
                    .FirstOrDefault();
                if (Temp != null)
                {
                    var Query = _servicerequest.Where(c =>
                        c.ServiceReceiverServiceLocationId == ServiceReceiverServiceLocationId);
                    var TempRequest = Query.OrderByDescending(c => c.Id).FirstOrDefault();
                    if (TempRequest != null)
                    {
                        TempRequest.PriceReceived = Price;
                        TempRequest.PriceReceivedDate = Pd.PersianDateLow();
                    }

                    Temp.CalcPriceReceived = Price + Temp.CalcPriceReceived;
                    //    Temp.CalcPriceReceived = Price + Query.Sum(c=>c.PriceReceived);

                    var temp1 = _DebtDb.OfType<DebtServiceReceiverServiceLocation>()
                        .Where(c => c.ServiceReceiverServiceLocationId == ServiceReceiverServiceLocationId)
                        .FirstOrDefault();
                    if (temp1 != null)
                    {
                        temp1.TotalCostReceived = Price + temp1.TotalCostReceived;
                        // temp1.TotalCostReceived = Price + Query.Sum(c => c.PriceReceived);

                        double percent = Temp.ServiceLocations.PercentOfShares / 100f;
                        temp1.CompanyCost = Math.Round(percent * temp1.TotalCostReceived);
                    }

                    await _uow.SaveAllChangesAsync();
                }

                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<int> AddFromTimeAndToTimeRequest(int ServiceReceiverServiceLocationId, int Status,
            string Time = "", string Date = "")
        {
            try
            {
                var TempRequest = _servicerequest
                    .Where(c => c.ServiceReceiverServiceLocationId == ServiceReceiverServiceLocationId)
                    .OrderByDescending(c => c.Id).FirstOrDefault();
                if (TempRequest != null)
                {
                    if (TempRequest.FromTime != "0" && TempRequest.ToTime != "0"
                    ) // اگر آخرین رکورد دارای ساعت ورود و خروج باشد
                    {
                        ServiceReceiverRequest Request = new ServiceReceiverRequest();
                        Request.Date = Date;
                        Request.FromTime = Time;
                        Request.ToTime = "0";
                        Request.StatusRequest = Status;
                        Request.ConfirmServiceReceiver = Convert.ToByte(StatusConfirmServiceReciverRequest.NotChecked);
                        Request.ServiceReceiverServiceLocationId = ServiceReceiverServiceLocationId;
                        _servicerequest.Add(Request);
                    }
                    else if (TempRequest.FromTime != "0" && TempRequest.ToTime == "0"
                    ) // اگر آخرین رکورد دارای ساعت ورود باشد وساعت  خروج نباشد
                    {
                        PersianDate Pd = new PersianDate();
                        var FromDateMiladi = Pd.shamsiToMiladi(Date + " " + TempRequest.FromTime);
                        var ToDateMiladi = Pd.shamsiToMiladi(Date + " " + Time);
                        DateTime first = DateTime.Parse(Convert.ToString(FromDateMiladi));
                        DateTime second = DateTime.Parse(Convert.ToString(ToDateMiladi));
                        TimeSpan ts = second - first;
                        var priceCalcBySystem = 0;
                        var y = _ServiceReceiverServiceLocations.Where(c => c.Id == ServiceReceiverServiceLocationId);
                        var x = _ServiceLocationWorkUnit.Where(c =>
                            c.WorkUnitId == y.FirstOrDefault().WorkUnitId &&
                            c.ServiceLocationId == y.FirstOrDefault().ServiceLocationId);
                        if (x.Count() > 0)
                        {
                            /// رند کردن تا 3 رقم
                            priceCalcBySystem =
                                Convert.ToInt32(
                                    Math.Round(((x.FirstOrDefault().PriceWorkUnit / 60) * ts.TotalMinutes) / 1000) *
                                    1000);
                        }

                        TempRequest.ToTime = Time;
                        TempRequest.PriceCalcBySystem = priceCalcBySystem;
                        TempRequest.StatusRequest = Status;
                        //   await _uow.SaveAllChangesAsync();

                        //******************************************* افزودن به بدهی های کاربر 
                        var tempDebt = _DebtDb.OfType<DebtServiceReceiverServiceLocation>()
                            .Where(c => c.ServiceReceiverServiceLocationId == ServiceReceiverServiceLocationId)
                            .FirstOrDefault();

                        if (tempDebt != null)
                        {
                            double d = 0;
                            if ((float) tempDebt.TotalCost != 0)
                                d = tempDebt.CompanyCost / (float) tempDebt.TotalCost;
                            tempDebt.CompanyCost = Math.Round((d * TempRequest.PriceCalcBySystem) / 1000) * 1000;
                            tempDebt.TotalCost =
                                Math.Round((TempRequest.PriceCalcBySystem + tempDebt.TotalCost) / 1000) * 1000;
                        }
                        else
                        {
                            int ServiceLocationId = 0;
                            double PercentOfShares = 0;
                            int WorkUnitId = 0;
                            var Q = y.FirstOrDefault();
                            if (Q != null)
                            {
                                ServiceLocationId = Q.ServiceLocationId;
                                PercentOfShares = Q.ServiceLocations.PercentOfShares;
                                WorkUnitId = (int) Q.WorkUnitId;
                            }

                            setDebtAfterEnd(ServiceReceiverServiceLocationId, WorkUnitId, ServiceLocationId,
                                PercentOfShares);
                        }

                        //******************************************** مبلغ کلی که باید تاکنون دریافت می شده.
                        var TempSRSL = _ServiceReceiverServiceLocations
                            .Where(c => c.Id == ServiceReceiverServiceLocationId).FirstOrDefault();
                        TempSRSL.CalcPrice =
                            Convert.ToInt32(Math.Round((TempSRSL.CalcPrice + TempRequest.PriceCalcBySystem) / 1000.0) *
                                            1000);
                    }

                    await _uow.SaveAllChangesAsync();
                }
                else
                {
                    ServiceReceiverRequest Request = new ServiceReceiverRequest();
                    Request.Date = Date;
                    Request.FromTime = Time;
                    Request.ToTime = "0";
                    Request.StatusRequest = Status;
                    Request.ConfirmServiceReceiver = Convert.ToByte(StatusConfirmServiceReciverRequest.NotChecked);
                    Request.ServiceReceiverServiceLocationId = ServiceReceiverServiceLocationId;
                    _servicerequest.Add(Request);
                    await _uow.SaveAllChangesAsync();
                }

                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public double ReturnPricePayment(string[] ListId)
        {
            try
            {
                string ListIdSend = "";
                var sum = 0.0;
                foreach (var item in ListId)
                {
                    var su = Convert.ToInt32(item);
                    var t = _DebtDb.Where(c => c.Id == su);
                    sum = sum + t.FirstOrDefault().CompanyCost;
                    ListIdSend += "," + item.ToString();
                }

                ListIdSend = ListIdSend.Substring(1, ListIdSend.Count() - 1);
                return sum;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        //---------------------------------------------------------------------

        /// <summary>
        /// ثبت اطلاعات  پرداخت قبل از رفتن به بانک
        /// </summary>
        /// <param name="DeptId"></param>
        /// <param name="Price"></param>
        /// <param name="CurrentUserId"></param>
        /// <returns></returns>
        public async Task<int[]> FunInsertPaymentPrivateTraning(string[] DeptId, int Price = 0, int CurrentUserId = 0)
        {
            int[] Result = new int[2];

            try
            {
                double AllPrice = 0;

                var ActivePayments = Convert.ToByte(ActivePayment.Pending);
                PersianDate Pd = new PersianDate();
                payment Payment = new payment();
                var MemberId = GetMemberId(Convert.ToInt32(DeptId[0]));

                //--------------------------------------  جمع مبلغ همه رکوردهای انتخاب شده
                if (DeptId.Length > 1)
                {
                    foreach (var item in DeptId)
                    {
                        var s = Convert.ToUInt32(item);
                        var d = _DebtDb.Where(c => c.IsEnable == true && c.Id == s).FirstOrDefault();
                        if (d != null)
                            AllPrice += d.CompanyCost;
                    }
                }
                else
                    AllPrice = MemberId[1];

                //-----------------------------------------------
                Payment.CodeBank = "";
                //   Payment.Price = Price;
                Payment.Price = (int) AllPrice;
                Payment.Date = Pd.PersianDateLow();
                Payment.MemberId = MemberId[0];
                Payment.ModratorId = CurrentUserId;
                Payment.verified = Convert.ToInt32(Paymentverified.NotOnline);
                Payment.DeptId = Convert.ToInt32(DeptId[0]);
                Payment.ActivePayment = ActivePayments;
                _payment.Add(Payment);

                foreach (var item in DeptId)
                {
                    if (DeptId.Length > 1)
                        MemberId = GetMemberId(Convert.ToInt32(item));

                    paymentDetail PaymentD = new paymentDetail();
                    PaymentD.CompanyCostDebt = MemberId[1];
                    PaymentD.Date = Pd.PersianDateLow();
                    PaymentD.MemberId = MemberId[0];
                    PaymentD.ModratorId = CurrentUserId;
                    PaymentD.DebtId = Convert.ToInt32(item);
                    PaymentD.paymentId = Payment.Id;
                    _paymentDetail.Add(PaymentD);
                }

                var t = _uow.SaveAllChanges();
                Result[0] = Payment.Id;
                Result[1] = (int) AllPrice;

                return Result;
            }
            catch (Exception ex)
            {
                Result[0] = 0;
                Result[1] = 0;
                return Result;
            }
        }

        public int[] GetMemberId(int DebtId = 0)
        {
            int[] Result = new int[2];
            var Query = _DebtDb.OfType<DebtServiceReceiverServiceLocation>()
                .Where(c => c.IsEnable == true && c.Id == DebtId).FirstOrDefault();
            if (Query != null)
            {
                Result[0] = Query.ServiceReceiverServiceLocations.ServiceProviderId;
                Result[1] = (int) Query.CompanyCost;
            }
            else
            {
                var Query2 = _DebtDb.OfType<DebtServiceProvider>().Where(c => c.IsEnable == true && c.Id == DebtId)
                    .FirstOrDefault();
                if (Query2 != null)
                {
                    Result[0] = Query2.ServiceProviderId;
                    Result[1] = (int) Query2.CompanyCost;
                }
            }

            return Result;
        }

        /// <summary>
        /// آپدیت اطلاعات  پرداخت بعد از برگشت از بانک
        /// </summary>
        /// <param name="PaymentId"></param>
        /// <param name="RefasId"></param>
        /// <param name="Type"></param>
        /// <returns></returns>
        public string UpdatePaymentPrivateTraning(int PaymentId = 0, string RefasId = "", string Type = "")
        {
            try
            {
                PersianDate Pd = new PersianDate();
                var ActivePayments = Convert.ToByte(ActivePayment.Ok);

                var Payment = _payment.Where(c => c.Id == PaymentId).FirstOrDefault();
                if (Payment != null)
                {
                    Payment.CodeBank = RefasId + "_" + Type;
                    Payment.ActivePayment = ActivePayments;

                    var Debt = _DebtDb.Where(c => c.Id == Payment.DeptId).FirstOrDefault();
                    if (Debt != null)
                        Debt.Status = 1;

                    var t = _uow.SaveAllChanges();
                }

                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        //-----------------------------------

        /// <summary>
        /// نمایش نام خدمت تا ریشه
        /// </summary>
        /// <param name="GId"></param>
        /// <returns></returns>
        public string FunGroupName(int GId)
        {
            string ListGroupName = "";

            try
            {
                int Level = _Service.Where(c => c.Id == GId).FirstOrDefault().Level;
                if (Level > 0)
                {
                    for (int i = 1; i <= Level; i++)
                    {
                        ListGroupName += _Service.Where(c => c.Id == GId).FirstOrDefault().Title + "|" + GId + ">";
                        int ParentId = _Service.Where(c => c.Id == GId).FirstOrDefault().ParentId;
                        GId = ParentId;
                    }
                }

                string[] ListArray = ListGroupName.Substring(0, ListGroupName.Length - 1).Split('>');
                ListGroupName = "";
                //for (int i = ListArray.Length - 1; i >= 0; i--)

                var Index = ListArray.Length - 1;

                if (ListArray.Length > 5)
                {
                    Index = ListArray.Length - 4;
                }
                else if (ListArray.Length > 4)
                {
                    Index = ListArray.Length - 3;
                }
                else if (ListArray.Length > 3)
                {
                    Index = ListArray.Length - 2;
                }

                for (int i = Index; i >= 0; i--)
                {
                    string[] Name = ListArray[i].Split('|');
                    ListGroupName += "<span style=\"color:#2dc3e8;\" >" + Name[0] + "</span> >>";
                }

                ListGroupName = ListGroupName.Substring(0, ListGroupName.Length - 2)
                    .Replace(">>", "<span style=\"color:#888;\"> > </span>");
            }
            catch (Exception ex)
            {
                var m = ex.Message;
            }

            return ListGroupName;
        }
    }
}