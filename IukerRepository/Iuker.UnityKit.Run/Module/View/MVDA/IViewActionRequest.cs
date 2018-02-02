/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/3/26 下午5:55:59
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


namespace Iuker.UnityKit.Run.Module.View.MVDA
{
    /// <summary>
    /// 视图行为请求
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IViewActionRequest<T> : IActionRequest<T>
    {
        /// <summary>
        /// 视图行为处理请求者
        /// </summary>
        IViewActionRequester<T> ActionRequester { get; }

        /// <summary>
        /// 初始化视图行为处理请求
        /// </summary>
        /// <param name="requester"></param>
        /// <param name="actionToken"></param>
        /// <param name="viewScriptType"></param>
        /// <returns></returns>
        IViewActionRequest<T> Init(IViewActionRequester<T> requester, string actionToken,
            ViewScriptType viewScriptType = ViewScriptType.WidgetAction);

        /// <summary>
        /// 请求所需的处理器脚本类型
        /// </summary>
        ViewScriptType ViewScriptType { get; }


    }
}
