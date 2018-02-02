/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/3/26 上午9:48:05
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


using Iuker.UnityKit.Run.Module.View.MVDA;

namespace Iuker.UnityKit.Run.Module.View.ViewWidget
{
    /// <summary>
    /// 输入框视图控件
    /// </summary>
    public interface IInputField : IukViewWidget, IViewElement
    {
        /// <summary>
        /// 输入框指示文本
        /// </summary>
        string PlaceHolder { get; set; }

        /// <summary>
        /// 输入框指示文本
        /// 供脚本调用
        /// </summary>
        /// <returns></returns>
        string GetPlaceHolder();

        /// <summary>
        /// 输入框文本
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// 输入框文本
        /// 供脚本调用
        /// </summary>
        /// <returns></returns>
        string GetText();
    }
}
