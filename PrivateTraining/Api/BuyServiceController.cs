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
using PrivateTraining.IocConfig;
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
using ApplicationDbContext = PrivateTraining.DataLayer.Context.ApplicationDbContext;
using ApplicationUser = PrivateTraining.DomainClasses.Security.ApplicationUser;

#pragma warning disable 1591

namespace PrivateTraining.Api
{
    /// <summary>
    /// </summary>
    public class BuyServiceController : BaseApiController
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

        private IDbSet<Payment> _Payment;

        private readonly IMessage _message;

        private IServiceReceiverServiceLocation _servicereceiveservicelocation;

        public BuyServiceController(IUnitOfWork uow, IServiceProviderInfo serviceProviderInfo,
            IServiceLocation servicelocation
            , IServiceReceiverServiceLocation servicereceiveservicelocation,
            IMessage message)
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

            _Payment = _uow.Set<Payment>();

            _servicelocation = servicelocation;

            _servicereceiveservicelocation = servicereceiveservicelocation;

            _message = message;
        }

        #endregion

        public int GetUserId()
        {
            return Convert.ToInt32(User.Identity.GetUserId());
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> CheckMinPayment(JObject input)
        {
            var buyServiceId = input["id"]?.Value<int>();
            if (buyServiceId == null)
            {
                return Ok(new {result = "error", message = "سرویس مورد نظر پیدا نشد!"});
            }

//            var buyService = _BuyService.FirstOrDefault(bs => bs.id == buyServiceId);
//
//            if (buyService == null)
//            {
//                return Ok(new {result = "error", message = "سرویس مورد نظر پیدا نشد!"});
//            }

//
            Task.Run(async delegate
            {
                try
                {
                    await Task.Delay(TimeSpan.FromMinutes(60));

                    var uow = SmObjectFactory.Container.GetInstance<IUnitOfWork>();
//                    var uow = _uow;
//
                    var buyService = uow.Set<BuyService>().FirstOrDefault(bs => bs.id == buyServiceId);
                    if (buyService.status == 2)
                    {
                        buyService.status = 8;
                    }

//
                    var rowAffected = await uow.SaveAllChangesAsync();
                    if (rowAffected != 1)
                    {
                        //error
                    }

//                    using (var  db = new ApplicationDbContext())
//                    {
//                        var buyService = db.BuyService.FirstOrDefault(bs => bs.id == 2008);
//                        buyService.status = 0;
//                        db.SaveAllChanges();


//                    }
                }
                catch (Exception e)
                {
                }
            });

//            var timer = new System.Threading.Timer(
//                e =>
//                {
//                    var uow = SmObjectFactory.Container.GetInstance<IUnitOfWork>();
////
//                    var buyService = uow.Set<BuyService>().FirstOrDefault(bs => bs.id == 2008);
//                    buyService.status = 9;
//
//                    uow.SaveAllChanges();
//                },
//                null,
//                1000,
//                1000);

            return Ok(new {result = "done"});
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> ActiveAgainService(JObject input)
        {
            var buyServiceId = input["buyServiceId"]?.Value<int>();
            var status = input["status"]?.Value<int>();
            if (buyServiceId == null || status == null)
            {
                return Ok(new {result = "error", message = "ورودی صحیح نیست!"});
            }

            // var buyService = _BuyService.First(bs => bs.id == buyServiceId);
            var buyService = await _BuyService.FirstAsync(bs => bs.id == buyServiceId);
            if (buyService == null)
            {
                return Ok(new {result = "error", message = "سفارش مورد نظر یافت نشد!"});
            }

            var currentDate = new PersianDate();

            var newBuyService = new BuyService
            {
            };


            //copy
            newBuyService.attachmentPath = buyService.attachmentPath;
            newBuyService.buyServiceId = buyService.buyServiceId;
            newBuyService.date = buyService.date;
            newBuyService.dateTimeSyncByProvider = buyService.dateTimeSyncByProvider;
            newBuyService.locationId = buyService.locationId;
            newBuyService.providerServiceLocationStatus = buyService.providerServiceLocationStatus;
            newBuyService.providerType = buyService.providerType;
            newBuyService.serviceId = buyService.serviceId;
            newBuyService.serviceLevelListId = buyService.serviceLevelListId;
            newBuyService.serviceLocationId = buyService.serviceLocationId;
            newBuyService.serviceProviderId = buyService.serviceProviderId;
            newBuyService.serviceReceiverId = buyService.serviceReceiverId;
            newBuyService.time = buyService.time;
            newBuyService.userAddress = buyService.userAddress;
            newBuyService.userCityId = buyService.userCityId;
            newBuyService.userCityTitle = buyService.userCityTitle;
            newBuyService.userDescription = buyService.userDescription;
            newBuyService.userMobile = buyService.userMobile;
            newBuyService.workPriceListJson = buyService.workPriceListJson;
            //copy

            //init
            newBuyService.dateRegister = currentDate.PersianDateLow();
            newBuyService.timeRegister = currentDate.CurrentTime();
            newBuyService.payed = 0;
            newBuyService.payedOffline = 0;
            newBuyService.status = status ?? BuyServiceStatusEnum.certain;
            newBuyService.statusChangeJson = null;

            var added = await AddBuyService(newBuyService);

            if (!added)
            {
                return Ok(new {result = "error", message = "خطایی در ثبت سفارش رخ داد!"});
            }

            return Ok(new {result = "done"});
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> ChangeProvider(JObject input)
        {
            var buyServiceId = input["buyServiceId"]?.Value<int>();
            var providerId = input["providerId"]?.Value<int>();
            var serviceLocationId = input["serviceLocationId"]?.Value<int>();

            if (buyServiceId == null || providerId == null || serviceLocationId == null)
            {
                return Ok(new {result = "error", message = "ورودی صحیح نیست!"});
            }

            var buyService = _BuyService.First(bs => bs.id == buyServiceId);
            if (buyService == null)
            {
                return Ok(new {result = "error", message = "سفارش مورد نظر یافت نشد!"});
            }

            buyService.serviceProviderId = (int) providerId;
            buyService.serviceLocationId = (int) serviceLocationId;
            buyService.status = 0;

            var rowAffected = await _uow.SaveAllChangesAsync();
            if (rowAffected != 1)
            {
                //error
                return Ok(new {result = "error", message = "ذخیره سازی تغییرات در پایگاه داده به خطایی برخورد!"});
            }

            var subject = "خدمت جدید";
            var text = $"خدمت جدید  با کد {buyService.id} دارید!";

            SendMessage(text, subject, buyService);

            return Ok(new {result = "done", buyService});
        }

        public BuyService CreateBuyService(JObject input)
        {
            var request = HttpContext.Current.Request;
            var currentDate = new PersianDate();

            var serviceLevelListId = input["serviceLevelListId"]?.Value<int>() ?? -1;
            var serviceId = input["serviceId"]?.Value<int>() ?? -1;
            var locationId = input["locationId"]?.Value<int>() ?? -1;

            var providerServiceLocationStatus =
                input["providerServiceLocationStatus"]?.Value<string>() ?? ""; //string provider customer (none|empty)

            var userDescription = input["userDescription"]?.Value<string>() ?? ""; // -1 => unknown
            var userCityId = input["userCityId"]?.Value<int>() ?? -1; // -1 => unknown
            var userCityTitle = input["userCityTitle"]?.Value<string>() ?? ""; // -1 => unknown
            var userAddress = input["userAddress"]?.Value<string>() ?? ""; // -1 => unknown
            var userMobile = input["userMobile"]?.Value<string>() ?? ""; // -1 => unknown

            var workPriceListJson = input["workPriceList"]?.ToString(Formatting.None) ?? "[]"; // json list

            var serviceLocationId = input["serviceLocationId"]?.Value<int>() ?? -1; // -1 => unknown
//            var serviceProviderId = input["serviceProviderId"]?.Value<int>() ?? -1; // -1 => unknown

            var providerType =
                input["providerType"]
                    ?.Value<string>(); //providerSelectCustomer providerSelectSipars providerSelectProvider


            var date = input["date"]?.Value<string>() ?? ""; //
            var time = input["time"]?.Value<string>() ?? ""; //
            var dateTimeSyncByProvider = input["dateTimeSyncByProvider"]?.Value<bool>() ?? false; //

            //attachment

            var attachmentFile = request.Files["attachment"];
            var attachment = ""; // attachment address (empty none)
            if (attachmentFile != null)
            {
                var dupFileCount = 0;
                var path = System.Web.Hosting.HostingEnvironment.MapPath(
                    "~/UserFiles/attachments/" + serviceLocationId + "__" + attachmentFile.FileName);

                while (File.Exists(path))
                {
                    path = System.Web.Hosting.HostingEnvironment.MapPath(
                        "~/UserFiles/attachments/" + serviceLocationId + "__" + dupFileCount + "_" +
                        attachmentFile.FileName);
                    dupFileCount++;
                }

                attachmentFile.SaveAs(path);
                attachment = path;
            }


            var serviceReceiver = new BuyService();
            serviceReceiver.status = 0; //pending
            serviceReceiver.dateRegister = currentDate.PersianDateLow();
            serviceReceiver.timeRegister = currentDate.CurrentTime();

            //from system
//            serviceReceiver.user = "";

            //from user

            serviceReceiver.buyServiceId = Guid.NewGuid().GetHashCode();
            serviceReceiver.serviceId = serviceId;
            serviceReceiver.locationId = locationId;
            serviceReceiver.serviceLocationId = serviceLocationId;

            var id = input["id"]?.Value<int>();
            if (id != null)
            {
                var buyService = _BuyService.First(bs => bs.id == id);
                if (buyService == null)
                {
                    return null;
                }

                serviceReceiver.serviceReceiverId = buyService.serviceReceiverId;
            }
            else
            {
                var currentUserId = GetUserId();
                serviceReceiver.serviceReceiverId = currentUserId;
            }


            serviceReceiver.userCityId = userCityId;
            serviceReceiver.userCityTitle = userCityTitle;
            serviceReceiver.userAddress = userAddress;
            serviceReceiver.userMobile = userMobile;
            serviceReceiver.userDescription = userDescription;
            serviceReceiver.providerServiceLocationStatus = providerServiceLocationStatus;
            serviceReceiver.attachmentPath = attachment;
            serviceReceiver.providerType = providerType;
            serviceReceiver.workPriceListJson = workPriceListJson;

            serviceReceiver.date = date;
            serviceReceiver.time = time;
            serviceReceiver.dateTimeSyncByProvider = dateTimeSyncByProvider;

            serviceReceiver.serviceLevelListId = serviceLevelListId;

            return serviceReceiver;
        }

        public bool SendMessage(string text, string subject, BuyService buyService)
        {
            PersianDate PD = new PersianDate();
            var message = new Message();
            message.SenderUserId = buyService.serviceReceiverId;
            message.ReciverUserId = buyService.serviceProviderId;
            message.Desc = text;
            message.Subject = subject;
            message.Date = PD.PersianDateLow();
            message.Time = PD.CurrentTime();

            var Query = _message.AddMessage(message);
            var Status = _uow.SaveAllChanges();


            var user = _User.FirstOrDefault(u => u.Id == message.SenderUserId);
            var msg = user.Name + " " + user.Family +
                      " عزیز، سفارش شما با کد " +
                      buyService.id +
                      " ثبت شد. به زودی تماس می گیریم. جهت مدیریت سفارش به پنل اختصاصی در سامانه سی پارس وارد شوید";

            SendNotification("سفارش جدید " + buyService.id, msg, user.Id, buyService);

            msg += "\n  sipars.ir";

            SendSms(user.Mobile, msg, user.Id);

            var providerUser = _User.FirstOrDefault(u => u.Id == message.ReciverUserId);

            msg = "سفارش جدید از سی پارس با کد " +
                  buyService.id +
                  " در پنل شماست. در اسرع وقت پاسخ دهید. ";

            SendNotification("سفارش جدید " + buyService.id, msg, providerUser.Id, buyService);

            msg += "\n  sipars.ir";
            SendSms(providerUser.Mobile, msg, providerUser.Id);

            return Status == 1;
        }

        public async Task<bool> AddBuyService(BuyService buyService)
        {
            _BuyService.Add(buyService);
            var queryAffectCount = await _uow.SaveAllChangesAsync();
            if (queryAffectCount != 1) return false;

            //send message and notification

            //TODO Get Sms Text and ...

            var subject = "خدمت جدید";
            var text = $"خدمت جدید  با کد {buyService.id} دارید!";

            SendMessage(text, subject, buyService);

            Task.Run(async delegate
            {
                try
                {
                    var id = buyService.id;
                    await Task.Delay(TimeSpan.FromMinutes(20));

                    var uow = SmObjectFactory.Container.GetInstance<IUnitOfWork>();
                    var bs = uow.Set<BuyService>().FirstOrDefault(b => b.id == id);
                    if (bs != null)
                    {
                        if (bs.status == BuyServiceStatusEnum.pending)
                        {
                            //send sms
                            var user = uow.Set<ApplicationUser>().FirstOrDefault(u => u.Id == bs.serviceReceiverId);
                            var msg = "خدمتیار انتخابی، سفارش را تایید نکرده است. جهت انتخاب دیگر به پنل خود وارد شوید";

                            SendNotification("سفارش " + buyService.id, msg, user.Id, buyService);

                            msg += "\n  sipars.ir";

                            new SendSms(uow).SendSmsClass(user.Mobile, msg, user.Id);
                        }
                    }

//
                    var rowAffected = await uow.SaveAllChangesAsync();
                    if (rowAffected != 1)
                    {
                        //error
                    }

//          
                }
                catch (Exception e)
                {
                }
            });

            return true;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> BuyServiceSelectProvider()
        {
            var currentUserId = GetUserId();
//            var user = await _userManager.FindByIdAsync(currentUserId);
            if (currentUserId == 0)
            {
                return Ok(new {result = "error", message = "کاربری با این شماره یافت نشد!"});
            }

            var request = HttpContext.Current.Request;
            var input = JObject.Parse(request.Form["input"]);

            var serviceProviderId = input["serviceProviderId"]?.Value<int>() ?? -1; // json list

            //TODO check validity
            if (serviceProviderId == -1)
            {
                return Ok(new {result = "error", message = "خدمتیار مورد نظر یافت نشد!"});
            }

            var buyService = CreateBuyService(input);

            if (buyService == null)
            {
                return Ok(new {result = "error", message = "خطایی در شناسایی سفارش قبل رخ داد!"});
            }

            buyService.serviceProviderId = serviceProviderId;

            var added = await AddBuyService(buyService);

            if (!added)
            {
                return Ok(new {result = "error", message = "خطایی در ثبت سفارش رخ داد!"});
            }


            return Ok(new {result = "done"});
        }


        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> ChangeStatusTimeCost(JObject input)
        {
            var id = input["id"]?.Value<int>();
            var status = input["status"]?.Value<int>();
            var description = input["description"]?.Value<string>();

            if (id == null || status == null)
            {
                return Ok(new {result = "error", message = "ورودی صحیح نیست!"});
            }

            var costTime = _BuyServiceCostTime.First(bs => bs.id == id);
            if (costTime == null)
            {
                return Ok(new {result = "error", message = "ارائه مورد نظر یافت نشد!"});
            }

            var PD = new PersianDate();
            var dateNow = PD.PersianDateLow();
            var timeNow = PD.CurrentTime();

            var currentUserId = GetUserId();
            var statusChangeList = JArray.Parse(costTime.statusChangeJson ?? "[]");
            statusChangeList.Add(new JObject
            {
                ["date"] = dateNow,
                ["time"] = timeNow,
                ["userId"] = currentUserId,
                ["status"] = status,
                ["lastStatus"] = costTime.status,
            });

            costTime.status = (int) status;
            costTime.statusChangeJson = statusChangeList.ToString(Formatting.None);
            costTime.description = description;

            var rowAffected = await _uow.SaveAllChangesAsync();
            if (rowAffected != 1)
            {
                //error
                return Ok(new {result = "error", message = "ذخیره سازی تغییرات در پایگاه داده به خطایی برخورد!"});
            }


            return Ok(new {result = "done"});
        }


        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> AddCostTime(JObject input)
        {
            var buyServiceId = input["buyServiceId"]?.Value<int>();

            if (buyServiceId == null)
            {
                return Ok(new {result = "error", message = "ورودی صحیح نیست!"});
            }

            var buyService = _BuyService.First(bs => bs.id == buyServiceId);
            if (buyService == null)
            {
                return Ok(new {result = "error", message = "سفارش مورد نظر یافت نشد!"});
            }

            var PD = new PersianDate();
            var dateRegister = PD.PersianDateLow();
            var timeRegister = PD.CurrentTime();

            var date = input["date"]?.Value<string>();
            var toTime = input["toTime"]?.Value<string>();
            var fromTime = input["fromTime"]?.Value<string>();
            var priceReceived = input["priceReceived"]?.Value<long>() ?? 0;
            var notFinished = input["notFinished"]?.Value<bool>() ?? false;
            var next = input["next"]?.Value<bool>() ?? false;

            var buyServiceCostTime = new BuyServiceCostTime
            {
                buyServiceId = (int) buyServiceId,
                dateRegister = dateRegister,
                timeRegister = timeRegister,
                status = 0,

                date = date,
                toTime = toTime,
                fromTime = fromTime,
                priceReceived = priceReceived,
                notFinished = notFinished,
                next = next,
            };

            if (next)
            {
                var nextDate = input["nextDate"]?.Value<string>();
                var nextToTime = input["nextToTime"]?.Value<string>();
                var nextFromTime = input["nextFromTime"]?.Value<string>();
                buyServiceCostTime.nextDate = nextDate;
                buyServiceCostTime.nextToTime = nextToTime;
                buyServiceCostTime.nextFromTime = nextFromTime;
            }

            _BuyServiceCostTime.Add(buyServiceCostTime);

            var rowAffected = await _uow.SaveAllChangesAsync();
            if (rowAffected != 1)
            {
                //error
                return Ok(new {result = "error", message = "ذخیره سازی تغییرات در پایگاه داده به خطایی برخورد!"});
            }

            SendNotification("ارائه خدمت " + buyServiceId, "ارائه خدمت جدید برای این سفارش ثبت شده است.",
                buyService.serviceReceiverId, buyService);


            return Ok(new {result = "done", buyServiceCostTime});
        }

        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> GetPrice(JObject input)
        {
            var id = input["id"]?.Value<int>();

            if (id == null)
            {
                return Ok(new {result = "error", message = "ورودی صحیح نیست!"});
            }

            var buyService = _BuyService.First(bs => bs.id == id);
            if (buyService == null)
            {
                return Ok(new {result = "error", message = "سفارش مورد نظر یافت نشد!"});
            }

            var serviceId = buyService.serviceId;

            var provider = _User.First(user => user.Id == buyService.serviceProviderId);
            var serviceLevelListId = provider.UserServices.First(us => us.ServiceId == serviceId).ServiceLevelListId;
            var serviceLevelList = _ServiceLevelList.First(sl => sl.Id == serviceLevelListId);

            var workPriceList = JArray.Parse(buyService.workPriceListJson);
            var workUnitIdList = workPriceList.Select(wp => wp["workUnitId"]?.Value<int>()).Distinct()
                .Where(wuId => wuId != null);

            var workUnitList = _ServiceLocationWorkUnit
                .Where(w => w.ServiceLocations.ServiceId == buyService.serviceId &&
                            w.ServiceLocations.LocationId == buyService.locationId &&
                            workUnitIdList.Contains(w.WorkUnitId));

            //meetingUnknown
//            var workPriceListNew = workPriceList.Select(wp =>
//            {
////                wp["serviceLevelListId"] = serviceLevelListId;
////                wp["serviceLevelListPercent"] = serviceLevelList.PercentServiceLevel;
//                
//                var wu = workUnitList.First(w => w.Id == wp["workUnitId"].Value<int>());
//                wp["price"] =
//                    wu.PriceWorkUnit + (serviceLevelList.PercentServiceLevel / 100 * wu.PriceWorkUnit);
//                
//                return wp;
//            });
//            
            foreach (var wp in workPriceList)
            {
                var workUnitId = wp["workUnitId"]?.Value<int>();
                var wu = workUnitList.First(w => w.WorkUnitId == workUnitId);
                wp["price"] =
                    wu.PriceWorkUnit + (serviceLevelList.PercentServiceLevel / 100 * wu.PriceWorkUnit);
            }

            return Ok(new {result = "done", workPriceList, serviceLevelListId});
        }

        public void SendNotification(string title, string body, int receiverId, BuyService buyService)
        {
            Task.Run(async delegate
            {
                ApiUtils.SendPush(receiverId, new JObject
                {
                    ["type"] = "buyService",
                    ["buyServiceId"] = buyService.id,
                    ["userId"] = receiverId,
                    ["title"] = title,
                    ["body"] = body,
                });
            });
        }

        public void SendSms(string mobile, string msg, int userId)
        {
            try
            {
                var Sms = new SendSms(_uow);

                Sms.SendSmsClass(mobile, msg, userId);
            }
            catch (Exception e)
            {
            }
        }

        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> ChangeStatus(JObject input)
        {
            var id = input["id"]?.Value<int>();
            var status = input["status"]?.Value<int>();
            var text = input["text"]?.Value<string>();

            if (id == null || status == null)
            {
                return Ok(new {result = "error", message = "ورودی صحیح نیست!"});
            }

            var buyService = _BuyService.First(bs => bs.id == id);
            if (buyService == null)
            {
                return Ok(new {result = "error", message = "سفارش مورد نظر یافت نشد!"});
            }

            var PD = new PersianDate();
            var dateNow = PD.PersianDateLow();
            var timeNow = PD.CurrentTime();

            var currentUserId = GetUserId();
            var statusChangeList = JArray.Parse(buyService.statusChangeJson ?? "[]");
            statusChangeList.Add(new JObject
            {
                ["date"] = dateNow,
                ["time"] = timeNow,
                ["userId"] = currentUserId,
                ["status"] = status,
                ["lastStatus"] = buyService.status,
                ["text"] = text,
            });

            var oldStatus = buyService.status;
            buyService.status = (int) status;
            buyService.statusChangeJson = statusChangeList.ToString(Formatting.None);

            var rowAffected = await _uow.SaveAllChangesAsync();
            if (rowAffected != 1)
            {
                //error
                return Ok(new {result = "error", message = "ذخیره سازی تغییرات در پایگاه داده به خطایی برخورد!"});
            }

            if (oldStatus == BuyServiceStatusEnum.accept)
            {
                if (buyService.status == BuyServiceStatusEnum.certain ||
                    buyService.status == BuyServiceStatusEnum.doing)
                {
                    //send sms
                    var user = _User.FirstOrDefault(u => u.Id == buyService.serviceReceiverId);
                    var providerUser = _User.FirstOrDefault(u => u.Id == buyService.serviceProviderId);
                    var msg = "سفارش شما با کد " + buyService.id +
                              " قطعی و «" +
                              providerUser.Name + " " + providerUser.Family +
                              "» جهت انجام سفارش تعیین گردید";

                    SendNotification("قطعی شدن " + buyService.id, msg, user.Id, buyService);
                    SendSms(user.Mobile, msg, user.Id);
                }
            }

            if (buyService.status == BuyServiceStatusEnum.rejected ||
                buyService.status == BuyServiceStatusEnum.unCertain)
            {
                //send sms
                var user = _User.FirstOrDefault(u => u.Id == buyService.serviceReceiverId);
                var msg = "خدمتیار انتخابی، سفارش را تایید نکرده است. جهت انتخاب دیگر به پنل خود وارد شوید";

                SendNotification("سفارش " + buyService.id, msg, user.Id, buyService);

                msg += "\n  sipars.ir";

                SendSms(user.Mobile, msg, user.Id);
            }

            if (buyService.status == BuyServiceStatusEnum.finish)
            {
                //send sms
                var user = _User.FirstOrDefault(u => u.Id == buyService.serviceReceiverId);
                var msg =
                    "سفارش شما پایان یافته است. ضمن قدردانی از اعتماد شما، درخواست می شود، فرم ارزیابی خدمتیار را تکمیل نمایید.";

                SendNotification("اتمام سفارش " + buyService.id, msg, user.Id, buyService);

                SendSms(user.Mobile, msg, user.Id);
            }


            // switch ((StatusServiceLocationRequest) status)
            // {
            //     case StatusServiceLocationRequest.UnCertain:
            //     case StatusServiceLocationRequest.Rejected:
            //         //TODO NOTIFY notify Customer
            //         break;
            //
            //     case StatusServiceLocationRequest.Canceled:
            //         break;
            // }

            return Ok(new {result = "done", message = "درخواست انجام شد.", buyService});
        }

        public IHttpActionResult GetBuyServiceResult(IQueryable<BuyService> listQuery, int count,
            string userType)
        {
            //workPriceListJson
            var workIdList = new List<int>();
            var userIdList = new List<int>();
            var serviceLocationIdList = new List<int>();
            var buyServiceIdList = new List<int>();
            var serviceLevelIdList = new List<int>();
            var serviceIdList = new List<int>();

            var list = listQuery.ToList();

            JObject chatCountCustomer = new JObject();
            JObject chatCountProvider = new JObject();
            list.ForEach(buyService =>
            {
                buyServiceIdList.Add(buyService.id);

                userIdList.Add(buyService.serviceProviderId);
                userIdList.Add(buyService.serviceReceiverId);

                chatCountCustomer[buyService.id + ""] = _Chat.Count(s =>
                    s.buyServiceId == buyService.id && s.senderId == buyService.serviceReceiverId);
                chatCountProvider[buyService.id + ""] = _Chat.Count(s =>
                    s.buyServiceId == buyService.id && s.senderId == buyService.serviceProviderId);

                serviceLocationIdList.Add(buyService.serviceLocationId);
                serviceIdList.Add(buyService.serviceId);
//                serviceLevelIdList.Add(buyService.serviceLevelListId);

                if (buyService.workPriceListJson?.StartsWith("[") == true)
                    JArray.Parse(buyService.workPriceListJson)
                        .ForEach(w =>
                            workIdList.Add((w["workUnitId"]?.Value<int>() ?? -1)));
            });

            // foreach (var buyServiceId in buyServiceIdList)
            // {
            //     chatCounts[buyServiceId + ""] = _Chat.Count(s => s.buyServiceId == buyServiceId);
            // }


            buyServiceIdList = buyServiceIdList.Distinct().ToList();

            var workIdListDistinc = workIdList.Distinct();
            var serviceLocationIdListDistinc = serviceLocationIdList.Distinct();

            var workUnitList = _ServiceLocationWorkUnit
                .Where(w => serviceLocationIdListDistinc.Any(WS => WS == w.ServiceLocationId))
                .Where(w => workIdListDistinc.Any(WS => WS == w.WorkUnitId))
                .Select(w => new
                {
                    title = w.WorkUnits.Title,
                    id = w.WorkUnitId,
                    price = w.PriceWorkUnit,
                });

            var serviceIdListDistinct = serviceIdList.Distinct();

            userIdList = userIdList.Distinct().ToList();
            var userList = _User.Where(u => userIdList.Any(uId => uId == u.Id)).Select(user => new
            {
                name = user.Name,
                family = user.Family,
                sex = user.Sex,
                id = user.Id,
                email = user.Email,
                mobile = user.Mobile,
                credit = user.Credit,
                picture = user.Picture,
                path = user.Path,
                serviceLevelList = user.UserServices.Where(us => serviceIdListDistinct.Contains(us.ServiceId)).Select(
                    us => new
                    {
                        serviceId = us.ServiceId,
                        serviceLevelListId = us.ServiceLevelListId,
                    })
            });

            userList.ForEach(u => u.serviceLevelList.ForEach(usl => serviceLevelIdList.Add(usl.serviceLevelListId)));
            serviceLevelIdList = serviceLevelIdList.Distinct().ToList();

            var serviceLevelList = _ServiceLevelList.Where(sl => serviceLevelIdList.Contains(sl.Id)).Select(
                serviceList =>
                    new
                    {
                        id = serviceList.Id,
                        percent = serviceList.PercentServiceLevel,
                        title = serviceList.ServiceLevels.Title,
                    });

            var servicePropertyList = _ServiceProperties.Where(sp => serviceIdListDistinct.Contains(sp.Id));

            var costTimeList = _BuyServiceCostTime.Where(ct => buyServiceIdList.Contains(ct.buyServiceId));

            var paymentList = _Payment.Where(p => buyServiceIdList.Contains(p.refId));


            return Ok(new
            {
                result = "done", buyServiceList = list, count, workUnitList, userList, userType,
                serviceLevelList, servicePropertyList, costTimeList, paymentList, chatCountCustomer, chatCountProvider
            });
        }

        //[System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> GetList(JObject input)
        {
            var skipNumber = input["skipNumber"]?.Value<int>() ?? 0;
            var takeNumber = input["takeNumber"]?.Value<int>() ?? 10;
//            var serviceBuyStatus = input["serviceBuyStatus"]?.Value<int>() ?? -1;

            var currentUserId = GetUserId();

            var AllList = _BuyService;
            IQueryable<BuyService> list;

//            list = AllList.OrderByDescending(buyService => buyService.dateRegister)
//                .ThenByDescending(b => b.timeRegister)
//                .Skip(skipNumber).Take(takeNumber);
//            return GetBuyServiceResult(list, AllList.Count(), "admin");

            if (User.IsInRole("Admin") || User.IsInRole("Administrator") || User.IsInRole("Modrator"))
            {
                var count = AllList.Count();
                list = AllList.OrderByDescending(buyService => buyService.dateRegister)
                    .ThenByDescending(b => b.timeRegister)
                    .Skip(skipNumber).Take(takeNumber);
                return GetBuyServiceResult(list, count, "admin");
            }
            else if (User.IsInRole("ServiceProvider"))
            {
                list = AllList.Where(c => c.serviceProviderId == currentUserId && c.status != 8 && c.status != 9);
                var count = list.Count();

                list = list.OrderByDescending(buyService => buyService.dateRegister)
                    .ThenByDescending(b => b.timeRegister).Skip(skipNumber).Take(takeNumber);

                return GetBuyServiceResult(list, count, "provider");
            }

            list = AllList.Where(c => c.serviceReceiverId == currentUserId && c.status != 9);
            var countOfCustomerList = list.Count();

            list = list.OrderByDescending(buyService => buyService.dateRegister).ThenByDescending(b => b.timeRegister)
                .Skip(skipNumber).Take(takeNumber);


            return GetBuyServiceResult(list, countOfCustomerList, "customer");
        }


//        public IHttpActionResult GetBuyServiceResult2(IQueryable<ServiceReceiverServiceLocation> list, int count,
//            string userType)
//        {
//            //workPriceListJson
//            var workIdList = new List<string>();
//            var userIdList = new List<int>();
//            var buyServiceIdList = new List<int>();
//
//            list.ForEach(buyService =>
//            {
//                buyServiceIdList.Add((int) buyService.Id);
//
//                userIdList.Add(buyService.ServiceProviderId);
//                userIdList.Add(buyService.ServiceReceiverId);
//                //serviceLevelIdList.Add(buyService.ServiceLocationId);
//
//                if (buyService.workPriceListJson?.StartsWith("[") == true)
//                    JArray.Parse(buyService.workPriceListJson)
//                        .ForEach(w =>
//                            workIdList.Add((w["workUnitId"]?.Value<int>() ?? -1) + "-" + buyService.ServiceLocationId));
//            });
//
//            workIdList = workIdList.Distinct().ToList();
//            var workUnitList = _ServiceLocationWorkUnit
//                .Where(w => workIdList.Any(WS => WS == w.WorkUnitId + "-" + w.ServiceLocationId)).Select(w => new
//                {
//                    title = w.WorkUnits.Title,
//                    id = w.WorkUnitId,
//                    price = w.PriceWorkUnit,
//                });
//            var serviceIdList = list.Select(s => s.ServiceLocations.Services.Id);
//            var serviceIdListDistinc = serviceIdList.Distinct();
//
//            userIdList = userIdList.Distinct().ToList();
//            var userList = _User.Where(u => userIdList.Any(uId => uId == u.Id)).Select(user => new
//            {
//                name = user.Name,
//                family = user.Family,
//                sex = user.Sex,
//                id = user.Id,
//                email = user.Email,
//                mobile = user.Mobile,
//                credit = user.Credit,
//                picture = user.Picture,
//                path = user.Path,
//                serviceLevelList = user.UserServices.Where(us => serviceIdListDistinc.Contains(us.ServiceId)).Select(
//                    us => new
//                    {
//                        serviceId = us.ServiceId,
//                        serviceLevelListId = us.ServiceLevelListId,
//                    })
//            });
//
//            //serviceLevelIdList = serviceLevelIdList.Distinct().ToList();
//            //var serviceLevelList = _ServiceLevelList.Where(s => serviceLevelIdList.Any(sId => sId == s.Id)).Select(serviceList => new
//            //{
//            //    id = serviceList.Id,
//            //    percent = serviceList.PercentServiceLevel,
//            //    title = serviceList.ServiceLevels.Title,
//            //});
//
//
//            var serviceLevelIdList = new List<int>();
//            userList.ForEach(u => u.serviceLevelList.ForEach(usl => serviceLevelIdList.Add(usl.serviceLevelListId)));
//            serviceLevelIdList = serviceLevelIdList.Distinct().ToList();
//
//            var serviceLevelList = _ServiceLevelList.Where(sl => serviceLevelIdList.Contains(sl.Id)).Select(
//                serviceList =>
//                    new
//                    {
//                        id = serviceList.Id,
//                        percent = serviceList.PercentServiceLevel,
//                        title = serviceList.ServiceLevels.Title,
//                    });
//
//            var costTimeList = _BuyServiceCostTime.Where(ct => buyServiceIdList.Contains(ct.buyServiceId));
//
//            return Ok(new
//            {
//                result = "done", buyServiceList = list, count, workUnitList, userList, userType, serviceIdList,
//                serviceLevelList, costTimeList
//            });
//        }
    }
}