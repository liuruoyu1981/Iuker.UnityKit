/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/02/17 08:39:24
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
using System.Collections.Generic;
using Iuker.UnityKit.Run.Base.Config;
using Iuker.UnityKit.Run.Base.Config.Develop;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Iuker.UnityKit.Run
{
    /// <summary>
    /// u3d扩展方法
    /// </summary>
    public static class UnityExtensions
    {

        public static void SafeDestroy(this GameObject go)
        {
            if (go != null)
            {
                Object.Destroy(go);
            }
        }

        public static void DeleteAllChild(this GameObject targetGo)
        {
            for (int i = 0; i < targetGo.transform.childCount; i++)
            {
                var child = targetGo.transform.GetChild(i);
                child.gameObject.SafeDestroy();
            }
        }

        /// <summary>
        /// 添加一个Mono组件，如果目标类型组件已存在则删除已有
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="targetGo"></param>
        public static T AddComponentDeleteExist<T>(this GameObject targetGo) where T : Component
        {
            var mono = targetGo.GetComponent<T>();
            if (mono != null) UnityEngine.Object.Destroy(mono);
            mono = targetGo.AddComponent<T>();
            return mono;
        }

        /// <summary>
        /// 将一个游戏对象本地位置归0
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public static void SetLocalPostion(this Transform transform,
     float x, float y, float z = 0f)
        {
            transform.localPosition = new Vector3(x, y, z);
        }

        #region 常用视图功能

        /// <summary>
        /// 依据传入的int列表返回一个颜色对象,透明度可以调节
        /// </summary>
        /// <param name="listInt"></param>
        /// <returns></returns>
        public static Color ConvertListIntToColor(List<int> listInt)
        {
            var red = listInt[0] / 255f;
            var green = listInt[1] / 255f;
            var bule = listInt[2] / 255f;
            var alpha = listInt[3] / 255f;

            Color color = new Color(red, green, bule, alpha);
            return color;
        }

        /// <summary>
        /// 依据传入的int列表返回一个颜色对象，透明度不可调节，默认为不透明
        /// 用于战旗的绘制
        /// </summary>
        /// <param name="listInt"></param>
        /// <returns></returns>
        public static Color ConvertListIntToColorNoAlpha(List<int> listInt)
        {
            var red = listInt[0] / 255f;
            var green = listInt[1] / 255f;
            var bule = listInt[2] / 255f;

            var color = new Color(red, green, bule, 1f);
            return color;
        }

        /// <summary>
        /// 依据传入的int列表返回一个颜色对象,透明度可以调节
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        public static Color ConvertListIntToColor(int r, int g, int b, int a = 255)
        {
            var red = r / 255f;
            var green = g / 255f;
            var bule = b / 255f;
            var alpha = a / 255f;

            Color color = new Color(red, green, bule, alpha);
            return color;
        }

        #endregion

        #region GameObject和Transform

        /// <summary>
        /// 获得一个游戏对象下所有的直接子游戏对象
        /// </summary>
        /// <param name="go"></param>
        /// <returns></returns>
        public static List<GameObject> GetAllChild(this GameObject go)
        {
            if (go == null) return null;

            List<GameObject> result = new List<GameObject>();
            Transform trm = go.transform;

            for (int i = 0; i < trm.childCount; i++)
            {
                var child = trm.GetChild(i);
                result.Add(child.gameObject);
            }
            return result;
        }

        public static GameObject AddChild(this MonoBehaviour target, string sonname)
        {
            GameObject son = new GameObject(sonname);
            son.transform.localScale = Vector3.one;
            son.transform.SetParent(target.transform);
            return son;
        }

        public static GameObject AddChild(this GameObject target, string sonname)
        {
            GameObject son = new GameObject(sonname);
            son.transform.localScale = Vector3.one;
            son.transform.SetParent(target.transform);
            return son;
        }

        /// <summary>
        /// 给目标游戏对象添加一个指定的子对象
        /// </summary>
        /// <param name="target"></param>
        /// <param name="son"></param>
        /// <returns></returns>
        public static GameObject AddChild(this GameObject target, GameObject son)
        {
            son.transform.localScale = Vector3.one;
            son.layer = target.layer;
            son.transform.SetParent(target.transform, false);
            return son;
        }

        /// <summary>
        /// 给目标游戏对象添加一个预制件作为子对象
        /// </summary>
        /// <param name="prefab"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static GameObject AddPrefab(this GameObject parent, GameObject prefab)
        {
            if (prefab == null || parent == null)
            {
                return null;
            }

            var child = Object.Instantiate(prefab);
            child.SetActive(true);
            var result = parent.AddChild(child);
            return result;
        }

        /// <summary>
        /// 给目标游戏对象添加一个预制件作为子对象
        /// </summary>
        /// <param name="prefab"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static GameObject AddPrefab(this Transform parent, GameObject prefab)
        {
            if (prefab == null || parent == null)
            {
                return null;
            }

            var child = Object.Instantiate(prefab);
            child.SetActive(true);
            var result = parent.gameObject.AddChild(child);
            return result;
        }

        /// <summary>
        /// 将另一个游戏对象的所有子对象添加到目标游戏对象下
        /// </summary>
        /// <param name="target"></param>
        /// <param name="other"></param>
        public static void AddOtherChilds(this GameObject target, GameObject other)
        {
            var otherChilds = other.GetAllChild();
            foreach (GameObject otherChild in otherChilds)
            {
                target.AddChild(otherChild);
            }
        }

        public static void AddChilds(this GameObject target, List<GameObject> childs)
        {
            foreach (var child in childs)
            {
                target.AddChild(child);
            }
        }

        #endregion

        #region 集合

        //public static TSource First<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate,)
        //{

        //}





        #endregion

        #region 字符串

        /// <summary>
        /// 获得一个路径的Unity相对路径
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <returns></returns>
        public static string AssetsPath(this string sourcePath)
        {
            var result = "Assets" + sourcePath.Replace(Application.dataPath, "");
            return result;
        }

        /// <summary>
        /// 查找一个指定的Unity路径所在的子项目
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static SonProject PathInSonProject(this string path)
        {
            try
            {
                path = path.Replace(Application.dataPath + "/", "");
                path = path.Replace("Assets/", "");
                var compexName = path.Split('/')[1];
                var targetSon = RootConfig.Instance.AllProjectsSons.Find(son => son.CompexName == compexName);
                return targetSon;
            }
            catch
            {
                Debug.Log("所选择的路径不是一个合法的子项目目录，请检查！");
                return null;
            }
        }

        #endregion

        #region GUI

        /// <summary>
        /// 给定一个矩形区域并返回该区域内是否发生了鼠标点击事件的判断结果
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static bool IsClicked(this Rect rect, Action callback = null)
        {
            var result = Event.current.type == EventType.MouseDown &&
                         rect.Contains(Event.current.mousePosition);
            if (result)
            {
                if (callback != null)
                {
                    callback();
                }
            }

            return result;
        }

        public static bool IsHover(this Rect rect)
        {
            return Event.current.type == EventType.MouseMove && rect.Contains(Event.current.mousePosition);
        }

        public static bool IsExit(this Rect rect)
        {
            return Event.current.type == EventType.MouseMove && !rect.Contains
                  (Event.current.mousePosition);
        }

        public static bool IsUp(this Rect rect)
        {
            return Event.current.type == EventType.MouseUp &&
              rect.Contains(Event.current.mousePosition);
        }



        #endregion
    }
}
