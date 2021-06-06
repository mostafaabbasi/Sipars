using System;
using System.Collections;
using System.Data.Entity;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PrivateTraining.DataLayer.Context;
using PrivateTraining.DomainClasses.StaticMethods;
using PrivateTraining.IocConfig;

namespace PrivateTraining.Utils
{
    public  class Logger
    {

        public async Task<string> Test(HttpRequestMessage messageRequest, HttpRequest Request,
            HttpResponseMessage ResponseMessage, HttpResponse Response, DateTime startDateTime, string requestBody,
            string responseBody, IDictionary items)
        {
            try
            {
                IUnitOfWork _uow = SmObjectFactory.Container.GetInstance<IUnitOfWork>();
                IDbSet<DomainClasses.Entities.Log.Api> _Log = _uow.Set<DomainClasses.Entities.Log.Api>();

                //oldApiRequestTime
                var input = new JObject();
                var endTimes = ExtentionMethod.GetCurrentTimes();
                var startTimes = startDateTime.GetLocalDateTimes();

                var RequestIp = GetIPAddress(Request);
                //            var RequestUrl = Request.UrlReferrer?.AbsoluteUri;
                var RequestUrl = messageRequest.RequestUri.AbsoluteUri;
                //            var requestBody = messageRequest.Content.ReadAsStringAsync().Result;
                var Method = messageRequest.Method.Method;
                //            var responseBody = ResponseMessage.Content.ReadAsStringAsync().Result;
                var statusCode = ResponseMessage?.StatusCode ?? 0;
                var ReasonPhrase = ResponseMessage?.ReasonPhrase ?? "";

                input["endTime"] = endTimes.GetValue("time");
                input["endDate"] = endTimes.GetValue("date");
                input["startTime"] = startTimes.GetValue("time");
                input["startDate"] = startTimes.GetValue("date");
                input["senderIp"] = RequestIp;
                input["requestUrl"] = RequestUrl;
                try
                {
                    input["requestBody"] = (JObject) items["body"];
                }
                catch (Exception e)
                {
                }

                try
                {
                    input["responseBody"] = (JObject) items["output"];
                }
                catch (Exception e)
                {
                }

                try
                {
                    input["Token"] = (JObject) items["Token"];
                }
                catch (Exception e)
                {
                }

                input["TotalBytes"] = Request.TotalBytes;


                input["method"] = Method;

                input["statusCode"] = statusCode + "";
                input["reasonPhrase"] = ReasonPhrase;
                input["oldApiDurationTime"] = items["oldApiRequestTime"] + "";
                input["durationTime"] = (int) DateTime.Now.Subtract(startDateTime).TotalMilliseconds;
                input["agant"] = Request.UserAgent;

                _Log.Add(new DomainClasses.Entities.Log.Api
                {
                    text = input.ToString(Formatting.None),
                });

                _uow.SaveAllChanges();
//                var db = Mongo.GetDataBase("Logs");
////                throw new NullReferenceException();
//                if (Method == "OPTION")
//                {
//                    await db.GetCollection<BsonDocument>("optionRequest")
//                        .InsertOneAsync(BsonDocument.Parse(input.ToString(Formatting.None)));
//                }
//                else
//                {
//                    await db.GetCollection<BsonDocument>("apiRequest")
//                        .InsertOneAsync(BsonDocument.Parse(input.ToString(Formatting.None)));
//                }

//                            var a = db.GetCollection<BsonDocument>("Requests").Count(new BsonDocument());
//                var a2 = db.GetCollection<BsonDocument>("Requests").FindSync(new BsonDocument());
            }
            catch (Exception e)
            {
                Telegram.SendLogMessage("Log: \n" + e.ToString());
                return e.ToString();
            }

            return "ok";
        }


//        public async void LogExeption(Exception exception, HttpRequest request, bool isSignalR = false,
//            string Name = "Exceptions")
//        {
//            try
//            {
//                var times = ExtentionMethod.GetCurrentTimes();
//                var input = new JObject
//                {
//                    ["senderIp"] = GetIPAddress(request),
//                    ["requestUrl"] = request.Url.AbsoluteUri,
//                    ["startTime"] = times.GetValue("time"),
//                    ["startDate"] = times.GetValue("date"),
//                    //                ["method"] = Request.HttpMethod,
//                    ["exeptionMessage"] = exception.Message,
//                    ["exeptionSource"] = exception.Source,
//                    ["exeptionTrace"] = exception.StackTrace,
//                    ["isSignalr"] = isSignalR,
//                };
//
//                var logs = Mongo.GetDataBase("Logs").GetCollection<BsonDocument>(Name);
//                await logs.InsertOneAsync(BsonDocument.Parse(input.ToString(Formatting.None)));
//            }
//            catch (Exception e)
//            {
//                Console.WriteLine(e);
//            }
//        }
//
//
//        public async void LogApplicationPublish(string url, JObject application, HttpRequest request, JObject pageList,
//            JObject blockList)
//        {
//            try
//            {
//                var times = ExtentionMethod.GetCurrentTimes();
//                var input = new JObject
//                {
//                    ["senderIp"] = GetIPAddress(request),
//                    ["requestUrl"] = request.Url.AbsoluteUri,
//                    ["startTime"] = times.GetValue("time"),
//                    ["startDate"] = times.GetValue("date"),
//                    ["url"] = url,
//                    ["application"] = application.ToString(Formatting.Indented),
//                    ["pageList"] = pageList,
//                    ["blockList"] = blockList,
//                };
//
//                var logs = Mongo.GetDataBase("Logs").GetCollection<BsonDocument>("ApplicationPublish");
//                await logs.InsertOneAsync(BsonDocument.Parse(input.ToString(Formatting.None)));
//            }
//            catch (Exception e)
//            {
//                Console.WriteLine(e);
//            }
//        }


        public async void LogResponse(HttpResponseMessage ResponseMessage)
        {
            //            messageRequest.RequestUri;
        }

        public string GetIPAddress(HttpRequest Request)
        {
            try
            {
                if (Request.Headers["CF-CONNECTING-IP"] != null) return Request.Headers["CF-CONNECTING-IP"];

                if (Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
                {
                    string ipAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                    if (!string.IsNullOrEmpty(ipAddress))
                    {
                        string[] addresses = ipAddress.Split(',');
                        if (addresses.Length != 0)
                        {
                            return addresses[0];
                        }
                    }
                }

                return Request.UserHostAddress;
            }
            catch (Exception)
            {
                return "";
            }
        }

//        public async void LogTaskReport(HttpRequest request, OuTaskReportInsertViewModel response,
//            InTaskReportInsertViewModel model)
//        {
//            try
//            {
//                var times = ExtentionMethod.GetCurrentTimes();
//                var input = new JObject
//                {
//                    ["senderIp"] = GetIPAddress(request),
//                    ["requestUrl"] = request.Url.AbsoluteUri,
//                    ["startTime"] = times.GetValue("time"),
//                    ["startDate"] = times.GetValue("date"),
//                    ["response"] = JObject.FromObject(response).ToString(Formatting.Indented),
//                    ["input"] = JObject.FromObject(model).ToString(Formatting.Indented),
//                };
//
//                var logs = Mongo.GetDataBase("Logs").GetCollection<BsonDocument>("TaskReport");
//                await logs.InsertOneAsync(BsonDocument.Parse(input.ToString(Formatting.None)));
//            }
//            catch (Exception e)
//            {
//                Console.WriteLine(e);
//            }
//        }
//
//        public async void LogDailyTime(HttpRequest request, OuDailyTimeLogInsertViewModel response,
//            InDailyTimeLogInsertViewModel model)
//        {
//            try
//            {
//                var times = ExtentionMethod.GetCurrentTimes();
//                var input = new JObject
//                {
//                    ["senderIp"] = GetIPAddress(request),
//                    ["requestUrl"] = request.Url.AbsoluteUri,
//                    ["startTime"] = times.GetValue("time"),
//                    ["startDate"] = times.GetValue("date"),
//                    ["response"] = JObject.FromObject(response).ToString(Formatting.Indented),
//                    ["input"] = JObject.FromObject(model).ToString(Formatting.Indented),
//                };
//
//                var logs = Mongo.GetDataBase("Logs").GetCollection<BsonDocument>("DailyTime");
//                await logs.InsertOneAsync(BsonDocument.Parse(input.ToString(Formatting.None)));
//            }
//            catch (Exception e)
//            {
//                Console.WriteLine(e);
//            }
//        }
    }
}
