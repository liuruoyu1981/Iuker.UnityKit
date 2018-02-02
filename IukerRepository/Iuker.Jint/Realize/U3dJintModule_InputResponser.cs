/***********************************************************************************************
/*  Author：        liuruoyu1981
/*  CreateDate:     2017/12/19 下午 08:31:30 
/*  Email:          35490136@qq.com
/*  QQCode:         35490136
/*	Machine:		DESKTOP-M1OBR70
/*  CreateNote: 
***********************************************************************************************/

using System;
using System.Collections.Generic;
using Iuker.Common;
using Iuker.UnityKit.Run.Module.InputResponse;
using Jint.Native;
using UnityEngine;

namespace Iuker.Jint
{
    public class U3dJintModule_InputResponser : DefaultU3dModule_InputResponse
    {
        private readonly Dictionary<string, JintInputProcesserProxy> mInputProxys = new Dictionary<string, JintInputProcesserProxy>();

        public void WatchInputEventByJint(string name, string type, JsValue jsFunction, int executeCount)
        {
            if (mInputProxys.ContainsKey(name))
                throw new Exception(string.Format("已经存在相同名字{0}的输入响应代理！", name));

            var proxy = new JintInputProcesserProxy(jsFunction);
            mInputProxys.Add(name, proxy);
            var inputEventType = type.AsEnum<InputEventType>();
            WatchInputEvent(inputEventType, proxy.ExecuteProxy, executeCount);
        }

        public void RemoveInputEventByJint(string name, string typeStr)
        {
            if (!mInputProxys.ContainsKey(name))
            {
#if UNITY_EDITOR || DEBUG
                Debug.Log(string.Format("不存在名为{0}的输入响应代理！", name));
#endif
                return;
            }

            var proxy = mInputProxys[name];
            var inputEventType = typeStr.AsEnum<InputEventType>();
            mInputProxys.Remove(name);
            RemoveInputEvent(inputEventType, proxy.ExecuteProxy);
        }

        private class JintInputProcesserProxy
        {
            private JsValue mJsFunction;
            public void ExecuteProxy()
            {
                mJsFunction.Invoke();
            }

            public JintInputProcesserProxy(JsValue jsFunction)
            {
                mJsFunction = jsFunction;
            }
        }

    }
}
