/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/06/10 05:52:13
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
    /// 输入框创建器
    /// </summary>
    public class InputFieldCreater
    {
        public static IInputField Create()
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






    }
}