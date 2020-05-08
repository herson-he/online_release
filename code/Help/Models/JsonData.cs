using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Help.Models
{
    /// <summary>
    /// Ajax异步请求返回数据类
    /// </summary>
    public class JsonData
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int Code { get; set; } = 403;

        /// <summary>
        /// 消息内容
        /// </summary>
        public string Msg { get; set; } = "操作失败！";

        /// <summary>
        /// 数据项列表
        /// </summary>
        public object Payload { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public JsonData()
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public JsonData(bool state)
        {
            if (state)
                SetSuccess();
        }

        /// <summary>
        /// 为数据项添加数据
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public void AddDataItem(string key, object value)
        {
            Dictionary<string, object> dic = Payload as Dictionary<string, object>;
            if (dic == null && Payload != null)
                dic = TypeParse.ObjectToDictionary(Payload);
            if (dic == null)
                dic = new Dictionary<string, object>();
            dic.Add(key, value);
            Payload = dic;
        }

        /// <summary>
        /// 为数据项赋值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public void SetDataItem(string key, object value)
        {
            Dictionary<string, object> dic = Payload as Dictionary<string, object>;
            if (dic == null && Payload != null)
                dic = TypeParse.ObjectToDictionary(Payload);
            if (dic == null)
                dic = new Dictionary<string, object>();
            dic[key] = value;
            Payload = dic;
        }

        /// <summary>
        /// 获取数据项值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public object GetDataItemValue(string key, object value)
        {
            Dictionary<string, object> dic = Payload as Dictionary<string, object>;
            return dic[key];
        }

        /// <summary>
        /// 设置跳转地址
        /// </summary>
        /// <param name="url"></param>
        public void SetdRidectUrl(string url)
        {
            Dictionary<string, object> dic = Payload as Dictionary<string, object>;
            if (dic == null)
                dic = new Dictionary<string, object>();
            if (dic.ContainsKey("Url"))
            {
                dic["Url"] = url;
            }
            else
            {
                dic.Add("Url", url);
            }
            Payload = dic;
        }

        /// <summary>
        /// 设置页面数据模板
        /// </summary>
        /// <param name="t"></param>
        public void SetDataModel<T>(T t)
        {
            Dictionary<string, object> dic = Payload as Dictionary<string, object>;
            if (dic == null)
                dic = new Dictionary<string, object>();
            if (dic.ContainsKey("Model"))
            {
                dic["Model"] = t;
            }
            else
            {
                dic.Add("Model", t);
            }
            Payload = dic;
        }

        /// <summary>
        /// 设置操作成功
        /// </summary>
        public void SetSuccess()
        {
            Code = 1;
            Msg = "操作成功！";
        }

        /// <summary>
        /// 设置操作成功
        /// </summary>
        public void SetSuccess(string msg)
        {
            Code = 1;
            Msg = msg;
        }

        /// <summary>
        /// 设置操作失败
        /// </summary>
        public void SetFail()
        {
            Code = 0;
            Msg = "操作失败！";
        }

        /// <summary>
        /// 是否成功
        /// </summary>
        /// <returns></returns>
        public bool IsSuccess()
        {
            if (Code == 0)
                return true;
            return false;
        }
    }
}
