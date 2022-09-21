using AutoMapper;
using Common.Abstract.Configurations;
using Common.Abstract.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NodaTime;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Web;

namespace Common.Abstract.Extension
{
    public static class Methods
    {
        public static bool IsNullOrZero(this int value) => value == 0;
        public static string TokenFetch(this HttpContext httpContext)
            => Convert.ToString(httpContext.Request.Headers["token"]);
        public static string Token(this HttpContext httpContext)
       => httpContext.TokenFetch();
        public static bool ValidatePermission(this HttpContext httpContext)
        {
            string permissions = Convert.ToString(httpContext.Request.Headers["permission"]).Decode();
            if (!string.IsNullOrEmpty(permissions))
            {
                List<ModuleAccessLevel> moduleAccessLevels = JsonConvert.DeserializeObject<List<ModuleAccessLevel>>(permissions);
                var url = "/" + httpContext.Request.RouteValues["controller"]+ "/" + httpContext.Request.RouteValues["action"];   
                //if (!moduleAccessLevels.Count(x => x.Path == string.Concat("/", string.Join("/", httpContext.Request.Path.Value.Split("/").Reverse().Take(2).Reverse())).ToLower()).IsNullOrZero())
                if (!moduleAccessLevels.Count(x => x.Path.ToLower() == url.ToLower()).IsNullOrZero())
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
        public static string InvalidRequest()
        {
            return JsonConvert.SerializeObject(new Response()
            {
                StatusCode = (int)HttpStatusCode.Unauthorized,
                Message = "Invalid request params"
            });
        }
        public static IActionResult Format(this Response response, ControllerBase controllerBase)
         => controllerBase.StatusCode((int)response.StatusCode, response);
        public static string NullToEmpty(this string str) => string.IsNullOrEmpty(str) ? string.Empty : str;        
        public static int ToInt(this string value) => string.IsNullOrEmpty(value) ? 0 : Convert.ToInt32(value);
        public static LocalDate? DbLocalDate(this string date, IMapper _mapper)
        {
            try
            {
                if (!string.IsNullOrEmpty(date))
                {
                    DateTime dt = Convert.ToDateTime(date);
                    return _mapper.Map<LocalDate?>(DateTimeOffset.Parse(string.Format("{0:MM/dd/yyyy HH:mm}", dt)).DateTime);
                }
                else
                    new LocalDate();

            }
            catch (Exception)
            {

            }

            return null;
        }
        public static string EncodeQueryString(this object queryString)
        {
            return HttpUtility.UrlEncode(Convert.ToString(queryString).CompressString());
        }
        public static string DecodeQueryString(this string queryString)
        {
            if (!queryString.Contains("%"))
                return Convert.ToString(queryString).DecompressString();
            else
                return HttpUtility.UrlDecode(Convert.ToString(queryString)).DecompressString();
        }

        public static string DecompressString(this string compressedText)
        {
            if (!string.IsNullOrEmpty(compressedText))
            {
                byte[] compressedData = Convert.FromBase64String(compressedText);
                using (var ms = new MemoryStream())
                {
                    int dataLength = BitConverter.ToInt32(compressedData, 0);
                    ms.Write(compressedData, 4, compressedData.Length - 4);

                    var buffer = new byte[dataLength];

                    ms.Position = 0;
                    using (var stream = new GZipStream(ms, CompressionMode.Decompress))
                    {
                        stream.Read(buffer, 0, buffer.Length);
                    }

                    return Encoding.UTF8.GetString(buffer);
                }
            }
            else
                return compressedText;
        }
        public static string CompressString(this string text)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(text);
            var ms = new MemoryStream();
            using (var stream = new GZipStream(ms, CompressionMode.Compress, true))
                stream.Write(buffer, 0, buffer.Length);

            ms.Position = 0;

            var rawData = new byte[ms.Length];
            ms.Read(rawData, 0, rawData.Length);

            var compressedData = new byte[rawData.Length + 4];
            Buffer.BlockCopy(rawData, 0, compressedData, 4, rawData.Length);
            Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, compressedData, 0, 4);
            return Convert.ToBase64String(compressedData);
        }
        public static string Serialize<T>(this T data)
        {
            return JsonConvert.SerializeObject(data);
        }
        //public static string DbDate(this DateTime date, bool viewTimewithDate = false)
        //{
        //    if (date == DateTime.MinValue)
        //    {
        //        return String.Empty;
        //    }
        //    else if (!ReferenceEquals(date, null))
        //    {
        //        if (((DateTime)date).Hour != 0)
        //        {
        //            date = TimeZoneInfo.ConvertTime((DateTime)date, TimeZoneInfo.Utc, TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time"));
        //            // date = ((DateTime)date).AddHours(Convert.ToDouble(WebConfigurationManager.AppSettings["TimeDifference"]));

        //        }
        //    }
        //    if (viewTimewithDate)
        //        return string.Format("{0:MM/dd/yyyy HH:mm}", date);

        //    return string.Format("{0:MM/dd/yyyy}", date);
        //}
    }
}
