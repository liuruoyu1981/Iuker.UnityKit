/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/03/02 10:49:42
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


namespace Iuker.UnityKit.Run.Module.InputResponse
{
    /// <summary>
    /// 输入检测器
    /// </summary>
    public interface IInputChecker
    {
        /// <summary>
        /// 检测输入
        /// </summary>
        void InputCheck();

        /// <summary>
        /// 初始化一个输入检测器
        /// </summary>
        /// <param name="inputResponseModule"></param>
        IInputChecker Init(IU3dInputResponseModule inputResponseModule);

    }
}
