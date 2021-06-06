using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using Fasterflect;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using PrivateTraining.DataLayer.Context;
using PrivateTraining.DomainClasses.Security;
using PrivateTraining.IocConfig;
using PrivateTraining.ServiceLocationLayer.Interface.PrivateTraining;
using WebPush;
using WebPush.Util;

namespace PrivateTraining.Utils
{
    public static class ApiUtils
    {
        public static async Task<bool> SendPush(int userId, JObject payload)
        {
            var _uow = SmObjectFactory.Container.GetInstance<IUnitOfWork>();
            var user = _uow.Set<ApplicationUser>().FirstOrDefault(a => a.Id == userId);
            
            if (user == null) return false;
            if (string.IsNullOrEmpty(user.Subscription))  return false;
            var suc = await SendPush(user.Subscription, payload.ToString(Formatting.None));
            if (!suc)
            {
                //user.Subscription = null;
                //_uow.SaveAllChangesAsync();
            }

            return suc;
        }

        public static async Task<bool> SendPush(string userSubscription, string payload)
        {
            var subscriptionJson = JObject.Parse(userSubscription);

            var pushEndpoint = subscriptionJson["endpoint"].Value<string>();

            var p256dh = subscriptionJson["keys"]["p256dh"].Value<string>();
            var auth = subscriptionJson["keys"]["auth"].Value<string>();

            var subject = @"mailto:seyedali.farjad@gmail.com";
            var publicKey = @"BNkHnP2i4eiINK_Rje0dsMZX5wOd3WT51hziIyA-lmtf8vnmuJadPgcIYWlVB9G1cG5OMKa0hjZnS6dftPLQ1iE";
            var privateKey = @"R_BFNQObsTcCqD8uvoYWuSm_yF866j34gEm0vHHUmHs";

            var vapidDetails = new VapidDetails(subject, publicKey, privateKey);

            var subscription = new PushSubscription(pushEndpoint, p256dh, auth);

            var options = new Dictionary<string, object>();
            options["vapidDetails"] = new VapidDetails(subject, publicKey, privateKey);

            try
            {
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, subscription.Endpoint);
                int num = 2419200;
                httpRequestMessage.Headers.Add("TTL", num.ToString());
                httpRequestMessage.Headers.Add("vapidDetails", options["vapidDetails"].ToString());

                var str2 = "";

                EncryptionResult encryptionResult = Encryptor.Encrypt(subscription.P256DH, subscription.Auth, payload);
                httpRequestMessage.Content = new ByteArrayContent(encryptionResult.Payload);
                httpRequestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                httpRequestMessage.Content.Headers.ContentLength = encryptionResult.Payload.Length;
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

                return httpResponseMessage.StatusCode == HttpStatusCode.Created;
            }
            catch (Exception exception)
            {
            }

            return false;
        }

        public static int GetServiceLocationId(int serviceId, int locationId)
        {
            IServiceLocation _ServiceLocation = IocConfig.SmObjectFactory.Container.GetInstance<IServiceLocation>();
            int? serviceLocationId = _ServiceLocation.GetAllServiceLocation().Result
                .FirstOrDefault(serviceLocation => serviceLocation.ServiceId == serviceId &&
                                                   serviceLocation.LocationId == locationId && serviceLocation.IsEnable)
                ?.Id;
            return serviceLocationId ?? 0;
        }

        public class PropertyRenameAndIgnoreSerializerContractResolver : DefaultContractResolver
        {
            protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
            {
                var prop = base.CreateProperty(member, memberSerialization);
                if (member.DeclaringType != typeof(ICollection) && !member.IsInvokable() &&
                    !member.ReflectedType.IsGenericType)
                {
                    return prop;
                }

                prop.Ignored = true;
                return prop;
            }
        }

        public static string GetJsonOutput(object output)
        {
            var a = new PropertyRenameAndIgnoreSerializerContractResolver();
            return JsonConvert.SerializeObject(output, Formatting.Indented,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    PreserveReferencesHandling = PreserveReferencesHandling.Arrays,
                    ContractResolver = a
                });
        }
    }
}