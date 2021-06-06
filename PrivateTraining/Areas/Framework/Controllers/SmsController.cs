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
using PrivateTraining.ServiceLayer.BLL;
using System.Data.SqlClient;
using System.Configuration;

namespace PrivateTraining.Areas.Framework.Controllers
{
    public partial class SmsController : Controller
    {
        private IUnitOfWork _uow;
        private IDbSet<ServiceReceiverServiceLocation> _ServiceReceiverServiceLocations;
        private IDbSet<Setting> _Setting;
        private IDbSet<SMSSended> _SMSSended;
        private IDbSet<SMSReceived> _SMSReceived;
        private IDbSet<Debt> _DebtDb;
        private IDbSet<DebtServiceProvider> _DebtServiceProvider;
        private IDbSet<DebtServiceReceiverServiceLocation> _DebtServiceReceiverServiceLocation;
        private readonly IApplicationUserManager _userManager;
        PersianDate PD = new PersianDate();

        public SmsController(IUnitOfWork uow, IApplicationUserManager userManager)
        {
            _uow = uow;
            _ServiceReceiverServiceLocations = _uow.Set<ServiceReceiverServiceLocation>();
            _userManager = userManager;
            _Setting = _uow.Set<Setting>();
            _SMSSended = _uow.Set<SMSSended>();
            _SMSReceived = _uow.Set<SMSReceived>();
            _DebtDb = _uow.Set<Debt>();
            _DebtServiceProvider = _uow.Set<DebtServiceProvider>();
            _DebtServiceReceiverServiceLocation = _uow.Set<DebtServiceReceiverServiceLocation>();
        }

        /// <summary>
        /// تابع دریافت sms
        /// </summary>
        /// <param name="Text"></param>
        /// <param name="Mobile"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public virtual async Task<ActionResult> ReceiveSms(string from = "", string to = "", string text = "", string udh = "")
        {
            ViewBag.PMReceive = "";
            if (from == null) from = "0";
            if (to == null) to = "0";
            if (text == null) text = "0";

            try
            {
                if (from != null && text != null && to != null)
                {

                    SMSReceived temp = new SMSReceived();
                    temp.Content = text;
                    temp.UserId = 0;
                    temp.ReceiverNumber = to;
                    temp.SenderNumber = from;
                    temp.Date = PD.PersianDateLow();
                    temp.Time = PD.CurrentTime();
                    temp.StatusType = Convert.ToByte(TypeStatusDeliverSms.Success);
                    temp.Status = "1";
                    _SMSReceived.Add(temp);
                    _uow.SaveAllChanges();
                }
            }
            catch (Exception ex)
            {
            }
            //----------------------
            try
            {
                var Pm = UpdateRecordBySms(text, from);
                ViewBag.PMReceive = Pm;
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        /// <summary>
        /// عملیات بعد از اس ام اس  و ذخیره سازی در بانک
        /// </summary>
        /// <param name="ContentSms"></param>
        /// <param name="Mobile"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public virtual async Task<string> UpdateRecordBySms(string ContentSms, string Mobile)
        {
            //if (Mobile.Length == 12)
            //    Mobile = Mobile.Substring(0, 9);

            try
            {
                SendSms Sms = new SendSms(_uow);
                string Domainn = " http://" + Request.Url.Host;
                var SendByUserId = 0;
                PersianDate pd = new PersianDate();
                string[] tokens = ContentSms.Split('*');
                var TempId = Convert.ToInt32(tokens[1]);
                var Temp = _ServiceReceiverServiceLocations.Where(c => c.Id == TempId).FirstOrDefault();
                if (Temp != null)
                {
                    var alluser = await _userManager.GetAlTypelUsers();
                    var IsUser = alluser.ToList().Where(c => c.Mobile == Mobile.Substring(2, 10) || c.Mobile == "0" + Mobile.Substring(2, 10)).FirstOrDefault();
                    if (IsUser != null)
                    {
                        SendByUserId = IsUser.Id;
                    }

                    PrivateTraining.ServiceLayer.BLL.PrivateTraining Private = new PrivateTraining.ServiceLayer.BLL.PrivateTraining(_uow,_userManager);

                    switch (tokens[0])
                    {   // موافق 
                        case "2":
                            Private.ChangeStatusRequestService(TempId, (int)StatusServiceLocationRequest.Accept, SendByUserId, 0, "", Domainn);
                            break;
                        // مخالف 
                        case "12":
                            // await Private.RefrenceServiceReceiverServiceLocations(TempId);
                            Private.ChangeStatusRequestService(TempId, 12, SendByUserId, 0, "", Domainn);
                            break;
                        // در صورتی که خدمتیار مخالفت کرده و. مشتری درخواست خدمتیار دیگر دارد
                        case "9":
                            await Private.RefrenceServiceReceiverServiceLocations(TempId, SendByUserId, Domainn);
                            break;
                        // قطعی
                        case "3":
                            Private.ChangeStatusRequestService(TempId, (int)StatusServiceLocationRequest.certain, SendByUserId, 0, "", Domainn);
                            break;
                        //غیر قطعی
                        case "13":
                            Private.ChangeStatusRequestService(TempId, (int)StatusServiceLocationRequest.UnCertain, SendByUserId, 0, "", Domainn);
                            Temp.TypeProblem = Convert.ToByte(Problem.UnCertain);
                            Temp.ReasonProblem = Problem.UnCertain.GetDescription();
                            Temp.ReasonProblemByUserId = SendByUserId;
                            Temp.DateProblem = pd.PersianDateLow();
                            Temp.TimeProblem = pd.CurrentTime();
                            break;
                        //ثبت بروز مشکل توسط مشتری
                        case "20":
                            Temp.TypeProblem = Convert.ToByte(Problem.AfterCertainByServiceReceiver);
                            Temp.ReasonProblem = Problem.AfterCertainByServiceReceiver.GetDescription();
                            Temp.ReasonProblemByUserId = SendByUserId;
                            Temp.DateProblem = pd.PersianDateLow();
                            Temp.TimeProblem = pd.CurrentTime();
                            break;
                        //ثبت بروز مشکل توسط خدمتیار
                        case "10":
                            Temp.TypeProblem = Convert.ToByte(Problem.AfterCertainByServiceProvider);
                            Temp.ReasonProblem = Problem.AfterCertainByServiceProvider.GetDescription();
                            Temp.ReasonProblemByUserId = SendByUserId;
                            Temp.DateProblem = pd.PersianDateLow();
                            Temp.TimeProblem = pd.CurrentTime();
                            break;
                        //- پیام 5 - ناتمام
                        //case "11":
                        //     Private.ChangeStatusRequestService(TempId, 3, SendByUserId, 0, "",  Domainn);
                        //    if (tokens.Count() > 3 && Convert.ToInt32(tokens[2]) < 24 && Convert.ToInt32(tokens[3]) < 60) // اگر ساعت صحیح وارد کرده بود 
                        //        await Private.AddFromTimeAndToTimeRequest(TempId, 3, tokens[2] + ":" + tokens[3], pd.PersianDateLow());
                        //    break;
                        // تمام 
                        case "4":
                            Private.ChangeStatusRequestService(TempId, (int)StatusServiceLocationRequest.final, SendByUserId, 0, "", Domainn);
                            if (tokens.Count() > 3 && Convert.ToInt32(tokens[2]) < 24 && Convert.ToInt32(tokens[3]) < 60) // اگر ساعت صحیح وارد کرده بود 
                                await Private.AddFromTimeAndToTimeRequest(TempId, 4, tokens[2] + ":" + tokens[3], pd.PersianDateLow());
                            break;
                        //if (tokens[2] != null)
                        //{
                        //    await Private.AddPriceReceivedByServiceProvider(TempId, Convert.ToInt32(tokens[2]));
                        //}
                        // ناتمام - پیام 6
                        case "14":
                            Private.ChangeStatusRequestService(TempId, (int)StatusServiceLocationRequest.Unfinished, SendByUserId, 0, "", Domainn);
                            if (tokens.Count() > 3 && Convert.ToInt32(tokens[2]) < 24 && Convert.ToInt32(tokens[3]) < 60) // اگر ساعت صحیح وارد کرده بود 
                                await Private.AddFromTimeAndToTimeRequest(TempId, 3, tokens[2] + ":" + tokens[3], pd.PersianDateLow());
                            break;
                        //if (tokens[2] != null) // اگر مبلغ ارسال شده بود
                        //{
                        //    await Private.AddPriceReceivedByServiceProvider(TempId, Convert.ToInt32(tokens[2]));
                        //}
                        case "5":  // ساعت ورود
                            if (tokens.Count() > 3 && Convert.ToInt32(tokens[2]) < 24 && Convert.ToInt32(tokens[3]) < 60) // اگر ساعت صحیح وارد کرده بود 
                                await Private.AddFromTimeAndToTimeRequest(TempId, 3, tokens[2] + ":" + tokens[3], pd.PersianDateLow());
                            break;
                        case "7":// مبلغ جلسه
                            if (tokens[2] != null)
                            {
                                await Private.AddPriceReceivedByServiceProvider(TempId, Convert.ToInt32(tokens[2]));
                            }
                            break;
                        default:
                            Sms.SendSmsClass(Mobile.Substring(2, 10), "پیام ارسال شده نامعتبر است", SendByUserId);
                            break;
                    }
                }
                else
                {
                    Sms.SendSmsClass(Mobile.Substring(2, 10), "کد درخواست نامعتبر است", 0);
                }
                var s = _uow.SaveAllChanges();
                return "دریافت sms با  موفقیت انجام شد.";
            }
            catch (Exception ex)
            {
                //---------------------  در صورتیکه پیام با فرمت معین شده نباشد به خطا میخورد و برای ارسال کننده پیام زیر ارسال می شود
                //SendSms Sms = new SendSms(_uow, _userManager);
                //Sms.SendSmsClass(Mobile, "پیام ارسال شده نامعتبر است", 0);

                return "دریافت sms با شکست مواجه شد. " + ex.Message;
            }
        }

        [HttpPost]
        public virtual async Task<JsonResult> SaveSmsSended(string ContentSms, int[] UserId)
        {
            try
            {
                SendSms SendSms = new SendSms(_uow);
                var result = "";
                foreach (var item in UserId)
                {
                    var ReceiverNumber = _userManager.FindById(item).Mobile;
                    //  result = SendSms.ContentSmsAndSendSms("", ReceiverNumber, ContentSms, item);
                    result = ContentSmsAndSendSms("", ReceiverNumber, ContentSms, item);

                }
                await _uow.SaveAllChangesAsync();
                //return Json(new { Result = true, Message = "عملیات با موفقیت انجام شد." });
                return Json(new { Result = true, Message = result });
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Message = "خطا" });
            }
        }

        public string ContentSmsAndSendSms(string TypeSms, string NumTo, string Text, int UserId = 0)
        {
            try
            {
                // var ListSms = _Setting.Where(c => c.Value2 == "sms").ToList();

                //switch (TypeSms)
                //{
                //    case "[debt]":

                //        double Sumdebt = 0;
                //        var DebtSRSL = _DebtServiceReceiverServiceLocation.Include("ServiceReceiverServiceLocations").Include("ApplicationProviderUsers").Where(c => c.ServiceReceiverServiceLocations.ApplicationProviderUsers.Id == UserId && c.Status == 0).ToList();
                //        foreach (var item in DebtSRSL)
                //        {
                //            Sumdebt += item.CompanyCost;
                //        }
                //        var DebtSP = _DebtServiceProvider.Where(c => c.ServiceProviderId == UserId && c.Status == 0).ToList();
                //        foreach (var item2 in DebtSP)
                //        {
                //            Sumdebt += item2.CompanyCost;
                //        }
                //        Text = Text.Replace("[debt]", Sumdebt.ToString());
                //        break;

                //    case "[name]":
                //        var User = _userManager.FindById(UserId);
                //        Text = Text.Replace("[name]", User.Name);
                //        break;

                //    case "[family]":
                //        var User2 = _userManager.FindById(UserId);
                //        Text = Text.Replace("[family]", User2.Family);
                //        break;
                //}
                //   Text.Contains(strStart)

                if (Text.Contains("[debt]"))
                {
                    double Sumdebt = 0;
                    var DebtSRSfL = _DebtServiceReceiverServiceLocation.Where(c => c.ServiceReceiverServiceLocations.ApplicationProviderUsers.Id == UserId && c.Status == 0).ToList();

                    var DebtSRSL = _DebtServiceReceiverServiceLocation.Include("ServiceReceiverServiceLocations").Where(c => c.ServiceReceiverServiceLocations.ApplicationProviderUsers.Id == UserId && c.Status == 0).ToList();
                    foreach (var item in DebtSRSL)
                    {
                        Sumdebt += item.CompanyCost;
                    }
                    var DebtSP = _DebtServiceProvider.Where(c => c.ServiceProviderId == UserId && c.Status == 0).ToList();
                    foreach (var item2 in DebtSP)
                    {
                        Sumdebt += item2.CompanyCost;
                    }
                    Text = Text.Replace("[debt]", Sumdebt.ToString("N0"));
                }

                if (Text.Contains("[name]"))
                {
                    var User = _userManager.FindById(UserId);
                    Text = Text.Replace("[name]", User.Name);
                }

                if (Text.Contains("[family]"))
                {
                    var User2 = _userManager.FindById(UserId);
                    Text = Text.Replace("[family]", User2.Family);
                }

                SendSms SendSms = new SendSms(_uow);
                var result = SendSms.SendSmsClass(NumTo, Text, UserId);

                return result;
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }
    }

}