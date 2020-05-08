namespace Help
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text.RegularExpressions;

    /// <summary>
    /// 类型之间的转换,如:字符串与数字之间的转换
    /// </summary>
    public sealed class TypeParse
    {
        /// <summary>
        /// 类型相互转换，属性保留还是删除
        /// </summary>
        public enum PropertyConvertType
        {
            /// <summary>
            /// 删除
            /// </summary>
            Delete = 1,
            /// <summary>
            /// 保留
            /// </summary>
            Retain = 2
        }

        private TypeParse()
        {
        }

        /// <summary>
        /// long型安全转换为int型
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static int SafeLongToInt32(long expression)
        {
            if (expression > 0x7fffffffL)
            {
                return 0x7fffffff;
            }
            if (expression < -2147483648L)
            {
                return -2147483648;
            }
            return (int)expression;
        }

        /// <summary>
        ///  object型转换为bool型
        /// </summary>
        /// <param name="expression">要转换的字符串 true OR false</param>
        /// <param name="defValue">缺省值</param>
        /// <returns></returns>
        public static bool StrToBool(object expression, bool defValue)
        {
            if (expression != null)
            {
                if (string.Compare(expression.ToString(), "true", true) == 0)
                {
                    return true;
                }
                if (string.Compare(expression.ToString(), "false", true) == 0)
                {
                    return false;
                }
            }
            return defValue;
        }

        /// <summary>
        /// string型转换为float型
        /// </summary>
        /// <param name="expression">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns></returns>
        public static float StrToFloat(object expression, float defValue)
        {
            if ((expression == null) || (expression.ToString().Length > 10))
            {
                return defValue;
            }
            float num = defValue;
            if ((expression != null) && Regex.IsMatch(expression.ToString(), @"^([-]|[0-9])[0-9]*(\.\w*)?$"))
            {
                num = Convert.ToSingle(expression);
            }
            return num;
        }

        /// <summary>
        /// string型转换为int型
        /// </summary>
        /// <param name="expression">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns></returns>
        public static int StrToInt(object expression, int defValue)
        {
            if (expression == null)
            {
                return defValue;
            }
            string input = expression.ToString();
            if (!(((input.Length > 0) && (input.Length <= 11)) && Regex.IsMatch(input, "^[-]?[0-9]*$")))
            {
                return defValue;
            }
            if (((input.Length >= 10) && ((input.Length != 10) || (input[0] != '1'))) && (((input.Length != 11) || (input[0] != '-')) || (input[1] != '1')))
            {
                return defValue;
            }
            return Convert.ToInt32(input);
        }

        ///<summary> 
        /// 序列化 
        /// </summary> 
        /// <param name="data">要序列化的对象</param> 
        /// <returns>返回存放序列化后的数据缓冲区</returns> 
        public static byte[] Serialize(object data)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream rems = new MemoryStream();
            formatter.Serialize(rems, data);
            return rems.GetBuffer();
        }

        /// <summary> 
        /// 反序列化 
        /// </summary> 
        /// <param name="data">数据缓冲区</param> 
        /// <returns>对象</returns> 
        public static object Deserialize(byte[] data)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream rems = new MemoryStream(data);
            data = null;
            return formatter.Deserialize(rems);
        }

        /// <summary>
        /// 深度Copy对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public static T CreateDeepCopy<T>(T item)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            formatter.Serialize(stream, item);
            stream.Seek(0, SeekOrigin.Begin);
            T result = (T)formatter.Deserialize(stream);
            stream.Close();
            return result;
        }

        /// <summary>
        /// 将中文的数字转换为阿拉伯数字
        /// </summary>
        /// <param name="chineseNumber"></param>
        /// <returns></returns>
        public static int ChineseNumberToArabicNumeral(string chineseNumber)
        {
            switch (chineseNumber)
            {
                case "一":
                    return 1;
                case "二":
                    return 2;
                case "三":
                    return 3;
                case "四":
                    return 4;
                case "五":
                    return 5;
                case "六":
                    return 6;
                case "七":
                    return 7;
                case "八":
                    return 8;
                case "九":
                    return 9;
                case "十":
                    return 10;
                case "十一":
                    return 11;
                case "十二":
                    return 12;
                case "十三":
                    return 13;
                case "十四":
                    return 14;
                case "十五":
                    return 15;
                case "十六":
                    return 16;
                case "十七":
                    return 17;
                case "十八":
                    return 18;
                case "十九":
                    return 19;
                case "二十":
                    return 20;
                case "二十一":
                    return 21;
                case "二十二":
                    return 22;
                case "二十三":
                    return 23;
                case "二十四":
                    return 24;
                case "二十五":
                    return 25;
                case "二十六":
                    return 26;
                case "二十七":
                    return 27;
                case "二十八":
                    return 28;
                case "二十九":
                    return 29;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// 将对象转化为字典集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Dictionary<string, object> ObjectToDictionary<T>(T t)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            PropertyInfo[] pps = t.GetType().GetProperties();
            foreach (PropertyInfo item in pps)
            {
                dic.Add(item.Name, item.GetValue(t, null));
            }
            return dic;
        }

        /// <summary>
        /// 将对象转化为字典集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="arrProperty"></param>
        /// <param name="retain">1:删除属性 2:保留字段</param>
        /// <returns></returns>
        public static Dictionary<string, object> ObjectToDictionary<T>(T t, string[] arrProperty, PropertyConvertType convertType = PropertyConvertType.Delete)
        {
            if (convertType == PropertyConvertType.Retain && (arrProperty == null || arrProperty.Length == 0))
                return null;

            Dictionary<string, object> dic = new Dictionary<string, object>();
            PropertyInfo[] pps = t.GetType().GetProperties();
            foreach (PropertyInfo item in pps)
            {
                if (convertType == PropertyConvertType.Retain && arrProperty.Contains(item.Name))
                {
                    dic.Add(item.Name, item.GetValue(t, null));
                }
                else if (convertType == PropertyConvertType.Delete && (arrProperty != null && !arrProperty.Contains(item.Name)))
                {
                    dic.Add(item.Name, item.GetValue(t, null));
                }
            }
            return dic;
        }

        /// <summary>
        /// 对象转化为字典集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<Dictionary<string, object>> ObjectToDictionaryList<T>(List<T> list)
        {
            if (list == null)
                return null;
            List<Dictionary<string, object>> listDic = new List<Dictionary<string, object>>();
            foreach (var item in list)
            {
                PropertyInfo[] pps = list[0].GetType().GetProperties();
                Dictionary<string, object> dic = new Dictionary<string, object>();
                foreach (PropertyInfo item2 in pps)
                {
                    dic.Add(item2.Name, item2.GetValue(item, null));
                }
            }
            return listDic;
        }

        /// <summary>
        /// 对象转化为字典集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="arrProperty"></param>
        /// <param name="retain"></param>
        /// <returns></returns>
        public static List<Dictionary<string, object>> ObjectToDictionaryList<T>(List<T> list, string[] arrProperty, PropertyConvertType convertType = PropertyConvertType.Delete)
        {
            if (list == null)
                return null;

            List<Dictionary<string, object>> listDic = new List<Dictionary<string, object>>();
            PropertyInfo[] pps = list[0].GetType().GetProperties();
            foreach (var item in list)
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                foreach (PropertyInfo item2 in pps)
                {
                    if (convertType == PropertyConvertType.Retain && arrProperty.Contains(item2.Name))
                    {
                        dic.Add(item2.Name, item2.GetValue(item, null));
                    }
                    else if (convertType == PropertyConvertType.Delete && (arrProperty != null && !arrProperty.Contains(item2.Name)))
                    {
                        dic.Add(item2.Name, item2.GetValue(item, null));
                    }
                }
                listDic.Add(dic);
            }
            return listDic;
        }

        /// <summary>
        /// 实体映射
        /// </summary>
        /// <typeparam name="S">赋值对象</typeparam>
        /// <typeparam name="T">被赋值对象</typeparam>
        /// <param name="s"></param>
        /// <param name="t"></param>
        public static void AutoMapping<S, T>(S s, T t)
        {
            PropertyInfo[] pps = s.GetType().GetProperties();
            Type target = t.GetType();
            foreach (var pp in pps)
            {
                PropertyInfo targetPP = target.GetProperty(pp.Name);
                object value = pp.GetValue(s, null);
                if (targetPP != null && value != null)
                {
                    targetPP.SetValue(t, value, null);
                }
            }
        }
    }
}

