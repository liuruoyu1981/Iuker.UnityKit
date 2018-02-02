/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 6/14/2017 21:13
Email: liuruoyu1981@gmail.com
CreateNote: 
***********************************************************************************************/


/****************************************修改日志***********************************************
1. 修改日期： 修改人： 修改内容：
2. 修改日期： 修改人： 修改内容：
3. 修改日期： 修改人： 修改内容：
4. 修改日期： 修改人： 修改内容：
5. 修改日期： 修改人： 修改内容：
****************************************修改日志***********************************************/


using Iuker.Common;
using Iuker.Common.Module.Debugger;

namespace Iuker.UnityKit.Run.Module.Debugger
{
    /// <summary>
    /// 调试器模块
    /// </summary>
    public interface IU3dDebuggerModule : IModule
    {
        /// <summary>
        /// 获得一个日志输出器
        /// 能够获得的日志输出器需要在运行时配置中手动配置
        /// </summary>
        /// <param name="creater"></param>
        /// <returns></returns>
        ILoger GetLoger(string creater);














    }
}