namespace Iuker.UnityKit.Run.Module.View.ViewWidget
{
    public enum ButtonActionType : byte
    {
        /// <summary>
        /// 等于按下（OnPointerClick）
        /// </summary>
        OnClick,

        /// <summary>
        /// 按下
        /// </summary>
        OnPointerClick,

        /// <summary>
        /// 进入
        /// </summary>
        OnPointerEnter,

        /// <summary>
        /// 按压
        /// </summary>
        OnPointerDown,

        /// <summary>
        /// 退出
        /// </summary>
        OnPointerExit,

        /// <summary>
        /// 抬起
        /// </summary>
        OnPointerUp,

        /// <summary>
        /// 拖动中
        /// </summary>
        OnDrag,

        /// <summary>
        /// 开始拖动
        /// </summary>
        OnBeginDrag,

        /// <summary>
        /// 开始拖动
        /// </summary>
        OnEndDrag
    }
}