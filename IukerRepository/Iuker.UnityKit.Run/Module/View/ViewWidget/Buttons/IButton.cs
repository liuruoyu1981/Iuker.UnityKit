using System;
using Iuker.UnityKit.Run.Module.View.MVDA;
using UnityEngine.EventSystems;

namespace Iuker.UnityKit.Run.Module.View.ViewWidget
{
    /// <summary>
    /// 视图控件按钮
    /// </summary>
    public interface IButton : IukViewWidget, IViewElement
    {
        /// <summary>
        /// 设置或更换按钮的Image
        /// </summary>
        string ImageName { get; set; }

        string RawImageName { get; set; }

        /// <summary>
        /// 绑定指定按钮行为的动态处理
        /// </summary>
        /// <param name="type"></param>
        /// <param name="action"></param>
        /// <param name="isDoCsharp"></param>
        void BindingHotAction(ButtonActionType type, Action action, bool isDoCsharp = true);

        /// <summary>
        /// 指针数据
        /// 指针指pc平台的鼠标或移动平台的手指。
        /// </summary>
        PointerEventData PointerEventData { get; }

    }
}
