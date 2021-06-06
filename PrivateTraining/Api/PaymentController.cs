using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Castle.Core.Internal;
using Fasterflect;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Asn1.Ocsp;
using PrivateTraining.DataLayer.Context;
using PrivateTraining.DomainClasses.Entities;
using PrivateTraining.DomainClasses.Entities.BaseTable;
using PrivateTraining.DomainClasses.Entities.FrameWork;
using PrivateTraining.DomainClasses.Entities.Payment;
using PrivateTraining.DomainClasses.Entities.PrivateTraining;
using PrivateTraining.DomainClasses.Entities.Security;
using PrivateTraining.DomainClasses.EntitiesView.PrivateTrainig;
using PrivateTraining.DomainClasses.StaticMethods;
using PrivateTraining.LocationLayer.Interface.PrivateTraining;
using PrivateTraining.Models;
using PrivateTraining.ServiceLayer.BLL;
using PrivateTraining.ServiceLayer.Extention;
using PrivateTraining.ServiceLayer.Interface;
using PrivateTraining.ServiceLayer.Interface.Framework;
using PrivateTraining.ServiceLayer.Interface.PrivateTraining;
using PrivateTraining.ServiceLayer.Interface.Security;
using PrivateTraining.ServiceLocationLayer.Interface.PrivateTraining;
using PrivateTraining.Utils;
using ApplicationUser = PrivateTraining.DomainClasses.Security.ApplicationUser;

#pragma warning disable 1591

namespace PrivateTraining.Api
{
    /// <summary>
    /// </summary>
    public class PaymentController : BaseApiController
    {
        #region DI

        private IUnitOfWork _uow;
        private IDbSet<ServiceProperties> _ServiceProperties;
        private readonly IServiceProviderInfo _ServiceProviderInfo;

        private readonly IServiceReceiverInfo _ServiceReceiverInfo;
        private readonly IServiceLocation _servicelocation;
        private IDbSet<Location> _Location;
        private IDbSet<UserServiceLocation> _UserServiceLocations;
        private IDbSet<UserLocation> _UserLocation;
        private IDbSet<UserServiceScore> _UserServiceScore;
        private IDbSet<UserService> _UserService;
        private IDbSet<Chat> _Chat;
        private IDbSet<ServiceLocationWorkUnit> _ServiceLocationWorkUnit;
        private IDbSet<ApplicationUser> _User;
        private IDbSet<ServiceLevelList> _ServiceLevelList;
        private IDbSet<WorkUnit> _WorkUnit;
        private IDbSet<BuyService> _BuyService;

        private IDbSet<BuyServiceCostTime> _BuyServiceCostTime;

        private readonly IMessage _message;

        private IServiceReceiverServiceLocation _servicereceiveservicelocation;


        private readonly IApplicationUserManager _userManager;
        private IDbSet<BankPayment> _BankPayment;
        private IDbSet<Payment> _Payment;

        public PaymentController(IUnitOfWork uow, IServiceProviderInfo serviceProviderInfo,
            IServiceLocation servicelocation
            , IServiceReceiverServiceLocation servicereceiveservicelocation,
            IMessage message,
            IApplicationUserManager userManager)
        {
            _uow = uow;
            _ServiceProperties = _uow.Set<ServiceProperties>();

            //   _LeaveRequest = leaveRequest;
            _ServiceProviderInfo = serviceProviderInfo;
            _Location = _uow.Set<Location>();
            _ServiceProperties = _uow.Set<ServiceProperties>();
            _UserServiceLocations = _uow.Set<UserServiceLocation>();
            _UserService = _uow.Set<UserService>();
            _UserLocation = _uow.Set<UserLocation>();
            _UserServiceScore = _uow.Set<UserServiceScore>();

            _Chat = _uow.Set<Chat>();
            _ServiceLocationWorkUnit = _uow.Set<ServiceLocationWorkUnit>();
            _User = _uow.Set<ApplicationUser>();
            _WorkUnit = _uow.Set<WorkUnit>();
            _BuyServiceCostTime = _uow.Set<BuyServiceCostTime>();
            _BuyService = _uow.Set<BuyService>();

            _ServiceLevelList = _uow.Set<ServiceLevelList>();


            _servicelocation = servicelocation;

            _servicereceiveservicelocation = servicereceiveservicelocation;

            _message = message;

            _userManager = userManager;
            _BankPayment = _uow.Set<BankPayment>();
            _Payment = _uow.Set<Payment>();
        }

        #endregion

        public int GetUserId()
        {
            return Convert.ToInt32(User.Identity.GetUserId());
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> ProviderPayments(JObject input)
        {
            var providerUserId = GetUserId();
            var providerServices = _BuyService.Where(s => s.serviceProviderId == providerUserId);
            var buyServiceIdList = providerServices.Select(s => s.id);
            int[] paymentTypes =
                {PaymentTypeEnum.customerCash, PaymentTypeEnum.customerFinishPay, PaymentTypeEnum.minPricePay};
            var paymentList =
                _Payment.Where(p =>
                    p.userId == providerUserId ||
                    (paymentTypes.Contains(p.refType) && buyServiceIdList.Contains(p.refId)));
            
            var serviceIdListDistinct = providerServices.Select(s => s.serviceId).Distinct();
            var servicePropertyList = _ServiceProperties.Where(sp => serviceIdListDistinct.Contains(sp.Id));

            return Ok(new {result = "done", paymentList, providerServices, servicePropertyList});
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> CustomerCash(JObject input)
        {
            var providerUserId = GetUserId();

            var buyServiceId = input["buyServiceId"]?.Value<int>() ?? -1;
            var buyService = _BuyService.FirstOrDefault(bs => bs.id == buyServiceId);

            if (buyService == null)
            {
                return Ok(new {result = "error", message = "سرویس مورد نظر پیدا نشد!"});
            }

            var price = input["price"]?.Value<int>() ?? -1;

            if (buyServiceId == -1)
            {
                return Ok(new {result = "error", message = "قیمت غیرمجاز است!"});
            }

            var PD = new PersianDate();
            var dateNow = PD.PersianDateLow();
            var timeNow = PD.CurrentTime();

            var payment = new Payment
            {
                userId = providerUserId,
                price = price,
                refId = buyServiceId,
                refType = PaymentTypeEnum.customerCash,
                date = dateNow,
                time = timeNow,
                status = 1,
            };

            _Payment.Add(payment);

            var rowAffected = _uow.SaveAllChanges();
            if (rowAffected != 1)
            {
                //error
                return Ok(new {result = "error", message = "ذخیره سازی تغییرات در پایگاه داده به خطایی برخورد!"});
            }

            buyService.payedOffline += price;

            if (rowAffected != 1)
            {
                //error
                return Ok(new {result = "error", message = "ذخیره سازی تغییرات در پایگاه داده به خطایی برخورد!"});
            }

            return Ok(new {result = "done", payment});
        }


        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> ServicePayment(JObject input)
        {
            var userId = GetUserId();
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Ok(new {result = "error", message = "کاربر مورد نظر پیدا نشد!"});
            }

            var price = input["price"]?.Value<int>() ?? 0;

            if (price > user.Credit)
            {
                return Ok(new {result = "error", message = "اعتبار کافی نیست !"});
            }

            var buyServiceId = input["buyServiceId"]?.Value<int>() ?? -1;
            var buyService = _BuyService.FirstOrDefault(bs => bs.id == buyServiceId);

            if (buyService == null)
            {
                return Ok(new {result = "error", message = "سرویس مورد نظر پیدا نشد!"});
            }

            if (buyService.status != 2 && buyService.status != 10 && buyService.status != 12)
            {
                return Ok(new {result = "error", message = "پرداخت برای این سرویس مجاز نیست!"});
            }


            var PD = new PersianDate();
            var dateNow = PD.PersianDateLow();
            var timeNow = PD.CurrentTime();

            var refType = input["refType"]?.Value<int>() ?? PaymentTypeEnum.minPricePay;

            var payment = new Payment
            {
                userId = userId,
                price = price,
                refId = buyServiceId,
                refType = refType,
                date = dateNow,
                time = timeNow,
                status = 1,
            };

            _Payment.Add(payment);

            var rowAffected = _uow.SaveAllChanges();
            if (rowAffected != 1)
            {
                //error
                return Ok(new {result = "error", message = "ذخیره سازی تغییرات در پایگاه داده به خطایی برخورد!"});
            }

            user.Credit = user.Credit - price;
            rowAffected = _uow.SaveAllChanges();
            if (rowAffected != 1)
            {
                //error
                return Ok(new {result = "error", message = "ذخیره سازی تغییرات در پایگاه داده به خطایی برخورد!"});
            }

            buyService.payed += price;

            if (buyService.status == 2) //certain
            {
                buyService.status = 10; //doing
            }

            rowAffected = _uow.SaveAllChanges();
            if (rowAffected != 1)
            {
                //error
                return Ok(new {result = "error", message = "ذخیره سازی تغییرات در پایگاه داده به خطایی برخورد!"});
            }

            return Ok(new
            {
                result = "done", buyService, user = new
                {
                    name = user.Name,
                    family = user.Family,
                    sex = user.Sex,
                    id = user.Id,
                    email = user.Email,
                    mobile = user.Mobile,
                    credit = user.Credit,
                    addressJson = user.AddressJson,
                },
                payment
            });
        }


        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> AdminPayment(JObject input)
        {
            if (!User.IsInRole("Admin"))
            {
                return Ok(new
                    {result = "error", message = "باید ادمین باشید!"});
            }

            var price = input["price"]?.Value<int>() ?? 0;

            if (price == 0)
            {
                return Ok(new
                    {result = "error", message = "مبلغ ورودی غیرمجاز است!"});
            }

            var userId = input["userId"]?.Value<int>() ?? 0;

            if (userId == 0)
            {
                return Ok(new
                    {result = "error", message = "شناسه کاربر ورودی غیرمجاز است!"});
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Ok(new {result = "error", message = "کاربر مورد نظر پیدا نشد!"});
            }

            var PD = new PersianDate();
            var dateNow = PD.PersianDateLow();
            var timeNow = PD.CurrentTime();

            var payment = new Payment
            {
                userId = userId,
                price = price,
                refId = GetUserId(),
                refType = PaymentTypeEnum.admin,
                date = dateNow,
                time = timeNow,
                status = 1,
            };

            _Payment.Add(payment);

            var rowAffected = _uow.SaveAllChanges();
            if (rowAffected != 1)
            {
                //error
                return Ok(new {result = "error", message = "ذخیره سازی تغییرات در پایگاه داده به خطایی برخورد!"});
            }

            user.Credit += price;

            rowAffected = _uow.SaveAllChanges();
            if (rowAffected != 1)
            {
                //error
                return Ok(new {result = "error", message = "ذخیره سازی تغییرات در پایگاه داده به خطایی برخورد!"});
            }

            return Ok(new
                {result = "done", message = "افزایش اعتبار انجام شد.", credit = user.Credit});
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> AddBankPayment(JObject input)
        {
            var price = input["price"]?.Value<int>() ?? -1;
            if (price < 1000)
            {
                return Ok(new {result = "error", message = "مبلغ باید بیش از 1000 تومن باشد!"});
            }

            var userId = GetUserId();

            var bankCode = input["bankCode"]?.Value<int>() ?? 1; //default zarin pal

            if (bankCode == 1)
            {
                //zarin pal
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return Ok(new {result = "error", message = "کاربر پیدا نشد!"});
                }

                var zarinPalClient = new ZarinPal.PaymentGatewayImplementationServicePortTypeClient();


                var PD = new PersianDate();
                var dateNow = PD.PersianDateLow();
                var timeNow = PD.CurrentTime();
                var bankPayment = new BankPayment
                {
                    price = price,
                    userId = userId,
                    bankCode = bankCode,
                    status = 0,
                    date = dateNow,
                    time = timeNow,
                };

                _BankPayment.Add(bankPayment);
                var rowAffected = await _uow.SaveAllChangesAsync();
                if (rowAffected != 1)
                {
                    //error
                    return Ok(new {result = "error", message = "ذخیره سازی تغییرات در پایگاه داده به خطایی برخورد!"});
                }

                string authority;
//                var CallBackUrls = "http://" +
//                                   Request.RequestUri.Host + "/v1/Payment/CallBackUrlZarinPal";

                string CallBackUrls = "http://" + HttpContext.Current.Request.Url.Host +
                                      "/Framework/Payment/CallBackUrlZarinPalCustomer";

                var Status = zarinPalClient.PaymentRequest("9549f856-a1d3-11e7-94c7-000c295eb8fc", price * 10,
                    bankPayment.id.ToString(), user.Email, user.Mobile, CallBackUrls, out authority);

                bankPayment.detailJson = new JObject
                {
                    ["status"] = Status,
                    ["authority"] = authority,
                }.ToString(Formatting.None);

                rowAffected = await _uow.SaveAllChangesAsync();
                if (rowAffected != 1)
                {
                    //error
                    return Ok(new {result = "error", message = "ذخیره سازی تغییرات در پایگاه داده به خطایی برخورد!"});
                }

                return Ok(new
                    {result = "done", bankPayment, url = "https://www.zarinpal.com/pg/StartPay/" + authority});
            }

            return Ok(new
                {result = "error", message = "کدبانک اشتباه است!"});
        }
    }
}