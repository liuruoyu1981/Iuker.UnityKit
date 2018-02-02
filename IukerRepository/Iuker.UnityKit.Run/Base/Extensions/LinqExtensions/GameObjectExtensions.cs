/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/03/11 20:33:13
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

using Iuker.UnityKit.Run.LinqExtensions.OPerate;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Iuker.UnityKit.Run.LinqExtensions
{
    public static partial class GameObjectExtensions
    {
        private static GameObject GetGameObject<T>(T obj) where T : UnityEngine.Object
        {
            var gameObject = obj as GameObject;
            if (gameObject == null) // 如果转型GameObject失败则尝试转型为Component
            {
                var component = obj as Component;
                if (component == null)  // 如果转型Component也失败，说明obj不是游戏对象也不是组件则返回空。
                {
                    return null;
                }

                gameObject = component.gameObject;
            }

            return gameObject;
        }

        #region Add

        public static T Add<T>(this GameObject parent, T childOriginal, TransformCloneType cloneType = TransformCloneType.KeepOriginal, bool? setActive = null, string specifiedName = null, bool setLayer = false)
            where T : UnityEngine.Object
        {
            if (parent == null) throw new ArgumentNullException("parent");
            if (childOriginal == null) throw new ArgumentNullException("childOriginal");

            var child = UnityEngine.Object.Instantiate(childOriginal);

            var childGameObject = GetGameObject(child);

            // 如果是uGUI，应该使用SetParent(parent,false)
            var childTransform = childGameObject.transform;
#if !(UNITY_4_0 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5)
            var rectTransform = childTransform as RectTransform;
            if (rectTransform != null)
            {
                rectTransform.SetParent(parent.transform, worldPositionStays: false);
            }
            else
            {
#endif
                var parentTransform = parent.transform;
                childTransform.parent = parentTransform;
                switch (cloneType)
                {
                    case TransformCloneType.FollowParent:
                        childTransform.localPosition = parentTransform.localPosition;
                        childTransform.localScale = parentTransform.localScale;
                        childTransform.localRotation = parentTransform.localRotation;
                        break;
                    case TransformCloneType.Origin:
                        childTransform.localPosition = Vector3.zero;
                        childTransform.localScale = Vector3.one;
                        childTransform.localRotation = Quaternion.identity;
                        break;
                    case TransformCloneType.KeepOriginal:
                        var co = GetGameObject(childOriginal);
                        var childOriginalTransform = co.transform;
                        childTransform.localPosition = childOriginalTransform.localPosition;
                        childTransform.localScale = childOriginalTransform.localScale;
                        childTransform.localRotation = childOriginalTransform.localRotation;
                        break;
                    case TransformCloneType.DoNothing:
                    default:
                        break;
                }
#if !(UNITY_4_0 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5)
            }
#endif
            if (setLayer)
            {
                childGameObject.layer = parent.layer;
            }

            if (setActive != null)
            {
                childGameObject.SetActive(setActive.Value);
            }
            if (specifiedName != null)
            {
                child.name = specifiedName;
            }

            return child;
        }

        public static T[] AddRange<T>(this GameObject parent, IEnumerable<T> childOriginals, TransformCloneType cloneType = TransformCloneType.KeepOriginal, bool? setActive = null, string specifiedName = null, bool setLayer = false)
        where T : UnityEngine.Object
        {
            if (parent == null) throw new ArgumentNullException("parent");
            if (childOriginals == null) throw new ArgumentNullException("childOriginals");

            // iteration optimize
            {
                var array = childOriginals as T[];
                if (array != null)
                {
                    var result = new T[array.Length];
                    for (int i = 0; i < array.Length; i++)
                    {
                        var child = Add(parent, array[i], cloneType, setActive, specifiedName, setLayer);
                        result[i] = child;
                    }
                    return result;
                }
            }

            {
                var iterList = childOriginals as IList<T>;
                if (iterList != null)
                {
                    var result = new T[iterList.Count];
                    for (int i = 0; i < iterList.Count; i++)
                    {
                        var child = Add(parent, iterList[i], cloneType, setActive, specifiedName, setLayer);
                        result[i] = child;
                    }
                    return result;
                }
            }

            {
                var result = new List<T>();
                foreach (var childOriginal in childOriginals)
                {
                    var child = Add(parent, childOriginal, cloneType, setActive, specifiedName, setLayer);
                    result.Add(child);
                }

                return result.ToArray();
            }
        }


        public static T AddFirst<T>(this GameObject parent, T childOriginal, TransformCloneType cloneType = TransformCloneType.KeepOriginal, bool? setActive = null, string specifiedName = null, bool setLayer = false)
    where T : UnityEngine.Object
        {
            var child = Add(parent, childOriginal, cloneType, setActive, specifiedName, setLayer);
            var go = GetGameObject(child);
            if (go == null) return child;

            go.transform.SetAsFirstSibling();
            return child;
        }

        public static T[] AddFirstRange<T>(this GameObject parent, IEnumerable<T> childOriginals, TransformCloneType cloneType = TransformCloneType.KeepOriginal, bool? setActive = null, string specifiedName = null, bool setLayer = false)
    where T : UnityEngine.Object
        {
            var child = AddRange(parent, childOriginals, cloneType, setActive, specifiedName, setLayer);
            for (int i = child.Length - 1; i >= 0; i--)
            {
                var go = GetGameObject(child[i]);
                if (go == null) continue;
                go.transform.SetAsFirstSibling();
            }
            return child;
        }

        public static T AddBeforeSelf<T>(this GameObject parent, T childOriginal, TransformCloneType cloneType = TransformCloneType.KeepOriginal, bool? setActive = null, string specifiedName = null, bool setLayer = false)
    where T : UnityEngine.Object
        {
            var root = parent.Parent();
            if (root == null) throw new InvalidOperationException("The parent root is null");

            var sibilingIndex = parent.transform.GetSiblingIndex();

            var child = Add(root, childOriginal, cloneType, setActive, specifiedName, setLayer);

            var go = GetGameObject(child);
            if (go == null) return child;

            go.transform.SetSiblingIndex(sibilingIndex);
            return child;
        }

        public static T[] AddBeforeSelfRange<T>(this GameObject parent, IEnumerable<T> childOriginals, TransformCloneType cloneType = TransformCloneType.KeepOriginal, bool? setActive = null, string specifiedName = null, bool setLayer = false)
    where T : UnityEngine.Object
        {
            var root = parent.Parent();
            if (root == null) throw new InvalidOperationException("The parent root is null");

            var sibilingIndex = parent.transform.GetSiblingIndex();
            var child = AddRange(root, childOriginals, cloneType, setActive, specifiedName, setLayer);
            for (int i = child.Length - 1; i >= 0; i--)
            {
                var go = GetGameObject(child[i]);
                if (go == null) continue;
                go.transform.SetSiblingIndex(sibilingIndex);
            }

            return child;
        }

        public static T AddAfterSelf<T>(this GameObject parent, T childOriginal, TransformCloneType cloneType = TransformCloneType.KeepOriginal, bool? setActive = null, string specifiedName = null, bool setLayer = false)
    where T : UnityEngine.Object
        {
            var root = parent.Parent();
            if (root == null) throw new InvalidOperationException("The parent root is null");

            var sibilingIndex = parent.transform.GetSiblingIndex() + 1;
            var child = Add(root, childOriginal, cloneType, setActive, specifiedName, setLayer);
            var go = GetGameObject(child);
            if (go == null) return child;

            go.transform.SetSiblingIndex(sibilingIndex);
            return child;
        }

        public static T[] AddAfterSelfRange<T>(this GameObject parent, IEnumerable<T> childOriginals, TransformCloneType cloneType = TransformCloneType.KeepOriginal, bool? setActive = null, string specifiedName = null, bool setLayer = false)
    where T : UnityEngine.Object
        {
            var root = parent.Parent();
            if (root == null) throw new InvalidOperationException("The parent root is null");

            var sibilingIndex = parent.transform.GetSiblingIndex() + 1;
            var child = AddRange(root, childOriginals, cloneType, setActive, specifiedName, setLayer);
            for (int i = child.Length - 1; i >= 0; i--)
            {
                var go = GetGameObject(child[i]);
                if (go == null) continue;
                go.transform.SetSiblingIndex(sibilingIndex);
            }

            return child;
        }

        #endregion

        #region Move

        /// <summary>
        /// <para>Move the GameObject/Component as children of this GameObject.</para>
        /// </summary>
        /// <param name="parent">Parent GameObject.</param>
        /// <param name="child">Target.</param>
        /// <param name="moveType">Choose set type of cloned child GameObject's localPosition/Scale/Rotation.</param>      
        /// <param name="setActive">Set activates/deactivates child GameObject. If null, doesn't set specified value.</param>
        /// <param name="setLayer">Set layer of child GameObject same with parent.</param>
        public static T MoveToLast<T>(this GameObject parent, T child, TransformMoveType moveType = TransformMoveType.DoNothing, bool? setActive = null, bool setLayer = false)
            where T : UnityEngine.Object
        {
            if (parent == null) throw new ArgumentNullException("parent");
            if (child == null) throw new ArgumentNullException("child");

            var childGameObject = GetGameObject(child);
            if (child == null) return child;

            // for uGUI, should use SetParent(parent, false)
            var childTransform = childGameObject.transform;
#if !(UNITY_4_0 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5)
            var rectTransform = childTransform as RectTransform;
            if (rectTransform != null)
            {
                rectTransform.SetParent(parent.transform, worldPositionStays: false);
            }
            else
            {
#endif
                var parentTransform = parent.transform;
                childTransform.parent = parentTransform;
                switch (moveType)
                {
                    case TransformMoveType.FollowParent:
                        childTransform.localPosition = parentTransform.localPosition;
                        childTransform.localScale = parentTransform.localScale;
                        childTransform.localRotation = parentTransform.localRotation;
                        break;
                    case TransformMoveType.Origin:
                        childTransform.localPosition = Vector3.zero;
                        childTransform.localScale = Vector3.one;
                        childTransform.localRotation = Quaternion.identity;
                        break;
                    case TransformMoveType.DoNothing:
                    default:
                        break;
                }
#if !(UNITY_4_0 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5)
            }
#endif
            if (setLayer)
            {
                childGameObject.layer = parent.layer;
            }

            if (setActive != null)
            {
                childGameObject.SetActive(setActive.Value);
            }

            return child;
        }

        /// <summary>
        /// <para>Move the GameObject/Component as children of this GameObject.</para>
        /// </summary>
        /// <param name="parent">Parent GameObject.</param>
        /// <param name="childs">Target.</param>
        /// <param name="moveType">Choose set type of moved child GameObject's localPosition/Scale/Rotation.</param>
        /// <param name="setActive">Set activates/deactivates child GameObject. If null, doesn't set specified value.</param>
        /// <param name="setLayer">Set layer of child GameObject same with parent.</param>
        public static T[] MoveToLastRange<T>(this GameObject parent, IEnumerable<T> childs, TransformMoveType moveType = TransformMoveType.DoNothing, bool? setActive = null, bool setLayer = false)
            where T : UnityEngine.Object
        {
            if (parent == null) throw new ArgumentNullException("parent");
            if (childs == null) throw new ArgumentNullException("childs");

            // iteration optimize
            {
                var array = childs as T[];
                if (array != null)
                {
                    var result = new T[array.Length];
                    for (int i = 0; i < array.Length; i++)
                    {
                        var child = MoveToLast(parent, array[i], moveType, setActive, setLayer);
                        result[i] = child;
                    }
                    return result;
                }
            }

            {
                var iterList = childs as IList<T>;
                if (iterList != null)
                {
                    var result = new T[iterList.Count];
                    for (int i = 0; i < iterList.Count; i++)
                    {
                        var child = MoveToLast(parent, iterList[i], moveType, setActive, setLayer);
                        result[i] = child;
                    }
                    return result;
                }
            }
            {
                var result = new List<T>();
                foreach (var childOriginal in childs)
                {
                    var child = MoveToLast(parent, childOriginal, moveType, setActive, setLayer);
                    result.Add(child);
                }

                return result.ToArray();
            }
        }

        /// <summary>
        /// <para>Move the GameObject/Component as the first children of this GameObject.</para>
        /// </summary>
        /// <param name="parent">Parent GameObject.</param>
        /// <param name="child">Target.</param>
        /// <param name="moveType">Choose set type of cloned child GameObject's localPosition/Scale/Rotation.</param>      
        /// <param name="setActive">Set activates/deactivates child GameObject. If null, doesn't set specified value.</param>
        /// <param name="setLayer">Set layer of child GameObject same with parent.</param>
        public static T MoveToFirst<T>(this GameObject parent, T child, TransformMoveType moveType = TransformMoveType.DoNothing, bool? setActive = null, bool setLayer = false)
            where T : UnityEngine.Object
        {
            MoveToLast(parent, child, moveType, setActive, setLayer);
            var go = GetGameObject(child);
            if (go == null) return child;

            go.transform.SetAsFirstSibling();
            return child;
        }

        /// <summary>
        /// <para>Move the GameObject/Component as the first children of this GameObject.</para>
        /// </summary>
        /// <param name="parent">Parent GameObject.</param>
        /// <param name="childs">Target.</param>
        /// <param name="moveType">Choose set type of cloned child GameObject's localPosition/Scale/Rotation.</param>       
        /// <param name="setActive">Set activates/deactivates child GameObject. If null, doesn't set specified value.</param>
        /// <param name="setLayer">Set layer of child GameObject same with parent.</param>
        public static T[] MoveToFirstRange<T>(this GameObject parent, IEnumerable<T> childs, TransformMoveType moveType = TransformMoveType.DoNothing, bool? setActive = null, bool setLayer = false)
            where T : UnityEngine.Object
        {
            var child = MoveToLastRange(parent, childs, moveType, setActive, setLayer);
            for (int i = child.Length - 1; i >= 0; i--)
            {
                var go = GetGameObject(child[i]);
                if (go == null) continue;

                go.transform.SetAsFirstSibling();
            }
            return child;
        }

        /// <summary>
        /// <para>Move the GameObject/Component before this GameObject.</para>
        /// </summary>
        /// <param name="parent">Parent GameObject.</param>
        /// <param name="child">Target.</param>
        /// <param name="moveType">Choose set type of cloned child GameObject's localPosition/Scale/Rotation.</param>      
        /// <param name="setActive">Set activates/deactivates child GameObject. If null, doesn't set specified value.</param>
        /// <param name="setLayer">Set layer of child GameObject same with parent.</param>
        public static T MoveToBeforeSelf<T>(this GameObject parent, T child, TransformMoveType moveType = TransformMoveType.DoNothing, bool? setActive = null, bool setLayer = false)
            where T : UnityEngine.Object
        {
            var root = parent.Parent();
            if (root == null) throw new InvalidOperationException("The parent root is null");

            var sibilingIndex = parent.transform.GetSiblingIndex();

            MoveToLast(root, child, moveType, setActive, setLayer);
            var go = GetGameObject(child);
            if (go == null) return child;

            go.transform.SetSiblingIndex(sibilingIndex);
            return child;
        }

        /// <summary>
        /// <para>Move the GameObject/Component before GameObject.</para>
        /// </summary>
        /// <param name="parent">Parent GameObject.</param>
        /// <param name="childs">Target.</param>
        /// <param name="moveType">Choose set type of cloned child GameObject's localPosition/Scale/Rotation.</param>       
        /// <param name="setActive">Set activates/deactivates child GameObject. If null, doesn't set specified value.</param>
        /// <param name="setLayer">Set layer of child GameObject same with parent.</param>
        public static T[] MoveToBeforeSelfRange<T>(this GameObject parent, IEnumerable<T> childs, TransformMoveType moveType = TransformMoveType.DoNothing, bool? setActive = null, bool setLayer = false)
            where T : UnityEngine.Object
        {
            var root = parent.Parent();
            if (root == null) throw new InvalidOperationException("The parent root is null");

            var sibilingIndex = parent.transform.GetSiblingIndex();
            var child = MoveToLastRange(root, childs, moveType, setActive, setLayer);
            for (int i = child.Length - 1; i >= 0; i--)
            {
                var go = GetGameObject(child[i]);
                if (go == null) continue;

                go.transform.SetSiblingIndex(sibilingIndex);
            }

            return child;
        }

        /// <summary>
        /// <para>Move the GameObject/Component after this GameObject.</para>
        /// </summary>
        /// <param name="parent">Parent GameObject.</param>
        /// <param name="child">Target.</param>
        /// <param name="moveType">Choose set type of cloned child GameObject's localPosition/Scale/Rotation.</param>      
        /// <param name="setActive">Set activates/deactivates child GameObject. If null, doesn't set specified value.</param>
        /// <param name="setLayer">Set layer of child GameObject same with parent.</param>
        public static T MoveToAfterSelf<T>(this GameObject parent, T child, TransformMoveType moveType = TransformMoveType.DoNothing, bool? setActive = null, bool setLayer = false)
            where T : UnityEngine.Object
        {
            var root = parent.Parent();
            if (root == null) throw new InvalidOperationException("The parent root is null");

            var sibilingIndex = parent.transform.GetSiblingIndex() + 1;
            MoveToLast(root, child, moveType, setActive, setLayer);
            var go = GetGameObject(child);
            if (go == null) return child;

            go.transform.SetSiblingIndex(sibilingIndex);
            return child;
        }

        /// <summary>
        /// <para>Move the GameObject/Component after this GameObject.</para>
        /// </summary>
        /// <param name="parent">Parent GameObject.</param>
        /// <param name="childs">Target.</param>
        /// <param name="moveType">Choose set type of cloned child GameObject's localPosition/Scale/Rotation.</param>       
        /// <param name="setActive">Set activates/deactivates child GameObject. If null, doesn't set specified value.</param>
        /// <param name="setLayer">Set layer of child GameObject same with parent.</param>
        public static T[] MoveToAfterSelfRange<T>(this GameObject parent, IEnumerable<T> childs, TransformMoveType moveType = TransformMoveType.DoNothing, bool? setActive = null, bool setLayer = false)
            where T : UnityEngine.Object
        {
            var root = parent.Parent();
            if (root == null) throw new InvalidOperationException("The parent root is null");

            var sibilingIndex = parent.transform.GetSiblingIndex() + 1;
            var child = MoveToLastRange(root, childs, moveType, setActive, setLayer);
            for (int i = child.Length - 1; i >= 0; i--)
            {
                var go = GetGameObject(child[i]);
                if (go == null) continue;

                go.transform.SetSiblingIndex(sibilingIndex);
            }

            return child;
        }

        #endregion

        /// <summary>
        /// 安全删除游戏对象
        /// </summary>
        /// <param name="self"></param>
        /// <param name="useDestroyImmediate">如果在编辑器模式下，使用传递true或者使用!Application.isPlaying.</param>
        /// <param name="detachParent">是否将父物体设置为空</param>
        public static void Destroy(this GameObject self, bool useDestroyImmediate = false, bool detachParent = false)
        {
            if (self == null) return;

            if (detachParent)
            {
#if !(UNITY_4_0 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5)
                self.transform.SetParent(null);
#else
                self.transform.parent = null;   //   为了防止方法过期编译器报错，所以这里使用预编译。
#endif
            }

            if (useDestroyImmediate)
            {
                UnityEngine.Object.DestroyImmediate(self);
            }
            else
            {
                UnityEngine.Object.Destroy(self);
            }
        }

    }
}
