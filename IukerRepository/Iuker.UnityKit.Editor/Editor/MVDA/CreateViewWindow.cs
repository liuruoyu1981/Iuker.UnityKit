/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/07/24 10:06:12
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
using Iuker.UnityKit.Editor.Assets;
using Iuker.UnityKit.Run.Base;
using Iuker.UnityKit.Run.Base.Config.Develop;
using Iuker.UnityKit.Run.Module.View.Assist;
using Iuker.UnityKit.Run.Module.View.MVDA;
using UnityEditor;
using UnityEngine;

namespace Iuker.UnityKit.Editor.MVDA
{
    /// <summary>
    /// 视图创建窗口
    /// </summary>
    public class CreateViewWindow : EditorWindow
    {
        /// <summary>
        /// 标题样式
        /// </summary>
        private static GUIStyle sTitleStyle;

        /// <summary>
        /// 当前子项目默认的视图Id名
        /// </summary>
        private static string sDefaultViewId;

        /// <summary>
        /// 普通视图挂载根节点
        /// </summary>
        private static RectTransform normalRoot;

        /// <summary>
        /// 寄生视图挂载根节点
        /// </summary>
        private static RectTransform parasiticRoot;

        /// <summary>
        /// 弹出
        /// </summary>
        private static RectTransform popupRoot;

        /// <summary>
        /// 背景层挂载根
        /// </summary>
        private static RectTransform backgroundRoot;

        /// <summary>
        /// 置顶模态视图挂载根节点
        /// </summary>
        private static RectTransform _topRoot;

        private static CreateViewWindow sWindow;

        public static void ShowWindow()
        {
            sWindow = GetWindow<CreateViewWindow>();
            var sonProject = RootConfig.GetCurrentProject().GetCurrentSonProject();
            var sonParentNameLower = sonProject.CompexName.ToLower();
            sDefaultViewId = string.Format("view_{0}_default", sonParentNameLower);
            mViewAssetName = mViewId = sDefaultViewId;
            mViewTypeArray = ViewType.Background.GetAllEnumFiled().ToArray();
            sTitleStyle = new GUIStyle { fontStyle = FontStyle.Bold };

            backgroundRoot = GameObject.Find("view_background_root").GetComponent<RectTransform>();
            normalRoot = GameObject.Find("view_normal_root").GetComponent<RectTransform>();
            parasiticRoot = GameObject.Find("view_parasitic_root").GetComponent<RectTransform>();
            popupRoot = GameObject.Find("view_popup_root").GetComponent<RectTransform>();
            _topRoot = GameObject.Find("view_top_root").GetComponent<RectTransform>();

            sWindow.titleContent.text = "视图创建窗口";
        }

        private static int mViewTypeIndex = 1;
        private static string[] mViewTypeArray;
        private static ViewType mViewType = ViewType.Normal;
        private static string mViewId;
        private static string mViewAssetName;
        private static bool mIsBlankClose;
        private static bool mIsHideOther;
        private static bool mIsCloseTop;

        private void OnGUI()
        {
            EditorGUILayout.LabelField("视图Id", sTitleStyle);
            mViewId = EditorGUILayout.TextField(mViewId);
            EditorGUILayout.LabelField("视图资源名", sTitleStyle);
            mViewAssetName = EditorGUILayout.TextField(mViewAssetName);
            EditorGUILayout.LabelField("视图类型", sTitleStyle);
            mViewTypeIndex = EditorGUILayout.Popup(mViewTypeIndex, mViewTypeArray);
            EditorGUILayout.Space();
            using (new IukHorizontalLayout())
            {
                EditorGUILayout.LabelField("是否需要开启时关闭自身类型置顶视图", sTitleStyle);
                mIsCloseTop = EditorGUILayout.Toggle(mIsCloseTop);
            }
            using (new IukHorizontalLayout())
            {
                EditorGUILayout.LabelField("是否需要开启时隐藏自身类型其他视图", sTitleStyle);
                mIsHideOther = EditorGUILayout.Toggle(mIsHideOther);
            }
            using (new IukHorizontalLayout())
            {
                EditorGUILayout.LabelField("是否需要开启时点击空白处关闭自身", sTitleStyle);
                mIsBlankClose = EditorGUILayout.Toggle(mIsBlankClose);
            }

            if (GUILayout.Button("创建视图")) CreateView();
        }


        private void CreateView()
        {
            if (mViewId == sDefaultViewId || mViewAssetName == sDefaultViewId)
            {
                EditorUtility.DisplayDialog("错误", "视图Id和视图资源名不能为默认值，请修改后重试！", "确定");
                return;
            }

            var viewTemplatePath = U3dConstants.ViewTemplatePath;
            var viewTemplate = IukAssetDataBase.LoadAssetAtPath<GameObject>(viewTemplatePath);
            var newView = Instantiate(viewTemplate);
            var viewAssister = newView.AddComponent<ViewAssister>();
            viewAssister.SetViewConfig(mViewAssetName, mViewAssetName, mViewType, mIsCloseTop, mIsHideOther, mIsBlankClose);
            newView.name = mViewId;

            // 挂载视图
            MoutViewPrefab(newView);

            var newViewPrefabPath = RootConfig.GetCurrentSonProject().ViewPrefabDir + newView.name + ".prefab";
            var newViewPrafab = IukPrafabUtility.CreatePrefab(newViewPrefabPath, newView, ReplacePrefabOptions.Default);
            PrefabUtility.ConnectGameObjectToPrefab(newView, newViewPrafab);
            EditorGUIUtility.PingObject(newViewPrafab);
            Debug.Log(viewTemplate);
            Close();
        }

        private void MoutViewPrefab(GameObject newView)
        {
            switch (mViewType)
            {
                case ViewType.Background:
                    newView.transform.SetParent(backgroundRoot);
                    break;
                case ViewType.Normal:
                    newView.transform.SetParent(normalRoot);
                    break;
                case ViewType.Parasitic:
                    newView.transform.SetParent(parasiticRoot);
                    break;
                case ViewType.Popup:
                    newView.transform.SetParent(popupRoot);
                    break;
                case ViewType.Top:
                    newView.transform.SetParent(_topRoot);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            newView.transform.localScale = Vector3.one;
        }








    }
}