using System.Collections.Generic;
using System.Threading;

namespace Iuker.UnityKit.Run.Base.Parallel
{
    /// <summary>
    /// 任务调度器
    /// 1. 调度普通任务
    /// 2. 调度Unity任务
    /// </summary>
    public static class TaskDispacher
    {
        private static readonly Queue<Thread> ThreadPool = new Queue<Thread>();
        private static readonly Queue<ITask> TaskQueue = new Queue<ITask>();
        private static int MaxTheadCount;
        public static void SetMaxThread(int count)
        {
            MaxTheadCount = count;
        }

        public static void EnqueueWork(ITask task)
        {
            TaskQueue.Enqueue(task);
        }


    }
}
