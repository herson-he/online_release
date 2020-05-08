using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Help
{
    public class SoftwareVersion
    {
        /// <summary>
        /// 计算版本号，返回数组
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public static uint[] CalculateVersionToArray(uint version)
        {
            uint[] arrayVersion = new uint[4];
            arrayVersion[0] = (version >> 24);
            arrayVersion[1] = ((version >> 16) << 24) >> 24;
            arrayVersion[2] = ((version >> 8) << 24) >> 24;
            arrayVersion[3] = (version << 24) >> 24;
            return arrayVersion;
        }

        /// <summary>
        /// 计算版本号，返回数组
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public static uint[] CalculateVersionToArray(string version)
        {
            if(string.IsNullOrEmpty(version))
            {
                return null;
            }

            string[] arrayStringVersion = version.Split('.');
            if(arrayStringVersion.Length != 4)
            {
                return null;
            }

            uint[] arrayIntVersion = new uint[4];
            for(int i = 0; i < arrayStringVersion.Length; i++)
            {
                try
                {
                    arrayIntVersion[i] = Convert.ToUInt32(arrayStringVersion[i]);
                    if(arrayIntVersion[i]<0 || arrayIntVersion[i]>255)
                    {
                        return null;
                    }
                }
                catch
                {
                    return null;
                }
            }
            return arrayIntVersion;
        }

        /// <summary>
        /// 计算版本号，返回字符串
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public static string CalculateVersionToString(uint version)
        {
            string returnValue = "";
            returnValue += (version >> 24).ToString() + ".";
            returnValue += (((version >> 16) << 24) >> 24).ToString() + ".";
            returnValue += (((version >> 8) << 24) >> 24).ToString() + ".";
            returnValue += ((version << 24) >> 24).ToString();
            return returnValue;
        }

        /// <summary>
        /// 计算版本号，返回整型
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public static uint CalculateVersionToInt(string version)
        {
            uint rValue = 0;
            string[] verArray = version.Split('.');
            rValue = (uint.Parse(verArray[0]) << 24) | (uint.Parse(verArray[1]) << 16) | (uint.Parse(verArray[2]) << 8) | uint.Parse(verArray[3]);
            return rValue;
        }
    }
}
