using System;
using System.Data;
using System.Text.RegularExpressions;

namespace Help
{
    /// <summary>
    /// 数据验证类
    /// </summary>
    public sealed class Validate
    {
        private static readonly Regex regex_FileNameFilter = new Regex("[<>/\";#$*%]+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex regex_ImgFormat = new Regex(@"\.(gif|jpg|bmp|png|jpeg)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex regex_IsDate = new Regex(@"^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-))$", RegexOptions.Compiled);
        private static readonly Regex regex_IsDecimalFraction = new Regex(@"^([0-9]{1,10})\.([0-9]{1,10})$", RegexOptions.Compiled);
        private static readonly Regex regex_IsDouble = new Regex(@"^([0-9])[0-9]*(\.\w*)?$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex regex_IsLongDate = new Regex(@"^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-)) (20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d$", RegexOptions.Compiled);
        private static readonly Regex regex_IsNegativeInt = new Regex(@"^-\d+$", RegexOptions.Compiled);
        private static readonly Regex regex_IsNickName = new Regex(@"^[a-zA-Z\u4e00-\u9fa5\d_]+$", RegexOptions.Compiled);
        private static readonly Regex regex_IsFullName = new Regex(@"^[a-zA-Z\u4e00-\u9fa5\d_]+$", RegexOptions.Compiled);
        private static readonly Regex regex_IsNumeric = new Regex("^[-]?[0-9]*[.]?[0-9]*$", RegexOptions.Compiled);
        private static readonly Regex regex_IsPositiveInt = new Regex(@"^\d+$", RegexOptions.Compiled);
        private static readonly Regex regex_IsShortDate = new Regex(@"^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-))$", RegexOptions.Compiled);
        private static readonly Regex regex_IsUserName = new Regex(@"^[a-zA-Z\u4e00-\u9fa5\d_]{2,12}$", RegexOptions.Compiled);
        private static readonly Regex regex_IsPassword = new Regex(@"^[a-zA-Z\d_]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex regex_IsBase64String = new Regex(@"[A-Za-z0-9\+\/\=]", RegexOptions.Compiled);
        private static readonly Regex regex_IsCnChar = new Regex(@"^(?:[一-龥])+$", RegexOptions.Compiled);
        private static readonly Regex regex_IsCnCharAndWordAndNum = new Regex(@"^[0-9a-zA-Z一-龥]+$", RegexOptions.Compiled);
        private static readonly Regex regex_IsEmail = new Regex(@"^[A-Za-z0-9]+([_\.]+[A-Za-z0-9]+)*@([A-Za-z0-9\-]+\.)+[A-Za-z]{2,6}$", RegexOptions.Compiled);
        private static readonly Regex regex_IsIP4 = new Regex(@"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$", RegexOptions.Compiled);
        private static readonly Regex regex_IsIP6 = new Regex(@"(([0-9A-F]{1,4}:){7}?[0-9A-F]{1,4})|((([0-9A-F]{1,4}:){0,6}::([0-9A-F]{1,4}:){0,6}))", RegexOptions.Compiled);
        private static readonly Regex regex_IsIPAndPort = new Regex(@"^(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9]),\d{1,5}?$", RegexOptions.Compiled);
        private static readonly Regex regex_IsMobileCode = new Regex(@"\d{11}", RegexOptions.Compiled);
        private static readonly Regex regex_IsQQNumber = new Regex(@"^\d{4,20}$", RegexOptions.Compiled);
        private static readonly Regex regex_IsPhoneCode = new Regex(@"^(86)?(-)?(0\d{2,3})?(-)?(\d{7,8})(-)?(\d{3,5})?$", RegexOptions.Compiled);
        private static readonly Regex regex_IsWordAndNum = new Regex(@"^[0-9a-zA-Z]$", RegexOptions.Compiled);
        private static readonly Regex regex_IsMeachineID = new Regex(@"^[0-9a-zA-z]{32}$", RegexOptions.Compiled);
        private static readonly Regex regex_IsUnicode = new Regex(@"^[\u4E00-\u9FA5\uE815-\uFA29]+$", RegexOptions.Compiled);
        private static readonly Regex regex_IsUrl = new Regex(@"^(http|https)\://([a-zA-Z0-9\.\-]+(\:[a-zA-Z0-9\.&%\$\-]+)*@)*((25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])|localhost|([a-zA-Z0-9\-]+\.)*[a-zA-Z0-9\-]+\.(com|edu|gov|int|mil|net|org|biz|arpa|info|name|pro|aero|coop|museum|[a-zA-Z]{1,10}))(\:[0-9]+)*(/($|[a-zA-Z0-9\.\,\?\'\\\+&%\$#\=~_\-]+))*$", RegexOptions.Compiled);
        private static readonly Regex regex_IsYearMonth = new Regex(@"^[0-9]{4}(-){0,1}[0-9]{2}$", RegexOptions.Compiled);
        private static readonly Regex regex_IsOrganizationCode = new Regex(@"^\d{8}-([a-z]|[0-9]){1}$", RegexOptions.Compiled);

        private Validate()
        {
        }

        /// <summary>
        /// 验证数据DataSet是否有数据
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static bool CheckedDataSet(DataSet ds)
        {
            if (((ds == null) || (ds.Tables == null)) || ((ds.Tables != null) && (((ds.Tables.Count == 0) || (ds.Tables[0] == null)) || (ds.Tables[0].Rows.Count == 0))))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 验证数据DataTable是否有数据
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static bool CheckedDataTable(DataTable dt)
        {
            if ((object.Equals(dt, null) || object.Equals(dt.Rows, null)) || (dt.Rows.Count == 0))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 验证object[]是否有数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool CheckedObjcetArray(object[] obj)
        {
            if ((object.Equals(obj, null) || (obj.Length == 0)) || object.Equals(obj[0], null))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 验证数据是否是Base64String格式
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static bool IsBase64String(string expression)
        {
            return regex_IsBase64String.IsMatch(expression);
        }

        /// <summary>
        /// 验证是否是中文
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static bool IsCnChar(string expression)
        {
            return regex_IsCnChar.IsMatch(expression);
        }

        /// <summary>
        /// 验证字符串是否由中文、字母、数字组成
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static bool IsCnCharAndWordAndNum(string expression)
        {
            return regex_IsCnCharAndWordAndNum.IsMatch(expression);
        }

        /// <summary>
        /// 验证字符串是否是日期格式
        /// </summary>
        /// <param name="dateval"></param>
        /// <returns></returns>
        public static bool IsDate(string dateval)
        {
            return regex_IsDate.IsMatch(dateval);
        }

        /// <summary>
        /// 验证字符串是否是Decimal格式
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static bool IsDecimalFraction(string expression)
        {
            return regex_IsDecimalFraction.IsMatch(expression);
        }

        /// <summary>
        /// 验证是否是域名格式
        /// </summary>
        /// <param name="strHost"></param>
        /// <returns></returns>
        public static bool IsDomain(string strHost)
        {
            Regex regex = new Regex(@"^\d+$");
            if (strHost.IndexOf(".") == -1)
            {
                return false;
            }
            return !regex.IsMatch(strHost.Replace(".", string.Empty));
        }

        /// <summary>
        /// 验证是否是Double格式
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static bool IsDouble(object expression)
        {
            if (expression == null)
                return false;
            return ((expression != null) && regex_IsDouble.IsMatch(expression.ToString()));
        }

        /// <summary>
        /// 验证是否是邮箱格式
        /// </summary>
        /// <param name="strEmail"></param>
        /// <returns></returns>
        public static bool IsEmail(string strEmail)
        {
            if (string.IsNullOrEmpty(strEmail))
                return false;
            return regex_IsEmail.IsMatch(strEmail);
        }

        /// <summary>
        /// 验证是否是文件名
        /// HACK 该方法待测试
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static bool IsFileName(string filename)
        {
            if (string.IsNullOrEmpty(filename))
                return false;
            return !regex_FileNameFilter.IsMatch(filename);
        }

        /// <summary>
        /// 验证是否是身份证号码
        /// </summary>
        /// <param name="strIDCard"></param>
        /// <returns></returns>
        public static bool IsIDCard(string strIDCard)
        {
            Regex regex;
            string[] strArray;
            DateTime time;
            if (string.IsNullOrEmpty(strIDCard))
            {
                return false;
            }
            if ((strIDCard.Length != 15) && (strIDCard.Length != 0x12))
            {
                return false;
            }
            if (strIDCard.Length == 15)
            {
                regex = new Regex(@"^(\d{6})(\d{2})(\d{2})(\d{2})(\d{3})$");
                if (!regex.Match(strIDCard).Success)
                {
                    return false;
                }
                strArray = regex.Split(strIDCard);
                try
                {
                    time = new DateTime(int.Parse("19" + strArray[2]), int.Parse(strArray[3]), int.Parse(strArray[4]));
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            regex = new Regex(@"^(\d{6})(\d{4})(\d{2})(\d{2})(\d{3})([0-9Xx])$");
            if (!regex.Match(strIDCard).Success)
            {
                return false;
            }
            strArray = regex.Split(strIDCard);
            try
            {
                time = new DateTime(int.Parse(strArray[2]), int.Parse(strArray[3]), int.Parse(strArray[4]));
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 验证是否是图片
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static bool IsImage(string filename)
        {
            if (string.IsNullOrEmpty(filename))
                return false;
            return regex_ImgFormat.Match(filename).Success;
        }

        /// <summary>
        /// 验证是否是IP格式
        /// HACK 该方法待测试
        /// </summary>
        /// <param name="ipval"></param>
        /// <returns></returns>
        public static bool IsIP(string ipval)
        {
            if (string.IsNullOrEmpty(ipval))
                return false;

            // IP4
            if (regex_IsIP4.IsMatch(ipval))
                return true;

            // IP6
            if (ipval.IndexOf(":::") != -1)
                return false;
            int count = 0;
            for (int i = 0; i < ipval.Length; i++)
            {
                if (ipval[i] == ':')
                {
                    count++;
                }
            }
            if (count > 7)
                return false;
            if (regex_IsIP6.IsMatch(ipval))
                return true;

            return false;
        }

        /// <summary>
        /// 验证是否是IP+端口格式
        /// </summary>
        /// <param name="ipval"></param>
        /// <returns></returns>
        public static bool IsIPAndPort(string ipval)
        {
            if (string.IsNullOrEmpty(ipval))
                return false;
            return regex_IsIPAndPort.IsMatch(ipval);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ipval"></param>
        /// <returns></returns>
        public static bool IsIPSect(string ipval)
        {
            if (string.IsNullOrEmpty(ipval))
                return false;
            return Regex.IsMatch(ipval, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){2}((2[0-4]\d|25[0-5]|[01]?\d\d?|\*)\.)(2[0-4]\d|25[0-5]|[01]?\d\d?|\*)$");
        }

        /// <summary>
        /// 验证是否是长日期格式
        /// </summary>
        /// <param name="dateval"></param>
        /// <returns></returns>
        public static bool IsLongDate(string dateval)
        {
            if (string.IsNullOrEmpty(dateval))
                return false;
            return regex_IsLongDate.IsMatch(dateval);
        }

        /// <summary>
        /// 验证密码
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static bool IsPassword(string expression)
        {
            if (string.IsNullOrEmpty(expression))
                return false;
            return regex_IsPassword.IsMatch(expression);
        }

        /// <summary>
        /// 验证是否是手机号码
        /// </summary>
        /// <param name="strMobile"></param>
        /// <returns></returns>
        public static bool IsMobileCode(string strMobile)
        {
            if (string.IsNullOrEmpty(strMobile))
                return false;
            return regex_IsMobileCode.IsMatch(strMobile);
        }

        /// <summary>
        /// 验证是否是QQ号码
        /// </summary>
        /// <param name="strMobile"></param>
        /// <returns></returns>
        public static bool IsQQNumber(string str)
        {
            if (string.IsNullOrEmpty(str))
                return false;
            return regex_IsQQNumber.IsMatch(str);
        }

        /// <summary>
        /// 验证是否是负整数
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static bool IsNegativeInt(string expression)
        {
            if (string.IsNullOrEmpty(expression))
                return false;
            return (regex_IsNegativeInt.Match(expression).Success && (long.Parse(expression) >= -2147483648L));
        }

        /// <summary>
        /// 验证昵称
        /// </summary>
        /// <param name="strVal"></param>
        /// <returns></returns>
        public static bool IsNickName(string strVal)
        {
            if (string.IsNullOrEmpty(strVal))
                return false;
            return regex_IsNickName.IsMatch(strVal);
        }

        /// <summary>
        /// 验证真实姓名
        /// </summary>
        /// <param name="strVal"></param>
        /// <returns></returns>
        public static bool IsFullName(string strVal)
        {
            if (string.IsNullOrEmpty(strVal))
                return false;
            return regex_IsFullName.IsMatch(strVal);
        }

        /// <summary>
        /// 验证是否是数字
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static bool IsNumeric(object expression)
        {
            if (expression != null)
            {
                string input = expression.ToString();
                if ((((input.Length > 0) && (input.Length <= 11)) && regex_IsNumeric.IsMatch(input)) && (((input.Length < 10) || ((input.Length == 10) && (input[0] == '1'))) || (((input.Length == 11) && (input[0] == '-')) && (input[1] == '1'))))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 验证是否是数字数组
        /// </summary>
        /// <param name="strNumber"></param>
        /// <returns></returns>
        public static bool IsNumericArray(string[] strNumber)
        {
            if (strNumber == null)
            {
                return false;
            }
            if (strNumber.Length < 1)
            {
                return false;
            }
            foreach (string str in strNumber)
            {
                if (!IsNumeric(str))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 验证是否是电话号码
        /// </summary>
        /// <param name="strPhone"></param>
        /// <returns></returns>
        public static bool IsPhoneCode(string strPhone)
        {
            if (string.IsNullOrEmpty(strPhone))
                return false;
            return regex_IsPhoneCode.IsMatch(strPhone);
        }

        /// <summary>
        /// 验证是否是无符号Byte
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static bool IsPositiveByte(string expression)
        {
            if (string.IsNullOrEmpty(expression))
                return false;
            return (regex_IsPositiveInt.Match(expression).Success && (long.Parse(expression) <= 255));
        }

        /// <summary>
        /// 验证是否是无符号Int
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static bool IsPositiveInt(string expression)
        {
            if (string.IsNullOrEmpty(expression))
                return false;
            return (regex_IsPositiveInt.Match(expression).Success && (long.Parse(expression) <= 0x7fffffffL));
        }

        /// <summary>
        /// 验证是否是无符号Long
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static bool IsPositiveInt64(string expression)
        {
            if (string.IsNullOrEmpty(expression))
                return false;
            return (regex_IsPositiveInt.Match(expression).Success && (long.Parse(expression) <= 0x7fffffffffffffffL));
        }

        /// <summary>
        /// 验证是否是邮政号码
        /// </summary>
        /// <param name="strPostalCode"></param>
        /// <returns></returns>
        public static bool IsPostalCode(string strPostalCode)
        {
            if (string.IsNullOrEmpty(strPostalCode))
                return false;
            return Regex.IsMatch(strPostalCode, @"^\d{6}$", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 验证是否是短日期格式
        /// </summary>
        /// <param name="dateval"></param>
        /// <returns></returns>
        public static bool IsShortDate(string dateval)
        {
            return regex_IsShortDate.IsMatch(dateval);
        }

        /// <summary>
        /// 验证是否是时间类型？
        /// HACK 很怀疑准确性
        /// </summary>
        /// <param name="timeval"></param>
        /// <returns></returns>
        public static bool IsTime(string timeval)
        {
            return Regex.IsMatch(timeval, "^((([0-1]?[0-9])|(2[0-3])):([0-5]?[0-9])(:[0-5]?[0-9])?)$");
        }

        /// <summary>
        /// 验证是否是Unicode编码
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsUnicode(string s)
        {
            return regex_IsUnicode.IsMatch(s);
        }

        /// <summary>
        /// 验证数据是否是URL格式
        /// </summary>
        /// <param name="strUrl"></param>
        /// <returns></returns>
        public static bool IsURL(string strUrl)
        {
            if (string.IsNullOrEmpty(strUrl))
                return false;
            return regex_IsUrl.IsMatch(strUrl);
        }

        /// <summary>
        /// 验证是否是用户名格式
        /// </summary>
        /// <param name="strVal"></param>
        /// <returns></returns>
        public static bool IsUserName(string strVal)
        {
            if (string.IsNullOrEmpty(strVal))
                return false;
            return regex_IsUserName.IsMatch(strVal);
        }

        /// <summary>
        /// 是否由数字和字母组成
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static bool IsWordAndNum(string expression)
        {
            if (string.IsNullOrEmpty(expression))
                return false;
            return regex_IsWordAndNum.IsMatch(expression);
        }

        /// <summary>
        /// 验证是机器码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsMeachineID(string str)
        {
            if (string.IsNullOrEmpty(str))
                return false;
            return regex_IsMeachineID.IsMatch(str);
        }

        /// <summary>
        /// 验证是否是Decimal(18,2)
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsDecimalPoint2(object str, bool isAllowNegative = false)
        {
            string pattern = "";
            if (isAllowNegative)
                pattern = @"^(-){0,1}[0-9]{1,32}(\.[0-9]{1,2}){0,1}$";
            else
                pattern = @"^[0-9]{1,32}(\.[0-9]{1,2}){0,1}$";

            return Regex.IsMatch(str.ToString(), pattern);
        }

        /// <summary>
        /// 验证是否是yyyy-MM模式 比如 2016-05
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsYearMonth(string str)
        {
            return regex_IsYearMonth.IsMatch(str);
        }

        /// <summary>
        /// 验证是否是企业组织代码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsOrganizationCode(string str)
        {
            return regex_IsOrganizationCode.IsMatch(str);
        }
    }
}

