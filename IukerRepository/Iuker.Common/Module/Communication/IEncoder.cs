/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/06/17 17:03
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

using System.Collections.Generic;
using Iuker.Common.Base.Interfaces;

namespace Iuker.Common.Module
{
    /// <summary>
    /// socket消息编码解码器
    /// </summary>
    public interface IEncoder
    {
        /// <summary>
        /// 网络消息编码
        /// </summary>
        /// <param name="message"></param>
        /// <param name="type">服务器类型用于填写第一级协议中字段</param>
        /// <returns></returns>
        byte[] Encode<T>(T message, int type = 5);

        /// <summary>
        /// 网络消息编码
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="message"></param>
        /// <param name="type">服务器类型用于填写第一级协议中字段</param>
        /// <returns></returns>
        byte[] Encode(string typeName, object message, int type = 5);

        /// <summary>
        /// 基础消息编码
        /// 根据type编号来区分不同的类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        byte[] EncodeMessageBase(int type);

        /// <summary>
        /// 网络消息解码
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        byte[] Decode(ref List<byte> data);

        /// <summary>
        /// 编码解码器初始化
        /// </summary>
        /// <param name="frame"></param>
        IEncoder Init(IFrame frame);

    }
}