/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/06/10 05:51:34
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
using Iuker.UnityKit.Run.Module.View.ViewWidget;
using Iuker.UnityKit.Run.ViewWidget;
using UnityEngine;

namespace Iuker.UnityKit.Run.Module.View.WidgetCreaters
{
    /// <summary>
    /// 按钮创建器 
    /// </summary>
    public class ButtonCreater
    {
        public static IButton Create()
        {
            GameObject source = new GameObject("button_" + Constant.GetTimeToken);
            source.AddComponent<IukImage>();
            var button = source.AddComponent<IukButton>();
            source.AddChild("Text").AddComponent<IukText>();


            return button;
        }


    }
}