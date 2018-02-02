/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 6/18/2017 11:23
Email: liuruoyu1981@gmail.com
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
using UnityEditor;
using UnityEngine;

namespace Iuker.UnityKit.Editor.Console.Core.Editor
{
    // ReSharper disable once ClassNeverInstantiated.Global
    /// <summary>
    /// Unity编辑器原生Api事件
    /// 编译完成
    /// 编译中
    /// 运行中
    /// </summary>
    public class UnityApiEvents : ScriptableObject
    {
        private bool _isCompiling;
        private bool _isPlaying;

        public event Action OnBeforeCompileEvent;
        public event Action OnAfterCompileEvent;
        public event Action OnBeginPlayEvent;
        public event Action OnStopPlayEvent;

        /// <summary>
        /// 获得UnityApiEvents实例
        /// 如果当前没有实例则会创建一个新实例
        /// </summary>
        /// <returns></returns>
        public static UnityApiEvents GetOrCreate() => FindObjectOfType<UnityApiEvents>() ?? CreateInstance<UnityApiEvents>();

        private void OnEnable()
        {
            // ReSharper disable once DelegateSubtraction
            EditorApplication.update -= Update;
            EditorApplication.update += Update;
            hideFlags = HideFlags.HideAndDontSave;
        }

        private void Update()
        {
            if (EditorApplication.isCompiling && !_isCompiling)
            {
                _isCompiling = true;
                OnBeforeCompile();
            }
            else if (!EditorApplication.isCompiling && _isCompiling)
            {
                _isCompiling = false;
                OnAfterCompile();
            }

            if (EditorApplication.isPlaying && !_isPlaying)
            {
                _isPlaying = true;
                OnBeginPlay();
            }
            else if (!EditorApplication.isPlaying && _isPlaying)
            {
                _isPlaying = false;
                OnStopPlay();
            }

        }

        private void OnBeforeCompile() => OnBeforeCompileEvent?.Invoke();

        private void OnAfterCompile() => OnAfterCompileEvent?.Invoke();

        private void OnBeginPlay() => OnBeginPlayEvent?.Invoke();

        private void OnStopPlay() => OnStopPlayEvent?.Invoke();
    }
}