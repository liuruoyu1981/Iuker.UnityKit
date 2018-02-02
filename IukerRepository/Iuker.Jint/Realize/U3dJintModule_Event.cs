/***********************************************************************************************
/*  Author：        liuruoyu1981
/*  CreateDate:     2017/11/26 下午 04:42:44 
/*  Email:          35490136@qq.com
/*  QQCode:         35490136
/*	Machine:		DESKTOP-M1OBR70
/*  CreateNote: 
***********************************************************************************************/


/****************************************修改日志***********************************************
/* 	1. 修改日期： 修改人： 修改内容：
/* 	2. 修改日期： 修改人： 修改内容：
/* 	3. 修改日期： 修改人： 修改内容：
/* 	4. 修改日期： 修改人： 修改内容：
/* 	5. 修改日期： 修改人： 修改内容：
****************************************修改日志***********************************************/


using Iuker.Common;
using Iuker.UnityKit.Run.Module.Event;
using Iuker.UnityKit.Run.Module.JavaScript;
using Jint;
using Jint.Native;
using Jint.Runtime.Interop;

namespace Iuker.Jint
{
    public class U3dJintModule_Event : DefaultU3dModule_Event
    {
        private IU3dJavaScriptModule mJsModule;
        private Engine mEngine;

        protected override void OnHotUpdateComplete()
        {
            base.OnHotUpdateComplete();

            mJsModule = U3DFrame.GetModule<IU3dJavaScriptModule>();
            mEngine = mJsModule.Engine.As<Engine>();
        }

        public void WatchEventByJint(string eventId, JsValue processer, int num = -1)
        {
            WatchEvent(eventId, () =>
            {
                processer.Invoke();
            }, num);
        }

        public void WatchEventByJintAsData(string eventId, JsValue processer, int num = -1)
        {
            WatchEvent(eventId, data =>
            {
                var args = new ObjectWrapper(mEngine, data);
                processer.Invoke(args);
            }, num);
        }

        public void IssueEventByJint(string eventId, JsValue complete, object data)
        {
            IssueEvent(eventId, () =>
            {
                if (complete.ToObject() == null) return;

                complete.Invoke();
            }, data);
        }


    }
}
