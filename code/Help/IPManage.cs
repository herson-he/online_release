using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Help
{
    /// <summary>
    /// IP数据库操作类
    /// 采用的IP数据库源为 http://www.ipip.net 免费版
    /// </summary>
    public static class IPManage
    {
        public static bool EnableFileWatch = false;                         // 是否监控IP数据库是否被修改
        private static int offset;
        private static uint[] index = new uint[256];                        // 0-255 对应的IP数
        private static byte[] dataBuffer;                                   // 完整数据缓存
        private static byte[] indexBuffer;                                  // 初步理解为IP数据缓存
        private static long lastModifyTime = 0L;
        private static string ipFile;                                       // IP文件
        private static readonly object @lock = new object();

        /// <summary>
        /// 获取IP地址对应的物理地址
        /// </summary>
        /// <returns></returns>
        public static string GetIPAare(string IP)
        {
            if (string.IsNullOrEmpty(IP))
                return "";
            Load(Help.FileManage.GetFullPath("/Data/17monipdb.dat"));
            EnableFileWatch = false;
            string[] arrIP = IPManage.Find(IP);
            string area = string.Empty;
            if (arrIP[0] == arrIP[1] && string.IsNullOrEmpty(arrIP[2]) && string.IsNullOrEmpty(arrIP[3]))
                return arrIP[0];
            foreach (string item in arrIP)
            {
                area += item + " ";
            }
            return area.TrimEnd();
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="filename"></param>
        public static void Load(string filename)
        {
            ipFile = new FileInfo(filename).FullName;
            if (dataBuffer == null)
                Load();
            if (EnableFileWatch)
                Watch();
        }

        /// <summary>
        /// 查询IP对应的物理地址
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static string[] Find(string ip)
        {
            lock (@lock)
            {
                var ips = ip.Split('.');
                var ip_prefix_value = int.Parse(ips[0]);
                long ip2long_value = BytesToLong(byte.Parse(ips[0]), byte.Parse(ips[1]), byte.Parse(ips[2]),
                    byte.Parse(ips[3]));
                var start = index[ip_prefix_value];
                var max_comp_len = offset - 1028;
                long index_offset = -1;
                var index_length = -1;
                byte b = 0;
                for (start = start * 8 + 1024; start < max_comp_len; start += 8)
                {
                    if (BytesToLong(indexBuffer[start + 0], indexBuffer[start + 1], indexBuffer[start + 2], indexBuffer[start + 3]) >= ip2long_value)
                    {
                        index_offset = BytesToLong(b, indexBuffer[start + 6], indexBuffer[start + 5], indexBuffer[start + 4]);
                        index_length = 0xFF & indexBuffer[start + 7];
                        break;
                    }
                }
                var areaBytes = new byte[index_length];
                Array.Copy(dataBuffer, offset + (int)index_offset - 1024, areaBytes, 0, index_length);
                return Encoding.UTF8.GetString(areaBytes).Split('\t');
            }
        }

        /// <summary>
        /// 监控文件
        /// </summary>
        private static void Watch()
        {
            var file = new FileInfo(ipFile);
            if (file.DirectoryName == null) return;
            var watcher = new FileSystemWatcher(file.DirectoryName, file.Name) { NotifyFilter = NotifyFilters.LastWrite };
            watcher.Changed += (s, e) =>
            {
                var time = System.IO.File.GetLastWriteTime(ipFile).Ticks;
                if (time > lastModifyTime)
                {
                    Load();
                }
            };
            watcher.EnableRaisingEvents = true;
        }

        /// <summary>
        /// 载入IP数据库文件
        /// </summary>
        private static void Load()
        {
            lock (@lock)
            {
                // 记录读取文件次数
                //string path = Game.Utils.TextUtility.GetFullPath("/Log/IPDataReadNumber.log");
                //string strNum = "1";
                //if(!File.Exists(path))
                //    File.WriteAllText(path, "1", Encoding.Default);
                //else
                //{
                //    strNum = File.ReadAllText(path);
                //    File.WriteAllText(path, (Convert.ToInt32(strNum) + 1).ToString(), Encoding.Default);
                //}

                var file = new FileInfo(ipFile);
                lastModifyTime = file.LastWriteTime.Ticks;
                try
                {
                    dataBuffer = new byte[file.Length];
                    using (var fin = new FileStream(file.FullName, FileMode.Open, FileAccess.Read))
                    {
                        fin.Read(dataBuffer, 0, dataBuffer.Length);
                    }

                    var indexLength = BytesToLong(dataBuffer[0], dataBuffer[1], dataBuffer[2], dataBuffer[3]);
                    indexBuffer = new byte[indexLength];
                    Array.Copy(dataBuffer, 4, indexBuffer, 0, indexLength);
                    offset = (int)indexLength;

                    for (var loop = 0; loop < 256; loop++)
                    {
                        index[loop] = BytesToLong(indexBuffer[loop * 4 + 3], indexBuffer[loop * 4 + 2],
                            indexBuffer[loop * 4 + 1],
                            indexBuffer[loop * 4]);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
        }

        /// <summary>
        /// 将4个bye转换为无符号整型
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        private static uint BytesToLong(byte a, byte b, byte c, byte d)
        {
            return ((uint)a << 24) | ((uint)b << 16) | ((uint)c << 8) | d;
        }

        ///// <summary>
        ///// 获取客户端的 IP 信息
        ///// </summary>
        ///// <returns></returns>
        //public static string GetUserIP()
        //{
        //    if (Help.HttpContext.Current == null)
        //    {
        //        return string.Empty;
        //    }
        //    string ipval = string.Empty;
        //    ipval = Help.HttpContext.Current.Request.Headers["HTTP_X_FORWARDED_FOR"];
        //    switch (ipval)
        //    {
        //        case null:
        //        case "":
        //            ipval = Help.HttpContext.Current.Request.Headers["REMOTE_ADDR"];
        //            break;
        //    }
        //    if ((ipval == null) || (ipval == string.Empty))
        //    {
        //        ipval = Help.HttpContext.Current.Request.Host.Value;
        //    }
        //    if (!(((ipval != null) && (ipval != string.Empty)) && Validate.IsIP(ipval)))
        //    {
        //        return "0.0.0.0";
        //    }
        //    return ipval;
        //}

        ///// <summary>
        ///// 获取客户端的IP数值
        ///// </summary>
        ///// <returns></returns>
        //public static long GetIPNumber()
        //{
        //    if (Help.HttpContext.Current == null)
        //    {
        //        return 0;
        //    }
        //    string ipval = Help.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
        //    if ((ipval == null) || (ipval == string.Empty))
        //    {
        //        ipval = Help.HttpContext.Current.Request.UserHostAddress;
        //    }
        //    if (!string.IsNullOrEmpty(ipval) && Validate.IsIP(ipval))
        //    {
        //        return Utility.IP2Int(ipval);
        //    }
        //    return 0;
        //}
    }
}
