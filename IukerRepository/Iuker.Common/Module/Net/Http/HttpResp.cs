/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/02/16 21:46:53
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
    /// 数据交互模型—答复模型
    /// </summary>
    public class HttpResp : AbsNetModel, IHttpResp
    {
        /// <summary>
        /// 通信错误码
        /// </summary>
        public ushort ErrorCode { get; private set; }


        public HttpResp(byte moduleCode, ushort errorCode = 0, byte[] messageBytes = null, byte sonModuleCode = 0)
            : base(moduleCode, messageBytes, sonModuleCode)
        {
            ErrorCode = errorCode;
        }
    }
}
