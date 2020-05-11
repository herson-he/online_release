using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineRelease.Model
{
    /// <summary>
    /// 版本文件存放地
    /// </summary>
    public enum VersionFileStore
    {
        /// <summary>
        /// 本地
        /// </summary>
        Native = 1,
        /// <summary>
        /// 远程FTP
        /// </summary>
        RemoteFtp = 2
    }
}
