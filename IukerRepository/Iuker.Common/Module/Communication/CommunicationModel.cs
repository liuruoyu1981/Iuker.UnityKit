/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/04/23 20:27:58
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


namespace Iuker.Common.Module.Socket
{
    /// <summary>
    /// 网络通信模型
    /// </summary>
    public class CommunicationModel
    {
        /// <summary>
        /// 一级协议 用于区分所属模块
        /// </summary>
        public byte ModuleCode { get; set; }

        /// <summary>
        /// 二级协议 用于区分 模块下所属子模块
        /// </summary>
        public ushort SonModuleCode { get; set; }

        /// <summary>
        /// 三级协议  用于区分当前处理逻辑功能
        /// </summary>
        public ushort LogicCode { get; set; }

        /// <summary>
        /// 通信状态码
        /// </summary>
        public ushort StateCode { get; set; }


        public byte[] MessageBytes { get; set; }

        public CommunicationModel() { }
        public CommunicationModel(byte t, ushort sonModuleCode, ushort logicCode, ushort stateCode = 0, byte[] messageBytes = null)
        {
            ModuleCode = t;
            SonModuleCode = sonModuleCode;
            LogicCode = logicCode;
            StateCode = stateCode;
            MessageBytes = messageBytes;
        }

    }
}
