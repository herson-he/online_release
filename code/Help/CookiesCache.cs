using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Text.Json;

namespace Help
{
    /// <summary>
    /// 实现了数据的Cookies存储方式
    /// </summary>
    public class CookiesCache
    {
        /// <summary>
        /// 保存数据到cookies
        /// </summary>
        /// <param name="dic"></param>
        public static void Set(string key, Dictionary<string, object> value, int? timeout)
        {
            CookieOptions options = new CookieOptions();
            if (options != null)
                options.Expires = new DateTimeOffset(DateTime.Now.AddSeconds(Convert.ToInt32(timeout)));
            HttpContext.Current.Response.Cookies.Delete(key);
            HttpContext.Current.Response.Cookies.Append(key, JsonSerializer.Serialize(value), options);
        }

        /// <summary>
        /// 保存数据到cookies
        /// </summary>
        /// <param name="dic"></param>
        public static void Set(string key, string value, int? timeout)
        {
            HttpContext.Current.Response.Cookies.Delete(key);
            CookieOptions options = new CookieOptions();
            if (options != null)
                options.Expires = new DateTimeOffset(DateTime.Now.AddSeconds(Convert.ToInt32(timeout)));
            HttpContext.Current.Response.Cookies.Delete(key);
            HttpContext.Current.Response.Cookies.Append(key, value, options);
        }

        /// <summary>
        /// 获取cookie值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetString(string key)
        {
            if (!HttpContext.Current.Request.Cookies.ContainsKey(key))
            {
                return "";
            }
            return HttpContext.Current.Request.Cookies[key];
        }

        /// <summary>
        /// 获取cookie值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Get<T>(string key) where T : class
        {
            if (!HttpContext.Current.Request.Cookies.ContainsKey(key))
            {
                return null;
            }
            string cookie = HttpContext.Current.Request.Cookies[key];
            if (string.IsNullOrEmpty(cookie))
            {
                return null;
            }
            return JsonSerializer.Deserialize<T>(cookie);
        }
    }
}
