using System;
using System.Collections.Generic;
using Iuker.Common.Base;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace Iuker.UnityKit.Editor.Base
{
#if DEBUG
    /// <summary>
    /// 编辑器事件广播器，用于通知编辑器的开始编译、停止编译、启动、停止等事件。
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20170927 07:43:28")]
    [ClassPurposeDesc("编辑器事件广播器，用于通知编辑器的开始编译、停止编译、启动、停止等事件。", "编辑器事件广播器，用于通知编辑器的开始编译、停止编译、启动、停止等事件。")]
#endif
    [InitializeOnLoad]
    public class EditorEventBroadcaster : SingleScriptableObject<EditorEventBroadcaster>
    {
        private static bool _isCompiling;
        private static bool _isPlaying;

        private readonly List<Action> _beforeCompileActions = new List<Action>();
        private readonly List<Action> _afterCompileActions = new List<Action>();
        private readonly List<Action> _playStartActions = new List<Action>();
        private readonly List<Action> _playStopActions = new List<Action>();

        public EditorEventBroadcaster BeforeCompile([NotNull]Action action)
        {
            _beforeCompileActions.Add(action);
            return this;
        }

        public EditorEventBroadcaster AfterCompile([NotNull]Action action)
        {
            _afterCompileActions.Add(action);
            return this;
        }

        public EditorEventBroadcaster PlayStart([NotNull]Action action)
        {
            _playStartActions.Add(action);
            return this;
        }

        public EditorEventBroadcaster PlayStop([NotNull]Action action)
        {
            _playStopActions.Add(action);
            return this;
        }

        static EditorEventBroadcaster()
        {
            EditorApplication.update -= Update;
            EditorApplication.update += Update;
        }

        private void OnEnable()
        {
            hideFlags = HideFlags.HideAndDontSave;
        }

        private static void Update()
        {
            if (EditorApplication.isCompiling && !_isCompiling)
            {
                _isCompiling = true;
                Instance._beforeCompileActions.ForEach(del => del());
            }
            else if (!EditorApplication.isCompiling && _isCompiling)
            {
                _isCompiling = false;
                Instance._afterCompileActions.ForEach(del => del());
            }

            if (EditorApplication.isPlaying && !_isPlaying)
            {
                _isPlaying = true;
                Instance._playStartActions.ForEach(del => del());
            }
            else if (!EditorApplication.isPlaying && _isPlaying)
            {
                _isPlaying = false;
                Instance._playStopActions.ForEach(del => del());
            }
        }

    }
}
