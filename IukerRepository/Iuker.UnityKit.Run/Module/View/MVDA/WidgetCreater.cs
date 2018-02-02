/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/04/18 23:23:59
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

namespace Iuker.UnityKit.Run.Module.View.MVDA
{
    /// <summary>
    /// 视图控件创建器
    /// </summary>
    public static class WidgetCreater
    {
        public static IImage CreateImage()
        {
            GameObject source = new GameObject("image_" + Constant.GetTimeToken);
            var image = source.AddComponent<IukImage>();
            return image;
        }

        public static IInputField CreateInputField()
        {
            GameObject source = new GameObject("inputfield_" + Constant.GetTimeToken);
            var inputField = source.AddComponent<IukInputField>();
            var image = source.AddComponent<IukImage>();
            var text = source.AddChild("Text").AddComponent<IukText>();
            text.supportRichText = false;
            var placeholderText = source.AddChild("Placeholder").AddComponent<IukText>();
            inputField.textComponent = text;
            inputField.targetGraphic = image;
            inputField.placeholder = placeholderText;
            inputField.transform.localScale = Vector3.one;

            return inputField;
        }

        public static IButton CreateButton()
        {
            GameObject source = new GameObject("button_" + Constant.GetTimeToken);
            source.AddComponent<IukImage>();
            var button = source.AddComponent<IukButton>();
            source.AddChild("Text").AddComponent<IukText>();

            return button;
        }












    }
}
