using PrivateTraining.DataLayer.Context;
using PrivateTraining.DomainClasses.Entities.FrameWork;
using PrivateTraining.ServiceLayer.Interface.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using PrivateTraining.ServiceLayer.Extention;
using System.Security.Principal;
using PrivateTraining.DomainClasses.Entities.Climbings;
using PrivateTraining.ServiceLayer.Interface;

namespace PrivateTraining.ServiceLayer.BLL
{

    public class InsertPayment
    {
        private readonly IPayment _payment;
        private IDbSet<PaymentOrderDetail> _PaymentOrderDetails;
        private readonly IUnitOfWork _uow;
        private readonly IIdentity _identity;
        private IDbSet<ProgramModrator> _programmodrators;
        private IDbSet<Program> _program;
        private readonly IApplicationUserManager _user;
        private IDbSet<SettingSms> _SettingSms;

        public InsertPayment(IUnitOfWork uow, IPayment payment, IIdentity identity, IApplicationUserManager user)
        {
            _uow = uow;
            _payment = payment;
            _PaymentOrderDetails = _uow.Set<PaymentOrderDetail>();
            _identity = identity;
            _programmodrators = _uow.Set<ProgramModrator>();
            _program = _uow.Set<Program>();
            _user = user;
            _SettingSms = _uow.Set<SettingSms>();
        }

        #region Insert Payment Climing

        /// <summary>
        /// ثبت اطلاعات  پرداخت قبل از رفتن به بانک 
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="FirstPrice"></param>
        /// <param name="LastPrice"></param>
        /// <param name="TransactionTypes"></param>
        /// <param name="PaymentTypes"></param>
        /// <param name="OrderId"></param>
        /// <param name="TransactionName"></param>
        /// <returns></returns>
        public async Task<int> FunInsertPayment(string[] UserId, int FirstPrice = 0, int LastPrice = 0, byte TransactionTypes = 0, byte PaymentTypes = 0,
           Nullable<int> OrderId = 0, byte TransactionName = 1)
        {
            try
            {
                //if (OrderId == 0)
                //  OrderId = null;

                var ActivePayments = Convert.ToByte(ActivePayment.Pending);
                if (TransactionTypes == Convert.ToByte(TransactionType.Payable))
                    ActivePayments = Convert.ToByte(ActivePayment.Ok);

                PersianDate Pd = new PersianDate();
                int Count = 1;

                if (UserId != null)
                    Count = UserId.Length + 1;

                string[] CounUser = new string[Count];
                if (UserId != null)
                {
                    for (int j = 0; j <= UserId.Length - 1; j++)
                    {
                        CounUser[j] = UserId[j].ToString();
                    }
                }
                CounUser[Count - 1] = _identity.GetUserId<int>().ToString();

                PaymentOrder Payment = new PaymentOrder();
                Payment.SaleOrderId = 0;
                Payment.SaleReferenceId = "";
                Payment.CodeBank = "";
                Payment.ReturnValueBank = "";
                Payment.AllPrice = LastPrice;
                Payment.TransactionType = TransactionTypes;
                Payment.PaymentType = PaymentTypes;
                Payment.SaveDatePayment = Pd.PersianDateLow();
                Payment.UserIdPayment = Convert.ToInt32(_identity.GetUserId<int>());
                Payment.DigitalReceipt = "";
                Payment.verified = Convert.ToInt32(Paymentverified.NotOnline);
                Payment.TransactionCode = "";
                Payment.OrderId = OrderId;
                Payment.TransactionName = TransactionName;

                await _payment.AddPayment(Payment);
                var t = _uow.SaveAllChanges();

                for (int i = 0; i <= CounUser.Length - 1; i++)
                {
                    //  PrivateTraining.DataLayer.Context.ApplicationDbContext context = new PrivateTraining.DataLayer.Context.ApplicationDbContext();
                    DataLayer.Context.ApplicationDbContext context = new DataLayer.Context.ApplicationDbContext();
                    var Query = "exec SP_InsertPaymentOrderDetail " + Payment.Id;

                    if (OrderId == 0)
                        Query += ",null,";
                    else
                        Query += "," + OrderId + ",";

                    Query += Convert.ToInt32(_identity.GetUserId<int>()) + "," + Convert.ToInt32(CounUser[i]) + ",'" + Pd.PersianDateLow() + "'," +
                        FirstPrice + ",0," + PaymentTypes + "," + TransactionTypes + "," + ActivePayments + ",'',''," + TransactionName;

                    context.Database.ExecuteSqlCommand(Query);
                }
                return Payment.Id;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        /// ثبت اطلاعات  پرداخت بعد از برگشت از بانک
        /// </summary>
        /// <param name="PaymentId"></param>
        /// <param name="SaleOrderId"></param>
        /// <param name="SaleReferenceId"></param>
        /// <param name="RefasId"></param>
        /// <param name="Order"></param>
        /// <param name="Type"></param>
        /// <returns></returns>
        public string UpdatePayment(int PaymentId = 0, string SaleOrderId = "0", string SaleReferenceId = "", string RefasId = "", string Order = "", string Type = "")
        {
            try
            {
                PersianDate Pd = new PersianDate();
                var Payment = _payment.GetAllPayment(PaymentId).FirstOrDefault();
                if (Payment != null)
                {
                    Payment.SaleOrderId = Convert.ToInt32(SaleOrderId);
                    Payment.SaleReferenceId = SaleReferenceId;
                    Payment.CodeBank = RefasId;
                    Payment.ReturnValueBank = Order;
                    Payment.TransactionCode = RefasId + "_" + Type;
                    var t = _uow.SaveAllChanges();

                    //----------------- شماره تماس سرپرست برای ارسال اس ام اس
                    string ModratorName = "";
                    var d = _programmodrators.Where(c => c.ProgramId == Payment.OrderId).FirstOrDefault();
                    if (d != null)
                        ModratorName = d.Modrators.Mobile;
                    //------------------ نظیمات ارسال پیامک
                    var setting = _SettingSms.FirstOrDefault();
                    //------------------

                    //--------------- برنامه برای همه کاربرها ک ثبت نام شدن فعال شود
                    var Query = _PaymentOrderDetails.Where(c => c.PaymentOrderId == PaymentId).ToList();
                    foreach (var item in Query)
                    {
                        item.ActivePayment = Convert.ToByte(ActivePayment.Ok);
                        item.SaveDateActivePayment = Pd.PersianDateLow();
                        _uow.SaveAllChanges();
                        //------------------------- ارسال اس ام اس برای کاربر ان
                        SendSmsForProgramUser(item, setting, ModratorName);
                    }

                    //---------------------   در صورت پر شدن ظرفیت برنامه برای سرپرست ها اس ام اس شود
                    SendSmsForModrator(setting, (int)Payment.OrderId);
                }
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// ارسال اس ام اس بعد از ثبت نام
        /// </summary>
        /// <param name="item"></param>
        /// <param name="ModratorName"></param>
        public void SendSmsForProgramUser(PaymentOrderDetail item, SettingSms setting, string ModratorName = "")
        {
            try
            {
                SendSms sms = new SendSms(_uow);
                //----------------- ارسال پیام ثبت نام برای کاربر
                if (item.RegisterUsers.Mobile != null && item.RegisterUsers.Mobile != "" && setting.SmsRegistrationTimeForUser)
                {
                    string text = item.RegisterUsers.Name + " " + item.RegisterUsers.Family + " عزیز ثبت نام شما در برنامه " + item.Programs.ProgramName + " قطعی شد. " +
                        " شماره تماس سرپرست: " + ModratorName +
                        " ساعت حرکت برنامه: " + item.Programs.FromTime +
                        " از " + item.Programs.Place +
                        "\n" + " با تشکر " + "http://koohgardy.com";
                    sms.SendSmsClass(item.RegisterUsers.Mobile, text, 0);
                }
                //----------------- ارسال پیام ثبت نام برای سرپرستان
                if (setting.SmsRegistrationTimeForModrator)
                {
                    var ModratorList = _programmodrators.Where(c => c.ProgramId == item.ProgramId).ToList();
                    foreach (var items in ModratorList)
                    {
                        var text = " سرپرست محترم برنامه ی " + item.Programs.ProgramName + " " + item.RegisterUsers.Name + " " + item.RegisterUsers.Family +
                                   " در برنامه ثبت نام نموده است. " +
                                   "\n" + " با تشکر " + "http://koohgardy.com";
                        if (items.Modrators.Mobile != null && items.Modrators.Mobile != "")
                            sms.SendSmsClass(items.Modrators.Mobile, text, 0);
                    }
                }
                //------------------ ارسال پیام ثبت نام برای مدیران سایت
                if (setting.SmsRegistrationTimeForAdmin)
                {
                    var Admins = _user.GetAllAdmin().ToList();
                    foreach (var item3 in Admins)
                    {
                        var text = " مدیر محترم سایت کوهگردی ، " + item.RegisterUsers.Name + " " + item.RegisterUsers.Family + " در برنامه" + item.Programs.ProgramName + " ثبت نام نموده است. ";
                        if (item3.Mobile != null && item3.Mobile != "")
                            sms.SendSmsClass(item3.Mobile, text, 0);
                    }
                }
                //-------------------

            }
            catch (Exception ex) { }
        }

        /// <summary>
        /// بدست آوردن ظرفیت مانده برنامه
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int CountRegisterinProgram(int Id)
        {
            byte Re = Convert.ToByte(TransactionType.Received);
            byte Pa = Convert.ToByte(TransactionType.Payable);
            var a = Convert.ToByte(ActivePayment.No);
            var q = _payment.GetAllPaymentDetail(0).Where(c => c.ProgramId == Id && c.ActivePayment != a && c.ActivePaymentOfUser == false &&
            c.IsEnable == true && (c.TransactionType == Re || c.TransactionType == Pa)).Count();
            return q;
        }

        /// <summary>
        /// در صورت پر شدن ظرفیت برنامه برای  سرپرست ها اس ام اس شود
        /// </summary>
        /// <param name="ProgramId"></param>
        public void SendSmsForModrator(SettingSms setting, int ProgramId = 0)
        {
            try
            {
                SendSms sms = new SendSms(_uow);
                var Count = CountRegisterinProgram(ProgramId);
                var query = _program.Where(c => c.Id == ProgramId).FirstOrDefault();
                if (query != null)
                {
                    if (Count >= query.MaxUser)
                    {
                        //----------------- ارسال پیام برای سرپرستان
                        if (setting.SmsCapacityFillingTimeForModrator)
                        {
                            var ModratorList = _programmodrators.Where(c => c.ProgramId == ProgramId).ToList();
                            foreach (var item in ModratorList)
                            {
                                //var text = item.Modrators.Name + " " + item.Modrators.Family + " عزیز ظرفیت برنامه " + query.ProgramName + " پر شده است. "+
                                var text = " سرپرست محترم برنامه ی " + query.ProgramName + " ظرفیت ثبت نام آنلاین برنامه تکمیل شده است. " +
                                           "\n" + " با تشکر " + "http://koohgardy.com";
                                if (item.Modrators.Mobile != null && item.Modrators.Mobile != "")
                                    sms.SendSmsClass(item.Modrators.Mobile, text, 0);
                            }
                        }
                        //------------------ ارسال پیام برای مدیران سایت
                        if (setting.SmsCapacityFillingTimeForAdmin)
                        {
                            var Admins = _user.GetAllAdmin().ToList();
                            foreach (var item in Admins)
                            {
                                var text = " مدیر محترم سایت کوهگردی ، ظرفیت ثبت نام آنلاین برنامه ی" + query.ProgramName + " تکمیل شده است. ";
                                if (item.Mobile != null && item.Mobile != "")
                                    sms.SendSmsClass(item.Mobile, text, 0);
                            }
                        }
                        //-------------------
                    }
                }
            }
            catch (Exception ex) { }
        }

        /// <summary>
        ///  بدست آوردن مبلغ شارژ پنل کاربر
        /// </summary>
        /// <returns></returns>
        public string GetSharjPanel()
        {
            var a = Convert.ToByte(PaymentType.Panel);
            var p = Convert.ToByte(TransactionName.Panel);
            var y = Convert.ToByte(TransactionName.Buy);
            var r = Convert.ToByte(TransactionType.Received);
            var t = Convert.ToByte(TransactionType.Payable);
            var b = Convert.ToByte(TransactionType.Back);
            var v = Convert.ToByte(ActivePayment.Ok);
            var n = Convert.ToByte(TransactionType.Account);

            //  var UserId = Convert.ToInt32(User.Identity.GetUserId());
            var UserId = _identity.GetUserId<int>();

            var Query1 = _payment.GetAllPaymentDetail(0).Where(c => c.TransactionName == p && c.RegisterUserId == UserId && c.TransactionType == r &&
            c.IsEnable == true && c.ActivePayment == v).ToList();
            var Query2 = _payment.GetAllPaymentDetail(0).Where(c => c.TransactionName == y && c.RegisterUserId == UserId && c.TransactionType == t &&
            c.IsEnable == true && c.ActivePayment == v).ToList();
            var Query3 = _payment.GetAllPaymentDetail(0).Where(c => c.PaymentType == a && c.RegisterUserId == UserId && c.TransactionType == b &&
            c.IsEnable == true && c.ActivePayment == v).ToList();
            var Query4 = _payment.GetAllPaymentDetail(0).Where(c => c.PaymentType == 0 && c.RegisterUserId == UserId && c.TransactionType == n &&
            c.IsEnable == true).ToList();

            decimal Received = 0;
            decimal Payable = 0;
            decimal Back = 0;
            decimal Account = 0;

            if (Query1.Count > 0)
                Received = Query1.Sum(c => c.price) - Query1.Sum(c => c.Fine);
            if (Query2.Count > 0)
                Payable = Query2.Sum(c => c.price) - Query2.Sum(c => c.Fine);
            if (Query3.Count > 0)
                Back = Query3.Sum(c => c.price) - Query3.Sum(c => c.Fine);
            if (Query4.Count > 0)
                Account = Query4.Sum(c => c.price) - Query4.Sum(c => c.Fine);

            // ViewBag.SharjPanel = (Received).ToString();

            //       دریافتی پنل + برگشتی به پنل  - برداشت از پنل - برگشت به کارت
            return ((Received + Back) - (Payable + Account)).ToString();
        }

        #endregion

        #region insert payment golbahar

        public virtual async Task<int> FunInsertPayGolbahar(int UserId, int Price = 0, byte PaymentTypes = 0,string ReceiptNumber="",string ReceiptDate="")
        {

            var Query = _payment.GetAllPayment(0).Where(c => c.CodeBank == ReceiptNumber).ToList();
            if ( ( Query.Count() <= 0 && PaymentTypes== Convert.ToByte(PaymentTypeGolbahar.Receipt) ) ||
                PaymentTypes == Convert.ToByte(PaymentTypeGolbahar.Online))
            {
                PersianDate Pd = new PersianDate();
                PaymentOrder Payment = new PaymentOrder();
                Payment.SaleOrderId = 0;
                Payment.SaleReferenceId = "";
                Payment.CodeBank = ReceiptNumber;
                Payment.ReturnValueBank = "";
                Payment.AllPrice = Price;
                Payment.TransactionType = 0;
                Payment.PaymentType = PaymentTypes;
                Payment.SaveDatePayment = Pd.PersianDateLow();
                Payment.UserIdPayment = Convert.ToInt32(_identity.GetUserId<int>());
                Payment.DigitalReceipt =Pd.ConvertFaToEnNumber( ReceiptDate);
                Payment.verified = Convert.ToInt32(Paymentverified.NotOnline);
                Payment.TransactionCode = "";
                Payment.OrderId = UserId;
                Payment.TransactionName = 0;

                await _payment.AddPayment(Payment);
                var t = _uow.SaveAllChanges();
                return Payment.Id;
            }

            else
                return 0;
        }

        #endregion



    }
}
