using System;
using System.Collections;
using System.Collections.Generic;
using Iuker.Common.Base;

namespace Iuker.UnityKit.Run.Parallel.Core
{
#if DEBUG
    /// <summary>
    /// 协程助手
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20170923 09:33:24")]
    [ClassPurposeDesc("协程助手", "协程助手")]
#endif
    // ReSharper disable once ClassNeverInstantiated.Global
    public class CoroutineHelper : MonoSingleton<CoroutineHelper>
    {
        public static CoroutineContext<T1, T2> WhenAll<T1, T2>(Func<T1, T2, Action, IEnumerator> task, T1 args1, T2 args2, Action complete)
        {
            var ctx = new CoroutineContext<T1, T2>();
            return ctx.WhenAll(task, args1, args2, complete);
        }

        /// <summary>
        /// 协程运行环境
        /// </summary>
        public class CoroutineContext<T1, T2>
        {
            private readonly List<Func<T1, T2, Action, IEnumerator>> mTasks =
                new List<Func<T1, T2, Action, IEnumerator>>();
            private readonly List<CoroutineArgs<T1, T2>> mArgs = new List<CoroutineArgs<T1, T2>>();
            private int mIndex;
            private Action mAllDone;

            private void StateUpdate()
            {
                mIndex++;
                if (mIndex == mTasks.Count)
                {
                    if (mAllDone != null)
                    {
                        mAllDone();
                    }
                }
            }

            public CoroutineContext<T1, T2> WhenAll(Func<T1, T2, Action, IEnumerator> task, T1 args1, T2 args2, Action complete)
            {
                complete += StateUpdate;
                mArgs.Add(new CoroutineArgs<T1, T2>(args1, args2, complete));
                mTasks.Add(task);

                return this;
            }

            public void Do(Action allDone)
            {
                mAllDone = allDone;
                for (int i = 0; i < mTasks.Count; i++)
                {
                    var t = mTasks[i];
                    var a = mArgs[i];
                    Instance.StartCoroutine(t.Invoke(a.Args1, a.Args2, a.Complete));
                }
            }

            private class CoroutineArgs<F1, F2>
            {
                public readonly F1 Args1;
                public readonly F2 Args2;
                public readonly Action Complete;

                public CoroutineArgs(F1 f1, F2 f2, Action complete)
                {
                    Args1 = f1;
                    Args2 = f2;
                    Complete = complete;
                }
            }
        }












    }
}
