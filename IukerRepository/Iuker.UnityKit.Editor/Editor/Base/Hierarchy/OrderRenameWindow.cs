using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Iuker.UnityKit.Editor.Hierarchy
{
    public class OrderRenameWindow : EditorWindow
    {
        private static OrderRenameWindow _window;
        private static List<GameObject> _selectGameObjectList;
        private static string _TargetName;

        public static void ShowWindow()
        {
            _selectGameObjectList = Selection.gameObjects.ToList();
            _window = GetWindow<OrderRenameWindow>();
            _window.titleContent.text = "顺序命名";
        }

        private void OnGUI()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("目标名");
            _TargetName = EditorGUILayout.TextField(_TargetName);
            if (GUILayout.Button("确定"))
            {
                RenameAllSelect();
            }
        }

        private static void RenameAllSelect()
        {
            for (int i = 0; i < _selectGameObjectList.Count; i++)
            {
                var targetGo = _selectGameObjectList[i];
                targetGo.name = _TargetName + "_" + i;
            }
        }

    }
}