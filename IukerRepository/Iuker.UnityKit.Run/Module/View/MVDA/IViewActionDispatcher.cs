using Iuker.UnityKit.Run.Base;

namespace Iuker.UnityKit.Run.Module.View.MVDA
{
    /// <summary>
    /// 视图行为处理请求调度器
    /// </summary>
    public interface IViewActionDispatcher
    {
        /// <summary>
        /// 调度一个视图行为请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <param name="agent"></param>
        void DispatchRequest<T>(IViewActionRequest<T> request, WidgetActionAgent<T> agent = null) where T : IViewElement;

        /// <summary>
        /// 初始化视图行为请求调度器
        /// </summary>
        /// <param name="u3DFrame"></param>
        /// <returns></returns>
        IViewActionDispatcher Init(IU3dFrame u3DFrame);

    }
}
