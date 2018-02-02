using System;
using System.Diagnostics;
using Iuker.Common.Base;

namespace Run.Iuker.Common.Base
{
#if DEBUG
    /// <summary>
    /// 时间相关函数
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20170911 10:06:12")]
    [ClassPurposeDesc("时间相关函数", "时间相关函数")]
#endif
    public static class TimeUtility
    {
        /// <summary>
        /// 计算一个函数的耗时
        /// </summary>
        /// <param name="action"></param>
        /// <param name="costAction"></param>
        /// <returns></returns>
        public static decimal CalculateMethodCostTime(Action action, Action<decimal> costAction = null)
        {
            var stopWatchWrapper = new StopWatchWrapper();
            stopWatchWrapper.Start();
            if (action != null)
            {
                action();
            }

            var costTime = stopWatchWrapper.Stop();
            if (costAction != null)
            {
                costAction(costTime);
            }

            return costTime;
        }

        private class StopWatchWrapper
        {
            private Stopwatch mStopwatch;

            public void Start()
            {
                mStopwatch = new Stopwatch();
                mStopwatch.Start();
            }

            public decimal Stop()
            {
                mStopwatch.Stop();
                var costTime = mStopwatch.ElapsedTicks / (decimal)Stopwatch.Frequency;
                return costTime;
            }


        }

    }
}
