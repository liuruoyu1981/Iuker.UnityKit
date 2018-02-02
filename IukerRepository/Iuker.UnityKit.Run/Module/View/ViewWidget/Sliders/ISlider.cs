/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/06/12 14:03
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
    /// 滑动杆接口
    /// </summary>
    public interface ISlider : IukViewWidget, IViewElement
    {
        /// <summary>
        /// 获取当前的进度值
        /// </summary>
        float CurrentValue { get; }

        /// <summary>
        /// 设置滑动条的进度值
        /// </summary>
        /// <param name="newValue"></param>
        void SetValue(float newValue);
    }
}