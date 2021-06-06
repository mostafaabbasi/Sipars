using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
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
using PrivateTraining.DomainClasses.Entities.PrivateTraining;
using PrivateTraining.DomainClasses.Entities.Security;
using PrivateTraining.DomainClasses.EntitiesView.PrivateTrainig;
using PrivateTraining.DomainClasses.StaticMethods;
using PrivateTraining.LocationLayer.Interface.PrivateTraining;
using PrivateTraining.Models;
using PrivateTraining.ServiceLayer.BLL;
using PrivateTraining.ServiceLayer.Interface;
using PrivateTraining.ServiceLayer.Interface.Framework;
using PrivateTraining.ServiceLayer.Interface.PrivateTraining;
using PrivateTraining.ServiceLayer.Interface.Security;
using PrivateTraining.ServiceLocationLayer.Interface.PrivateTraining;
using PrivateTraining.Utils;
using ApplicationUser = PrivateTraining.DomainClasses.Security.ApplicationUser;
using WebPush;
using WebPush.Util;


#pragma warning disable 1591

namespace PrivateTraining.Api
{
    /// <summary>
    /// </summary>
    public class PrivateTrainingController : BaseApiController
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
        private IDbSet<ServiceReceiverServiceLocation> _ServiceReceiverServiceLocations;
        private IDbSet<Chat> _Chat;
        private IDbSet<ServiceLocationWorkUnit> _ServiceLocationWorkUnit;
        private IDbSet<ApplicationUser> _User;
        private IDbSet<ServiceLevelList> _ServiceLevelList;
        private IDbSet<BuyService> _BuyService;

        private readonly IMessage _message;

        private IServiceReceiverServiceLocation _servicereceiveservicelocation;

        private readonly IMenu _menu;

        public PrivateTrainingController(IUnitOfWork uow, IServiceProviderInfo serviceProviderInfo,
            IServiceLocation servicelocation
            , IServiceReceiverServiceLocation servicereceiveservicelocation,
            IMessage message, IMenu menu)
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
            _ServiceReceiverServiceLocations = _uow.Set<ServiceReceiverServiceLocation>();

            _Chat = _uow.Set<Chat>();
            _ServiceLocationWorkUnit = _uow.Set<ServiceLocationWorkUnit>();
            _User = _uow.Set<ApplicationUser>();

            _ServiceLevelList = _uow.Set<ServiceLevelList>();
            _BuyService = _uow.Set<BuyService>();


            _servicelocation = servicelocation;

            _servicereceiveservicelocation = servicereceiveservicelocation;

            _message = message;

            _menu = menu;
        }

        #endregion


        public IHttpActionResult GetBuyServiceResult(IQueryable<ServiceReceiverServiceLocation> list, int count,
            string userType)
        {
            //workPriceListJson
            var workIdList = new List<string>();
            var userIdList = new List<int>();
            //var serviceLocationIdList = new List<int>();

            list.ForEach(buyService =>
            {
                userIdList.Add(buyService.ServiceProviderId);
                userIdList.Add(buyService.ServiceReceiverId);
                //serviceLevelIdList.Add(buyService.ServiceLocationId);

                if (buyService.workPriceListJson?.StartsWith("[") == true)
                    JArray.Parse(buyService.workPriceListJson)
                        .ForEach(w =>
                            workIdList.Add((w["workUnitId"]?.Value<int>() ?? -1) + "-" + buyService.ServiceLocationId));
            });

            workIdList = workIdList.Distinct().ToList();
            var workUnitList = _ServiceLocationWorkUnit
                .Where(w => workIdList.Any(WS => WS == w.WorkUnitId + "-" + w.ServiceLocationId)).Select(w => new
                {
                    title = w.WorkUnits.Title,
                    id = w.WorkUnitId,
                    price = w.PriceWorkUnit,
                });
            var serviceIdList = list.Select(s => s.ServiceLocations.Services.Id);
            var serviceIdListDistinc = serviceIdList.Distinct();

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
                serviceLevelList = user.UserServices.Where(us => serviceIdListDistinc.Contains(us.ServiceId)).Select(
                    us => new
                    {
                        serviceId = us.ServiceId,
                        serviceLevelListId = us.ServiceLevelListId,
                    })
            });

            //serviceLevelIdList = serviceLevelIdList.Distinct().ToList();
            //var serviceLevelList = _ServiceLevelList.Where(s => serviceLevelIdList.Any(sId => sId == s.Id)).Select(serviceList => new
            //{
            //    id = serviceList.Id,
            //    percent = serviceList.PercentServiceLevel,
            //    title = serviceList.ServiceLevels.Title,
            //});


            var serviceLevelIdList = new List<int>();
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

            var servicePropertyList = _ServiceProperties.Where(sp => serviceIdListDistinc.Contains(sp.Id));

            return Ok(new
            {
                result = "done", buyServiceList = list, count, workUnitList, userList, userType, serviceIdList,
                serviceLevelList, servicePropertyList
            });
        }

        //[System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> GetBuyServiceList(JObject input)
        {
            var skipNumber = input["skipNumber"]?.Value<int>() ?? 0;
            var takeNumber = input["takeNumber"]?.Value<int>() ?? 10;
            var serviceBuyStatus = input["serviceBuyStatus"]?.Value<int>() ?? -1;


            var currentUserId = GetUserId();


            if (User.IsInRole("Admin") || User.IsInRole("Administrator") || User.IsInRole("Modrator"))
            {
                var count = _ServiceReceiverServiceLocations.Count();
                var list = _ServiceReceiverServiceLocations.Skip(skipNumber).Take(takeNumber);
                return GetBuyServiceResult(list, count, "admin");
            }
            else if (User.IsInRole("ServiceProvider"))
            {
                var list = _ServiceReceiverServiceLocations.Where(c => c.ServiceProviderId == currentUserId);
                var count = list.Count();

                list = list.OrderByDescending(buyService => buyService.Id).Skip(skipNumber).Take(takeNumber);

                return GetBuyServiceResult(list, count, "provider");
            }

            var listForCustomer = _ServiceReceiverServiceLocations.Where(c => c.ServiceReceiverId == currentUserId);
            var countOfCustomerList = listForCustomer.Count();

            listForCustomer = listForCustomer.OrderByDescending(buyService => buyService.Id).Skip(skipNumber)
                .Take(takeNumber);

            return GetBuyServiceResult(listForCustomer, countOfCustomerList, "customer");

            return Ok(new {result = "done", list = listForCustomer, count = countOfCustomerList});
        }

        //[System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> GetBuyServiceList2(JObject input)
        {
            var skipNumber = input["skipNumber"]?.Value<int>() ?? 0;
            var takeNumber = input["takeNumber"]?.Value<int>() ?? 10;
            var serviceBuyStatus = input["serviceBuyStatus"]?.Value<int>() ?? -1;


            var currentUserId = GetUserId();


            if (User.IsInRole("Admin") || User.IsInRole("Administrator") || User.IsInRole("Modrator"))
            {
                var count = _ServiceReceiverServiceLocations.Count();
                var list = _ServiceReceiverServiceLocations.Skip(count - (skipNumber + takeNumber)).Take(takeNumber);
                return Ok(new {result = "done", list, count});
            }
            else if (User.IsInRole("ServiceProvider"))
            {
                var list = _ServiceReceiverServiceLocations.Where(c => c.ServiceProviderId == currentUserId);
                var count = list.Count();

                list = list.OrderBy(buyService => buyService.Id).Skip(count - (skipNumber + takeNumber))
                    .Take(takeNumber);
                return Ok(new {result = "done", list, count});
            }

            var listForCustomer = _ServiceReceiverServiceLocations.Where(c => c.ServiceReceiverId == currentUserId);
            var countOfCustomerList = listForCustomer.Count();
            listForCustomer = listForCustomer.Skip(countOfCustomerList - (skipNumber + takeNumber)).Take(takeNumber);
            return Ok(new {result = "done", list = listForCustomer, count = countOfCustomerList});
        }

        public int GetUserId()
        {
            return Convert.ToInt32(User.Identity.GetUserId() ?? "0");
        }

        public bool SendMessage(int receiverId, string text, string subject, int? sendId = null)
        {
            PersianDate PD = new PersianDate();
            var message = new Message();
            message.SenderUserId = sendId ?? GetUserId();
            message.ReciverUserId = receiverId;
            message.Desc = text;
            message.Subject = subject;
            message.Date = PD.PersianDateLow();
            message.Time = PD.CurrentTime();

            var Query = _message.AddMessage(message);
            var Status = _uow.SaveAllChanges();

            return Status == 1;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> GetChatList(JObject input)
        {
            var buyServiceId = input["buyServiceId"]?.Value<int>();
            var type = input["userType"]?.Value<string>()??"admin";

            var chatList = _Chat.Where(s => s.buyServiceId == buyServiceId);

            var count = 0;
            if (type != "admin")
            {
                var userId = GetUserId();
                 count = chatList.Count(chat => chat.senderId != userId);

                if (count != 0)
                {
                    var buyService = await _BuyService.FirstAsync(bs => bs.id == buyServiceId);
                    if (type == "provider")
                    {
                        buyService.chatReadProvider = count;
                    }
                    else
                    {
                        //customer
                        buyService.chatReadCustomer = count;
                    }

                    _uow.SaveAllChanges();
                }

            }
            
            
            return Ok(new {result = "done", chatList, count});
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> AddChat(JObject input)
        {
            var buyServiceId = input["buyServiceId"]?.Value<int>();
            var text = input["text"]?.Value<string>() ?? "";
            var type = input["type"]?.Value<string>() ?? "";

            var date = input["date"]?.Value<string>() ?? "";
            var time = input["time"]?.Value<string>() ?? "";

            var senderId = input["senderId"]?.Value<int>() ?? -1;
            var receiverId = input["receiverId"]?.Value<int>() ?? -1;

            if (buyServiceId == null)
            {
                return Ok(new {result = "error", message = "سرویس یافت نشد!"});
            }

            var chat = new Chat();
            chat.buyServiceId = (int) buyServiceId;
            chat.receiverId = receiverId;
            chat.senderId = senderId;
            chat.type = type;
            chat.text = text;
            chat.date = date;
            chat.time = time;

            _Chat.Add(chat);

            var rowAffected = await _uow.SaveAllChangesAsync();
            if (rowAffected != 1)
            {
                //error
                return Ok(new {result = "error", message = "ذخیره سازی تغییرات در پایگاه داده به خطایی برخورد!"});
            }


            Task.Run(async delegate
            {
                ApiUtils.SendPush(receiverId, new JObject
                {
                    ["type"] = "chat",
                    ["buyServiceId"] = buyServiceId,
                    ["userId"] = receiverId,
                    ["title"] ="پیام جدید",
                    ["body"] ="پیام جدید در سی پارس برای به سفارش " + buyServiceId
                });
            });
            
            return Ok(new {result = "done", chat});
        }


        //android
        public static string ExcutePushNotification(string title, string msg, string fcmToken, object data)
        {
            var serverKey =
                "AAAAQ3KWdVs:APA91bGEz0YxVAPjlt-F6UuZvNKFRjNCziZX9DTsH2C13Jqv-2Pb-UQcr9ZQeJt-x_GA7BD8pxt7Y-dwZvDbpxatsjjQ_vEP1e0jYP3uyfU9ysi0hetco1_01VYAvVkIeXLzg8TTfro_";
            var senderId = "289685271899";


            var result = "-1";

            var httpWebRequest = (HttpWebRequest) WebRequest.Create("https://fcm.googleapis.com/fcm/send");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Headers.Add(string.Format("Authorization: key={0}", serverKey));
            httpWebRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
            httpWebRequest.Method = "POST";


            var payload = new
            {
                notification = new
                {
                    title = title,
                    body = msg,
                    sound = "default"
                },

                data = new
                {
                    info = data
                },
                to = fcmToken,
                priority = "high",
                content_available = true,
            };


            var serializer = new JavaScriptSerializer();

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = serializer.Serialize(payload);
                streamWriter.Write(json);
                streamWriter.Flush();
            }

            var httpResponse = (HttpWebResponse) httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }

            return result;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> InitMenu(JObject input)
        {
            IDbSet<Menu> menu = _uow.Set<Menu>();

            return Ok(new
            {
                result = "done", menuList = menu.ToList()
            });
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> Test(JObject input)
        {
            var txt = "test";
            var currentDate =  Telegram.GetCurrentTimes();

            var name = "Log__" + currentDate[0] + "_" + currentDate[1] + ".txt";

            var path = System.Web.Hosting.HostingEnvironment.MapPath("~/Logs/" + name);

            if (File.Exists(path))
            {
                txt = await File.OpenText(path).ReadToEndAsync() + "\n________\n" + txt;
            }

            File.WriteAllText(path, txt);
            // File.CreateText(path).WriteAsync(txt);
              return Ok(new
            {
                result = "done"
            });
            
            var pushEndpoint =
                @"https://fcm.googleapis.com/fcm/send/eOSciskrf34:APA91bFjts7F8pisLmycMmv724e0IFxXtNaJ0fFZBYHiSEHY6ibCaoKdrYnrfUfbVde2XIQa8resIFlXeAH3I6Z-AEuHswlMBDBvsJ0a3ceWTMg_9DLSZpi8EyHzrnrtVuHSgGvKXbF9";
            var p256dh = @"BF-6j-JHb-AJ1MjQTPDP0CeqPf5RGUs3T7BBJHqdD-acQ9rd7vOPxiR4oyc9MYDkdxcyLiiyQusiik5MgPHijc0";
            var auth = @"rARerw0L0dggMrxsEIGNCw";

            var subject = @"mailto:example@example.com";
            var publicKey = @"BNkHnP2i4eiINK_Rje0dsMZX5wOd3WT51hziIyA-lmtf8vnmuJadPgcIYWlVB9G1cG5OMKa0hjZnS6dftPLQ1iE";
            var privateKey = @"R_BFNQObsTcCqD8uvoYWuSm_yF866j34gEm0vHHUmHs";

            var vapidDetails = new VapidDetails(subject, publicKey, privateKey);
//var gcmAPIKey = @"[your key here]";

            var subscription = new PushSubscription(pushEndpoint, p256dh, auth);

            var options = new Dictionary<string, object>();
            options["vapidDetails"] = new VapidDetails(subject, publicKey, privateKey);
//options["gcmAPIKey"] = @"[your key here]";

            //await new WebPushClient().SendNotificationAsync(subscription, "payload", vapidDetails);

            try
            {
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, subscription.Endpoint);
                int num = 2419200;
                httpRequestMessage.Headers.Add("TTL", num.ToString());
                httpRequestMessage.Headers.Add("vapidDetails", options["vapidDetails"].ToString());

                //httpRequestMessage.Content = (HttpContent) new ByteArrayContent(new byte[0]);
                //httpRequestMessage.Content.Headers.ContentLength = new long?(0L);

                var str2 = "";
                
                string payload = "Hi";
                EncryptionResult encryptionResult = Encryptor.Encrypt(subscription.P256DH, subscription.Auth, payload);
                httpRequestMessage.Content = (HttpContent) new ByteArrayContent(encryptionResult.Payload);
                httpRequestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                httpRequestMessage.Content.Headers.ContentLength = new long?((long) encryptionResult.Payload.Length);
                httpRequestMessage.Content.Headers.ContentEncoding.Add("aesgcm");
                httpRequestMessage.Headers.Add("Encryption", "salt=" + encryptionResult.Base64EncodeSalt());
                str2 = "dh=" + encryptionResult.Base64EncodePublicKey();
                
                Uri uri = new Uri(subscription.Endpoint);
                Dictionary<string, string> vapidHeaders = VapidHelper.GetVapidHeaders(
                    uri.Scheme + Uri.SchemeDelimiter + uri.Host, vapidDetails.Subject, vapidDetails.PublicKey,
                    vapidDetails.PrivateKey, -1L);
                httpRequestMessage.Headers.Add("Authorization", vapidHeaders["Authorization"]);

                
                str2 = !string.IsNullOrEmpty(str2)
                    ? str2 + ";" + vapidHeaders["Crypto-Key"]
                    : vapidHeaders["Crypto-Key"];
                httpRequestMessage.Headers.Add("Crypto-Key", str2);

                HttpResponseMessage httpResponseMessage = await new HttpClient().SendAsync(httpRequestMessage);

                return Ok(new
                {
                    result = "done", httpResponseMessage.StatusCode, httpResponseMessage.Headers,
                    content = await httpResponseMessage.Content.ReadAsStringAsync(),
                    auth = vapidHeaders["Authorization"]
                });
            }
            catch (Exception exception)
            {
                return Ok(new
                {
                    result = "done", exception.Message,
                });
            }

            var webPushClient = new WebPushClient();
            try
            {
                webPushClient.SendNotification(subscription, null, options);

                return Ok(new
                {
                    result = "done"
                });
            }
            catch (WebPushException exception)
            {
                return Ok(new
                {
                    result = "done", exception.StatusCode, exception.Message
                });
            }

            // var webPushClient = new WebPushClient();
            // try
            // {
            //     await webPushClient.SendNotificationAsync(subscription, "payload", vapidDetails);
            //     //webPushClient.SendNotification(subscription, "payload", gcmAPIKey);
            //     return Ok(new
            //     {
            //         result = "done"
            //     });
            // }
            // catch (WebPushException exception)
            // {
            //     Console.WriteLine("Http STATUS code" + exception.StatusCode);
            //     return Ok(new
            //     {
            //         result = "done", exception.StatusCode, exception.Message
            //     });
            // }
            // var users = _User.Where(u => u.Id == 1087 || u.Id == 1 || u.Id == 2 || u.NationalCode == "admin").Select(
            //     user => new
            //     {
            //         name = user.Name,
            //         family = user.Family,
            //         sex = user.Sex,
            //         id = user.Id,
            //         email = user.Email,
            //         mobile = user.Mobile,
            //         credit = user.Credit,
            //         addressJson = user.AddressJson,
            //         path = user.Path,
            //         picture = user.Picture,
            //         nationalCode = user.NationalCode,
            //         homePhone = user.HomePhone,
            //         userType = user.UserType,
            //     });


            return Ok(new
            {
                result = "done"
            });

            // var list = _ServiceLevelList.Select(s => new
            // {
            //     ServiceId = s.ServiceLevels.ServiceId,
            //     s.ServiceLevelId,
            //     s.ServicePropertiesId,
            //     UserServicesList = s.ServiceProperties.UserServices.Select(u => new
            //     {
            //         u.UserId
            //     }),
            // });

            // return Ok(new {result = "done", list});
            //var send = await Telegram.SendTelegramLogMessage("asdasd");
            //return Ok(new {result = "done", send});

            //id: 1087
            // var user = _ServiceProviderInfo.GetAllServiceProviderInfoById(1087).FirstOrDefault();
            //
            // user.WorkPhone = "09393013397";
            // user.HomePhone = "09393013397";
            // user.Mobile = "09393013397";
            // user.Email = "seyedali.farjad@gmail.com";
            // user.Resume += "test";
            // _uow.SaveAllChanges();
            //
            // return Ok(new {result = "done", user.UserName, user.ActiveCode});
        }


        public ServiceReceiverServiceLocation CreateBuyService(JObject input)
        {
            var request = HttpContext.Current.Request;
            var currentDate = new PersianDate();
            var currentUserId = Convert.ToInt32(User.Identity.GetUserId() ?? "0");


            var serviceLevelListId = input["serviceLevelListId"]?.Value<int>() ?? -1;

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


            var serviceReceiver = new ServiceReceiverServiceLocation();
            serviceReceiver.Status = 0; //pending
            serviceReceiver.IsEnable = true;
            serviceReceiver.DateRegister = currentDate.PersianDateLow();
            serviceReceiver.timeRegister = currentDate.CurrentTime();
            serviceReceiver.WhoChangeStatus = currentUserId;

            //from system
//            serviceReceiver.user = "";

            //from user
            serviceReceiver.CalcPrice = 0;
            serviceReceiver.CalcPriceReceived = 0;

            serviceReceiver.buyServiceId = Guid.NewGuid().GetHashCode();
            serviceReceiver.ServiceLocationId = serviceLocationId;
//            serviceReceiver.ServiceProviderId = serviceProviderId;
            serviceReceiver.ServiceReceiverId = currentUserId;

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


        public async Task<bool> AddBuyService(ServiceReceiverServiceLocation buyService)
        {
            await _servicereceiveservicelocation.AddServiceReceiverServiceLocation(buyService);
            var queryAffectCount = await _uow.SaveAllChangesAsync();
            if (queryAffectCount != 1) return false;

            //send message and notification

            //TODO Get Sms Text and ...

            var subject = "خدمت جدید";
            var text = "خدمت جدید دارید!";

            SendMessage(buyService.ServiceProviderId, text, subject, 1);

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

            buyService.ServiceProviderId = serviceProviderId;

            var added = await AddBuyService(buyService);

            if (!added)
            {
                return Ok(new {result = "error", message = "خطایی در ثبت سفارش رخ داد!"});
            }


            return Ok(new {result = "done"});
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> BuyService()
        {
            var request = HttpContext.Current.Request;
            var input = JObject.Parse(request.Form["input"]);

            var serviceId = input["serviceId"]?.Value<int>();
            var locationId = input["locationId"]?.Value<int>();


//          filter providers

//          providerSex
//          providerServiceLevelListId

            var providerSex = input["providerSex"]?.Value<bool?>();
            var serviceLevelListId = input["serviceLevelListId"]?.Value<int?>();

            if (serviceId == null || locationId == null)
            {
                return Ok(new {result = "error", message = "آیدی پیدا نشد!"});
            }

            var ListUser = _ServiceProviderInfo.ListServiceLocation((int) serviceId, (int) locationId);
            //TODO Check sort by STAR...
            ListUser.Sort((p1, p2) => p1.Star > p2.Star ? 1 : (p1.Star < p2.Star ? -1 : 0));

            //filter
            var max5List = ListUser.Where(p =>
            {
                if (providerSex != null && p.Sex != providerSex) return false;
                if (serviceLevelListId != null && p.ServiceLevelListId != serviceLevelListId) return false;
                return true;
            }).Take(5);

            IEnumerable<SP_ListServiceProviderBySL> GetProviderList()
            {
                var tempListUser = _ServiceProviderInfo.ListServiceLocation((int) serviceId, (int) locationId);
                //TODO Check sort by STAR...
                tempListUser.Sort((p1, p2) => p1.Star > p2.Star ? 1 : (p1.Star < p2.Star ? -1 : 0));
                var List = tempListUser.Where(p =>
                {
                    if (providerSex != null && p.Sex != providerSex) return false;
                    if (serviceLevelListId != null && p.ServiceLevelListId != serviceLevelListId) return false;
                    return true;
                });
                return List;
            }

            if (!max5List.Any())
            {
                //no provider by this filters...
                return Ok(new {result = "error", message = "خدمتیاری برای این سفارش پیدا نشد!"});
            }

            var currentDate = new PersianDate();
            var currentUserId = Convert.ToInt32(User.Identity.GetUserId() ?? "0");
//            var user = await _userManager.FindByIdAsync(currentUserId);
            if (currentUserId == 0)
            {
                return Ok(new {result = "error", message = "کاربری با این شماره یافت نشد!"});
            }


//          user Get
//          serviceLocation = "provider"; //string provider customer (none|empty)
//          workUnitId = 1; //تعرفه
//          workCount = 1; // -1 => unknown
//          userDescription = ''; 
//          attachment = ''; 

//          receiveDate = '';
//          receiveTime = '';

//          userCity = '';
//          userAddress = '';
//          userMobile = '';


//          added
            var providerServiceLocationStatus =
                input["providerServiceLocationStatus"]?.Value<string>() ?? ""; //string provider customer (none|empty)
            var workCount = input["workCount"]?.Value<int>() ?? -1; // -1 => unknown
            var userDescription = input["userDescription"]?.Value<string>() ?? ""; // -1 => unknown
            var userCityId = input["userCityId"]?.Value<int>() ?? -1; // -1 => unknown
            var userCityTitle = input["userCityTitle"]?.Value<string>() ?? ""; // -1 => unknown
            var userAddress = input["userAddress"]?.Value<string>() ?? ""; // -1 => unknown
            var userMobile = input["userMobile"]?.Value<string>() ?? ""; // -1 => unknown

            var workPriceListJson = input["workPriceList"]?.ToString(Formatting.None) ?? "[]"; // json list


            var workUnitId = input["workUnitId"]?.Value<int>() ?? -1; // -1 => unknown
            var serviceLocationId = input["serviceLocationId"]?.Value<int>() ?? -1; // -1 => unknown
//            var serviceProviderId = input["serviceProviderId"]?.Value<int>() ?? -1; // -1 => unknown

            var providerType =
                input["providerType"]
                    ?.Value<string>(); //providerSelectCustomer providerSelectSipars providerSelectProvider

            //attachment

            var attachmentFile = request.Files["attachment"];
            var attachment = ""; // attachment address (empty none)
            if (attachmentFile != null)
            {
                var dupFileCount = 0;
                var path = System.Web.Hosting.HostingEnvironment.MapPath(
                    "~/UserFiles/attachments/" + currentUserId + "__" + attachmentFile.FileName);

                while (File.Exists(path))
                {
                    path = System.Web.Hosting.HostingEnvironment.MapPath(
                        "~/UserFiles/attachments/" + serviceId + "__" + dupFileCount + "_" + attachmentFile.FileName);
                    dupFileCount++;
                }

                attachmentFile.SaveAs(path);
                attachment = path;
            }

//            var user = input["user"]?.Value<JObject>();
//            if (user == null)
//                return Ok(new {result = "error", message = "کاربر پیدا نشد!"});

//          user
//            var user = new
//            {
//                HomeAddress = param.HomeAddress,
//                UnitNumber = param.UnitNumber,
//                Sex = param.Sex,
//                HomePhone = param.HomePhone,
//                Email = param.Email,
//                HomeNumber = param.HomeNumber,
//            };

//            service.ServiceLocationId = serviceLocationId;
//            service.ServiceProviderId = item.ServiceProviderId;
//            service.ServiceReceiverId = CurentUserId;
//            service.DateRegister = PD.PersianDateLow();
//            service.Status = 0;
//            service.WorkUnitId = item.WorkUnitId;


            var serviceReceiver = new ServiceReceiverServiceLocation();
            serviceReceiver.Status = 0; //pending
            serviceReceiver.IsEnable = true;
            serviceReceiver.DateRegister = currentDate.PersianDateLow();
            serviceReceiver.WhoChangeStatus = currentUserId;

            //from system
//            serviceReceiver.user = "";

            //from user
            serviceReceiver.CalcPrice = 0;
            serviceReceiver.CalcPriceReceived = 0;

            serviceReceiver.buyServiceId = Guid.NewGuid().GetHashCode();
            serviceReceiver.ServiceLocationId = serviceLocationId;
//            serviceReceiver.ServiceProviderId = serviceProviderId;
            serviceReceiver.ServiceReceiverId = currentUserId;

            serviceReceiver.userCityId = userCityId;
            serviceReceiver.userCityTitle = userCityTitle;
            serviceReceiver.userAddress = userAddress;
            serviceReceiver.userMobile = userMobile;
            serviceReceiver.userDescription = userDescription;
            serviceReceiver.providerServiceLocationStatus = providerServiceLocationStatus;
            serviceReceiver.attachmentPath = attachment;
            serviceReceiver.providerType = providerType;
            serviceReceiver.workPriceListJson = workPriceListJson;

//            await _servicereceiveservicelocation.AddServiceReceiverServiceLocation(serviceReceiver);
//            var Status = await _uow.SaveAllChangesAsync();
//            if (Status != 1)
//            {
//                return Ok(new {result = "error", message = "خطایی سمت سرور برای سفارش شما روی داد! دوباره سعی نمایید"});
//
//            }


            var serviceReceiverList = new List<ServiceReceiverServiceLocation>();
            foreach (var provider in max5List)
            {
//                var send = Telegram
//                    .SendTelegramLogMessage("max5List.ForEach " + provider.Id + "  " + provider.Name + "  " +
//                                            currentUserId);
                var serviceReceiverCloned = serviceReceiver.DeepClone();
                serviceReceiverCloned.ServiceProviderId = provider.Id;
                await _servicereceiveservicelocation.AddServiceReceiverServiceLocation(serviceReceiverCloned);
                serviceReceiverList.Add(serviceReceiverCloned);
            }

            async Task AddProviderList(IEnumerable<SP_ListServiceProviderBySL> providerListToAdd,
                List<ServiceReceiverServiceLocation> serviceReceiverListTimer)
            {
                var send = Telegram
                    .SendTelegramLogMessage("From Timer max5List.ForEach " + providerListToAdd.Count());

                foreach (var provider in providerListToAdd)
                {
                    var serviceReceiverCloned = serviceReceiver.DeepClone();
                    serviceReceiverCloned.ServiceProviderId = provider.Id;
                    await _servicereceiveservicelocation.AddServiceReceiverServiceLocation(serviceReceiverCloned);
                    serviceReceiverList.Add(serviceReceiverCloned);
                    serviceReceiverListTimer.Add(serviceReceiverCloned);
                }

                await _uow.SaveAllChangesAsync();
            }

            var Status = await _uow.SaveAllChangesAsync();

            //TODO
//            if (Status != 1)
//            {
//                return Ok(new {result = "error", message = "خطایی سمت سرور برای سفارش شما روی داد! دوباره سعی نمایید"});
//            }

//            var id = serviceReceiver.Id;

            //TimerBuyService.AddTimer(serviceReceiver.buyServiceId, serviceReceiverList, GetProviderList,
            //    AddProviderList);


            return Ok(new {result = "done"});
        }


        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> ServiceProviderLocationTop5(JObject input)
        {
            //provider sex
            //provider serviceLevelList
            //provider star

            //TODO date time
            //TODO Address
            //TODO only in provider place

            var serviceId = input["serviceId"]?.Value<int>();
            var locationId = input["locationId"]?.Value<int>();

            var providerSex = input["providerSex"]?.Value<bool>();
            var serviceLevelList = input["serviceLevelList"]?.Value<int>();

            if (serviceId == null || locationId == null)
            {
                return Ok(new {result = "error", message = "خدمتیار پیدا نشد!"});
            }

            var ListUser = _ServiceProviderInfo.ListServiceLocation((int) serviceId, (int) locationId);
            ListUser.Sort((p1, p2) => p1.Star > p2.Star ? 1 : (p1.Star < p2.Star ? -1 : 0));
            var max5List = ListUser.Where(p => p.Sex == providerSex && p.ServiceLevelListId == serviceLevelList)
                .Take(5);
            return Ok(new {result = "done", items = max5List});
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> ServiceProviderLocation(JObject input)
        {
            var serviceId = input["serviceId"]?.Value<int>();
            var locationId = input["locationId"]?.Value<int>();

            if (serviceId == null || locationId == null)
            {
                return Ok(new {result = "error", message = "خدمتیار پیدا نشد!"});
            }

            var ListUser = _ServiceProviderInfo.ListServiceLocation((int) serviceId, (int) locationId);
            return Ok(new {result = "done", items = ListUser});
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> EditServiceTag(JObject input)
        {
            var id = input["id"]?.Value<int>();
            var tagTitle = input["tagTitle"]?.Value<string>() ?? "";
            var showTag = input["showTag"]?.Value<bool>() ?? false;

            if (id == null)
            {
                return Ok(new {result = "error", message = "سرویس پیدا نشد!"});
            }

            var service = _ServiceProperties.First(s => s.Id == id);
            if (service == null)
            {
                return Ok(new {result = "error", message = "سرویس پیدا نشد!"});
            }

            service.tagTitle = tagTitle;
            service.showTag = showTag;
            _uow.SaveAllChanges();

            return Ok(new {result = "done"});
        }

        /// <summary>
        /// ویرایش خدمت
        /// </summary>
        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> DeleteServiceImage(int id)
        {
            var service = _ServiceProperties.First(s => s.Id == id);

            if (service == null)
            {
                return Ok(new {result = "error", message = "سرویس پیدا نشد!"});
            }

            var serviceChild = _ServiceProperties.Where(s => s.image == service.image);

            var path = System.Web.Hosting.HostingEnvironment.MapPath("~/UserFiles/serviceImages/" + service.image);
            File.Delete(path);
            foreach (var ServiceProp in serviceChild)
            {
                ServiceProp.image = "";
            }

            service.image = "";
            _uow.SaveAllChanges();
            return Ok(new {result = "done"});
        }

        public async Task<IHttpActionResult> SetServiceImage()
        {
            var request = HttpContext.Current.Request;
            if (!int.TryParse(request.Form["id"], out var serviceId))
            {
                return Ok(new {result = "error", message = "سرویس پیدا نشد!"});
            }

            var imageFile = request.Files["image"];
            if (imageFile == null)
            {
                return Ok(new {result = "error", message = "فایل به درستی آپلود نشده است!"});
            }

            var path = System.Web.Hosting.HostingEnvironment.MapPath(
                "~/UserFiles/serviceImages/" + serviceId + "__" + imageFile.FileName);
            imageFile.SaveAs(path);

            var service = _ServiceProperties.First(s => s.Id == serviceId);
            service.image = serviceId + "__" + imageFile.FileName;

            if (request.Form["setForChildren"] == "1")
            {
                var allChildServiceProperties = new List<ServiceProperties>();

                void SetChildsProps(int parentId)
                {
                    var TempService = _ServiceProperties.Where(c => c.ParentId == parentId).ToList();
                    allChildServiceProperties.AddRange(TempService);
                    TempService.ForEach(s => SetChildsProps(s.Id));
                }

                SetChildsProps(serviceId);

                allChildServiceProperties.ForEach(serviceProp => { serviceProp.image = service.image; });
            }

            _uow.SaveAllChanges();

            return Ok(new {result = "done", image = service.image});
        }

        /// <summary>
        /// ویرایش خدمت
        /// </summary>
        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> ServiceProperty(int id)
        {
            var service = _ServiceProperties.First(s => s.Id == id);
            return Ok(new {result = "done", serviceProperty = JObject.FromObject(service)});
        }

        [System.Web.Http.HttpGet]
        public async Task<IHttpActionResult> MenuList()
        {
            var items = _ServiceProperties.Select(service => new
            {
                title = service.Title,
                id = service.Id,
                parentId = service.ParentId,
                level = service.Level,
                isEnable = service.IsEnable,
                image = service.image,
                tagTitle = service.tagTitle,
                serviceDescription = service.serviceDescription,
                showTag = service.showTag,
            });
            return Ok(new {result = "done", items});
        }

        /// <summary>
        /// لیست خدمت دهنده ها بر اساس خدمت و محل زمان درخواست
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IHttpActionResult> ServiceProviderList()
        {
            var ListUser = await _ServiceProviderInfo.GetAllServiceProviderInfo();
            var items = ListUser.Select(s => new
            {
                s.Id,
                s.Name,
                s.Family,
                s.Level,
                s.Resume,
                s.CityId,
                s.StarScore,
                s.Path,
                s.Sex,
                s.Picture,
                s.ServiceCode,
                s.ServiceProviderCode,
                s.BrithDay,
                s.YearBrithDay,
                s.MonthBrithDay,
            });
            return Ok(new {result = "done", items});
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IHttpActionResult> LoadServiceProviderMaxScore(JObject input)
        {
            var serviceId = input["serviceId"].Value<int?>();
            if (serviceId == null)
            {
                return Ok(new {result = "error", message = "سرویس مورد نظر یافت نشد !"});
            }

            var ServiceLocationId = input["ServiceLocationId"].Value<int?>();
            if (ServiceLocationId == null)
            {
                return Ok(new {result = "error", message = "سرویس مورد نظر یافت نشد !"});
            }

            var userServiceLocation = _UserServiceLocations.Where(a =>
                a.ServiceLocationId == ServiceLocationId && a.IsEnable &&
                a.StatusServiceLocationUser == 1);


            if (userServiceLocation.Any())
            {
                foreach (var item in userServiceLocation)
                {
                    //13960605
                    var h = 0;
                    var temp = _ServiceReceiverServiceLocations.Where(x =>
                        x.ServiceProviderId == item.UserId &&
                        (x.Status == 0 || x.Status == 1 || x.Status == 2 || x.Status == 3)).ToList();
                    if (temp.Any())
                        h = temp.Count;

                    var exitUserService = _UserService.Include("Users").Where(b =>
                        b.UserId == item.UserId && b.ServiceId == serviceId && b.CapacityServiceUser >= h &&
                        b.IsEnable && b.ActiveServiceForUser == 1);

                    if (exitUserService.Any())
                    {
                        var UserService = await exitUserService.OrderByDescending(c => c.CountSTarScoreServiceUser)
                            .Take(5).ToListAsync();

                        var ListServiceProvider = await _ServiceProviderInfo.GetAllServiceProviderInfo();
                        var items = await ListServiceProvider
                            .Where(sp => UserService.Exists(user => user.UserId == sp.Id))
                            .Select(a =>
                                new ServiceProviderInfo
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
                                }).ToListAsync();

                        return Ok(new {result = "done", items});
                    }
                }
            }

            return Ok(new
            {
                result = "error",
                Message =
                    "با عرض پوزش خدمتیار برای سفارش شما وجود ندارد. اًمید که خدمتگذار خوبی برای سفارشات بعدی شما باشیم."
            });
        }


        [HttpPost]
        [AllowAnonymous]
        public virtual async Task<IHttpActionResult> ListServiceProviderLocation(JObject input)
        {
            var serviceId = input["serviceId"].Value<int?>();
            var locationId = input["locationId"].Value<int?>();
            if (serviceId == null || locationId == null)
            {
                return Ok(new {result = "error", message = "سرویس مورد نظر یافت نشد !"});
            }

            var userServiceLocation = _UserServiceLocations.Where(a =>
                a.ServiceId == serviceId && a.LocationId == locationId && a.IsEnable &&
                a.StatusServiceLocationUser == 1);

            var ListUser = _ServiceProviderInfo.ListServiceLocation((int) serviceId, (int) locationId);
            return Ok(new
            {
                result = "done",
                ListUser,
                userServiceLocation.ToListAsync().Result,
                Message =
                    ""
            });
        }
    }
}