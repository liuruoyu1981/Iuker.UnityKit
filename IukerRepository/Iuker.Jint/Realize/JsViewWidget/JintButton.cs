using System;
using System.Collections.Generic;
using Iuker.Common;
using Iuker.Common.Base;
using Iuker.UnityKit.Run.Base;
using Iuker.UnityKit.Run.Base.Context;
using Iuker.UnityKit.Run.Module.JavaScript;
using Iuker.UnityKit.Run.Module.View.MVDA;
using Iuker.UnityKit.Run.Module.View.ViewWidget;
using Jint.Native;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Iuker.Jint.JsViewWidget
{
#if DEBUG
    /// <summary>
    /// 用于Jint（js）脚本环境的按钮代理
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20170930 16:41:03")]
    [ClassPurposeDesc("用于Jint（js）脚本环境的按钮代理", "用于Jint（js）脚本环境的按钮代理")]
#endif
    public class JintButton : IButton
    {
        #region 字段

        public GameObject DependentGo
        {
            get
            {
                return mButton.DependentGo;
            }
        }

        public GameObject GetDependentGo()
        {
            return DependentGo;
        }

        public GameObject ViewRoot
        {
            get
            {
                return mButton.ViewRoot;
            }
        }

        public string WidgetToken
        {
            get
            {
                return mButton.WidgetToken;
            }
        }

        public IView AttachView
        {
            get
            {
                return mButton.AttachView;
            }
        }

        public JintButton(IButton button) { mButton = button; }
        private JsValue mJsProcesser;
        private readonly IButton mButton;
        private IU3dJavaScriptModule mJsModule;
        private IU3dFrame mU3DFrame;
        public string ImageName
        {
            get { return mButton.ImageName; }
            set { mButton.ImageName = value; }
        }

        public string RawImageName
        {
            get
            {
                return mButton.RawImageName;
            }
            set
            {
                mButton.RawImageName = value;
            }
        }

        public PointerEventData PointerEventData
        {
            get
            {
                return mButton.PointerEventData;
            }
        }

        #endregion

        #region 基类实现
        public IukViewWidget Init(IU3dFrame u3DFrame, IView view, IFragment fragment = null)
        {
            mU3DFrame = u3DFrame;
            mJsModule = mU3DFrame.GetModule<IU3dJavaScriptModule>();

            //            BindingJintAction(ButtonActionType.OnPointerClick, WidgetToken + "_onclick_jint");
            BindingJintAction(ButtonActionType.OnClick, WidgetToken + "_onclick_jint");

            return this;
        }

        #endregion

        #region JavaScript特性

        private readonly Dictionary<ButtonActionType, JsValue> mJsActions = new Dictionary<ButtonActionType, JsValue>();
        private readonly Dictionary<ButtonActionType, string> mJsNames = new Dictionary<ButtonActionType, string>();

        private void BindingJintAction(ButtonActionType type, string jintId)
        {
            if (!mJsModule.Exist(jintId) || mJsActions.ContainsKey(type)) return;

            mJsNames.Add(type, jintId);
            var isDo = LoadJsFile(type, jintId);

            BindingHotAction(type, () =>
            {
#if UNITY_EDITOR || DEBUG

                if (mU3DFrame.AppContext.DevelopContextType == DevelopContextType.Editor)
                {
                    if (mJsModule.IsMd5Change(mJsNames[type]))
                    {
                        mJsModule.DoFile(jintId);
                        LoadJsFile(type, jintId);
                    }
                }

                var outPutStr = "[JS] " + WidgetToken + string.Format(" is {0}!", type.ToString());
                Debug.Log(outPutStr);
#endif
                mJsActions[type].InvokeByThis(mJsProcesser);
            }, isDo);
        }

        bool LoadJsFile(ButtonActionType type, string jintId)
        {
            if (mJsActions.ContainsKey(type)) mJsActions.Remove(type);

            var code = string.Format("Iuker.ViewModule.ActionProcessers.{0} = new {1}.{0}();", jintId,
                mU3DFrame.AppContext.CurrentSonProject);
            mJsModule.DoString(code);
            var jsIuker = mJsModule.GetGlobalValue("Iuker").As<JsValue>();
            var jsViewModule = jsIuker.GetJsValue("ViewModule");
            var processers = jsViewModule.GetJsValue("ActionProcessers");
            mJsProcesser = processers.GetJsValue(jintId);

            var iD = mJsProcesser.GetJsValue("IsDoCsharp");
            var tmIsDo = iD.Invoke().AsBoolean();
            var initFunc = mJsProcesser.GetJsValue("Init");
#if UNITY_EDITOR
            if (initFunc == "undifined")
            {
                Debug.LogError(string.Format("Js脚本{0}的初始化函数Init为空！", jintId));
            }
#endif

            initFunc.InvokeByThis(mJsProcesser);
            var pQFunc = mJsProcesser.GetJsValue("ProcessRequest");
            mJsActions.Add(type, pQFunc);
            return tmIsDo;
        }

        #endregion

        public void BindingHotAction(ButtonActionType type, Action action, bool isDoCsharp = true)
        {
            mButton.BindingHotAction(type, action, isDoCsharp);
        }





    }
}
