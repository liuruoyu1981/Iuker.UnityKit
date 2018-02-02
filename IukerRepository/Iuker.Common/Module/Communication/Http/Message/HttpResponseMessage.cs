using System;
using System.Net;
using Iuker.Common.Base;

namespace Iuker.Common.Module.Communication.Http.Message
{
#if DEBUG
    /// <summary>
    /// Http答复消息（表示下载状态）
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20170912 13:33:41")]
    [ClassPurposeDesc("Http答复消息（表示下载状态）", "Http答复消息（表示下载状态）")]
#endif
    public class HttpResponseMessage<T>
    {
        /// <summary>
        /// 原始的Web请求对象
        /// </summary>
        public HttpWebRequest OriginalRequest
        {
            get; set;
        }

        /// <summary>
        /// 原始的Web答复对象
        /// </summary>
        public HttpWebResponse OriginalResponse
        {
            get; set;
        }

        /// <summary>
        /// 从服务器获得的答复数据
        /// </summary>
        public T Data
        {
            get; set;
        }

        /// <summary>
        /// 下载内容的长度
        /// </summary>
        public long ContentLength
        {
            get; set;
        }

        /// <summary>
        /// 到目前为止下载的内容长度
        /// </summary>
        public long TotalContentRead
        {
            get; set;
        }

        /// <summary>
        /// 自上次http答复下载了多少内容
        /// </summary>
        public long ContentReadThisRound
        {
            get; set;
        }

        /// <summary>
        /// 下载完成百分比
        /// </summary>
        public int PercentageComplete { get { return ContentLength <= 0 ? 0 : (int)((double)TotalContentRead / ContentLength * 100); } }

        /// <summary>
        /// Http通信状态码
        /// </summary>
        public HttpStatusCode StatusCode
        {
            get; set;
        }

        /// <summary>
        /// Http通信状态码细节
        /// </summary>
        public string ReasonPhrase
        {
            get; set;
        }

        /// <summary>
        /// 状态代码是否可以视为成功代码
        /// </summary>
        public bool IsSuccessStatusCode { get { return (int)StatusCode >= 200 && (int)StatusCode <= 299; } }

        /// <summary>
        /// 异常(如果有的话)
        /// </summary>
        public Exception Exception
        {
            get; set;
        }
    }
}
