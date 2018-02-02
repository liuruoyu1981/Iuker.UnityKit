/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/06/10 14:35
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
using UnityEngine.UI;

namespace Iuker.UnityKit.Run.Module.View.WidgetCreaters
{
    /// <summary>
    /// 选项卡创建器
    /// </summary>
    public class DropdownCreater
    {
        public static IDropdown Create()
        {
            GameObject source = new GameObject("dropdown_" + Constant.GetTimeToken);
            var dropdown = source.AddComponent<IukDropdown>();
            dropdown.gameObject.AddComponent<IukImage>();
            dropdown.targetGraphic = dropdown.GetComponent<IukImage>();
            var label = dropdown.AddChild("Label").AddComponent<IukText>();
            label.Text = "Option A";
            dropdown.captionText = label.gameObject.GetComponent<IukText>();
            var arrow = dropdown.AddChild("Arrow").AddComponent<IukImage>();


            var template = dropdown.AddChild("Template").AddComponent<IukImage>().gameObject.AddComponent<ScrollRect>();
            dropdown.template = template.GetComponent<RectTransform>();
            var viewport = template.AddChild("Viewport").AddComponent<Mask>().gameObject.AddComponent<IukImage>();
            template.viewport = viewport.GetComponent<RectTransform>();
            var contetn = viewport.AddChild("Content").AddComponent<RectTransform>();
            template.content = contetn.GetComponent<RectTransform>();
            var item = contetn.gameObject.AddChild("Item").AddComponent<IukToggle>();
            var itemBackground = item.AddChild("Item Background").AddComponent<IukImage>();
            var itemCheckmark = item.AddChild("Item Checkmark").AddComponent<IukImage>();
            var itemLabel = item.AddChild("Item Label").AddComponent<IukText>();
            item.targetGraphic = itemBackground.GetComponent<IukImage>();
            item.graphic = itemCheckmark.GetComponent<IukImage>();

            // scrollbar
            var scrollbar = template.AddChild("Scrollbar").AddComponent<IukImage>().gameObject.AddComponent<Scrollbar>();
            template.verticalScrollbar = scrollbar.GetComponent<Scrollbar>();
            template.AddChild("Sliding Area").gameObject.AddChild("Handle").AddComponent<IukImage>();




            return dropdown;
        }










    }
}