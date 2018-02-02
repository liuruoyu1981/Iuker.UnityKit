using System;
using System.Collections.Generic;
using Iuker.Common.Base;
using Iuker.Common.Base.Enums;
using Iuker.UnityKit.Run.Base;

namespace Iuker.UnityKit.Run.Module.Profiler
{
#if DEBUG
    /// <summary>
    /// 性能分析模块，用于动态调整App的性能及内存占用。
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20170910 15:59:31")]
    [ClassPurposeDesc("性能分析模块，用于动态调整App的性能及内存占用。", "性能分析模块，用于动态调整App的性能及内存占用。")]
#endif
    public class DefaultU3dModule_Profiler : AbsU3dModule, IU3dProfilerModule
    {
        public override string ModuleName
        {
            get
            {
                return ModuleType.Profiler.ToString();
            }
        }

        /// <summary>
        /// 对象池缓存容量字典
        /// </summary>
        private readonly Dictionary<string, int> mPoolCountDictionary = new Dictionary<string, int>();
        public void PoolOut(string poolKey, int poolCount)
        {
            if (mPoolCountDictionary.ContainsKey(poolKey))
            {
                //var count = mPoolCountDictionary[poolKey];
                mPoolCountDictionary[poolKey] += Convert.ToInt32(poolCount * 0.5);
            }
            else
            {
                mPoolCountDictionary[poolKey] = poolCount;
            }
        }
    }
}
