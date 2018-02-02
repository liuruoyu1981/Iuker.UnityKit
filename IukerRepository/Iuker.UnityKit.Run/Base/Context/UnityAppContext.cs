/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/02/27 11:29:04
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
using UnityEngine;

namespace Iuker.UnityKit.Run.Base.Context
{
    /// <summary>
    /// App运行上下文
    /// </summary>
    public class UnityAppContext
    {
        public U3dAppRunMode RunMode { get; set; }

        public U3dAppStatus AppStatus { get; set; }

        /// <summary>
        /// 视图上下文对象
        /// </summary>
        public ViewContext ViewContext = new ViewContext();

        /// <summary>
        /// 当前所在的子项目
        /// </summary>
        public string CurrentSonProject;

        private DevelopContextType mDevelopContextType;
        public DevelopContextType DevelopContextType
        {
            get
            {
                if (mDevelopContextType != DevelopContextType.None) return mDevelopContextType;

                if (Bootstrap.Instance.IsAssetBundleLoad
                    || Application.platform == RuntimePlatform.Android
                    || Application.platform == RuntimePlatform.IPhonePlayer
                    || Application.platform == RuntimePlatform.OSXPlayer
                    || Application.platform == RuntimePlatform.LinuxPlayer
                    || Application.platform == RuntimePlatform.WindowsPlayer)
                {
                    mDevelopContextType = DevelopContextType.Device;
                    return mDevelopContextType;
                }

                mDevelopContextType = DevelopContextType.Editor;
                return mDevelopContextType;
            }
        }

        public List<string> CsManagerTypes = new List<string>();


    }
}
