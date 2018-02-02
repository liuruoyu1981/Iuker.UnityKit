/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/02/16 21:39:14
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
    /// 数据交互模型—请求模型
    /// </summary>
    public class HttpReq : AbsNetModel, IHttpReq
    {
        /// <summary>
        /// 通信逻辑码
        /// </summary>
        public ushort LogicCode { get; private set; }

        public HttpReq(byte moduleCode, ushort logicCode, byte[] messageBytes = null, byte sonModuleCode = 0) : base(moduleCode, messageBytes, sonModuleCode)
        {
            LogicCode = logicCode;
        }
    }
}
