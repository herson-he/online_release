using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Help
{
    public class RandomManage
    {
        private static RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

        /// <summary>
        /// 创建验证码随机字符串(数字和字母)
        /// </summary>
        /// <param name="len">最大长度</param>
        /// <returns>返回指定最大长度的随机字符串</returns>
        public static string CreateAuthStr(int len)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < len; i++)
            {
                int num2 = random.Next();
                if ((num2 % 2) == 0)
                {
                    builder.Append((char)(0x30 + ((ushort)(num2 % 10))));
                }
                else
                {
                    builder.Append((char)(0x41 + ((ushort)(num2 % 0x1a))));
                }
            }
            return builder.ToString();
        }

        /// <summary>
        /// 创建验证码随机字符串
        /// </summary>
        /// <param name="len">最大长度</param>
        /// <param name="onlyNum">是否纯数字</param>
        /// <returns>返回指定最大长度的随机字符串</returns>
        public static string CreateAuthStr(int len, bool onlyNum)
        {
            if (!onlyNum)
            {
                return CreateAuthStr(len);
            }
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < len; i++)
            {
                int num2 = random.Next();
                builder.Append((char)(0x30 + ((ushort)(num2 % 10))));
            }
            return builder.ToString();
        }

        /// <summary>
        /// 生成随机字符串
        /// </summary>
        /// <param name="length">目标字符串的长度</param>
        /// <param name="useNum">是否包含数字，1=包含，默认为包含</param>
        /// <param name="useLow">是否包含小写字母，1=包含，默认为包含</param>
        /// <param name="useUpp">是否包含大写字母，1=包含，默认为包含</param>
        /// <param name="useSpe">是否包含特殊字符，1=包含，默认为不包含</param>
        /// <param name="custom">要包含的自定义字符，直接输入要包含的字符列表</param>
        /// <returns>指定长度的随机字符串</returns>
        public static string CreateRandom(int length, int useNum, int useLow, int useUpp, int useSpe, string custom)
        {
            byte[] data = new byte[4];
            new RNGCryptoServiceProvider().GetBytes(data);
            Random random = new Random(BitConverter.ToInt32(data, 0));
            string str = null;
            string str2 = custom;
            if (useNum == 1)
            {
                str2 = str2 + "0123456789";
            }
            if (useLow == 1)
            {
                str2 = str2 + "abcdefghijklmnopqrstuvwxyz";
            }
            if (useUpp == 1)
            {
                str2 = str2 + "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            }
            if (useSpe == 1)
            {
                str2 = str2 + "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~";
            }
            for (int i = 0; i < length; i++)
            {
                str = str + str2.Substring(random.Next(0, str2.Length - 1), 1);
            }
            return str;
        }

        /// <summary>
        /// 获取一个由26个小写字母组成的指定长度的随即字符串
        /// </summary>
        /// <param name="len">最大长度</param>
        /// <returns></returns>
        public static string CreateRandomLowercase(int len)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < len; i++)
            {
                int num2 = random.Next();
                builder.Append((char)(0x61 + ((ushort)(num2 % 0x1a))));
            }
            return builder.ToString();
        }

        /// <summary>
        /// 获取指定长度的纯数字随机数字串(以时间做随机种子)
        /// </summary>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string CreateRandomNum(int len)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random((int)DateTime.Now.Ticks);
            for (int i = 0; i < len; i++)
            {
                int num = random.Next();
                builder.Append((char)(0x30 + ((ushort)(num % 10))));
            }
            return builder.ToString();
        }

        public static string CreateRandomNum2(int len)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random(GetNewSeed());
            for (int i = 0; i < len; i++)
            {
                int num = random.Next();
                builder.Append((char)(0x30 + ((ushort)(num % 10))));
            }
            return builder.ToString();
        }

        /// <summary>
        /// 产生随机数的种子
        /// </summary>
        /// <returns></returns>
        public static int GetNewSeed()
        {
            byte[] data = new byte[4];
            rng.GetBytes(data);
            return BitConverter.ToInt32(data, 0);
        }

        /// <summary>
        /// 创建一串随机字符串，包括数字和字母
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string CreateRandom(int len)
        {
            string result = string.Empty;
            string str = "0123456789QWERTYUPASDFGHJKLZXCVBNM";
            Random random = new Random((int)DateTime.Now.Ticks);
            for (int i = 0; i < len; i++)
            {
                int num = random.Next();
                result += str.Substring(random.Next(0, str.Length - 1), 1);
            }
            return result;
        }
    }
}
