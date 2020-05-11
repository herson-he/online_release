using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineRelease.Model
{
    public class Project
    {
        /// <summary>
        /// 项目名称，必须唯一
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 项目路径
        /// </summary>
        public string ProjectPath { get; set; }

        /// <summary>
        /// 发布路径
        /// </summary>
        public string ReleasePath { get; set; }

        /// <summary>
        /// 执行的CMD
        /// </summary>
        public string CMD { get; set; }

        /// <summary>
        /// Ftp目录
        /// </summary>
        public string FtpDir { get; set; }

        /// <summary>
        /// 项目Ftp配置，如若为空，则使用PublicFtpConfig
        /// </summary>
        public FtpConfig ProjectFtpConfig { get; set; }
    }
}
