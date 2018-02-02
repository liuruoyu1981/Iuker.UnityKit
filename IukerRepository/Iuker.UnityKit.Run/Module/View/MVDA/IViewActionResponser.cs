/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/3/26 下午5:52:54
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

using Iuker.Common.ActionWorkflow;
using Iuker.UnityKit.Run.Base;


namespace Iuker.UnityKit.Run.Module.View.MVDA
{
    /// <summary>
    /// 视图行为处理器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IViewActionResponser<T> : IActionResponser<T>
    {
        /// <summary>
        /// 处理视图行为请求
        /// </summary>
        /// <param name="request"></param>
        void ProcessRequest(IViewActionRequest<T> request);

        /// <summary>
        /// 初始化视图行为答复器，该方法一个答复器只会执行一次
        /// </summary>
        /// <param name="u3DFrame"></param>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        IViewActionResponser<T> Init(IU3dFrame u3DFrame, IViewActionRequest<T> request, IViewModel model);

        /// <summary>
        /// 视图行为处理器所关注的视图Id
        /// </summary>
        string ConcernedViewId { get; }

        /// <summary>
        /// 视图行为处理器所关注的视图当前是否已经被回收
        /// </summary>
        bool IsConcernedViewClosed { get; set; }

    }
}
