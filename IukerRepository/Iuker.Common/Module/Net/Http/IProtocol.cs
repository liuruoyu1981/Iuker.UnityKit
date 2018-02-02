/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/03/12 14:54:11
Email: 35490136@qq.com
QQCode: 35490136
CreateNote: 
***********************************************************************************************/


/****************************************修改日志***********************************************
1. 修改日期： 修改人： 修改内容：
2. 修改日期： 修改人： 修改内容：
3. 修改日期： 修改人： 修改内容：
4. 修改日期： 修改人： 修改内容：
5. 修改日期： 修改人： 修改内容：
****************************************修改日志***********************************************/

using System;
using System.Collections.Generic;

namespace Iuker.Common.Module.Net.Http
{
    /// <summary>
    /// 通信协议
    /// </summary>
    public interface IProtocol
    {
        /// <summary>
        /// 协议的发送类型
        /// </summary>
        ProtocolSendType ProtocolSendType { get; }

        /// <summary>
        /// Http答复网络数据传输模型
        /// </summary>
        IHttpReq HttpReq { get; }

        /// <summary>
        /// 发送协议
        /// </summary>
        /// <param name="oopsatCallback"></param>
        void SendProtocol(Action<IProtocol> oopsatCallback = null);

        /// <summary>
        /// 通信完成处理
        /// </summary>
        void OnCompleted(Dictionary<string, string> headers, IHttpResp resp);

        /// <summary>
        /// 获取一个布尔数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool? GetBool(string key);

        /// <summary>
        /// 获取一个int数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        int? GetInt(string key);

        /// <summary>
        /// 获取一个float数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        float? GetFloat(string key);

        /// <summary>
        /// 获取一个double数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        double? GetDouble(string key);

        /// <summary>
        /// 获取一个字符串
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string GetString(string key);

        /// <summary>
        /// 追加一个字符串数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        IProtocol AppendData(string key, string data);

        /// <summary>
        /// 追加一个float数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        IProtocol AppendData(string key, float data);

        /// <summary>
        /// 追加一个double数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        IProtocol AppendData(string key, double data);

        /// <summary>
        /// 追加一个int数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        IProtocol AppendData(string key, int data);

        /// <summary>
        /// 获取主消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetMessage<T>() where T : class, new();

    }

}
