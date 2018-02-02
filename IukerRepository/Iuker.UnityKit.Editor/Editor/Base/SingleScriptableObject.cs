using Iuker.Common.Base;
using UnityEngine;

namespace Iuker.UnityKit.Editor.Base
{
#if DEBUG
    /// <summary>
    /// 单例脚本
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20170927 07:34:53")]
    [ClassPurposeDesc("单例脚本", "单例脚本")]
#endif
    public class SingleScriptableObject<T> : ScriptableObject where T : ScriptableObject
    {
        private static T mInstance;

        public static T Instance
        {
            get
            {
                return mInstance ?? (mInstance = CreateInstance<T>());
            }
        }

    }
}
