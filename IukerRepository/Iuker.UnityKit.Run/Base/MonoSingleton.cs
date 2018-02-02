/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/02/15 15:55:32
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
using UnityEngine;


namespace Iuker.UnityKit.Run
{
    /// <summary>
    /// MonoComponent组件单例类
    /// </summary>
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        [ThreadStatic]
        private static T mInstance;

        /// <summary>
        /// mono单例对象挂载根对象名
        /// </summary>
        private static readonly string mountRootName = "Iuker.UnityKit";

        /// <summary>
        /// 获取一个Mono类型的单例对象
        /// </summary>
        public static T Instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = GameObject.FindObjectOfType(typeof(T)) as T;
                    if (mInstance == null)
                    {
                        mInstance = new GameObject().AddComponent<T>();
                        mInstance.gameObject.name = mInstance.GetType().Name;
                    }
                }

                if (mInstance.gameObject.transform.parent != null) return mInstance;
                var monoRoot = GameObject.Find(mountRootName);
                if (monoRoot == null)
                {
                    monoRoot = new GameObject();
                    monoRoot.gameObject.name = mountRootName;
                    mInstance.transform.parent = monoRoot.transform;
                }
                else
                {
                    mInstance.transform.parent = monoRoot.transform;
                }

                return mInstance;
            }
        }

    }
}
