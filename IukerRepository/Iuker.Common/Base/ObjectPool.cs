/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2/12/2017 16:23:29
Email: 35490136@qq.com
QQCode: 35490136
CreateNote: 基础接口
***********************************************************************************************/


/****************************************修改日志***********************************************
1. 修改日期： 修改人： 修改内容：
2. 修改日期： 修改人： 修改内容：
3. 修改日期： 修改人： 修改内容：
4. 修改日期： 修改人： 修改内容：
5. 修改日期： 修改人： 修改内容：
****************************************修改日志***********************************************/

using System;
using Iuker.Common.Base.Interfaces;
using Iuker.Common.Module.Profiler;

namespace Iuker.Common
{
    /// <summary>
    /// 泛型对象池
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObjectPool<T> : IObjectPool<T> where T : class
    {
        /// <summary>
        /// 缓存对象数组
        /// </summary>
        private T[] mObjectArray;

        /// <summary>
        /// 性能分析模块
        /// </summary>
        private IProfilerModule mProfilerModule;

        /// <summary>
        /// 对象池关键字
        /// </summary>
        private readonly string mPoolKey;

        /// <summary>
        /// 基础泛型对象池
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="count"></param>
        /// <param name="poolKey"></param>
        /// <param name="profilerModule"></param>
        /// <param name="onCreated"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ObjectPool(IObjectFactory<T> factory, int count,
            string poolKey = null, IProfilerModule profilerModule = null, Action<T> onCreated = null)
        {
            mObjectArray = new T[count];
            //  填充对象池
            for (var i = 0; i < mObjectArray.Length; i++)
            {
                var obj = factory.CreateObject();
                if (onCreated != null)
                {
                    onCreated(obj);
                }
                mObjectArray[i] = obj;
            }

            mProfilerModule = profilerModule;
            mPoolKey = poolKey;
        }

        public void SetProfilerModule(IProfilerModule profilerModule)
        {
            mProfilerModule = profilerModule;
        }

        /// <summary>
        /// 已使用对象数量
        /// </summary>
        public int UseCount { get; private set; }

        /// <summary>
        /// 获取一个对象
        /// 如果对象池中有空闲对象，则默认返回第一个
        /// 如果对象池中没有空闲对象，则使用工厂创建一个
        /// </summary>
        /// <returns>返回一个实例</returns>
        public T Take()
        {
            while (true)
            {
                //  耗尽
                if (UseCount == mObjectArray.Length - 1)
                {
                    //  增长对象数组
                    var length = mObjectArray.Length;
                    length += Convert.ToInt32(length * 0.5);
                    var oldArray = mObjectArray;
                    mObjectArray = new T[length];
                    for (var i = 0; i < oldArray.Length; i++)
                    {
                        mObjectArray[i] = oldArray[i];
                    }

                    //  记录容量超出
                    if (string.IsNullOrEmpty(mPoolKey))
                    {
                        if (mProfilerModule != null)
                        {
                            mProfilerModule.PoolOut(mPoolKey, length);
                        }
                    }
                    continue;
                }

                var obj = mObjectArray[UseCount];
                UseCount++;
                return obj;
            }
        }

        /// <summary>
        /// 归还一个对象到对象池
        /// 如果对象池已达上限，则丢弃该对象
        /// 如果对象池没有到达上限，则回收该对象
        /// </summary>
        /// <param name="t">归还的实例</param>
        public void Restore(T t)
        {
            if (UseCount == 0) return;

            UseCount--;
            mObjectArray[UseCount] = t;
        }

        /// <summary>
        /// 清空对象池
        /// </summary>
        public void Clear()
        {
            mObjectArray = null;
        }

        /// <summary>
        /// 对对象池中的每个缓存对象执行指定操作
        /// </summary>
        /// <param name="del"></param>
        public void ForEach(Action<T> del)
        {
            foreach (var x1 in mObjectArray)
            {
                if (x1 != null)
                {
                    del(x1);
                }
            }
            mObjectArray = null;
        }
    }
}