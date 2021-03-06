using PrivateTraining.DataLayer.Context;
using PrivateTraining.DomainClasses.Entities.FrameWork;
using PrivateTraining.DomainClasses.Entities.PrivateTraining;
using PrivateTraining.ServiceLayer.BLL;
using PrivateTraining.ServiceLayer.Extention;
using PrivateTraining.ServiceLayer.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


namespace PrivateTraining.Areas.Framework
{
    public class SmsController : Controller
    {
        private IUnitOfWork _uow;
        private IDbSet<ServiceReceiverServiceLocation> _ServiceReceiverServiceLocations;
        private IDbSet<Setting> _Setting;
        PersianDate PD = new PersianDate();


        private readonly IApplicationUserManager _userManager;

        public SmsController(IUnitOfWork uow, IApplicationUserManager userManager)
        {
            _uow = uow;
            _ServiceReceiverServiceLocations = _uow.Set<ServiceReceiverServiceLocation>();
            _userManager = userManager;
            _Setting= _uow.Set<Setting>();


        }
      

        public virtual async Task<int> UpdateRecordBySms(string ContentSms, string Mobile)
        {
            try
            {
                PersianDate pd = new PersianDate();
                string[] tokens = ContentSms.Split('*');
                var TempId = Convert.ToInt32(tokens[0]);
                var alluser = await _userManager.GetAlTypelUsers();
                var IsUser = alluser.ToList().Where(c => c.Mobile == Mobile).FirstOrDefault();
                var SendByUserId = 0;
                var dateReceiveSms = pd.PersianDateLow();
                var Temp = _ServiceReceiverServiceLocations.Where(c => c.Id == TempId).FirstOrDefault();
                if (IsUser != null)
                {
                    SendByUserId = IsUser.Id;
                }
                PrivateTraining.ServiceLayer.BLL.PrivateTraining Private = new PrivateTraining.ServiceLayer.BLL.PrivateTraining(_uow);


                switch (tokens[1])
                {   // موافق 
                    case "2":
                        await Private.ChangeStatusRequestService(TempId, 1, SendByUserId, 0);
                        break;
                    // مخالف 
                    case "12":
                        await Private.RefrenceServiceReceiverServiceLocations(TempId);
                        break;
                    // قطعی
                    case "3":
                        await Private.ChangeStatusRequestService(TempId, 2, SendByUserId, 0);
                        break;
                    //غیر قطعی
                    case "13":
                        await Private.ChangeStatusRequestService(TempId, 6, SendByUserId, 0);
                        Temp.TypeProblem = Convert.ToByte(Problem.UnCertain);
                        Temp.ReasonProblem = "ReasonCancel";
                        Temp.ReasonProblemByUserId = SendByUserId;
                        Temp.DateProblem = pd.PersianDateLow();
                        Temp.TimeProblem = pd.CurrentTime();
                        break;
                    //ثبت بروز مشکل توسط خدمت گیرنده
                    case "20":
                        Temp.TypeProblem = Convert.ToByte(Problem.AfterCertainByServiceReceiver);
                        Temp.ReasonProblem = "ReasonCancel";
                        Temp.ReasonProblemByUserId = SendByUserId;
                        Temp.DateProblem = pd.PersianDateLow();
                        Temp.TimeProblem = pd.CurrentTime();
                        break;
                    //ثبت بروز مشکل توسط خدمت دهنده
                    case "10":
                        Temp.TypeProblem = Convert.ToByte(Problem.AfterCertainByServiceProvider);
                        Temp.ReasonProblem = "ReasonCancel";
                        Temp.ReasonProblemByUserId = SendByUserId;
                        Temp.DateProblem = pd.PersianDateLow();
                        Temp.TimeProblem = pd.CurrentTime();
                        break;
                    //- پیام 5 - ناتمام
                    case "11":
                        await Private.ChangeStatusRequestService(TempId, 3, SendByUserId, 0);
                        if (tokens.Count() > 3 && Convert.ToInt32(tokens[2]) < 24 && Convert.ToInt32(tokens[3]) < 60) // اگر ساعت صحیح وارد کرده بود 
                            await Private.AddFromTimeAndToTimeRequest(TempId, 3, tokens[2] + ":" + tokens[3], dateReceiveSms);
                        break;
                    // تمام 
                    case "4":
                        await Private.ChangeStatusRequestService(TempId, 4, SendByUserId, 0);
                        if (tokens[2] != null)
                        {
                            await Private.AddPriceReceivedByServiceProvider(TempId, Convert.ToInt32(tokens[2]));
                        }
                        break;
                    // ناتمام - پیام 6
                    case "14":
                        await Private.ChangeStatusRequestService(TempId, 3, SendByUserId, 0);
                        if (tokens[2] != null) // اگر مبلغ ارسال شده بود
                        {
                            await Private.AddPriceReceivedByServiceProvider(TempId, Convert.ToInt32(tokens[2]));
                        }
                        break;
                }


                return 1;
            }
            catch (Exception ex)
            {

                return 0;
            }
        }

        [HttpPost]
        public virtual async Task<JsonResult> SaveSmsSended(string ContentSms,int[] UserId)
        {
            try
            {
                var SenderNumber = "";
                var k= _Setting.Where(c => c.Subject == "sms").FirstOrDefault();
                if (k != null) {
                    SenderNumber = k.Value2;
                }
                foreach (var item in UserId)
                {
                    SMSSended temp = new SMSSended();
                    temp.Content = ContentSms;
                    temp.UserId = item;
                    temp.ReceiverNumber =_userManager.FindById(item).Mobile;
                    temp.SenderNumber = SenderNumber;
                    temp.Date = PD.PersianDateLow();
                    temp.Time = PD.CurrentTime();
                    temp.StatusType = Convert.ToByte(TypeStatusDeliverSms.Error);
                    temp.Status = TypeStatusDeliverSms.Error.GetDescription();
                }
              

                return Json(new { Result = true, Message = "" });
            }
            catch (Exception)
            {
                return Json(new { Result = false, Message = "خطا" });
            }
        }
    }
}