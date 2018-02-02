/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/4/4 上午7:42:26
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

using Iuker.UnityKit.Run.Base;


namespace Iuker.UnityKit.Run.Module.View.MVDA
{
    /// <summary>
    /// 视图数据模型调度器
    /// </summary>
    public interface IViewModelDispatcher
    {
        /// <summary>
        /// 获得一个视图数据模型的实例
        /// </summary>
        /// <param name="viewId"></param>
        /// <returns></returns>
        IViewModel GetModel(string viewId);

        /// <summary>
        /// 视图数据模型调度器初始化
        /// </summary>
        /// <param name="u3DFrame"></param>
        /// <returns></returns>
        IViewModelDispatcher Init(IU3dFrame u3DFrame);
    }
}
