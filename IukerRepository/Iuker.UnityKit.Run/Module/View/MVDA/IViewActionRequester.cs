/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/3/26 上午12:15:40
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
    /// 视图行为请求者
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IViewActionRequester<T> : IAcionRequester<T>
    {
        /// <summary>
        /// 视图身份 Id
        /// </summary>
        string ViewId { get; }

        /// <summary>
        /// 发起行为处理请求
        /// </summary>
        /// <param name="request"></param>
        void Issue(IViewActionRequest<T> request);

    }

}
