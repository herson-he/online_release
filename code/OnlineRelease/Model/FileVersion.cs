using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineRelease.Model
{
    public class FileVersion
    {
        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 最后写入时间
        /// </summary>
        public string HashValue { get; set; }
    }
}
