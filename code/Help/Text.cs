using System;
using System.Collections.Generic;
using System.Text;

namespace Help
{
    /// <summary>
    /// 文本帮助类
    /// </summary>
    public class Text
    {
        /// <summary>
        /// 在字符串最后添加一个字符，如果本身以这个字符结尾，则不添加
        /// </summary>
        /// <param name="originalVal"></param>
        /// <param name="lastStr"></param>
        /// <returns></returns>
        public static string AddLast(string originalVal, string lastStr)
        {
            if (originalVal.EndsWith(lastStr))
            {
                return originalVal;
            }
            return (originalVal + lastStr);
        }

        /// <summary>
        /// 隐藏身份证
        /// </summary>
        /// <param name="idCard"></param>
        /// <returns></returns>
        public static string HidenIDCard(string idCard)
        {
            if (string.IsNullOrEmpty(idCard))
                return "";

            return idCard.Substring(0, 3) + "************" + idCard.Substring(idCard.Length - 3);
        }

        /// <summary>
        /// 隐藏Mail
        /// </summary>
        /// <returns></returns>
        public static string HidenMail(string mail)
        {
            if (string.IsNullOrEmpty(mail))
                return "";

            // 截取后缀
            string suffix = mail.Substring(mail.LastIndexOf('.'));

            // 截取前面三个字符
            string start = mail.Substring(0, 3);

            return start + "****@**" + suffix;
        }

        /// <summary>
        /// 隐藏企业组织代码
        /// </summary>
        /// <param name="idCard"></param>
        /// <returns></returns>
        public static string HidenOrganizationCode(string code)
        {
            if (string.IsNullOrEmpty(code))
                return "";

            return code.Substring(0, 2) + "************" + code.Substring(code.Length - 2);
        }

        /// <summary>
        /// 创建Guid
        /// </summary>
        /// <returns></returns>
        public static string GetGuid()
        {
            return Guid.NewGuid().ToString().Replace("-", "").ToLower();
        }

        /// <summary>
        /// 转换字符串首字符为小写字符(对英文字符有效)
        /// </summary>
        /// <param name="originalVal"></param>
        /// <returns></returns>
        public static string TransformFirstToLower(string originalVal)
        {
            if (string.IsNullOrEmpty(originalVal))
            {
                return originalVal;
            }
            if (originalVal.Length >= 2)
            {
                return (originalVal.Substring(0, 1).ToLower() + originalVal.Substring(1));
            }
            return originalVal.ToUpper();
        }

        /// <summary>
        /// 转换字符串首字符为大写字符(对英文字符有效)
        /// </summary>
        /// <param name="originalVal">原始字符串</param>
        /// <returns></returns>
        public static string TransformFirstToUpper(string originalVal)
        {
            if (string.IsNullOrEmpty(originalVal))
            {
                return originalVal;
            }
            if (originalVal.Length >= 2)
            {
                return (originalVal.Substring(0, 1).ToUpper() + originalVal.Substring(1));
            }
            return originalVal.ToUpper();
        }

        /// <summary>
        /// 用字符串(separator)分割给定的字符串(originalStr)
        /// </summary>
        /// <param name="originalStr">原始字符串</param>
        /// <param name="separator">分割字符串</param>
        /// <returns></returns>
        public static string SplitStrUseStr(string originalStr, string separator = ",")
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(separator);
            for (int i = 0; i < originalStr.Length; i++)
            {
                builder.Append(originalStr[i]);
                builder.Append(separator);
            }
            return builder.ToString();
        }

        /// <summary>
        /// 分割文本内容 - 按行分割 <![CDATA[<br />,<p>]]>
        /// </summary>
        /// <param name="originalContent"></param>
        /// <param name="splitLines">分割行数</param>
        /// <returns></returns>
        public static string SplitStrUseLines(string originalContent, int splitLines)
        {
            if (string.IsNullOrEmpty(originalContent))
            {
                return string.Empty;
            }
            string str = string.Empty;
            int startIndex = 1;
            int num2 = 0;
            int num3 = originalContent.Length - 5;
            startIndex = 1;
            while (startIndex < num3)
            {
                if (originalContent.Substring(startIndex, 6).ToLower().Equals("<br />"))
                {
                    num2++;
                }
                if (originalContent.Substring(startIndex, 5).ToLower().Equals("<br/>"))
                {
                    num2++;
                }
                if (originalContent.Substring(startIndex, 4).ToLower().Equals("<br>"))
                {
                    num2++;
                }
                if (originalContent.Substring(startIndex, 3).ToLower().Equals("<p>"))
                {
                    num2++;
                }
                if (num2 >= splitLines)
                {
                    break;
                }
                startIndex++;
            }
            if (num2 >= splitLines)
            {
                if (startIndex == num3)
                {
                    str = originalContent.Substring(0, startIndex - 1);
                }
                else
                {
                    str = originalContent.Substring(0, startIndex);
                }
                return str;
            }
            return originalContent;
        }

        /// <summary>
        /// 格式化文件尺寸的方法
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static string FormatFileSize(long size)
        {
            string[] strArray = new string[] { "B", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB", "NB", "DB" };
            double num = size;
            int index = 0;
            while (num > 1024.0)
            {
                num /= 1024.0;
                index++;
                if (index == 4)
                {
                    break;
                }
            }
            return (num.ToString("F2") + strArray[index]);
        }
    }
}
