///***********************************************************************************************
//Author：liuruoyu1981
//CreateDate: 2017/03/04 23:30:04
//Email: 35490136@qq.com
//QQCode: 35490136
//CreateNote: 
//***********************************************************************************************/


///****************************************修改日志***********************************************
//1. 修改日期： 修改人： 修改内容：
//2. 修改日期： 修改人： 修改内容：
//3. 修改日期： 修改人： 修改内容：
//4. 修改日期： 修改人： 修改内容：
//5. 修改日期： 修改人： 修改内容：
//****************************************修改日志***********************************************/

//using System.Linq;
//using Iuker.UnityKit.Run;
//using UnityEditor;
//using UnityEngine;

//namespace Iuker.UnityKit.Editor.Hierarchy
//{
//    public class HirearchyMenu
//    {
//        private static GameObject currentSelectObj;

//        static void OnHierarchyGUI(int instanceID, Rect selectionRect)
//        {
//            if (Event.current != null && selectionRect.Contains(Event.current.mousePosition)
//                && Event.current.button == 1 && Event.current.type <= EventType.mouseUp)
//            {
//                currentSelectObj = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
//                //这里可以判断selectedGameObject的条件
//                if (currentSelectObj)
//                {
//                    Vector2 mousePosition = Event.current.mousePosition;

//                    EditorUtility.DisplayPopupMenu(new Rect(mousePosition.x, mousePosition.y, 0, 0), "Window/Test", null);
//                    Event.current.Use();
//                }
//            }
//            var e = Event.current;

//            if (e.type == EventType.mouseDown && e.button == 1 && selectionRect.Contains(e.mousePosition))
//            {
//                GameObject selectedGameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
//                // 设置当前右键选择的对象
//                currentSelectObj = selectedGameObject;
//            }
//        }

//        //[MenuItem("GameObject/更新预制件", false, 3)]
//        private static void UpdatePrefab()
//        {
//            currentSelectObj = Selection.activeGameObject;
//            if (PrefabUtility.GetPrefabType(currentSelectObj) == PrefabType.PrefabInstance)
//            {
//                Object parentObject = PrefabUtility.GetPrefabParent(currentSelectObj);
//                var path = AssetDatabase.GetAssetPath(parentObject);

//            }
//        }

//        [MenuItem("GameObject/启动Unity", false, 3)]
//        private static void DoPlay() => EditorApplication.ExecuteMenuItem("Edit/Play");

//        [MenuItem("GameObject/辅助功能/替换对象名", false, 3)]
//        private static void BatchReplaceName()
//        {
//            AssistWindow.ShowWindow();
//        }

//        [MenuItem("GameObject/辅助功能/顺序重命名", false, 3)]
//        private static void OrderRename() => OrderRenameWindow.ShowWindow();

//        [MenuItem("GameObject/辅助功能/替换前缀", false, 3)]
//        private static void BatchReplacePrefix()
//        {
//            BatchReplacePrefixWindow.ShowWindow();
//        }

//        [MenuItem("GameObject/辅助功能/增加前缀", false, 3)]
//        private static void BatchAddPrefix()
//        {
//            BatchAddPrefixWindow.ShowWindow();
//        }

//        [MenuItem("GameObject/辅助功能/增加后缀", false, 3)]
//        private static void BatchAddSuffix()
//        {
//            BatchAddSuffixWindow.ShowWindow();
//        }

//        [MenuItem("GameObject/辅助功能/替换后缀", false, 3)]
//        private static void BatchReplaceSuffix()
//        {
//            BatchReplaceSuffixWindow.ShowWindow();
//        }

//        [MenuItem("GameObject/辅助功能/正向排序", false, 3)]

//        public static void OrderBy()
//        {
//            var parent = Selection.activeGameObject;
//            var allSons = parent.GetAllChild();
//            var tempParent = new GameObject("tempParent");
//            allSons.ForEach(s => s.transform.SetParent(tempParent.transform));
//            allSons = allSons.OrderBy(s => s.name).ToList();
//            allSons.ForEach(s => s.transform.SetParent(parent.transform));
//            Object.DestroyImmediate(tempParent);
//        }
//    }
//}
