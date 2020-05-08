using System;
using System.Collections.Generic;
using System.Text;

namespace Help
{
    /// <summary>
    /// 文件帮助类
    /// </summary>
    public class FileManage
    {
        /// <summary>
        /// 获取文件完整的路径
        /// </summary>
        /// <param name="path">相对路径</param>
        /// <returns></returns>
        public static string GetFullPath(string strPath)
        {
            string str = Text.AddLast(AppDomain.CurrentDomain.BaseDirectory, @"\");
            if (strPath.IndexOf(":") < 0)
            {
                string str2 = strPath.Replace(@"..\", "");
                if (str2 != strPath)
                {
                    int num = ((strPath.Length - str2.Length) / @"..\".Length) + 1;
                    for (int i = 0; i < num; i++)
                    {
                        str = str.Substring(0, str.LastIndexOf(@"\"));
                    }
                    str2 = @"\" + str2;
                }
                strPath = str + str2;
            }
            return strPath;
        }
    }
}
