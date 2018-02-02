/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/02/16 21:34:13
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


namespace Iuker.Common.Module.Net.Http
{
    /// <summary>
    /// http通信数据模型编码解码器
    /// </summary>
    public interface IHttpModelEncoder
    {
        /// <summary>
        /// 通信答复编码
        /// </summary>
        /// <param name="resp"></param>
        /// <returns></returns>
        byte[] Encode(IHttpResp resp);

        /// <summary>
        /// 通信请求编码
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        byte[] Encode(IHttpReq req);

        /// <summary>
        /// 通信答复解码
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        IHttpResp GetResp(byte[] value);

        /// <summary>
        /// 通信请求解码
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        IHttpReq GetReq(byte[] value);

    }
}
