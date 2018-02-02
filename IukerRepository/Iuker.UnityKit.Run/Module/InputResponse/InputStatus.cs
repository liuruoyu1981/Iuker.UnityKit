namespace Iuker.UnityKit.Run.Module.InputResponse
{
    /// <summary>
    /// 输入输出控制状态
    /// </summary>
    public enum InputStatus
    {
        /// <summary>
        /// 无状态
        /// </summary>
        None,

        /// <summary>
        /// 鼠标左键
        /// </summary>
        LeftMouseDown,

        /// <summary>
        /// 鼠标右键
        /// </summary>
        RightMouseDown,

        /// <summary>
        /// 鼠标中键
        /// </summary>
        MiddleMouseDown,

        /// <summary>
        /// 按压鼠标左键保持按压和触摸屏单指保持按压都会触发
        /// </summary>
        Press,

        /// <summary>
        /// 拖动
        /// </summary>
        Drag,

    }
}