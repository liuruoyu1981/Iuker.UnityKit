using Iuker.UnityKit.Run.Base;
using Iuker.UnityKit.Run.Module.View.MVDA;
using UnityEngine;

namespace Iuker.UnityKit.Run.Module.View.ViewWidget
{
    /// <summary>
    /// 视图控件
    /// </summary>
    public interface IukViewWidget
    {
        /// <summary>
        /// 依赖的游戏对象
        /// </summary>
        GameObject DependentGo { get; }

        /// <summary>
        /// 寄生的视图根游戏对象
        /// </summary>
        GameObject ViewRoot { get; }

        /// <summary>
        /// 初始化一个视图控件实例
        /// </summary>
        /// <param name="u3DFrame">框架实例</param>
        /// <param name="view">视图实例</param>
        /// <param name="fragment">视图碎片实例默认为空</param>
        /// <returns></returns>
        IukViewWidget Init(IU3dFrame u3DFrame, IView view, IFragment fragment = null);

        /// <summary>
        /// 视图控件的唯一识别码
        /// </summary>
        string WidgetToken { get; }
    }
}
