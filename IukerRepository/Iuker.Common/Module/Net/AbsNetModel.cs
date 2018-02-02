/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/02/16 21:48:31
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

namespace Iuker.Common.Module.Net
{
    /// <summary>
    /// 网络数据模型基类
    /// </summary>
    public abstract class AbsNetModel : INetModel
    {
        public byte ModuleCode { get; protected set; }

        public byte SonModuleCode { get; protected set; }

        public byte[] MessageBytes { get; protected set; }

        /// <summary>
        /// 构建一个网络数据传输模型实例
        /// </summary>
        /// <param name="moduleCode">模块编码</param>
        /// <param name="messageBytes">主消息字节数组</param>
        /// <param name="sonModuleCode">子模块编码</param>
        public AbsNetModel(byte moduleCode, byte[] messageBytes = null, byte sonModuleCode = 0)
        {
            ModuleCode = moduleCode;
            SonModuleCode = sonModuleCode;
            MessageBytes = messageBytes;
        }



    }
}
