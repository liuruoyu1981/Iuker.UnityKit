/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/04/27 08:54:10
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

using Iuker.Common.Base.Enums;
using Iuker.UnityKit.Run.Base;

namespace Iuker.Common.Module.Log
{
    /// <summary>
    /// 默认日志模块
    /// </summary>
    public class DefaultU3dLogModule : AbsU3dModule
    {
        public override string ModuleName
        {
            get
            {
                return ModuleType.Log.ToString();
            }
        }

        /// <summary>
        /// 发送日志的socket对象
        /// </summary>
        private System.Net.Sockets.Socket _socket;

        /// <summary>
        /// 日志服务器主机地址
        /// </summary>
        private string _logHost;

        /// <summary>
        /// 日志服务器端口
        /// </summary>
        private int _logPort;

        /// <summary>
        /// 当前是否已连接
        /// </summary>
        private bool _isConnect;



    }
}
