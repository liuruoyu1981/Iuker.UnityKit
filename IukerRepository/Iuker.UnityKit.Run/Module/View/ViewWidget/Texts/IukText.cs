/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/02/17 08:46:42
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

using System.Collections.Generic;
using System.Linq;
using Iuker.UnityKit.Run.Base;
using Iuker.UnityKit.Run.Module.Il8n;
using Iuker.UnityKit.Run.Module.View.MVDA;
using Iuker.UnityKit.Run.Module.View.ViewWidget;
using UnityEngine;
using UnityEngine.UI;

namespace Iuker.UnityKit.Run.ViewWidget
{
    /// <summary>
    /// 支持多语言的Text文字组件
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    [RequireComponent(typeof(Outline))]
    [RequireComponent(typeof(Shadow))]
    // ReSharper disable once ClassNeverInstantiated.Global
    public class IukText : Text, IText, IViewActionRequester<IText>
    {
        private IU3dIl8nModule Il8NModule;

        protected IU3dFrame U3DFrame { get; private set; }

        public IViewActionDispatcher Dispatcher { get; protected set; }

        public IView AttachView { get; protected set; }

        public string ViewId { get { return ViewRoot.name; } }

        /// <summary>
        /// 主体文本内容在多语言数据列表中的索引键值
        /// </summary>
        private int bodyIndex;

        /// <summary>
        /// 文本占位符索引列表
        /// </summary>
        private readonly List<int> placeholderIndexList = new List<int>();

        /// <summary>
        /// 主体文本内容
        /// </summary>
        private string BodyText { get; set; }

        protected GameObject _viewRoot;
        public GameObject ViewRoot { get { return _viewRoot; } }

        public IukViewWidget Init(IU3dFrame u3DFrame, IView view, IFragment fragment = null)
        {
            U3DFrame = u3DFrame;
            Dispatcher = u3DFrame.ViewModule.ViewActionispatcher;
            AttachView = view;
            Il8NModule = U3DFrame.Il8NModule;

            return this;
        }

        public string WidgetToken { get; set; }

        /// <summary>
        /// 视图组件是否已经初始化
        /// </summary>
        private bool isInited;

        /// <summary>
        /// 文本占位符列表
        /// </summary>
        private List<string> PlaceholderArray { get; set; }

        /// <summary>
        /// 修改显示的文字内容并响应多语言事件
        /// </summary>
        /// <param name="bodyText"></param>
        /// <param name="placeholderArray"></param>
        public void SetIl8nText(string bodyText, params string[] placeholderArray)
        {
            // 绑定代码中传入的基础值
            BodyText = bodyText;
            PlaceholderArray = placeholderArray.ToList();
            // 获取主体文本在多语言数据中的索引
            bodyIndex = Il8NModule.GetIndex(BodyText);

            foreach (string placeholder in placeholderArray)
            {
                // 获取占位符文本在多语言数据中的索引
                var tempIndex = Il8NModule.GetIndex(placeholder);
                placeholderIndexList.Add(tempIndex);
            }
            UpdateIl8nCurrentVersion();

            Bootstrap.U3DFrame.EventModule.WatchEvent(U3dEventCode.App_Il8nChange.Literals, OnIl8nChange);
        }

        /// <summary>
        /// 内部UIlabel组件的游戏对象是否激活
        /// </summary>
        //public bool ActiveSelf { get { return mText.gameObject.activeSelf; } }

        private void UpateText(string[] placeholderArray)
        {
            switch (placeholderArray.Length)
            {
                case 0:
                    text = BodyText;
                    break;
                case 1:
                    UpdateTextBy1Placeholder(placeholderArray);
                    break;
                case 2:
                    UpdateTextBy2Placeholder(placeholderArray);
                    break;
                case 3:
                    UpdateTextBy3Placeholder(placeholderArray);
                    break;
                case 4:
                    UpdateTextBy4Placeholder(placeholderArray);
                    break;
                case 5:
                    UpdateTextBy5Placeholder(placeholderArray);
                    break;
                case 6:
                    UpdateTextBy6Placeholder(placeholderArray);
                    break;
                case 7:
                    UpdateTextBy7Placeholder(placeholderArray);
                    break;
                case 8:
                    UpdateTextBy8Placeholder(placeholderArray);
                    break;
                case 9:
                    UpdateTextBy9Placeholder(placeholderArray);
                    break;
                default:
                    Debuger.LogWarning("没有找到符合个数的处理函数");
                    break;
            }
        }

        #region 占位符多语言格式化

        private void UpdateTextBy1Placeholder(IList<string> placeholderArray)
        {
            text = string.Format(BodyText, placeholderArray[0]);
        }
        private void UpdateTextBy2Placeholder(IList<string> placeholderArray)
        {
            text = string.Format(BodyText, placeholderArray[0], placeholderArray[1]);
        }

        private void UpdateTextBy3Placeholder(IList<string> placeholderArray)
        {
            text = string.Format(BodyText, placeholderArray[0], placeholderArray[1], placeholderArray[2]);
        }

        private void UpdateTextBy4Placeholder(IList<string> placeholderArray)
        {
            text = string.Format(BodyText, placeholderArray[0], placeholderArray[1], placeholderArray[2], placeholderArray[3]);
        }

        private void UpdateTextBy5Placeholder(IList<string> placeholderArray)
        {
            text = string.Format(BodyText, placeholderArray[0], placeholderArray[1], placeholderArray[2], placeholderArray[3],
                placeholderArray[4]);
        }

        private void UpdateTextBy6Placeholder(IList<string> placeholderArray)
        {
            text = string.Format(BodyText, placeholderArray[0], placeholderArray[1], placeholderArray[2], placeholderArray[3],
                placeholderArray[4], placeholderArray[5]);
        }

        private void UpdateTextBy7Placeholder(IList<string> placeholderArray)
        {
            text = string.Format(BodyText, placeholderArray[0], placeholderArray[1], placeholderArray[2], placeholderArray[3],
                placeholderArray[4], placeholderArray[5], placeholderArray[6]);
        }

        private void UpdateTextBy8Placeholder(IList<string> placeholderArray)
        {
            text = string.Format(BodyText, placeholderArray[0], placeholderArray[1], placeholderArray[2], placeholderArray[3],
                placeholderArray[4], placeholderArray[5], placeholderArray[6], placeholderArray[7]);
        }

        private void UpdateTextBy9Placeholder(IList<string> placeholderArray)
        {
            text = string.Format(BodyText, placeholderArray[0], placeholderArray[1], placeholderArray[2], placeholderArray[3],
                placeholderArray[4], placeholderArray[5], placeholderArray[6], placeholderArray[7], placeholderArray[8]);
        }


        #endregion

        private void OnIl8nChange()
        {
            UpdateIl8nCurrentVersion();
            UpateText(PlaceholderArray.ToArray());
        }

        /// <summary>
        /// 更新内部的Text组件显示的内容为当前的多语言版本
        /// </summary>
        private void UpdateIl8nCurrentVersion()
        {
            var bodyValue = Il8NModule.GetTextByIndex(bodyIndex);
            if (bodyValue != null)
            {
                BodyText = bodyValue;
            }

            for (int i = 0, num = placeholderIndexList.Count; i < num; i++)
            {
                int index = placeholderIndexList[i];
                var result = Il8NModule.GetTextByIndex(index);
                if (result != null) PlaceholderArray[i] = result;
            }
            UpateText(PlaceholderArray.ToArray());
        }

        public string InstanceName { get { return gameObject.name + "_RyText"; } }

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public GameObject DependentGo { get { return gameObject; } }

        public IText Origin { get { return this; } }

        public void Issue(IViewActionRequest<IText> request)
        {
            Dispatcher.DispatchRequest(request);
        }

        public string Text
        {
            get { return text; }
            set
            {
                text = value;
            }
        }

        /// <summary>
        /// 文本控件的颜色
        /// </summary>
        public Color Color
        {
            get { return color; }
            set { color = value; }
        }
    }
}
