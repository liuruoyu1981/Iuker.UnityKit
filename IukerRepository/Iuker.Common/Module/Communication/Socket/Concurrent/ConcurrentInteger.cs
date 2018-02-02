/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/04/26 10:46:11
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


using System.Threading;

namespace Iuker.Common.Module.Socket.Concurrent
{
    /// <summary>
    /// 线程安全整数
    /// </summary>
    public class ConcurrentInteger
    {
        public int Value { get; private set; }
        private readonly Mutex tex = new Mutex();

        public ConcurrentInteger() { }
        public ConcurrentInteger(int value)
        {
            lock (this)
            {
                Value = value;
            }
        }

        /// <summary>
        /// 自增并返回值
        /// </summary>
        /// <returns></returns>
        public int GetAndAdd()
        {
            lock (this)
            {
                tex.WaitOne();
                Value++;
                tex.ReleaseMutex();
                return Value;
            }
        }

        /// <summary>
        /// 自减并返回值
        /// </summary>
        /// <returns></returns>
        public int GetAndReduce()
        {
            lock (this)
            {
                tex.WaitOne();
                Value--;
                tex.ReleaseMutex();
                return Value;
            }
        }

        /// <summary>
        /// 重置value为0
        /// </summary>
        public void Reset()
        {
            lock (this)
            {
                tex.WaitOne();
                Value = 0;
                tex.ReleaseMutex();
            }
        }

    }
}
