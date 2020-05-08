using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Help
{
    /// <summary>
    /// 对类型的方法拓展（避免造成混乱，请谨慎添加新的方法拓展）
    /// </summary>
    public static class TypeFunExten
    {
        /// <summary>
        /// 字符串转整型数组
        /// </summary>
        /// <param name="str"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static int[] ToIntArray(this string str)
        {
            return ToIntArray(str, ',');
        }

        /// <summary>
        /// 字符串转整型数组
        /// </summary>
        /// <param name="str"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static int[] ToIntArray(this string str, char separator)
        {
            if (string.IsNullOrEmpty(str))
                return new int[0];
            string[] arrStr = str.Split(separator);
            int[] arrInt = new int[arrStr.Length];
            try
            {
                for (int i = 0; i < arrStr.Length; i++)
                {
                    arrInt[i] = Convert.ToInt32(arrStr[i]);
                }
            }
            catch
            {

            }
            return arrInt;
        }

        /// <summary>
        /// 转化为自定义字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string ToCutsomString(this object[] str, char separator = ',')
        {
            string result = "";
            foreach (var item in str)
            {
                result += item + ",";
            }
            return result.TrimEnd(',');
        }

        /// <summary>
        /// 转化为自定义字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string ToCutsomString(this byte[] bytes, char separator = ',')
        {
            string result = "";
            foreach (var item in bytes)
            {
                result += item + ",";
            }
            return result.TrimEnd(',');
        }

        /// <summary>
        /// 日期转yyyy-MM-dd HH:mm:ss
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToStandardString(this DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 序列化对象
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Serialize(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T DeserializeObject<T>(this string str)
        {
            return JsonConvert.DeserializeObject<T>(str);
        }

        /// <summary>
        /// 分隔金钱
        /// </summary>
        /// <returns></returns>
        public static string SeparateMoney(this object number, string separator = ",")
        {
            if (number == null)
                return "0";
            string tmp = number.ToString();

            int digit = 3;

            // 处理小数位
            string decimalPlaces = string.Empty;
            int decimalPlacesIndex = tmp.IndexOf(".");
            if (decimalPlacesIndex > 0)
            {
                decimalPlaces = tmp.Substring(decimalPlacesIndex);
                tmp = tmp.Substring(0, decimalPlacesIndex);
            }

            // 处理负数
            string symbol = string.Empty;
            if (tmp.IndexOf("-") != -1)
            {
                symbol = "-";
                tmp = tmp.Substring(1);
            }

            int pos = tmp.Length - digit;
            while (pos > 0)
            {
                tmp = tmp.Insert(pos, separator);
                pos -= digit;
            }
            return symbol + tmp + decimalPlaces;
        }
    }
}
