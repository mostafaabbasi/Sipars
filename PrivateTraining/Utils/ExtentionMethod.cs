using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using Fasterflect;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PrivateTraining.Utils
{
    public static class ExtentionMethod
    {

        public static bool SuccessResult(this JToken token)
        {
            return token["result"].Value<string>() == "done";
        }



        public static T GetResult<T>(this Task<T> task)
        {
            task.Wait();
            return task.Result;
        }

        public static string ToString2(this JToken token)
        {
            var ret = token.ToString(Formatting.None);
            return ret == "[]" ? "" : ret;
        }

//        public static string GetJson(this SqlParameter[] list, string fileUrl = "", string fileId = "")
//        {
//            string GetName(string name)
//            {
//                name = name.Replace("@Param_", "");
//                name = char.ToLowerInvariant(name[0]) + name.Substring(1);
//                name = name.Replace("ID", "Id");
//                name = name.Replace("URL", "Url");
//                return name;
//            }
//
//
//            string GetValue(SqlParameter param)
//            {
//                string value;
//                if (param.Value is bool b)
//                {
//                    value = b ? "1" : "0";
//                }
//                else
//                {
//                    value = param.Value?.ToString();
//                    if (param.Value is string || string.IsNullOrEmpty(value))
//                    {
//                        if (value != null && (value.StartsWith("[") || value.StartsWith("{")))
//                        {
//                            try
//                            {
//                                value = JToken.Parse(value).ToString(Formatting.None);
//                            }
//                            catch (Exception)
//                            {
//                                value = "\"" + HttpUtility.JavaScriptStringEncode(value) + "\"";
//                            }
//                        }
//                        else
//                        {
//                            value = "\"" + HttpUtility.JavaScriptStringEncode(value ?? "") + "\"";
//                        }
//                    }
//                }
//
//
//                return value + ",";
//            }
//
//            var json = "{\"result\":";
//            bool insertItem = false;
//            bool toJson = true;
//            var output = new JObject();
//            var item = new JObject();
//
//            for (var i = 0; i < list.Length; i++)
//            {
//                if (list[i].Direction == ParameterDirection.Output || list[i].Direction == ParameterDirection.InputOutput)
//                {
//                    if (list[i].ParameterName == "@Param_Result")
//                    {
//                        json += GetValue(list[i]);
//                        output.Add("result", new JValue(list[i].Value));
//                    }
//                    else
//                    {
//                        var propName = GetName(list[i].ParameterName);
//                        var propValue = GetValue(list[i]);
//                        if (toJson)
//                        {
//                            if (!insertItem)
//                            {
//                                json += "\"item\":{";
//                            }
//
//                            json += "\"" + propName + "\":" + propValue;
//                            insertItem = true;
//                        }
//
//                        item.Add(propName, new JValue(list[i].Value));
//                    }
//                }
//                else
//                {
//                    toJson = false;
//                }
//            }
//
//            json = json.TrimEnd(',');
//
//            if (insertItem)
//            {
//                if (!string.IsNullOrEmpty(fileId))
//                {
//                    json += ",\"fileId\":\"" + fileId + "\"";
//                }
//
//                if (!string.IsNullOrEmpty(fileUrl))
//                {
//                    json += ",\"fileUrl\":\"" + fileUrl + "\"}";
//                }
//                else
//                {
//                    json += "}";
//                }
//            }
//            else
//            {
//                json += @",""item"":{";
//                if (!string.IsNullOrEmpty(fileId))
//                {
//                    json += "\"fileId\":\"" + fileId + "\",";
//                }
//
//                if (!string.IsNullOrEmpty(fileUrl))
//                {
//                    json += "\"fileUrl\":\"" + fileUrl + "\"}";
//                }
//                else
//                {
//                    json = json.TrimEnd(',');
//                    json += "}";
//                }
//            }
//
//            json += "}";
//            if (HttpContext.Current != null)
//            {
//                if (item.Count > 0)
//                {
//                    output.Add("item", item);
//                }
//
//                HttpContext.Current.Items["output"] = output;
//            }
//
//            return json;
//        }

        public static JObject GetJObject(this DataTable dt)
        {
            return JObject.FromObject(dt);
        }

//        public static string GetJson(this DataTable dt)
//        {
//            string GetName(string name)
//            {
//                name = char.ToLowerInvariant(name[0]) + name.Substring(1);
//                name = name.Replace("ID", "Id");
//                name = name.Replace("URL", "Url");
//                return "\"" + name + "\"";
//            }
//
//            string GetValue(object rowValue)
//            {
//                string value;
//                if (rowValue is bool b)
//                {
//                    value = b ? "1" : "0";
//                }
//                else
//                {
//                    value = rowValue?.ToString();
//                    if (rowValue is string || string.IsNullOrEmpty(value))
//                    {
//                        if (value != null && (value.StartsWith("[") || !value.StartsWith("{")))
//                        {
//                            try
//                            {
//                                value = JToken.Parse(value).ToString(Formatting.None);
//                            }
//                            catch (Exception)
//                            {
//                                value = "\"" + HttpUtility.JavaScriptStringEncode(value) + "\"";
//                            }
//                        }
//                        else
//                        {
//                            value = "\"" + HttpUtility.JavaScriptStringEncode(value ?? "") + "\"";
//                        }
//                    }
//                }
//
//                return value + ",";
//            }
//
//            var json = "[";
//            var columns = dt.Columns;
//            string[] names = new string[columns.Count];
//            for (var i = 0; i < columns.Count; i++)
//            {
//                names[i] = GetName(columns[i].ColumnName);
//            }
//
//            foreach (DataRow dtRow in dt.Rows)
//            {
//                json += "{";
//                for (var i = 0; i < names.Length; i++)
//                {
//                    json += names[i] + ":" + GetValue(dtRow[i]);
//                }
//
//                json = json.TrimEnd(',');
//                json += "},";
//            }
//
//            json = json.TrimEnd(',');
//            json += "]";
//            return json;
//        }
//

        public static long GetTimeInSecond()
        {
            return (long) (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
        }

        public static object GetCurrentTimes()
        {
            var d = DateTime.Now;
            var pc = new PersianCalendar();
            return new
            {
                date = $"{pc.GetYear(d)}/{pc.GetMonth(d):00}/{pc.GetDayOfMonth(d):00}",
                time = d.ToString("HH:mm:ss")
            };
        }

        public static object GetLocalDateTimes(this DateTime time)
        {
            var pc = new PersianCalendar();
            return new
            {
                date = $"{pc.GetYear(time)}/{pc.GetMonth(time):00}/{pc.GetDayOfMonth(time):00}",
                time = time.ToString("HH:mm:ss")
            };
        }
        
        public static dynamic GetValue(this object obj, string property)
        {
            return obj.GetPropertyValue(property);
        }

        public static async Task<JObject> CallRequest(this HttpRequestMessage request)
        {
            var result = await new HttpClient().SendAsync(request);
            return result.StatusCode == HttpStatusCode.OK ? JObject.Parse(await result.Content.ReadAsStringAsync()) : null;
        }

        public static string ToHashCode(this object id)
        {
            return ("#@!" + id + "!@#").GetHashCode() + "";
        }
//
//        public static string ToEnglishDigits(this string persianStr)
//        {
//            Dictionary<char, char> LettersDictionary = new Dictionary<char, char>
//            {
//                ['۰'] = '0',
//                ['۱'] = '1',
//                ['۲'] = '2',
//                ['۳'] = '3',
//                ['۴'] = '4',
//                ['۵'] = '5',
//                ['۶'] = '6',
//                ['۷'] = '7',
//                ['۸'] = '8',
//                ['۹'] = '9'
//            };
//            foreach (var item in persianStr)
//            {
//                if (LettersDictionary.TryGetValue(item, out char s))
//                {
//                    persianStr = persianStr.Replace(item, LettersDictionary[item]);
//                }
//            }
//            return persianStr;
//        }

//        public static string ToPascalCaseJson(this string json)
//        {
//            var settings = new JsonSerializerSettings()
//            {
//                ContractResolver = new CamelCasePropertyNamesContractResolver(),
//                Converters = new List<JsonConverter> {new CamelCaseToPascalCaseExpandoObjectConverter()}
//            };
//
//            dynamic deserialized = JsonConvert.DeserializeObject<ExpandoObject>(json, settings);
//
//            return JsonConvert.SerializeObject(deserialized);
//        }

//        public static string ToPascalCaseJson(this JArray jsonArray)
//        {
//            var temp = jsonArray.Aggregate("[",
//                (current, jToken) => current + (jToken.ToString(Formatting.None).ToPascalCaseJson() + ","));
//            temp = temp.TrimEnd(',');
//            temp += "]";
//            return temp;
//        }

        public static string ToPascalCase(this string s)
        {
            if (string.IsNullOrEmpty(s) || !char.IsLower(s[0]))
                return s;

            string str = char.ToUpper(s[0], CultureInfo.InvariantCulture)
                .ToString((IFormatProvider) CultureInfo.InvariantCulture);

            if (s.Length > 1)
                str = str + s.Substring(1);

            return str;
        }
//
//        public static string GetFileUrl(this string file)
//        {
//            if (!string.IsNullOrEmpty(file))
//                return file.Replace("~/", Constants.ServerDomainName);
//            return "";
//        }
//
//        public static string GetFileUrl(this string file, FileUploadType fileUploadType)
//        {
//            if (!string.IsNullOrEmpty(file))
//                return file.Replace("~/", Constants.ServerDomainName);
//
//            var defaultFile = Constants.AvatarFileDir.Replace("~/", Constants.ServerDomainName);
//            switch (fileUploadType)
//            {
//                case FileUploadType.UserProfile:
//                    defaultFile += "profile.jpg";
//                    break;
//                case FileUploadType.ProductImage:
//                    defaultFile += "productImage.jpg";
//                    break;
//                case FileUploadType.TaskDocument:
//                    defaultFile += "image-not-available.png";
//                    break;
//                default:
//                    throw new ArgumentOutOfRangeException(nameof(fileUploadType), fileUploadType, null);
//            }
//
//            return defaultFile;
            //MemoryStream memory = new MemoryStream();
            //byte[] imageBytes = null;
            //string image = Constants.AvatarFileDir.Replace("~/", Constants.ServerDomainName) + "image-not-available.png";
            //if (ImageFileId != -1)
            //{
            //    try
            //    {
            //        fileDir = fileDir.Replace("~/", Constants.ServerDomainName);
            //        string _personId = Encrypter.Encrypt(PersonId.ToString(), Encrypter.EncodeDecodeType.FileFolder, true);
            //        string _imageFileId = Encrypter.Encrypt(ImageFileId.ToString(), Encrypter.EncodeDecodeType.FileFolder, true);
            //        string fileName = fileDir + _personId + "/" + _imageFileId;
            //        string ex = ".jpg";
            //        //Image image = Image.FromFile(HttpContext.Current.Server.MapPath(personFileDir + _personId + @"\" + _imageFileId));
            //        //image.Save(memory, image.RawFormat);
            //        ////imageBytes = memory.ToArray();
            //        ////string imageFormat = "";
            //        //if (image.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Jpeg))
            //        //{
            //        //    //imageFormat = "data:image/jpeg;base64,";
            //        //    ex = ".jpeg";
            //        //}
            //        //else if (image.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Gif))
            //        //{
            //        //    //imageFormat = "data:image/gif;base64,";
            //        //    ex = ".gif";
            //        //}
            //        //else if (image.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Png))
            //        //{
            //        //    //imageFormat = "data:image/png;base64,";
            //        //    ex = ".png";
            //        //}
            //        //else
            //        //{
            //        //    //imageFormat = "data:image/jpeg;base64,";
            //        //    ex = ".jpeg";
            //        //}
            //        //fileName = fileName + ex;
            //        //image.Dispose();
            //        image = fileName + ex; //personFileDir.TrimStart('~') + Path.GetFileName(fileName);  //imageFormat + Convert.ToBase64String(imageBytes);
            //    }
            //    catch (Exception ex)
            //    {
            //    }
            //}

            //return image.ToLower();
//        }

        public static string ToString2(this object[] input, bool encrypted = true)
        {
            var output = input.Where(item => !string.IsNullOrEmpty(item.ToString())).Aggregate("",
                (current, item) => current + (item.GetType() == typeof(JObject)
                                       ? ((JObject) item).ToString(Formatting.None)
                                       : (encrypted ? item.ToString().ToDecryptID() : item)) + ",");
            output = output.TrimEnd(',');
            return output;
        }

        public static string ToStringList(this JToken input, bool isJsonArray)
        {
            var output = input.ToString(Formatting.None);
            //if (output == "[]") return "";
            return isJsonArray ? output : output.Substring(1, output.Length - 2);
        }
        public static string ToJsonString(this JToken input)
        {
            return input.ToString(Formatting.None);
        }

        public static string ToStringList(this JToken input)
        {
            return input?.Values<string>()?.Aggregate("", (current, value) => current + (value + ",")).TrimEnd(',') ??
                   "";
        }

        public static string ToString2(this List<string> input)
        {
            if (input == null || input.Count == 0)
            {
                return "";
            }

            var output = "";
            foreach (var item in input)
            {
                if (string.IsNullOrEmpty(item)) continue;
                output += item.ToDecryptID() + ",";
            }

            output = output.TrimEnd(',');
            return output;
        }

//        [Cache]
        public static long ToDecryptID(this string id)
        {
            if (string.IsNullOrEmpty(id)) return -1;
            return Convert.ToInt64(id);
//            return EncrypterAES.AES_DecryptId(id);
        }


//        [Cache]
        public static string ToEncryptID(this long id)
        {
            return id + "";
//            return EncrypterAES.AES_EncryptId(id);
        }


        //        public static string ToEncryptID(this long id)
        //        {
        //            return Encrypter.Encrypt(id, EncodeDecodeType.IdNO);
        //        }
        //        public static long ToDecryptID(this string id)
        //        {
        //            return long.Parse(Encrypter.Decrypt(id, EncodeDecodeType.IdNO));
        //        }

        //public static OuTokenDecodeViewModel DecodeToken(this InTokenDecodeViewModel token)
        //{
        //    return new Token().DecodeToken(token);
        //}

        //public static bool IsAuthenticated(this string token)
        //{
        //    if (token.DecodeToken().Status == AuthorizationStatus.NotAuthorized)
        //    {
        //        return false;
        //    }
        //    return true;
        //}
        public static string ToUrl(this string str)
        {
            return str.Replace("~/", WebConfigurationManager.AppSettings["ServerDomainName"]);
        }

        public static int ToInt(this string str)
        {
            try
            {
                return int.Parse(str);
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public static string ToJson<T>(this IEnumerable<T> input)
        {
            return JsonConvert.SerializeObject(input.ToList());
        }

        public static string ToJson(this object input)
        {
            return JsonConvert.SerializeObject(input);
        }

        public static T ParseJson<T>(this string input)
        {
            return JsonConvert.DeserializeObject<T>(input);
        }

        public static T ConvertTo<T>(this string input)
        {
            try
            {
                var converter = TypeDescriptor.GetConverter(typeof(T));
                return (T) converter.ConvertFromString(input);
            }
            catch (Exception)
            {
                return default(T);
            }
        }

//        public static IEnumerable<T> ToEnumerable<T>(this DataTable dt)
//        {
//            foreach (DataRow row in dt.Rows)
//            {
//                yield return GetItem<T>(row);
//            }
//        }

//        private static T GetItem<T>(DataRow dr)
//        {
//            Type temp = typeof(T);
//            T obj = Activator.CreateInstance<T>();
//
//            foreach (DataColumn column in dr.Table.Columns)
//            {
//                foreach (PropertyInfo pro in temp.GetProperties())
//                {
//                    if (pro.Name.EqualIncase(column.ColumnName))
//                    {
//                        Debug.WriteLine(column.ColumnName + ":" + column.DataType);
//                        pro.SetProp(obj, dr[column.ColumnName]);
//                    }
//                }
//            }
//
//            return obj;
//        }

//        public static string Upload(this HttpPostedFile file, string filePath, bool hideFile = true)
//        {
//            try
//            {
//                if (string.IsNullOrEmpty(filePath))
//                {
//                    throw new ArgumentNullException("filePath is null or empty");
//                }
//
//                filePath = HttpContext.Current.Server.MapPath(filePath);
//                if (!Directory.Exists(filePath))
//                {
//                    Directory.CreateDirectory(filePath);
//                }
//
//                var ext = Path.GetExtension(file.FileName);
//                var fileName = Guid.NewGuid() + ext;
//                var path = Path.Combine(filePath, fileName);
//                new TxFileManager().Snapshot(path);
//                file.SaveAs(path);
//                if (filePath.Contains("WorkSpace") && hideFile)
//                {
//                    var fileInfo = new FileInfo(path) {Attributes = FileAttributes.Hidden};
//                }
//
//
//                return fileName;
//            }
//            catch (Exception)
//            {
//                return "";
//            }
//        }

//        public static string Upload(this HttpPostedFile file, string filePath)
//        {
//            try
//            {
//                if (string.IsNullOrEmpty(filePath))
//                {
//                    throw new ArgumentNullException("filePath is null or empty");
//                }
//                filePath = HttpContext.Current.Server.MapPath(filePath);
//                if (!Directory.Exists(filePath))
//                {
//                    Directory.CreateDirectory(filePath);
//                }
//
//                var ext = Path.GetExtension(file.FileName);
//                var fileName = Guid.NewGuid() + ext;
//                var path = Path.Combine(filePath, fileName);
//                new TxFileManager().Snapshot(path);
//                file.SaveAs(path);
//                if (filePath.Contains("WorkSpace"))
//                {
//                    var fileInfo = new FileInfo(path) {Attributes = FileAttributes.Hidden};
//                }
//
//                return fileName;
//            }
//            catch (Exception)
//            {
//                return "";
//            }
//        }

//        public static bool RemoveFile(this string fileUrl, HttpServerUtility server = null)
//        {
//            try
//            {
//                if (fileUrl == "") return false;
//                var fileUrlMapPath = (server ?? HttpContext.Current.Server).MapPath(fileUrl);
//                if (!File.Exists(fileUrlMapPath)) return false;
//                new TxFileManager().Snapshot(fileUrlMapPath);
//                if (fileUrlMapPath != null) File.Delete(fileUrlMapPath);
//                return true;
//            }
//            catch (Exception)
//            {
//                return false;
//            }
//        }
    }
}