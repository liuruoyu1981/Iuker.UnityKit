/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/02/27 21:48:08
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

using System;
using Iuker.Common;
using Iuker.Common.Base.Interfaces;

namespace Iuker.UnityKit.Run.Base
{
    /// <summary>
    /// unity3d模块
    /// </summary>
    public abstract class AbsU3dModule : AbsModule
    {
        protected IU3dFrame U3DFrame { get; private set; }
        public override void Init(IFrame frame)
        {
            U3DFrame = (IU3dFrame)frame;
            if (U3DFrame == null)
                throw new NullReferenceException("U3DFrame");

            U3DFrame.EventModule.WatchEvent(U3dEventCode.Frame_Inited.Literals, onFrameInited); // 注册框架启动完毕
            U3DFrame.EventModule.WatchEvent(U3dEventCode.App_HotUpdateComplete.Literals, OnHotUpdateComplete); // 注册资源热更完成事件
            RegisterEvent();    // 注册子类关注事件
        }

    }
}
