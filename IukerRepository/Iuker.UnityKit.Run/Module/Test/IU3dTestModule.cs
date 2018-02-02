/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/3/26 上午7:32:40
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
using Iuker.Common;
using Iuker.Common.Base.Interfaces;


namespace Iuker.UnityKit.Run.Module.Test
{
    /// <summary>
    /// 测试模块
    /// </summary>
    public interface IU3dTestModule : IModule
    {
        /// <summary>
        /// 注册一个可测试对象
        /// </summary>
        /// <param name="tester"></param>
        void Register(ITester tester);

        /// <summary>
        /// 执行一个行为
        /// </summary>
        /// <param name="test">可测试对象名</param>
        /// <param name="action">行为名</param>
        void ExecuteTest(string test, string action);

        /// <summary>
        /// 处理异常
        /// </summary>
        /// <param name="exception"></param>
        void SloveException(Exception exception);


    }
}
