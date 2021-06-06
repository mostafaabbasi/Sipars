using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Fasterflect;
using iTextSharp.text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using PrivateTraining.DomainClasses.Entities.PrivateTraining;
using PrivateTraining.DomainClasses.EntitiesView.PrivateTrainig;
using PrivateTraining.DomainClasses.StaticMethods;
using PrivateTraining.ServiceLayer.BLL;
using PrivateTraining.ServiceLocationLayer.Interface.PrivateTraining;

namespace PrivateTraining.Utils
{
    public static class TimerBuyService
    {
        public class TimerModel
        {
            public bool enable { get; set; } = true;
            public Timer timer { get; set; }
            public List<ServiceReceiverServiceLocation> serviceReceiverList { get; set; }
        }

        public static Dictionary<int, TimerModel> BuyTimers = new Dictionary<int, TimerModel>();

        public static void AddTimer(int buyServiceId, List<ServiceReceiverServiceLocation> serviceReceiverList,
            Func<IEnumerable<SP_ListServiceProviderBySL>> GetProviderListFunction,
            Func<IEnumerable<SP_ListServiceProviderBySL>, List<ServiceReceiverServiceLocation>, Task>
                AddProviderListFunction)
        {
            var startDateTime = DateTime.Now;
            var timer = new System.Threading.Timer(
                e =>
                {
                    var timerModel = BuyTimers[buyServiceId];
                    if (timerModel == null) return;

                    if (!timerModel.enable)
                    {
                        timerModel.enable = false;
                        timerModel.timer.Dispose();
                        return;
                    }

                    //1 check status not accepted or ...
                    //TODO
                    var acceptService = serviceReceiverList.Any(serviceReceiver => serviceReceiver.Status == 1);
                    if (acceptService)
                    {
                        timerModel.enable = false;
                        timerModel.timer.Dispose();
                        return;
                    }

                    //2 get providers top not selected
                    //3 repeat


                    //find next providers


//              var ListUser = _ServiceProviderInfo.ListServiceLocation((int) serviceId, (int) locationId);
//            //TODO Check sort by STAR...
//            ListUser.Sort((p1, p2) => p1.Star > p2.Star ? 1 : (p1.Star < p2.Star ? -1 : 0));
//
//            //filter
//            var max5List = ListUser.Where(p =>
//            {
//                if (providerSex != null && p.Sex != providerSex) return false;
//                if (serviceLevelListId != null && p.ServiceLevelListId != serviceLevelListId) return false;
//                return true;
//            }).Take(5);
//
//            if (!max5List.Any())
//            {
//                //no provider by this filters...
//                return Ok(new {result = "error", message = "خدمتیاری برای این سفارش پیدا نشد!"});
//            }

                    var providerList = GetProviderListFunction();
                    providerList = providerList.Where(p =>
                            timerModel.serviceReceiverList.All(serviceReceiver =>
                                serviceReceiver.ServiceProviderId != p.Id))
                        .Take(5);


                    if (providerList.Any())
                    {
                        AddProviderListFunction(providerList, serviceReceiverList);
                        return;
                    }

                    //if not exit wait 1 hour and alarm to manager after 30min

                    AddProviderNotFoundTimer(buyServiceId, serviceReceiverList, startDateTime);
                    timerModel.enable = false;
                    timerModel.timer.Dispose();
                },
                null,
                TimeSpan.FromMinutes(1),
                TimeSpan.FromMinutes(1));

            BuyTimers[buyServiceId] = new TimerModel
            {
                timer = timer, serviceReceiverList = serviceReceiverList
            };
        }


        public static Dictionary<int, Timer> AlarmTimers = new Dictionary<int, Timer>();

        public static void AddProviderNotFoundTimer(int buyServiceId,
            List<ServiceReceiverServiceLocation> serviceReceiverList, DateTime startDate)
        {
            var count = 0;

            var timer = new System.Threading.Timer(
                e =>
                {
                    var AlarmTimer = AlarmTimers[buyServiceId];
                    if (AlarmTimer == null) return;

                    var msg = "";

                    if (count > 1)
                    {
                        AlarmTimer.Dispose();
                        AlarmTimers[buyServiceId] = null;
                        return;
                    }

                    //if not exit wait 1 hour and alarm to manager after 30min
                    if (count == 0)
                    {
                        //TODO
                        var elapsedTime = new DateTime(DateTime.Now.Subtract(startDate).Ticks).ToString("HH:mm:ss");
                        //alarm

                        msg = "هشدار! پس از گذشت زمان " + elapsedTime + " و ارسال برای " +
                              serviceReceiverList.Count + "نفر موافقتی دریافت نشد!";
                        Telegram.SendTelegramLogMessage(msg);

                        count = 1;
                        return;
                    }

                    var elapsedTimeFinish = new DateTime(DateTime.Now.Subtract(startDate).Ticks).ToString("HH:mm:ss");

                    //TODO
                    //Delete last buy
                    //alarm to manager
                    //alarm to customer
                    msg = "هشدار! پس از گذشت زمان " + elapsedTimeFinish + " و ارسال برای " +
                          serviceReceiverList.Count + "نفر موافقتی دریافت نشد و درخواست لغو شد!";
                    Telegram.SendTelegramLogMessage(msg);
                    AlarmTimers[buyServiceId] = null;
                    AlarmTimer.Dispose();
                },
                null,
                TimeSpan.FromMinutes(1),
                TimeSpan.FromMinutes(1));

            AlarmTimers[buyServiceId] = timer;
        }
    }
}