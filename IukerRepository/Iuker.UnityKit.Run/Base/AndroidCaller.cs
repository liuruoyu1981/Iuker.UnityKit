using UnityEngine;

namespace Iuker.UnityKit.Run
{
    /// <summary>
    /// 原生安卓调用器
    /// </summary>
    public static class AndroidCaller
    {
        public enum ToastCost
        {
            /// <summary>
            /// 安卓原生长时间提示框
            /// </summary>
            LENGTH_LONG,

            /// <summary>
            /// 安卓原生长时间提示框
            /// </summary>
            LENGTH_SHORT,
        }

        /// <summary>
        /// Unity安卓接口对象
        /// </summary>
        private static AndroidJavaClass UnityPlayer
        {
            get
            {
                return new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            }
        }

        /// <summary>
        /// 当前的安卓活动
        /// </summary>
        private static AndroidJavaObject CurrentActivity
        {
            get
            {
                return UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            }
        }

        /// <summary>
        /// 安卓常用方法桥接对象
        /// </summary>
        private static AndroidJavaObject AndroidBridge
        {
            get
            {
                return new AndroidJavaObject("net.iuker.unitykits.IukerAndroidBridge");
            }
        }

        /// <summary>
        /// 弹出一个安卓原生的提示文字
        /// </summary>
        /// <param name="activity">安卓活动引用</param>
        /// <param name="message">要提示的文字</param>
        /// <param name="toastCost">显示时间长短</param>
        public static void makeText(string message, ToastCost toastCost = ToastCost.LENGTH_SHORT,
            AndroidJavaObject activity = null)
        {
            AndroidBridge.Call("makeText", message, activity ?? CurrentActivity, toastCost.ToString());
        }

        public static bool InstallAPK(string path, bool bReTry)
        {
            try
            {
                var interntClass = new AndroidJavaClass("android.content.Intent");
                var actionView = interntClass.GetStatic<string>("ACTION_VIEW");
                var flagActivityNewTask = interntClass.GetStatic<int>("FLAG_ACTIVITY_NEW_TASK");
                var intent = new AndroidJavaObject("android.content.Intent", actionView);

                var file = new AndroidJavaObject("java.io.File", path);
                var urlClass = new AndroidJavaClass("android.net.Uri");
                var uri = urlClass.CallStatic<AndroidJavaObject>("fromFile", file);

                intent.Call<AndroidJavaObject>("setDataAndType", uri, "application/vnd.android.package-archive");

                if (!bReTry)
                {
                    intent.Call<AndroidJavaObject>("addFlags", flagActivityNewTask);
                    intent.Call<AndroidJavaObject>("setClassName", "com.android.packageinstaller", "com.android.packageinstaller.PackageInstallerActivity");
                }

                var playerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                var currentActivity = playerClass.GetStatic<AndroidJavaObject>("currentActivity");
                currentActivity.Call("startActivity", intent);

                Debug.Log("Install New Apk Ok");
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error Install APK:" + e.Message + " -- " + e.StackTrace + "  bRetry=" + bReTry);
                return false;
            }
        }


    }

}
