/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/03/01 13:03:46
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
using Iuker.Common.Constant;
using UnityEngine;

namespace Iuker.UnityKit.Run.Tween
{
    /// <summary>
    /// 动画管理器
    /// 不要直接添加动画脚本，而是在其他脚本中使用代码创建
    /// </summary>
    public class TweenManager : MonoBehaviour
    {
        private static bool needsInitialize = true;
        private static GameObject root;
        private static readonly List<ITween> tweens = new List<ITween>();

        public static readonly Dictionary<EaseType, Func<float, float>> scaleFuncDictionary = new Dictionary
<EaseType, Func<float, float>>()
        {
            { EaseType.Linear, TweenScaleFunctions.Linear},
            { EaseType.QuadraticEaseIn, TweenScaleFunctions.QuadraticEaseIn},
            { EaseType.QuadraticEaseOut, TweenScaleFunctions.QuadraticEaseOut},
            { EaseType.QuadraticEaseInOut, TweenScaleFunctions.QuadraticEaseInOut},
            { EaseType.CubicEaseIn, TweenScaleFunctions.CubicEaseIn},
            { EaseType.CubicEaseOut, TweenScaleFunctions.CubicEaseOut},
            { EaseType.CubicEaseInOut, TweenScaleFunctions.CubicEaseInOut},
            { EaseType.QuarticEaseIn, TweenScaleFunctions.QuadraticEaseIn},
            { EaseType.QuarticEaseOut, TweenScaleFunctions.QuarticEaseOut},
            { EaseType.QuarticEaseInOut, TweenScaleFunctions.QuarticEaseInOut},
            { EaseType.QuinticEaseIn, TweenScaleFunctions.QuinticEaseIn},
            { EaseType.QuinticEaseOut, TweenScaleFunctions.QuinticEaseOut},
            { EaseType.QuinticEaseInOut, TweenScaleFunctions.QuinticEaseInOut},
            { EaseType.SineEaseIn, TweenScaleFunctions.SineEaseIn},
            { EaseType.SineEaseOut, TweenScaleFunctions.SineEaseOut},
            { EaseType.SineEaseInOut, TweenScaleFunctions.SineEaseInOut},
        };

        private static void EnsureCreated()
        {
            if (needsInitialize)
            {
                needsInitialize = false;
                root = new GameObject();
                root.name = "DigitalRubyTween";
                root.hideFlags = HideFlags.HideAndDontSave;
                root.AddComponent<TweenManager>();
                DontDestroyOnLoad(root);
            }
        }

        private void Start()
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += SceneManagerSceneLoaded;
        }

        private void SceneManagerSceneLoaded(UnityEngine.SceneManagement.Scene s,
            UnityEngine.SceneManagement.LoadSceneMode m)
        {
            tweens.Clear();
        }

        /// <summary>
        /// 更新所有动画对象
        /// </summary>
        private void Update()
        {
            for (int i = tweens.Count - 1; i >= 0; i--)
            {
                var tween = tweens[i];
                var check1 = tween.Update(Time.deltaTime);
                var check2 = i < tweens.Count;
                var check3 = tween == tweens[i];

                if (check1 && check2 && check3)
                {
                    var t = tweens[i];
                    t.Reset();  // 重置动画对象
                    tweens.RemoveAt(i);
                    TweenFactory.Restore(tween);
                }
            }
        }

        public static FloatTween Tween(object key, float start, float end, float duration, Func<float, float> scaleFunc,
    Action<ITween<float>> progress,
    Action<ITween<float>> completion)
        {
            //FloatTween t = new FloatTween();
            var t = TweenFactory.GetFloatTween();
            t.Key = key;
            t.Init(start, end, duration, scaleFunc, progress, completion);
            AddTween(t);

            return t;
        }

        /// <summary>
        /// Created and add a Vector2 tween
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="start">Created value</param>
        /// <param name="end">End value</param>
        /// <param name="duration">CostTime in seconds</param>
        /// <param name="scaleFunc">Scale function</param>
        /// <param name="progress">Progress handler</param>
        /// <param name="completion">Completion handler</param>
        /// <returns>Vector2Tween</returns>
        public static Vector2Tween Tween(object key, Vector2 start, Vector2 end, float duration, Func<float, float> scaleFunc, Action<ITween<Vector2>> progress, Action<ITween<Vector2>> completion)
        {
            //Vector2Tween t = new Vector2Tween();
            var t = TweenFactory.GetVector2Tween();
            t.Key = key;
            t.Init(start, end, duration, scaleFunc, progress, completion);
            AddTween(t);

            return t;
        }

        /// <summary>
        /// 开启一个Vector3动画
        /// 需要传递一个游戏对象
        /// </summary>
        /// <param name="key">动画key</param>
        /// <param name="start">动画开始值</param>
        /// <param name="end">动画结束值</param>
        /// <param name="duration">过渡时间</param>
        /// <param name="progress">动画进度更新委托</param>
        /// <param name="completion">完成委托</param>
        /// <param name="easeType">动画曲线类型</param>
        /// <param name="friendName">对人友好的动画名</param>
        /// <returns></returns>
        public static Vector3Tween Tween(object key, Vector3 start, Vector3 end, float duration, Action<ITween<Vector3>> progress, Action<ITween<Vector3>>
            completion = null, EaseType easeType = EaseType.Linear, string friendName = null)
        {
            //Run.QuaternionTween.Vector3Tween t = new Vector3Tween();
            var t = TweenFactory.GetVector3Tween();

            // 给动画对象赋一个友好的命名如果存在
            if (friendName == null)
            {
                t.FriendNme = Constant.GetTimeToken + "_Tween";
            }
            else
            {
                t.FriendNme = friendName;
            }
            t.Key = key;
            var scaleFunc = scaleFuncDictionary[easeType];
            t.Init(start, end, duration, scaleFunc, progress, completion);
            AddTween(t);

            return t;
        }

        /// <summary>
        /// Created and add a Vector4 tween
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="start">Created value</param>
        /// <param name="end">End value</param>
        /// <param name="duration">CostTime in seconds</param>
        /// <param name="scaleFunc">Scale function</param>
        /// <param name="progress">Progress handler</param>
        /// <param name="completion">Completion handler</param>
        /// <returns>Vector4Tween</returns>
        public static Vector4Tween Tween(object key, Vector4 start, Vector4 end, float duration, Func<float, float> scaleFunc, Action<ITween<Vector4>> progress, Action<ITween<Vector4>> completion)
        {
            //Vector4Tween t = new Vector4Tween();
            var t = TweenFactory.GetVector4Tween();
            t.Key = key;
            t.Init(start, end, duration, scaleFunc, progress, completion);
            AddTween(t);

            return t;
        }

        /// <summary>
        /// 启动一个颜色动画
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start">Created value</param>
        /// <param name="end">End value</param>
        /// <param name="duration">CostTime in seconds</param> 
        /// <param name="progress">Progress handler</param>
        /// <param name="completion">Completion handler</param>
        /// <param name="easeType"></param>
        /// <returns>ColorTween</returns>
        public static ColorTween Tween(object key, Color start, Color end, float duration, Action<ITween<Color>> progress, Action<ITween<Color>> completion, EaseType easeType = EaseType.Linear)
        {
            //ColorTween t = new ColorTween();
            var t = TweenFactory.GetColorTween();
            t.Key = key;
            var scaleFunc = scaleFuncDictionary[easeType];
            t.Init(start, end, duration, scaleFunc, progress, completion);
            AddTween(t);

            return t;
        }

        /// <summary>
        /// 启动一个QuaternionTween
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="duration"></param>
        /// <param name="progress"></param>
        /// <param name="completion"></param>
        /// <param name="easeType"></param>
        /// <param name="friendName"></param>
        /// <returns></returns>
        public static QuaternionTween QuaternionTween(object key, Quaternion start, Quaternion end, float duration, Action<ITween<Quaternion>> progress, Action<ITween<Quaternion>> completion, EaseType easeType = EaseType.Linear, string friendName = null)
        {
            var tween = TweenFactory.GetQuaternionTween();
            tween.FriendNme = friendName ?? Constant.GetTimeToken + "_Tween";   //给动画对象赋一个友好的命名如果存在
            tween.Key = key;
            var scaleFunc = scaleFuncDictionary[easeType];
            tween.Init(start, end, duration, scaleFunc, progress, completion);
            AddTween(tween);

            return tween;
        }

        /// <summary>
        /// 添加一个动画
        /// </summary>
        /// <param name="tween"></param>
        public static void AddTween(ITween tween)
        {
            EnsureCreated();
            if (tween.Key != null)
            {
                RemoveTweenKey(tween.Key, _addKeyStopBehavior);
            }
            tweens.Add(tween);
        }

        /// <summary>
        /// 移除一个动画
        /// </summary>
        /// <param name="tween"></param>
        /// <param name="stopBehavior"></param>
        /// <returns></returns>
        public static bool RemoveTween(ITween tween, TweenStopBehavior stopBehavior)
        {
            tween.Stop(stopBehavior);
            return tweens.Remove(tween);
        }

        /// <summary>
        /// 通过key对象移除一个动画
        /// </summary>
        /// <param name="key"></param>
        /// <param name="stopBehavior"></param>
        /// <returns></returns>
        private static bool RemoveTweenKey(object key, TweenStopBehavior stopBehavior)
        {
            if (key == null)
            {
                return false;
            }

            bool foundOne = false;
            for (int i = tweens.Count - 1; i >= 0; i--)
            {
                ITween t = tweens[i];
                if (key.Equals(t.Key))
                {
                    t.Stop(stopBehavior);
                    tweens.RemoveAt(i);
                    foundOne = true;
                }
            }
            return foundOne;
        }

        private static TweenStopBehavior _addKeyStopBehavior = TweenStopBehavior.DoNotModify;


    }
}
