using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineRelease.Model
{
    public class FtpConfig
    {
        /// <summary>
        /// Ftp地址
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Pass { get; set; }
    }
}
