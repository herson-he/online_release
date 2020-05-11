using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineRelease.Model
{
    public class Config
    {
        /// <summary>
        /// 项目列表
        /// </summary>
        public List<Project> Projects { get; set; }

        /// <summary>
        /// 输出目录
        /// </summary>
        public string OutDir { get; set; }

        /// <summary>
        /// 忽略目录
        /// </summary>
        public string[] IgnoreDir { get; set; }

        /// <summary>
        /// 忽略文件
        /// </summary>
        public string[] IgnoreFileName { get; set; }

        /// <summary>
        /// 忽略后缀
        /// </summary>
        public string[] IgnoreFileSuffix { get; set; }
    }
}
