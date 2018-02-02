/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/02/28 12:52:14
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
    public enum InputEventType
    {
        /// <summary>
        /// 鼠标左键按下
        /// </summary>
        LeftMouseDown,

        /// <summary>
        /// 鼠标右键按下
        /// </summary>
        RightMouseDown,

        /// <summary>
        /// 鼠标中键按下
        /// </summary>
        MiddleMouseDown,

        /// <summary>
        /// 鼠标左键抬起
        /// </summary>
        LeftMouseUp,

        /// <summary>
        /// 鼠标右键抬起
        /// </summary>
        RightMouseUp,

        /// <summary>
        /// 鼠标中键抬起
        /// </summary>
        MiddleMouseUp,

        /// <summary>
        /// 点击按下鼠标左键按下和触摸屏单指点击都会触发
        /// </summary>
        Click,

        /// <summary>
        /// 快速两次点击坐标左键或快速单指触摸屏幕都会触发
        /// </summary>
        DoubleClick,

        /// <summary>
        /// 按压开始鼠标左键开始按压和触摸屏单指开始按压都会触发
        /// </summary>
        PressStart,

        /// <summary>
        /// 按压超过一定时间触发并只触发一次
        /// </summary>
        LongPressStart,

        /// <summary>
        /// 按压鼠标左键保持按压和触摸屏单指保持按压都会触发
        /// </summary>
        Press,

        /// <summary>
        /// 一指触摸
        /// </summary>
        OneTap,

        /// <summary>
        /// 拖动开始这个事件鼠标或手指拖动都会触发
        /// </summary>
        DragStart,

        /// <summary>
        /// 拖动这个事件鼠标或手指拖动都会触发
        /// </summary>
        Drag,

        /// <summary>
        /// 拖动这个事件鼠标或手指拖动都会触发
        /// </summary>
        DragEnd,
    }

}
