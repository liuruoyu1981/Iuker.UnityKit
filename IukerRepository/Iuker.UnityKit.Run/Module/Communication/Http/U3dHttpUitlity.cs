using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Iuker.Common;
using Iuker.Common.Base;
using Iuker.Common.Base.Interfaces;
using Iuker.Common.Utility;
using UnityEngine;
using UnityEngine.Networking;

namespace Iuker.UnityKit.Run.Module.Communication.Http
{
#if DEBUG
    /// <summary>
    /// Unity3dHttp工具
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20170912 16:28:56")]
    [ClassPurposeDesc("Unity3dHttp工具", "Unity3dHttp工具")]
#endif
    // ReSharper disable once ClassNeverInstantiated.Global
    public class U3dHttpUitlity : MonoSingleton<U3dHttpUitlity>
    {
        private readonly List<Action> mUpdateActions = new List<Action>();
        private static readonly ObjectPool<U3dHttpAsyncObject> mU3dHttpAsyncObjPool = new ObjectPool<U3dHttpAsyncObject>(new U3dHttpAsyncObjectFactory(), 10);

        private class U3dHttpAsyncObjectFactory : IObjectFactory<U3dHttpAsyncObject>
        {
            public U3dHttpAsyncObject CreateObject()
            {
                return new U3dHttpAsyncObject();
            }
        }

        private void RecycleU3dAsyncObject(U3dHttpAsyncObject asyncObject)
        {
            mUpdateActions.Remove(asyncObject.Update);
            mU3dHttpAsyncObjPool.Restore(asyncObject);
        }

        private void Update()
        {
            mUpdateActions.ForEach(del => del());
        }

        #region Get

        private static void GetByteArray(string url, Action<byte[]> onDowned, Action<float> onProgressUpdate = null)
        {
            var asyncObj = mU3dHttpAsyncObjPool.Take();
            Instance.StartCoroutine(asyncObj.GetArrayByte(url, onDowned, onProgressUpdate));
            Instance.mUpdateActions.Add(asyncObj.Update);
        }

        /// <summary>
        /// 从远程http服务器下载一个文件到本地地址
        /// </summary>
        /// <param name="remoteUrl"></param>
        /// <param name="localUrl"></param>
        /// <param name="onCompleted"></param>
        /// <param name="onProgressUpdate"></param>
        public static void GetFile(string remoteUrl, string localUrl, Action onCompleted = null, Action<float> onProgressUpdate = null)
        {
            FileUtility.EnsureDirExist(localUrl);
            GetByteArray(remoteUrl, bytes =>
            {
                FileUtility.EnsureDirExist(localUrl);
                File.WriteAllBytes(localUrl, bytes);
                if (onCompleted != null)
                {
                    onCompleted();
                }
            }, onProgressUpdate);
        }

        public static IEnumerator GetFileByCoroutine(string remote, string local, Action complete)
        {
            var asyncObj = mU3dHttpAsyncObjPool.Take();
            yield return Instance.StartCoroutine(asyncObj.GetFile(remote, local, complete));
        }

        /// <summary>
        /// 从目标地址获得一个Json对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="onGeted"></param>
        public static void GetJson<T>(string url, Action<T> onGeted)
        {
            var asyncObj = mU3dHttpAsyncObjPool.Take();

            Instance.StartCoroutine(asyncObj.GetString(url, s =>
            {
                var jsonObj = JsonUtility.FromJson<T>(s);
                onGeted(jsonObj);
            }));
        }

        /// <summary>
        /// 从目标地址获得一个字符串文本
        /// </summary>
        /// <param name="url"></param>
        /// <param name="onGeted"></param>
        //public static void GetString(string url, Action<string> onGeted)
        //{
        //    var asyncObj = mU3dHttpAsyncObjPool.Take();

        //    GetInstance.StartCoroutine(asyncObj.GetString(url, onGeted));
        //}

        #endregion

        #region Upload

        /// <summary>
        /// 向目标地址上传二进制数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="onUploaded"></param>
        public void UploadBytes(string url, byte[] data, Action onUploaded)
        {
            var asyncObj = mU3dHttpAsyncObjPool.Take();

            StartCoroutine(asyncObj.Put(url, data, onUploaded));
        }

        /// <summary>
        /// 向目标地址上传文件
        /// </summary>
        /// <param name="url"></param>
        /// <param name="filePath"></param>
        /// <param name="onUploaded"></param>
        public void UploadFile(string url, string filePath, Action onUploaded)
        {
            var asyncObj = mU3dHttpAsyncObjPool.Take();

            StartCoroutine(asyncObj.Put(url, File.ReadAllBytes(filePath), onUploaded));
        }


        #endregion

        #region 内部异步对象类

        private class U3dHttpAsyncObject
        {
            private UnityWebRequest mUnityWebRequest;
            private Action<float> mOnDownedUpdate;

            public IEnumerator GetArrayByte(string url, Action<byte[]> onDowned, Action<float> onProgressUpdate = null)
            {
                mOnDownedUpdate = onProgressUpdate;
                mUnityWebRequest = UnityWebRequest.Get(url);
                yield return mUnityWebRequest.Send();

                if (!string.IsNullOrEmpty(mUnityWebRequest.error))
                {
                    Debug.LogError(string.Format("下载目标URL：{0}出错，错误信息为：{1}！", url, mUnityWebRequest.error));
                    Instance.RecycleU3dAsyncObject(this);
                    yield break;
                }

                if (onDowned != null)
                {
                    onDowned(mUnityWebRequest.downloadHandler.data);
                }
                Instance.RecycleU3dAsyncObject(this);
            }

            public IEnumerator GetFile(string remote, string local, Action complete)
            {
                mUnityWebRequest = UnityWebRequest.Get(remote);
                yield return mUnityWebRequest.Send();

                if (!string.IsNullOrEmpty(mUnityWebRequest.error))
                    throw new Exception(mUnityWebRequest.error);

                File.WriteAllBytes(local, mUnityWebRequest.downloadHandler.data);
                complete();
            }

            public IEnumerator GetString(string url, Action<string> onDowned)
            {
                mUnityWebRequest = UnityWebRequest.Get(url);
                yield return mUnityWebRequest.Send();

                if (!string.IsNullOrEmpty(mUnityWebRequest.error))
                    throw new Exception(mUnityWebRequest.error);

                if (onDowned != null)
                {
                    onDowned(mUnityWebRequest.downloadHandler.text);
                }
                Instance.RecycleU3dAsyncObject(this);
            }

            public IEnumerator Put(string url, byte[] data, Action onPuted)
            {
                mUnityWebRequest = UnityWebRequest.Put(url, data);
                yield return mUnityWebRequest.Send();

                if (!string.IsNullOrEmpty(mUnityWebRequest.error))
                    throw new Exception(mUnityWebRequest.error);

                if (onPuted != null)
                {
                    onPuted();
                }
                Instance.RecycleU3dAsyncObject(this);
            }

            public void Update()
            {
                if (mOnDownedUpdate != null)
                {
                    mOnDownedUpdate(mUnityWebRequest.downloadProgress);
                }
            }
        }

        #endregion


    }
}
