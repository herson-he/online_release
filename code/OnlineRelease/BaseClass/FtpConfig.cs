using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebFoxSitePackage
{
    public class FtpConfig
    {
        public string _serverIP = "172.16.3.208";

        public int _port = 8021;

        public string _userName = "HeTao_WWW";

        public string _password = "foxuc3112546";

        /// <summary>
        /// 服务器IP
        /// </summary>
        public string ServerIP
        {
            get
            {
                return _serverIP;
            }
            set
            {
                _serverIP = value;
            }
        }

        /// <summary>
        /// 端口
        /// </summary>
        public int Port
        {
            get
            {
                return _port;
            }
            set
            {
                _port = value;
            }
        }

        /// <summary>
        /// FTP帐号
        /// </summary>
        public string UserName
        {
            get
            {
                return _userName;
            }
            set
            {
                _userName = value;
            }
        }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
            }
        }
    }
}
