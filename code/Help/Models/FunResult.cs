using System;
using System.Collections.Generic;
using System.Text;

namespace Help.Models
{
    /// <summary>
    /// 函数执行结果返回类
    /// </summary>
    public class FunResult
    {
        /// <summary>
        ///  返回结果
        /// </summary>
        public bool Success { get; set; } = false;

        /// <summary>
        /// 消息内容
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 返回值
        /// </summary>
        public Dictionary<string, object> ReturnValue { get; set; } = new Dictionary<string, object>();
    }
}
