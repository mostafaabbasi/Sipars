using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PrivateTraining.DomainClasses.StaticMethods
{
    public class Telegram
    {
        const string Token = "488486104:AAERmtisDM9wuV0KHBdrPx1wp95wvQKYkF4";

        public static bool TelegramSend(string text)
        {
            var chatId = "@AlienLogs";

            try
            {
                var token = "863390029:AAHAP6A3TsI9bYeyju0Ek64ExDX_jsdlmAU";


                string urlString = "http://aliens.ir/api/index.php?" + text;

                WebClient webclient = new WebClient();

                webclient.DownloadString(urlString);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static string[] GetCurrentTimes()
        {
            var d = DateTime.Now;
            var pc = new PersianCalendar();
            return new[] {$"{pc.GetYear(d)}-{pc.GetMonth(d):00}-{pc.GetDayOfMonth(d):00}", d.ToString("HH-mm-ss")};
        }


        public static void LogText(string txt)
        {
            Task.Run(async delegate
            {
                var currentDate = GetCurrentTimes();

                var name = "Log__" + currentDate[0] + "_" + currentDate[1] + "_" + DateTime.Now.Ticks + ".txt";

                var path = System.Web.Hosting.HostingEnvironment.MapPath("~/Logs/" + name);

                if (File.Exists(path))
                {
                    txt = await File.OpenText(path).ReadToEndAsync() + "\n________\n" + txt;
                }

                // File.CreateText(path).WriteAsync(txt);
                File.WriteAllText(path, txt);
            });
        }

        public static async Task<string> SendLogFromGoogleMessage(string msg)
        {
            //https://translate.google.com/translate?hl=fa&sl=en&tl=fa&u=https%3A%2F%2Fapi.telegram.org%2Fbot762543868%3AAAHE7vE6LA63RLxWYTk2pX8p4kFgSMkAvGw%2FsendMessage%3Fchat_id%3D-1001395558540%26text%3Dfromgoogle&prev=search&sandbox=1

            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
                System.Net.ServicePointManager.SecurityProtocol =
                    SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback = (snder, cert, chain, error) => true;


                msg =  "ali%250A2" + HttpUtility.UrlEncode(DateTime.Now.Ticks + "\n---" + msg);
                // var url =
                //     "https://translate.googleusercontent.com/translate_c?depth=1&hl=fa&prev=search&rurl=translate.google.com&sl=en&sp=nmt4&tl=fa&u=https://api.telegram.org/bot762543868:AAHE7vE6LA63RLxWYTk2pX8p4kFgSMkAvGw/sendMessage%3Fchat_id%3D-1001395558540%26text%3D" +
                //     msg + "&usg=ALkJrhic3HwN5eKZh7Rdcv7Dd5qZs36jqw";

                // var url =
                //     "https://translate.googleusercontent.com/translate_c?sl=en&tl=fa&u=https://api.telegram.org/bot762543868:AAHE7vE6LA63RLxWYTk2pX8p4kFgSMkAvGw/sendMessage%3Fchat_id%3D-1001395558540%26text%3D" +
                //     msg;

                var url =
                    "https://www.google.com/searchbyimage?image_url=https%3A%2F%2Fapi.telegram.org%2Fbot762543868%3AAAHE7vE6LA63RLxWYTk2pX8p4kFgSMkAvGw%2FsendMessage%3Fchat_id%3D-1001395558540%26parse_mode%3DHTML%26text%3D" +
                    msg;

                return new WebClient().DownloadString(url);

//                await new HttpClient().GetAsync(url);
                return "done";
            }
            catch (Exception e)
            {
                return e.ToString();
                // ignored
            }
        }

        public static async Task<bool> SendLogMessage(string msg)
        {
            SendLogFromGoogleMessage(msg);

            return false;
            try
            {
                return TelegramSend(msg);

                var url = "https://api.aliens.ir/log/create";
                var parameters = new Dictionary<string, string>
                {
                    {"text", msg},
//                    {"chat_id", "@AlienLogs"},
                };


                var s = new JObject();
                s["msg"] = msg;

                var content = new StringContent(s.ToString(Formatting.Indented), Encoding.UTF8, "application/json");
                await new HttpClient().PostAsync(url, content);
                return true;
            }
            catch (Exception)
            {
                // ignored
            }

            return false;
        }

        public static async Task<string> SendTelegramLogMessage(string msg)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
                System.Net.ServicePointManager.SecurityProtocol =
                    SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback = (snder, cert, chain, error) => true;


                msg = HttpUtility.UrlEncode(msg);
                var url = "https://p.aliens.ir/api.telegram.org/bot" + Token +
                          "/sendMessage?chat_id=@AlienLogs&text=" + msg;

                return new WebClient().DownloadString(url);

//                await new HttpClient().GetAsync(url);
                return "done";
            }
            catch (Exception e)
            {
                return e.ToString();
                // ignored
            }

            return "??";
        }
    }
}