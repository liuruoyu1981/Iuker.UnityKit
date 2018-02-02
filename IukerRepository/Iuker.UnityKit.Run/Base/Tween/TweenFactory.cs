/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/03/02 06:53:03
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
using Iuker.Common.Base.Interfaces;

namespace Iuker.UnityKit.Run.Tween
{
    /// <summary>
    /// 动画工厂
    /// </summary>
    public static class TweenFactory
    {
        private const int tweenCount = 100;
        private static readonly ObjectPool<Vector3Tween> v3TweenPool = new ObjectPool<Vector3Tween>(new Vector3TweenFactory(), tweenCount);
        private static readonly ObjectPool<Vector2Tween> v2TweenPool = new ObjectPool<Vector2Tween>(new Vector2TweenFactory(), tweenCount);
        private static readonly ObjectPool<FloatTween> floatTweenPool = new ObjectPool<FloatTween>(new FloatTweenFactory(), tweenCount);
        private static readonly ObjectPool<Vector4Tween> v4TweenPool = new ObjectPool<Vector4Tween>(new Vector4TweenFactory(), tweenCount);
        private static readonly ObjectPool<ColorTween> colorTweenPool = new ObjectPool<ColorTween>(new ColorTweenFactory(), tweenCount);
        private static readonly ObjectPool<QuaternionTween> quaTweenPool = new ObjectPool<QuaternionTween>(new QuaternionTweenFactory(), tweenCount);
        private class Vector3TweenFactory : IObjectFactory<Vector3Tween>
        {
            public Vector3Tween CreateObject()
            {
                Vector3Tween tween = new Vector3Tween();
                return tween;
            }
        }

        private class Vector2TweenFactory : IObjectFactory<Vector2Tween>
        {
            public Vector2Tween CreateObject()
            {
                Vector2Tween tween = new Vector2Tween();
                return tween;
            }
        }

        private class FloatTweenFactory : IObjectFactory<FloatTween>
        {
            public FloatTween CreateObject()
            {
                FloatTween tween = new FloatTween();
                return tween;
            }
        }

        private class Vector4TweenFactory : IObjectFactory<Vector4Tween>
        {
            public Vector4Tween CreateObject()
            {
                Vector4Tween tween = new Vector4Tween();
                return tween;
            }
        }

        private class ColorTweenFactory : IObjectFactory<ColorTween>
        {
            public ColorTween CreateObject()
            {
                ColorTween tween = new ColorTween();
                return tween;
            }
        }

        private class QuaternionTweenFactory : IObjectFactory<QuaternionTween>
        {
            public QuaternionTween CreateObject()
            {
                QuaternionTween tween = new QuaternionTween();
                return tween;
            }
        }

        /// <summary>
        /// 归还一个动画对象
        /// </summary>
        /// <param name="tween"></param>
        public static void Restore(ITween tween)
        {
            Type type = tween.GetType();
            switch (type.Name)
            {
                case "Vector3Tween":
                    var v3Tween = tween as Vector3Tween;
                    v3TweenPool.Restore(v3Tween);
                    break;
                case "Vector4Tween":
                    var v4Tween = tween as Vector4Tween;
                    v4TweenPool.Restore(v4Tween);
                    break;
                case "Vector2Tween":
                    var v2Tween = tween as Vector2Tween;
                    v2TweenPool.Restore(v2Tween);
                    break;
                case "FloatTween":
                    var floatTween = tween as FloatTween;
                    floatTweenPool.Restore(floatTween);
                    break;
                case "ColorTween":
                    var colorTween = tween as ColorTween;
                    colorTweenPool.Restore(colorTween);
                    break;
                case "QuaternionTween":
                    var quaTween = tween as QuaternionTween;
                    quaTweenPool.Restore(quaTween);
                    break;
            }
        }

        public static Vector3Tween GetVector3Tween()
        {
            var tween = v3TweenPool.Take();
            return tween;
        }

        public static FloatTween GetFloatTween()
        {
            var tween = floatTweenPool.Take();
            return tween;
        }

        public static Vector2Tween GetVector2Tween()
        {
            var tween = v2TweenPool.Take();
            return tween;
        }

        public static Vector4Tween GetVector4Tween()
        {
            var tween = v4TweenPool.Take();
            return tween;
        }

        public static ColorTween GetColorTween()
        {
            var tween = colorTweenPool.Take();
            return tween;
        }

        public static QuaternionTween GetQuaternionTween()
        {
            var tween = quaTweenPool.Take();
            return tween;
        }

    }
}
