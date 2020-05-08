using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Help
{
    public class UserDevice
    {
        /// <summary>
        /// 判断是否移动端访问
        /// </summary>
        /// <returns></returns>
        public static bool IsMobileVisit()
        {
            if (Regex.IsMatch(Help.HttpContext.Current.Request.Headers["UserAgent"], "android|iphone|windows phone|ipod|ipad|kindle", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
