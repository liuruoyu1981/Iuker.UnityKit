/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/03/01 14:24:00
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
using System.Linq;
using Iuker.UnityKit.Run.Module.View.ViewWidget;
using UnityEngine;

namespace Iuker.UnityKit.Run.Tween
{
    /// <summary>
    /// 通过Do关键字调用的动画扩展方法
    /// 模仿DoTween实现
    /// </summary>
    public static class TweenExtensionsByDoKey
    {
        /// <summary>
        /// 将一组游戏对象依次移动到一组位置上
        /// 游戏对象数量必须与位置数量保持一致
        /// 每个游戏对象的移动都会在之前的游戏对象移动结束时开始
        /// </summary>
        /// <param name="goList"></param>
        /// <param name="start"></param>
        /// <param name="endList"></param>
        /// <param name="duration"></param>
        /// <param name="index"></param>
        private static void MoveTo(this List<GameObject> goList, Vector3 start, List<Vector3> endList, float duration, int index)
        {
            if (goList.Count != endList.Count)
            {
                throw new Exception("移动目标数量和终点数量不一致！");
            }

            if (index < goList.Count)
            {
                var go = goList[index];
                var end = endList[index];
                go.DoMove(start, end, duration).OnCompleted(tween =>
                {
                    goList.MoveTo(start, endList, duration, index + 1);
                });
            }
        }

        public static void MoveTo(this List<Transform> tramList, Vector3 start, List<Vector3> endList, float duration, int index)
        {
            if (tramList.Count != endList.Count)
            {
                throw new Exception("移动目标数量和终点数量不一致！");
            }

            var goList = tramList.Select(trm => trm.gameObject).ToList();

            if (index < tramList.Count)
            {
                var go = goList[index];
                var end = endList[index];
                go.DoMove(start, end, duration).OnCompleted(tween =>
                {
                    goList.MoveTo(start, endList, duration, index + 1);
                });
            }
        }


        /// <summary>
        /// 在指定的过渡时间内移动一个游戏对象到指定的Vector3位置
        /// </summary>
        /// <param name="target">目标游戏对象</param>
        /// <param name="start">开始位置</param>
        /// <param name="end">结束位置</param>
        /// <param name="duration">过渡时间</param>
        private static Vector3Tween DoMove(this GameObject target, Vector3 start, Vector3 end, float duration)
        {
            target.transform.position = start;
            var tween = TweenManager.Tween(null, start, end, duration, t3 =>
            {
                target.transform.position = t3.CurrentValue;
            });
            return tween;
        }

        #region 旋转动画

        private static QuaternionTween DoRotate(this GameObject target, Quaternion start, Quaternion end, float duration)
        {
            target.transform.rotation = start;
            var tween = TweenManager.QuaternionTween(null, start, end, duration, t3 =>
            {
                target.transform.rotation = t3.CurrentValue;
            }, null);
            return tween;
        }

        /// <summary>
        /// 在指定的过渡时间内旋转一个游戏对象的Y轴到指定的位置
        /// </summary>
        /// <param name="target"></param>
        /// <param name="end"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        public static QuaternionTween DoRotateZ(this GameObject target, float end, float duration)
        {
            var startQ = target.transform.rotation;
            var endQ = new Quaternion(target.transform.rotation.x, target.transform.rotation.y, end, target.transform.rotation.w);
            return DoRotate(target, startQ, endQ, duration);
        }


        #endregion

        /// <summary>
        /// 在指定的过渡时间内移动一个游戏对象到指定的Vector3位置
        /// </summary>
        /// <param name="target">目标游戏对象</param>
        /// <param name="start">开始位置</param>
        /// <param name="end">结束位置</param>
        /// <param name="duration">过渡时间</param>
        private static ColorTween DoColor(this IImage target, Color start, Color end, float duration)
        {
            target.Color = start;
            var tween = TweenManager.Tween(null, start, end, duration, t3 =>
            {
                target.Color = t3.CurrentValue;
            }, null);
            return tween;
        }

        /// <summary>
        /// 在指定的过渡时间内移动一个文字游戏对象到指定的Vector3位置
        /// </summary>
        /// <param name="target">目标游戏对象</param>
        /// <param name="start">开始位置</param>
        /// <param name="end">结束位置</param>
        /// <param name="duration">过渡时间</param>
        public static ColorTween DoColorText(this IText target, Color start, Color end, float duration)
        {
            target.Color = start;
            var tween = TweenManager.Tween(null, start, end, duration, t3 =>
            {
                target.Color = t3.CurrentValue;
            }, null);
            return tween;
        }

        /// <summary>
        /// 在指定的过渡时间内改变一个图片游戏对象的颜色透明只
        /// </summary>
        /// <param name="target">目标游戏对象</param>
        /// <param name="end">结束位置</param>
        /// <param name="duration">过渡时间</param>
        public static ColorTween DoColorA(this IImage target, float end, float duration)
        {
            var startV3 = target.Color;
            var endV3 = new Color(target.Color.r, target.Color.b, target.Color.g, end);
            return DoColor(target, startV3, endV3, duration);
        }

        /// <summary>
        /// 在指定的过渡时间内改变一个文字游戏对象的颜色透明只
        /// </summary>
        /// <param name="target">目标游戏对象</param>
        /// <param name="end">结束位置</param>
        /// <param name="duration">过渡时间</param>
        public static ColorTween DoColorAText(this IText target, float end, float duration)
        {
            var startV3 = target.Color;
            var endV3 = new Color(target.Color.r, target.Color.b, target.Color.g, end);
            return DoColorText(target, startV3, endV3, duration);
        }

        /// <summary>
        /// 在指定的过渡时间内移动一个游戏对象到指定的Vector3位置（本地坐标 ）
        /// </summary>
        /// <param name="target">目标游戏对象</param>
        /// <param name="start">开始位置</param>
        /// <param name="end">结束位置</param>
        /// <param name="duration">过渡时间</param>
        /// <param name="friendName">对人友好的动画名</param>
        public static Vector3Tween DoLocalMove(this GameObject target, Vector3 start, Vector3 end, float duration, string friendName = null)
        {
            target.transform.localPosition = start;
            var tween = TweenManager.Tween(null, start, end, duration, t3 =>
            {
                target.transform.localPosition = t3.CurrentValue;
            }, null, EaseType.Linear, friendName);
            return tween;
        }

        /// <summary>
        /// 在指定的过渡时间内移动一个游戏对象到指定的Vector3位置
        /// 起始位置为目标游戏对象的当前位置
        /// </summary>
        /// <param name="target"></param>
        /// <param name="end"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        public static Vector3Tween DoMove(this GameObject target, Vector3 end, float duration)
        {
            var start = target.transform.position;
            var tween = DoMove(target, start, end, duration);
            return tween;
        }

        /// <summary>
        /// 在指定的过渡时间内移动一个游戏对象的X轴到指定的位置
        /// </summary>
        /// <param name="target">目标游戏对象</param>
        /// <param name="end">结束位置</param>
        /// <param name="duration">过渡时间</param>
        public static Vector3Tween DoMoveX(this GameObject target, float end, float duration)
        {
            var startV3 = target.transform.position;
            var endV3 = new Vector3(end, target.transform.position.y, target.transform.position.z);
            return DoMove(target, startV3, endV3, duration);
        }

        /// <summary>
        /// 在指定的过渡时间内移动一个游戏对象的X轴到指定的位置(本地坐标)
        /// </summary>
        /// <param name="target">目标游戏对象</param>
        /// <param name="end">结束位置</param>
        /// <param name="duration">过渡时间</param>
        public static Vector3Tween DoLocalMoveX(this GameObject target, float end, float duration)
        {
            var startV3 = target.transform.localPosition;
            var endV3 = new Vector3(end, target.transform.localPosition.y, target.transform.localPosition.z);
            return DoLocalMove(target, startV3, endV3, duration);
        }

        /// <summary>
        /// 在指定的过渡时间内移动一个游戏对象的Y轴到指定的位置
        /// </summary>
        /// <param name="target">目标游戏对象</param>
        /// <param name="end">结束位置</param>
        /// <param name="duration">过渡时间</param>
        public static Vector3Tween DoMoveY(this GameObject target, float end, float duration)
        {
            var startV3 = target.transform.position;
            var endV3 = new Vector3(target.transform.position.x, end, target.transform.position.z);
            return DoMove(target, startV3, endV3, duration);
        }

        /// <summary>
        /// 在指定的过渡时间内移动一个游戏对象的Y轴到指定的位置(本地坐标)
        /// </summary>
        /// <param name="target">目标游戏对象</param>
        /// <param name="end">结束位置</param>
        /// <param name="duration">过渡时间</param>
        /// <param name="friendName">对人友好的动画名</param>
        public static Vector3Tween DoLocalMoveY(this GameObject target, float end, float duration, string friendName = null)
        {
            var startV3 = target.transform.localPosition;
            var endV3 = new Vector3(target.transform.localPosition.x, end, target.transform.localPosition.z);
            return DoLocalMove(target, startV3, endV3, duration);
        }

        /// <summary>
        /// 在指定的过渡时间内移动一个游戏对象的Z轴到指定的位置
        /// </summary>
        /// <param name="target">目标游戏对象</param>
        /// <param name="end">结束位置</param>
        /// <param name="duration">过渡时间</param>
        public static Vector3Tween DoMoveZ(this GameObject target, float end, float duration)
        {
            var startV3 = target.transform.position;
            var endV3 = new Vector3(target.transform.position.x, target.transform.position.y, end);
            return DoMove(target, startV3, endV3, duration);
        }

        private static Vector3Tween DoLocalScale(GameObject target, Vector3 start, Vector3 end, float duration, Action<ITween<Vector3>> completion = null)
        {
            var tween = TweenManager.Tween(null, start, end, duration, t3 =>
            {
                target.transform.localScale = t3.CurrentValue;
            }, completion);
            return tween;
        }

        public static Vector3Tween DoLocalScale(this GameObject target, float scale, float duration, Action<ITween<Vector3>> completion = null)
        {
            var start = target.transform.localScale;
            var end = target.transform.localScale * scale;
            return DoLocalScale(target, start, end, duration, completion);
        }


        /// <summary>
        /// 在指定的平均过渡时间内依照给定的路径移动游戏对象到指定位置
        /// </summary>
        /// <param name="target"></param>
        /// <param name="paths"></param>
        /// <param name="duration"></param>
        /// <param name="completeDelg"></param>
        /// <param name="pathIndex"></param>
        public static void DoMovePath(this GameObject target, Vector3[] paths, float duration, Action<ITween> completeDelg = null, int pathIndex = 0)
        {
            var path = paths[pathIndex];
            DoMove(target, path, duration)
                .OnCompleted(t =>
                {
                    if (pathIndex == paths.Length - 1)
                    {
                        if (completeDelg != null)
                        {
                            completeDelg(t);
                        }
                        return;
                    }
                    DoMovePath(target, paths, duration, completeDelg, pathIndex + 1);
                });
        }
    }
}
