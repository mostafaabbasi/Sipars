using PrivateTraining.DataLayer.Context;
using PrivateTraining.DomainClasses.Entities.FrameWork;
using PrivateTraining.ServiceLayer.BLL;
using PrivateTraining.ServiceLayer.Interface.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using PrivateTraining.Shaparak;
using PrivateTraining.ServiceLayer.Interface;
using PrivateTraining.ServiceLayer.Extention;
using System.Security.Principal;
using PrivateTraining.DomainClasses.Entities.Payment;
using PrivateTraining.DomainClasses.Entities.PrivateTraining;

namespace PrivateTraining.Areas.Framework.Controllers
{
    public partial class PaymentController : Controller
    {
        public string PgwSite = ConfigurationManager.AppSettings["PgwSite"];
        public string CallBackUrl = ConfigurationManager.AppSettings["CallBackUrl"];
        public string TerminalId = ConfigurationManager.AppSettings["TerminalId"];
        public string UserName = ConfigurationManager.AppSettings["UserName"];
        public string UserPassword = ConfigurationManager.AppSettings["UserPassword"];

        private readonly IUnitOfWork _uow;
        private readonly IPayment _payment;
        private IDbSet<Setting> _setting;
        private readonly IApplicationUserManager _user;
        private IDbSet<payment> _PaymentP;
        private IDbSet<BankPayment> _BankPayment;
        private IDbSet<Payment> _Payment;

        public PaymentController(IUnitOfWork uow, IPayment payment, IApplicationUserManager user)
        {
            _uow = uow;
            _setting = _uow.Set<Setting>();
            _payment = payment;
            _user = user;
            _PaymentP = _uow.Set<payment>();
            _BankPayment = _uow.Set<BankPayment>();
            _Payment = _uow.Set<Payment>();
        }

        ///[AllowAnonymous]
        public virtual ActionResult ShowPageOfOnlinePayment(int OrderId = 0, int TypeOrder = 0, string OrderPrice = "0",
            byte CountOfOrder = 0)
        {
            ViewBag.Banks = _setting.Where(c => c.Subject == "OnlinePayment").Where(c => c.IsEnable == true).ToList();
            ViewBag.Price = OrderPrice + "  تومان ";
            ViewData["Price"] = OrderPrice;
            ViewData["OrderId"] = OrderId;
            Session["ViewBagPrice"] = OrderPrice;
            Session["OrderId"] = OrderId;
            Session["TypeOrder"] = TypeOrder;
            Session["CountOfOrder"] = CountOfOrder;

            return View();
        }

        [HttpPost]
        //[AllowAnonymous]
        public virtual async Task<JsonResult> ListBanks()
        {
            try
            {
                var model = _setting.Where(c => c.Subject == "OnlinePayment").Where(c => c.IsEnable);
                return Json(new {Result = true, Message = model});
            }
            catch (Exception ex)
            {
                return Json(new {Result = false, Message = ""});
            }
        }

        //--------------------------------------------------------------------------- Mellat

        #region Mellat

        public string RefasId = "";
        public string Order = "";
        public string SaleOrderId = "";
        public string SaleReferenceId = "";
        public int Inquiry = 0;

        /// <summary>
        ///  خرید آنلاین - ملت
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public virtual async Task<JsonResult> LoadPageMellat(string[] DebtId, double Price = 0, int OrderId = 0,
            byte TransactionType = 1, int CurrentUserId = 0)
        {
            try
            {
                //  Session["ViewBagPrice"] = Price;
                //  Session["FirstPrice"] = FirstPrice;

                Session["Price"] = Price;
                Session["OrderId"] = OrderId;
                Session["TransactionType"] = TransactionType;
                if (CurrentUserId != 0)
                    Session["UserId"] = CurrentUserId;
                else
                    Session["UserId"] = Convert.ToInt32(User.Identity.GetUserId());

                var Us = "";
                for (int i = 0; i <= DebtId.Length - 1; i++)
                {
                    Us = DebtId[i] + ",";
                }

                //Session["RegisterUserId"] = Us.Substring(0, Us.Length - 1);
                Session["DebtId"] = Us.Substring(0, Us.Length - 1);

                PrivateTraining.ServiceLayer.BLL.PrivateTraining PT =
                    new PrivateTraining.ServiceLayer.BLL.PrivateTraining(_uow, _user);
                Price = PT.ReturnPricePayment(DebtId);

                return Json(new
                {
                    Result = true,
                    redirectUrl = Url.Action("CallBackUrlMellats", "Payment",
                        new {Area = "Framework", Price = Price, OrderId = OrderId}),
                });
            }
            catch (Exception ex)
            {
                return Json(new {Result = false, Message = ""});
            }
        }

        [AllowAnonymous]
        public virtual ActionResult CallBackUrlMellats(int Price, int OrderId)
        {
            PgwSite = "https://bpm.shaparak.ir/pgwchannel/startpay.mellat";
            string DateY = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString();
            string TimeD = DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() +
                           DateTime.Now.Second.ToString();
            CallBackUrl = Request.Url.Host + "/Framework/Payment/CallBackUrlMellat";

            Shaparak.IPaymentGateway FnPayment = new Shaparak.PaymentGatewayClient();
            bpPayRequestBody bpPey = new bpPayRequestBody();
            bpPey.amount = Convert.ToInt32(Price);
            bpPey.callBackUrl = CallBackUrl;
            bpPey.localDate = DateY;
            bpPey.localTime = TimeD;
            bpPey.orderId = OrderId;
            bpPey.payerId = 10;
            bpPey.terminalId = Convert.ToInt32(TerminalId);
            bpPey.userName = UserName;
            bpPey.userPassword = UserPassword;
            bpPey.additionalData = "توضیحات";

            bpPayRequest bpPey2 = new bpPayRequest(bpPey);
            bpPayRequestResponse bpRe = FnPayment.bpPayRequest(bpPey2);

            string result = bpRe.Body.@return;
            String[] resultArray = result.Split(',');
            string refIDValue = "";
            if (resultArray[0] == "0")
                refIDValue = resultArray[1];

            //   return Json(refIDValue, JsonRequestBehavior.AllowGet);

            ViewBag.RefID = "RefId";
            ViewBag.Value = refIDValue;
            ViewBag.PgwSite = PgwSite;
            ViewBag.MessageOfPayment = MessageOfPayment(result);
            return View();
        }

        public string MessageOfPayment(string result)
        {
            string Message = null;
            switch (Convert.ToInt32(result))
            {
                case 11:
                    Message = "شماره كارت نامعتبر است";
                    break;
                case 12:
                    Message = "موجودي كافي نيست";
                    break;
                case 13:
                    Message = "رمز نادرست است";
                    break;
                case 14:
                    Message = "تعداد دفعات وارد كردن رمز بيش از حد مجاز است";
                    break;
                case 15:
                    Message = "كارت نامعتبر است";
                    break;
                case 16:
                    Message = "دفعات برداشت وجه بيش از حد مجاز است";
                    break;
                case 17:
                    Message = "كاربر از انجام تراكنش منصرف شده است";
                    break;
                case 18:
                    Message = "تاريخ انقضاي كارت گذشته است";
                    break;
                case 19:
                    Message = "مبلغ برداشت وجه بيش از حد مجاز است";
                    break;
                case 111:
                    Message = "صادر كننده كارت نامعتبر است";
                    break;
                case 112:
                    Message = "خطاي سوييچ صادر كننده كارت";
                    break;
                case 113:
                    Message = "پاسخي از صادر كننده كارت دريافت نشد";
                    break;
                case 114:
                    Message = "دارنده كارت مجاز به انجام اين تراكنش نيست";
                    break;
                case 21:
                    Message = "پذيرنده نامعتبر است";
                    break;
                case 23:
                    Message = "خطاي امنيتي رخ داده است";
                    break;
                case 24:
                    Message = "اطلاعات كاربري پذيرنده نامعتبر است";
                    break;
                case 25:
                    Message = "مبلغ نامعتبر است";
                    break;
                case 31:
                    Message = "پاسخ نامعتبر است";
                    break;
                case 32:
                    Message = "فرمت اطلاعات وارد شده صحيح نمي باشد";
                    break;
                case 33:
                    Message = "حساب نامعتبر است";
                    break;
                case 34:
                    Message = "خطاي سيستمي";
                    break;
                case 35:
                    Message = "تاريخ نامعتبر است";
                    break;
                case 41:
                    Message = "شماره درخواست تكراري است";
                    break;
                case 42:
                    Message = "يافت نشد Sale تراكنش";
                    break;
                case 43:
                    Message = "داده شده است Verify قبلا درخواست";
                    break;
                case 44:
                    Message = "يافت نشد  Verfiy درخواست";
                    break;
                case 45:
                    Message = "شده است  Settle تراكنش";
                    break;
                case 46:
                    Message = "نشده است  Settle تراكنش";
                    break;
                case 47:
                    Message = "يافت نشد  Settle تراكنش";
                    break;
                case 48:
                    Message = "شده است  Reverse تراكنش";
                    break;
                case 49:
                    Message = "يافت نشد Refund تراكنش";
                    break;
                case 412:
                    Message = "شناسه قبض نادرست است";
                    break;
                case 413:
                    Message = "شناسه پرداخت نادرست است";
                    break;
                case 414:
                    Message = "سازمان صادر كننده قبض نامعتبر است";
                    break;
                case 415:
                    Message = "زمان جلسه كاري به پايان رسيده است";
                    break;
                case 416:
                    Message = "خطا در ثبت اطلاعات";
                    break;
                case 417:
                    Message = "شناسه پرداخت كننده نامعتبر است";
                    break;
                case 418:
                    Message = "اشكال در تعريف اطلاعات مشتري";
                    break;
                case 419:
                    Message = "تعداد دفعات ورود اطلاعات از حد مجاز گذشته است";
                    break;
                case 421:
                    Message = "نامعتبر است IP";
                    break;
                case 51:
                    Message = "تراكنش تكراري است";
                    break;
                case 54:
                    Message = "تراكنش مرجع موجود نيست";
                    break;
                case 55:
                    Message = "تراكنش نامعتبر است";
                    break;
                case 61:
                    Message = "خطا در واريز";
                    break;
                default:
                    Message = "خطا مشخص نمی باشد";

                    break;
            }

            return Message;
        }

        public virtual ActionResult CallBackUrlMellat()
        {
            RefasId = Request.Params["RefId"];
            Order = Request.Params["ResCode"];
            SaleOrderId = Request.Params["SaleOrderId"];
            SaleReferenceId = Request.Params["SaleReferenceId"];

            if (Order == "0")
            {
                int VeryFy = Convert.ToInt32(VerifyRequest());
                if (VeryFy == 0)
                {
                    ViewBag.Message = "کاربر گرامی پرداخت شما با موفقیت انجام شده است";
                    ViewBag.RefIdLabel = SaleReferenceId;
                    ViewBag.SaleOrderIdLabel = SaleOrderId;
                    ViewBag.lblCode = ": کد رهگیری  ";
                    ViewBag.lblOrder = ": شماره سفارش  ";
                    ViewBag.Label2 =
                        " کاربر گرامی با تشکر از خرید شما اطلاعات زیر را یادداشت فرمایید تا در صورت بروز مشکل از آن استفاده نمایید  ";

                    SetPayment();
                    _uow.SaveAllChanges();
                    SettleRequest();
                }
                else
                {
                    //if (Convert.ToByte(Session["TransactionType"]) == Convert.ToByte(TransactionType.Buy))
                    //{

                    //}

                    Inquiry = Convert.ToInt32(InquiryRequest());
                    if (Inquiry == 0)
                    {
                        VerifyRequest();
                        ViewBag.Message = "کاربر گرامی پرداخت شما با موفقیت انجام شده است";
                        ViewBag.lblCode = ": کد رهگیری  ";
                        ViewBag.lblOrder = ": شماره سفارش  ";
                        ViewBag.RefIdLabel = SaleReferenceId;
                        ViewBag.SaleOrderIdLabel = SaleOrderId;
                        ViewBag.Label2 =
                            " کاربر گرامی با تشکر از خرید شما اطلاعات زیر را یادداشت فرمایید تا در صورت بروز مشکل از آن استفاده نمایید  ";

                        SetPayment();
                        _uow.SaveAllChanges();
                        SettleRequest();
                    }
                    else
                    {
                        ViewBag.Message = MessageOfPayment(Inquiry.ToString());
                        ReversalRequest();
                    }
                }
            }
            else
            {
                ViewBag.Message = MessageOfPayment(Order);
            }

            //return View("test");
            return View();
        }

        public async void SetPayment()
        {
            PersianDate Pd = new PersianDate();
            var DebtIds = Convert.ToString(Session["DebtId"]);
            int Price = Convert.ToInt32(Session["Price"].ToString().Replace(",", "").Replace("،", ""));

            int Count = 1;
            if (DebtIds != null)
                Count = DebtIds.Length + 1;

            string[] CounUser = new string[Count];
            if (DebtIds != null)
            {
                for (int j = 0; j <= DebtIds.Length - 1; j++)
                {
                    CounUser[j] = DebtIds[j].ToString();
                }
            }
            //      CounUser[Count - 1] = User.Identity.GetUserId().ToString();

            //PaymentOrder Payment = new PaymentOrder();
            //Payment.SaleOrderId = Convert.ToInt32(SaleOrderId);
            //Payment.SaleReferenceId = SaleReferenceId;
            //Payment.CodeBank = RefasId;
            //Payment.ReturnValueBank = Order;
            //Payment.AllPrice = Convert.ToInt32(Session["ViewBagPrice"].ToString().Replace(",", "").Replace("،", ""));
            //Payment.TransactionType = Convert.ToByte(Session["TransactionType"]);
            //Payment.PaymentType = Convert.ToByte(PaymentType.Online);
            //Payment.SaveDatePayment = Pd.PersianDateLow();
            //Payment.UserIdPayment = Convert.ToInt32(User.Identity.GetUserId());
            //Payment.DigitalReceipt = "";
            //Payment.verified = Convert.ToInt32(Paymentverified.NotOnline);
            //Payment.TransactionCode = "";
            ////  Payment.DesOfReject = "";
            ////Payment.Price = Price;
            ////Payment.RegisterUserId = Convert.ToInt32(CounUser[i]);
            ////Payment.ActivePayment = Convert.ToByte(ActivePayment.Ok);
            ////Payment.SaveDateActivePayment = Pd.PersianDateLow();
            //await _payment.AddPayment(Payment);
            //var t = _uow.SaveAllChanges();

            //for (int i = 0; i <= CounUser.Length - 1; i++)
            //{
            //    //PrivateTraining.DataLayer.Context.ApplicationDbContext context = new PrivateTraining.DataLayer.Context.ApplicationDbContext();
            //    //context.Database.ExecuteSqlCommand("exec SP_InsertPaymentOrderDetail " + Payment.Id + "," + Convert.ToInt32(Session["OrderId"]) +
            //    //    "," + Convert.ToInt32(User.Identity.GetUserId()) + "," + Convert.ToInt32(CounUser[i]) + ",'" + Pd.PersianDateLow() + "'," +
            //    //    Price + ",0," + Convert.ToByte(PaymentType.idPay) + "," +
            //    //    Convert.ToByte(Session["TransactionType"]) + "," + Convert.ToByte(ActivePayment.Ok) + ",'',''");
            //}

            var TransactionNumber = 0;
            var StatusPayment = 0;
            DataLayer.Context.ApplicationDbContext context = new DataLayer.Context.ApplicationDbContext();
            string Query = "exec PrivateTraining.SP_CalculationPayment '" + Convert.ToInt32(Session["UserId"]) + "','" +
                           Convert.ToInt32(Session["UserId"]) + "','" + TransactionNumber + "','" + StatusPayment +
                           "','" + DebtIds + "'";
            var List = context.Database.ExecuteSqlCommand(Query);
        }


        //متد تاييد تراكنش خريد
        protected object VerifyRequest()
        {
            try
            {
                BypassCertificateError();

                Shaparak.IPaymentGateway FnPayment = new Shaparak.PaymentGatewayClient();
                bpVerifyRequestBody bpPey = new bpVerifyRequestBody();
                bpPey.orderId = Int64.Parse(SaleOrderId);
                bpPey.terminalId = Convert.ToInt32(TerminalId);
                bpPey.userName = UserName;
                bpPey.userPassword = UserPassword;
                bpPey.saleReferenceId = Int64.Parse(SaleReferenceId);
                bpPey.saleOrderId = Int64.Parse(SaleOrderId);

                bpVerifyRequest bpPey2 = new bpVerifyRequest(bpPey);
                bpVerifyRequestResponse bpRe = FnPayment.bpVerifyRequest(bpPey2);

                return bpRe.Body.@return;
            }
            catch (Exception exp)
            {
                ViewBag.VerifyOutputLabel = "Error: " + exp.Message;
                return "Error: " + exp.Message;
            }
        }

        //متد درخواست استعلام وجه
        protected object InquiryRequest()
        {
            try
            {
                BypassCertificateError();

                Shaparak.IPaymentGateway FnPayment = new Shaparak.PaymentGatewayClient();
                bpInquiryRequestBody bpPey = new bpInquiryRequestBody();
                bpPey.orderId = Int64.Parse(SaleOrderId);
                bpPey.terminalId = Convert.ToInt32(TerminalId);
                bpPey.userName = UserName;
                bpPey.userPassword = UserPassword;
                bpPey.saleReferenceId = Int64.Parse(SaleReferenceId);
                bpPey.saleOrderId = Int64.Parse(SaleOrderId);

                bpInquiryRequest bpPey2 = new bpInquiryRequest(bpPey);
                bpInquiryRequestResponse bpRe = FnPayment.bpInquiryRequest(bpPey2);

                return bpRe.Body.@return;
            }
            catch (Exception exp)
            {
                ViewBag.InquiryOutputLabel = "Error: " + exp.Message;
                return "Error: " + exp.Message;
            }
        }

        //متد درخواست برگشت وجه
        protected object ReversalRequest()
        {
            try
            {
                BypassCertificateError();

                Shaparak.IPaymentGateway FnPayment = new Shaparak.PaymentGatewayClient();
                bpReversalRequestBody bpPey = new bpReversalRequestBody();
                bpPey.orderId = Int64.Parse(SaleOrderId);
                bpPey.terminalId = Convert.ToInt32(TerminalId);
                bpPey.userName = UserName;
                bpPey.userPassword = UserPassword;
                bpPey.saleReferenceId = Int64.Parse(SaleReferenceId);
                bpPey.saleOrderId = Int64.Parse(SaleOrderId);

                bpReversalRequest bpPey2 = new bpReversalRequest(bpPey);
                bpReversalRequestResponse bpRe = FnPayment.bpReversalRequest(bpPey2);

                return bpRe.Body.@return;
            }
            catch (Exception exp)
            {
                ViewBag.ReversalOutputLabel = "Error: " + exp.Message;
                return "Error: " + exp.Message;
            }
        }

        // متد درخواست واريز وجه
        protected object SettleRequest()
        {
            try
            {
                BypassCertificateError();

                Shaparak.IPaymentGateway FnPayment = new Shaparak.PaymentGatewayClient();
                bpSettleRequestBody bpPey = new bpSettleRequestBody();
                bpPey.orderId = Int64.Parse(SaleOrderId);
                bpPey.terminalId = Convert.ToInt32(TerminalId);
                bpPey.userName = UserName;
                bpPey.userPassword = UserPassword;
                bpPey.saleReferenceId = Int64.Parse(SaleReferenceId);
                bpPey.saleOrderId = Int64.Parse(SaleOrderId);

                bpSettleRequest bpPey2 = new bpSettleRequest(bpPey);
                bpSettleRequestResponse bpRe = FnPayment.bpSettleRequest(bpPey2);

                return bpRe.Body.@return;
            }
            catch (Exception exp)
            {
                ViewBag.SettleOutputLabel = "Error: " + exp.Message;
                return "Error: " + exp.Message;
            }
        }

        public void BypassCertificateError()
        {
            ServicePointManager.ServerCertificateValidationCallback =
                new System.Net.Security.RemoteCertificateValidationCallback(BypassCertValidation);
        }

        private static bool BypassCertValidation(object sender, X509Certificate cert, X509Chain chain,
            SslPolicyErrors errors)
        {
            return true;
        }

        #endregion

        //--------------------------------------------------------------------------- ZarinPal

        #region ZarinPal

        [HttpPost]
        //[AllowAnonymous]
        public virtual async Task<JsonResult> LoadPageZarinPal(string[] DebtId, int Price = 0, byte Type = 1,
            int CurrentUserId = 0)
        {
            try
            {
                if (CurrentUserId == 0)
                    CurrentUserId = Convert.ToInt32(User.Identity.GetUserId());

                PrivateTraining.ServiceLayer.BLL.PrivateTraining In =
                    new PrivateTraining.ServiceLayer.BLL.PrivateTraining(_uow, _user);
                var PaymentId = In.FunInsertPaymentPrivateTraning(DebtId, Price, CurrentUserId);
                var S = PaymentId.Result;
                //Session["PaymentId"] = PaymentId.Result;
                //Session["PriceZ"] = Price;
                Session["PaymentId"] = S[0];
                Session["PriceZ"] = S[1];
                Session["TypeZ"] = Type;

                return Json(new
                {
                    Result = true,
                    redirectUrl = Url.Action("SendZarinPal", "Payment", new
                    {
                        Area = "Framework",
                        Price = Convert.ToInt32(S[1]),
                        PaymentIds = Convert.ToInt32(S[0]),
                        CurrentUserId = CurrentUserId
                    }),
                });
            }
            catch (Exception ex)
            {
                return Json(new {Result = false, Message = ""});
            }
        }

        //[AllowAnonymous]
        public virtual ActionResult SendZarinPal(int Price, int PaymentIds, int CurrentUserId = 0)
        {
            ViewBag.PmSendZarinPal = "";
            try
            {
                ZarinPal.PaymentGatewayImplementationServicePortTypeClient zp =
                    new ZarinPal.PaymentGatewayImplementationServicePortTypeClient();
                string Authority;
                string Email = "", Mobile = "";
                var Query = _user.GetAllUsersWithId(CurrentUserId).FirstOrDefault();

                if (Query != null)
                {
                    Email = Query.Email;
                    Mobile = Query.Mobile;
                }

                string CallBackUrls = "http://" + Request.Url.Host + "/Framework/Payment/CallBackUrlZarinPal";
                int Status = zp.PaymentRequest("9549f856-a1d3-11e7-94c7-000c295eb8fc", Price, PaymentIds.ToString(),
                    Email, Mobile, CallBackUrls, out Authority);
                Response.Redirect("https://www.zarinpal.com/pg/StartPay/" + Authority);

                //  ViewBag.PmSendZarinPal = Price.ToString()+ "___"+ PaymentIds.ToString() + "___" + Email + "___" + Mobile;
            }
            catch (Exception ex)
            {
                ViewBag.PmSendZarinPal = ex.Message;
            }

            return View();
        }

        public virtual ActionResult CallBackUrlZarinPalCustomer()
        {
            ViewBag.PmerrorOfZarinPal = "";
            ViewBag.PmOfZarinPal = "";
            ViewBag.OKZarinPal = 0;

            try
            {
                if (Request.QueryString["Status"] != "" && Request.QueryString["Status"] != null &&
                    Request.QueryString["Authority"] != "" && Request.QueryString["Authority"] != null)
                {
                    if (Request.QueryString["Status"].ToString().Equals("OK"))
                    {
                        //Session["PriceZ"] = 100;
                        //Session["PaymentId"] = 1017;
                        //Session["TypeZ"] = 1;
                        //Session["pu"] = 1;

                        var Authority = Request.QueryString["Authority"];
                        var bankPayment = _BankPayment.First(bank =>
                            bank.bankCode == 1 && bank.detailJson.IndexOf(Authority, StringComparison.Ordinal) != -1);

                        long RefID;
//                        System.Net.ServicePointManager.Expect100Continue = false;
                        ZarinPal.PaymentGatewayImplementationServicePortTypeClient zp =
                            new ZarinPal.PaymentGatewayImplementationServicePortTypeClient();

                        int Status = zp.PaymentVerification("9549f856-a1d3-11e7-94c7-000c295eb8fc", Authority,
                            bankPayment.price * 10, out RefID);

                        // Status = 100;
                        if (Status == 100)
                        {
                            ViewBag.OKZarinPal = 1;

                            ViewBag.PmOfZarinPal = " عملیات با موفقیت انجام شد!!  " + "کد تراکنش :" + RefID;
                            try
                            {
                                var user = _user.FindById(bankPayment.userId);
                                if (user == null)
                                {
                                    ViewBag.PmerrorOfZarinPal +=
                                        "در ثبت اطلاعات خطایی رخ داد. برای پیگیری بیشتر با پشتیبانی تماس بگیرید. کد خطا: ۱۰۰";
                                }
                                else
                                {
                                    var PD = new PersianDate();
                                    var dateNow = PD.PersianDateLow();
                                    var timeNow = PD.CurrentTime();
                                    bankPayment.okDate = dateNow;
                                    bankPayment.okTime = timeNow;
                                    bankPayment.status = 3; //ok
                                    bankPayment.transactionCode = RefID + ""; //ok

                                    var rowAffected = _uow.SaveAllChanges();
                                    if (rowAffected != 1)
                                    {
                                        //error
                                        ViewBag.PmerrorOfZarinPal +=
                                            "در ثبت اطلاعات خطایی رخ داد. برای پیگیری بیشتر با پشتیبانی تماس بگیرید. کد خطا: ۱۰۱";
                                    }
                                    else
                                    {
                                        var payment = new Payment
                                        {
                                            userId = bankPayment.userId,
                                            price = bankPayment.price,
                                            refId = bankPayment.id,
                                            refType = PaymentTypeEnum.bank,
                                            date = dateNow,
                                            time = timeNow,
                                            status = 1,
                                        };

                                        _Payment.Add(payment);

                                        rowAffected = _uow.SaveAllChanges();
                                        if (rowAffected != 1)
                                        {
                                            //error
                                            ViewBag.PmerrorOfZarinPal +=
                                                "در ثبت اطلاعات خطایی رخ داد. برای پیگیری بیشتر با پشتیبانی تماس بگیرید. کد خطا: ۱۰۲";
                                        }
                                        else
                                        {
                                            user.Credit += bankPayment.price;

                                            rowAffected = _uow.SaveAllChanges();
                                            if (rowAffected != 1)
                                            {
                                                //error
                                                ViewBag.PmerrorOfZarinPal +=
                                                    "در ثبت اطلاعات خطایی رخ داد. برای پیگیری بیشتر با پشتیبانی تماس بگیرید. کد خطا: ۱۰۳";
                                            }
                                            else
                                            {
                                                try
                                                {
                                                    if (!string.IsNullOrWhiteSpace(user.Mobile))
                                                    {
                                                        SendSms Sms = new SendSms(_uow);
                                                        var text = "تراکنش شما در سامانه سیپارس با موفقیت انجام شد. ";
                                                        text += "کد تراکنش :" + RefID;
                                                        text += "\n";
                                                        text += "اعتبار شما: " + user.Credit + " تومان";
                                                        Sms.SendSmsClass(user.Mobile, text);
                                                    }

                                                    //  ViewBag.PmOfZarinPal += "______________________" + " 44 ";
                                                }
                                                catch (Exception ex)
                                                {
                                                    //ViewBag.PmOfZarinPal += "______________________" + " 88 ";
                                                }
                                            }
                                        }
                                    }


                                    //    ------------------------------------------
                                }
                            }
                            catch (Exception ex)
                            {
                                ViewBag.PmerrorOfZarinPal += ex.Message;
                            }

                        }
                        else
                        {
                            ViewBag.PmOfZarinPal =
                                "  عملیات با شکست مواجه شد!!" + "وضعیت :" + MessageOfZarinPal(Status);
                            
                            bankPayment.status = 2; //ok
                            bankPayment.transactionCode = RefID + " " + Status; //ok

                            var rowAffected = _uow.SaveAllChanges();
                           
                        }
                    }
                    else
                    {
                        ViewBag.PmOfZarinPal = "عملیات با شکست مواجه شد! Authority: " +
                                               Request.QueryString["Authority"].ToString() + " Status: " +
                                               Request.QueryString["Status"].ToString();
                    }
                }
                else
                {
                    ViewBag.PmOfZarinPal = "عملیات با شکست مواجه شد";
                }
            }
            catch (Exception ex)
            {
                ViewBag.PmOfZarinPal = ex.Message;
            }

            return View();
        }

        //[AllowAnonymous]
        public virtual ActionResult CallBackUrlZarinPal()
        {
            ViewBag.PmerrorOfZarinPal = "";
            ViewBag.PmOfZarinPal = "";
            ViewBag.OKZarinPal = 1;

            try
            {
                if (Request.QueryString["Status"] != "" && Request.QueryString["Status"] != null &&
                    Request.QueryString["Authority"] != "" && Request.QueryString["Authority"] != null)
                {
                    if (Request.QueryString["Status"].ToString().Equals("OK"))
                    {
                        //Session["PriceZ"] = 100;
                        //Session["PaymentId"] = 1017;
                        //Session["TypeZ"] = 1;
                        //Session["pu"] = 1;

                        int Amount = Convert.ToInt32(Session["PriceZ"]);
                        long RefID;
                        System.Net.ServicePointManager.Expect100Continue = false;
                        ZarinPal.PaymentGatewayImplementationServicePortTypeClient zp =
                            new ZarinPal.PaymentGatewayImplementationServicePortTypeClient();

                        int Status = zp.PaymentVerification("9549f856-a1d3-11e7-94c7-000c295eb8fc",
                            Request.QueryString["Authority"].ToString(), Amount, out RefID);
                        // Status = 100;
                        if (Status == 100)
                        {
                            ViewBag.OKZarinPal = Session["TypeZ"].ToString();

                            ViewBag.PmOfZarinPal = " عملیات با موفقیت انجام شد!!  " + "کد تراکنش :" + RefID;
                            try
                            {
                                PrivateTraining.ServiceLayer.BLL.PrivateTraining In =
                                    new PrivateTraining.ServiceLayer.BLL.PrivateTraining(_uow, _user);
                                ViewBag.PmerrorOfZarinPal += In.UpdatePaymentPrivateTraning(
                                    Convert.ToInt32(Session["PaymentId"]), Convert.ToString(RefID), "ZarinPal");

                                if (ViewBag.OKZarinPal == "1")
                                {
                                    //  -----------------------------ارسال ایمیل و پیامک ثبت نام خدمت دهنده
                                    try
                                    {
                                        //  ViewBag.PmOfZarinPal += "______________________" + " 44 ";
                                        var ui = Convert.ToInt32(Session["PaymentId"]);
                                        var m = _PaymentP.Where(c => c.Id == ui).FirstOrDefault();
                                        if (m != null)
                                        {
                                            //  ViewBag.PmOfZarinPal += "______________________" + " 55 ";

                                            var MemberId = m.MemberId;
                                            var user = _user.GetAllUsersWithId(MemberId).FirstOrDefault();
                                            //   ViewBag.PmOfZarinPal += "______________________" + " 66 ";

                                            if (user != null)
                                            {
                                                string Domainn = Request.Url.Host;
                                                string Title = " خدمات آنلاین ";
                                                if (user.Mobile != null && user.Mobile != "")
                                                {
                                                    PrivateTraining.ServiceLayer.BLL.SendSms Sms =
                                                        new PrivateTraining.ServiceLayer.BLL.SendSms(_uow);
                                                    Sms.SensSmsRegisterProvider(Domainn, user.Name, user.Family,
                                                        user.NationalCode, Session["RegisterPassWord"].ToString(),
                                                        user.Mobile, Convert.ToInt32(User.Identity.GetUserId()), Title,
                                                        user.Sex);
                                                    // ViewBag.PmOfZarinPal += "______________________" + " 77 ";
                                                }
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        //ViewBag.PmOfZarinPal += "______________________" + " 88 ";
                                    }
                                }

                                //    ------------------------------------------
                            }
                            catch (Exception ex)
                            {
                                ViewBag.PmerrorOfZarinPal += ex.Message;
                            }

                            Session["PaymentId"] = "";
                            Session["PriceZ"] = "";
                            Session["PaymentId"] = "";
                            Session["TypeZ"] = "";
                        }
                        else
                        {
                            ViewBag.PmOfZarinPal =
                                "  عملیات با شکست مواجه شد!!" + "وضعیت :" + MessageOfZarinPal(Status);
                        }
                    }
                    else
                    {
                        ViewBag.PmOfZarinPal = "عملیات با شکست مواجه شد! Authority: " +
                                               Request.QueryString["Authority"].ToString() + " Status: " +
                                               Request.QueryString["Status"].ToString();
                    }
                }
                else
                {
                    ViewBag.PmOfZarinPal = "عملیات با شکست مواجه شد";
                }
            }
            catch (Exception ex)
            {
                ViewBag.PmOfZarinPal = ex.Message;
            }

            return View();
        }

        //[AllowAnonymous]
        public string MessageOfZarinPal(int result)
        {
            string Message = null;
            switch (result)
            {
                case -1:
                    Message = "اطلاعات ارسال شده ناقص است.";
                    break;
                case -2:
                    Message = "و يا مرچنت كد پذيرنده صحيح نيست. IP";
                    break;
                case -3:
                    Message = "با توجه به محدوديت هاي شاپرك امكان پرداخت با رقم درخواست شده ميسر نمي باشد.";
                    break;
                case -4:
                    Message = "سطح تاييد پذيرنده پايين تر از سطح نقره اي است.";
                    break;
                case -11:
                    Message = "درخواست مورد نظر يافت نشد.";
                    break;
                case -12:
                    Message = "امكان ويرايش درخواست ميسر نمي باشد.";
                    break;
                case -21:
                    Message = "هيچ نوع عمليات مالي براي اين تراكنش يافت نشد.";
                    break;
                case -22:
                    Message = "تراكنش نا موفق ميباشد.";
                    break;
                case -33:
                    Message = "رقم تراكنش با رقم پرداخت شده مطابقت ندارد.";
                    break;
                case -34:
                    Message = "سقف تقسيم تراكنش از لحاظ تعداد يا رقم عبور نموده است";
                    break;
                case -40:
                    Message = "اجازه دسترسي به متد مربوطه وجود ندارد.";
                    break;
                case -41:
                    Message = "غيرمعتبر ميباشد. AdditionalData اطلاعات ارسال شده مربوط به";
                    break;
                case -42:
                    Message = "مدت زمان معتبر طول عمر شناسه پرداخت بايد بين 30 دقيه تا 45 روز مي باشد";
                    break;
                case -54:
                    Message = "درخواست مورد نظر آرشيو شده است.";
                    break;
                case 100:
                    Message = "عمليات با موفقيت انجام گرديده است.";
                    break;
                case 101:
                    Message = "تراكنش انجام شده است. PaymentVerification عمليات پرداخت موفق بوده و قبلا";
                    break;
                default:
                    Message = "خطا مشخص نمی باشد";

                    break;
            }

            return Message;
        }

        #endregion

        //---------------------------------------
    }
}