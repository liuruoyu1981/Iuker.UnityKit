/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/06/12 13:57
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

using Iuker.Common.Constant;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Iuker.UnityKit.Run.Module.View.WidgetCreaters
{
    /// <summary>
    /// 滑动杆创建器 
    /// </summary>
    public class SliderCreater
    {
        public static GameObject Create()
        {
            var sliderTemplate = Resources.Load<GameObject>("iuker_slider");
            var slider = Object.Instantiate(sliderTemplate);
            slider.name = string.Format("slider_{0}", Constant.GetTimeToken);
            return slider;
        }


    }
}