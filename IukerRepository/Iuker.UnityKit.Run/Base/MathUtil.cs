using System;

namespace Iuker.UnityKit.Run.Base
{
    /// <summary>
    /// 数学工具
    /// </summary>
    public static class MathUtil
    {
        public static int Next(int min, int max)
        {
            Random random = new Random();
            var result = random.Next(min, max);
            return result;
        }



    }
}