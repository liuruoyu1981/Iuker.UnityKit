using System.Collections.Generic;
using Iuker.Common;
using Iuker.Common.Base;
using Iuker.Jint.JsViewWidget;
using Iuker.UnityKit.Run.Module.JavaScript;
using Iuker.UnityKit.Run.Module.View.MVDA;
using Jint;
using Jint.Native;
using Jint.Runtime.Interop;
using UnityEngine;

namespace Iuker.Jint
{
#if DEBUG
    /// <summary>
    /// 可调度js脚本的视图模块
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20170930 14:05:44")]
    [ClassPurposeDesc("可调度js脚本的视图模块", "可调度js脚本的视图模块")]
#endif
    public class U3dJintModule_View : DefaultU3dModule_View
    {
        private IU3dJavaScriptModule mJsModule;
        private Engine mEngine;

        protected override void onFrameInited()
        {
            base.onFrameInited();

            mJsModule = U3DFrame.GetModule<IU3dJavaScriptModule>();
            mEngine = mJsModule.Engine.As<Engine>();
        }

        public override void CreateView(string viewId, string assetid = null, bool isCache = true)
        {
            assetid = assetid ?? viewId;
            var jintId = viewId + "_jint";
            if (!mJsModule.Exist(jintId))
            {
                base.CreateView(viewId, assetid, isCache);
            }
            else
            {
#if UNITY_EDITOR || DEBUG
                Debug.Log(string.Format("发现Jint视图脚本{0}存在，将创建Jint视图！", jintId));
#endif
                var viewRef = LoadView(assetid, isCache);
                var v = new JintView();
                SetJintView(viewId, v.As<JintView>());
                v.Init(viewId, viewRef);
            }
        }

        public void WatchViewLifeEvent(string eventId, string viewLifeType, JsValue processer)
        {
            WatchViewLiefEvent(eventId, viewLifeType.AsEnum<ViewLifeEventType>(), view =>
             {
                 var args = new ObjectWrapper(mEngine, view);
                 processer.Invoke(args);
             });
        }

        #region JintView

        private readonly Dictionary<string, JintView> mJintViews = new Dictionary<string, JintView>();

        private void SetJintView(string key, JintView view)
        {
            if (!mJintViews.ContainsKey(key))
            {
                mJintViews.Add(key, view);
            }
        }

        public JintView GetJintView(string key)
        {
            try
            {
                return mJintViews[key];
            }
            catch
            {
                return null;
            }
        }

        public override void CloseView(string viewId)
        {
            base.CloseView(viewId);

            if (mJintViews.ContainsKey(viewId))
            {
                mJintViews.Remove(viewId);
            }
        }

        public void OpenDialogByJint(string content, string dialogViewId,
            JsValue jsFunc, bool isShowCancel = false)
        {
            OpenDialog(content, dialogViewId, () =>
            {
                jsFunc.Invoke();
            }, isShowCancel);
        }

        #endregion

    }
}
