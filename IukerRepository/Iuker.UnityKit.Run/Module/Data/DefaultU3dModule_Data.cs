/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 6/17/2017 11:03
Email: liuruoyu1981@gmail.com
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
using Iuker.Common.Base.Enums;
using Iuker.Common.DataTypes.ReactiveDatas;
using Iuker.UnityKit.Run.Base;

namespace Iuker.UnityKit.Run.Module.Data
{
    /// <summary>
    /// 默认U3d数据模块
    /// </summary>
    public class DefaultU3dModule_Data : AbsU3dModule, IU3dDataModule
    {
        #region 数据字典

        private readonly Dictionary<string, IReactiveStruct<int>> mIntDictionary =
            new Dictionary<string, IReactiveStruct<int>>();

        private readonly Dictionary<string, IReactiveStruct<long>> mLongDictionary =
            new Dictionary<string, IReactiveStruct<long>>();

        private readonly Dictionary<string, IReactiveStruct<float>> mFloatDictionary =
            new Dictionary<string, IReactiveStruct<float>>();

        private readonly Dictionary<string, IReactiveStruct<double>> mDoubleDictionary =
            new Dictionary<string, IReactiveStruct<double>>();

        private readonly Dictionary<string, IReactiveStruct<string>> mStringDictionary =
            new Dictionary<string, IReactiveStruct<string>>();

        private readonly Dictionary<string, List<Action<int>>> mIntInitActionDictionary =
            new Dictionary<string, List<Action<int>>>();

        private readonly Dictionary<string, List<Action<int>>> mIntUpdteActionDictionary =
            new Dictionary<string, List<Action<int>>>();

        private readonly Dictionary<string, List<Action<long>>> mLongInitActionDictionary =
            new Dictionary<string, List<Action<long>>>();

        private readonly Dictionary<string, List<Action<long>>> mLongUpdteActionDictionary =
            new Dictionary<string, List<Action<long>>>();

        private readonly Dictionary<string, List<Action<float>>> mFloatInitActionDictionary =
            new Dictionary<string, List<Action<float>>>();

        private readonly Dictionary<string, List<Action<double>>> mDoubleInitActionDictionary =
            new Dictionary<string, List<Action<double>>>();

        private readonly Dictionary<string, List<Action<float>>> mFloatUpdateActionDictionary =
            new Dictionary<string, List<Action<float>>>();

        private readonly Dictionary<string, List<Action<double>>> mDoubleUpdateActionDictionary =
            new Dictionary<string, List<Action<double>>>();

        private readonly Dictionary<string, List<Action<string>>> mStringInitActionDictionary =
            new Dictionary<string, List<Action<string>>>();

        private readonly Dictionary<string, List<Action<string>>> mStringUpdateActionDictionary =
            new Dictionary<string, List<Action<string>>>();

        #endregion


        public void SetInt(string key, int newData)
        {
            if (mIntDictionary.ContainsKey(key)) //  数据已存在因此为更新操作
            {
                if (mIntUpdteActionDictionary.ContainsKey(key)) //  如果该数据当前有更新事件委托列表存在
                {
                    var updateActions = mIntUpdteActionDictionary[key]; //  获得该数据的更新事件委托列表
                    var reactiveInt = mIntDictionary[key]; //  获得该响应式数据
                    updateActions.ForEach(del => reactiveInt.AddOnUpdateAction(del)); //  将更新事件委托列表添加到数据中
                    mIntUpdteActionDictionary.Remove(key); //  移除该数据的更新事件委托列表
                    reactiveInt.Assign(newData); //  更新该数据
                }
                //  没有更新事件委托列表存在
                else
                {
                    var reactiveInt = mIntDictionary[key]; //  获得该响应式数据
                    reactiveInt.Assign(newData); //  更新该数据
                }
            }
            else
            {
                var reactiveInt = U3DFrame.InjectModule.GetInstance<IReactiveStruct<int>>();
                if (mIntInitActionDictionary.ContainsKey(key))
                {
                    var initActions = mIntInitActionDictionary[key];
                    initActions.ForEach(del => reactiveInt.AddOnInitAction(del));
                    mIntInitActionDictionary.Remove(key);
                }
                reactiveInt.Init(newData);
                mIntDictionary.Add(key, reactiveInt);
            }
        }


        public void SetLong(string key, long newData)
        {
            if (mLongDictionary.ContainsKey(key)) //  数据已存在因此为更新操作
            {
                if (mLongUpdteActionDictionary.ContainsKey(key)) //  如果该数据当前有更新事件委托列表存在
                {
                    var updateActions = mLongUpdteActionDictionary[key]; //  获得该数据的更新事件委托列表
                    var reactiveLong = mLongDictionary[key]; //  获得该响应式数据
                    updateActions.ForEach(del => reactiveLong.AddOnUpdateAction(del)); //  将更新事件委托列表添加到数据中
                    mLongUpdteActionDictionary.Remove(key); //  移除该数据的更新事件委托列表
                    reactiveLong.Assign(newData); //  更新该数据
                }
                //  没有更新事件委托列表存在
                else
                {
                    var reactiveLong = mLongDictionary[key]; //  获得该响应式数据
                    reactiveLong.Assign(newData); //  更新该数据
                }
            }
            else
            {
                var reactiveLong = U3DFrame.InjectModule.GetInstance<IReactiveStruct<long>>();
                if (mLongInitActionDictionary.ContainsKey(key))
                {
                    var initActions = mLongInitActionDictionary[key];
                    initActions.ForEach(del => reactiveLong.AddOnInitAction(del));
                    mLongInitActionDictionary.Remove(key);
                }
                reactiveLong.Init(newData);
                mLongDictionary.Add(key, reactiveLong);
            }
        }

        public void SetString(string key, string newData)
        {
            if (mStringDictionary.ContainsKey(key)) //  数据已存在因此为更新操作
            {
                if (mStringUpdateActionDictionary.ContainsKey(key)) //  如果该数据当前有更新事件委托列表存在
                {
                    var updateActions = mStringUpdateActionDictionary[key]; //  获得该数据的更新事件委托列表
                    var reactiveString = mStringDictionary[key]; //  获得该响应式数据
                    updateActions.ForEach(del => reactiveString.AddOnUpdateAction(del)); //  将更新事件委托列表添加到数据中
                    mStringUpdateActionDictionary.Remove(key); //  移除该数据的更新事件委托列表
                    reactiveString.Assign(newData); //  更新该数据
                }
                //  没有更新事件委托列表存在
                else
                {
                    var reactiveString = mStringDictionary[key]; //  获得该响应式数据
                    reactiveString.Assign(newData); //  更新该数据
                }
            }
            else
            {
                var reactiveString = U3DFrame.InjectModule.GetInstance<IReactiveStruct<string>>();
                if (mStringInitActionDictionary.ContainsKey(key))
                {
                    var initActions = mStringInitActionDictionary[key];
                    initActions.ForEach(del => reactiveString.AddOnInitAction(del));
                    mStringInitActionDictionary.Remove(key);
                }
                reactiveString.Init(newData);
                mStringDictionary.Add(key, reactiveString);
            }
        }

        public void SetFloat(string key, float newData)
        {
            if (mFloatDictionary.ContainsKey(key)) //  数据已存在因此为更新操作
            {
                if (mFloatUpdateActionDictionary.ContainsKey(key)) //  如果该数据当前有更新事件委托列表存在
                {
                    var updateActions = mFloatUpdateActionDictionary[key]; //  获得该数据的更新事件委托列表
                    var reactiveFloat = mFloatDictionary[key]; //  获得该响应式数据
                    updateActions.ForEach(del => reactiveFloat.AddOnUpdateAction(del)); //  将更新事件委托列表添加到数据中
                    mFloatUpdateActionDictionary.Remove(key); //  移除该数据的更新事件委托列表
                    reactiveFloat.Assign(newData); //  更新该数据
                }
                //  没有更新事件委托列表存在
                else
                {
                    var reactiveFloat = mFloatDictionary[key]; //  获得该响应式数据
                    reactiveFloat.Assign(newData); //  更新该数据
                }
            }
            else
            {
                var reactiveFloat = U3DFrame.InjectModule.GetInstance<IReactiveStruct<float>>();
                if (mFloatInitActionDictionary.ContainsKey(key))
                {
                    var initActions = mFloatInitActionDictionary[key];
                    initActions.ForEach(del => reactiveFloat.AddOnInitAction(del));
                    mFloatInitActionDictionary.Remove(key);
                }
                reactiveFloat.Init(newData);
                mFloatDictionary.Add(key, reactiveFloat);
            }
        }

        public void SetDouble(string key, double newData)
        {
            if (mDoubleDictionary.ContainsKey(key)) //  数据已存在因此为更新操作
            {
                if (mDoubleUpdateActionDictionary.ContainsKey(key)) //  如果该数据当前有更新事件委托列表存在
                {
                    var updateActions = mDoubleUpdateActionDictionary[key]; //  获得该数据的更新事件委托列表
                    var reactiveFloat = mDoubleDictionary[key]; //  获得该响应式数据
                    updateActions.ForEach(del => reactiveFloat.AddOnUpdateAction(del)); //  将更新事件委托列表添加到数据中
                    mDoubleUpdateActionDictionary.Remove(key); //  移除该数据的更新事件委托列表
                    reactiveFloat.Assign(newData); //  更新该数据
                }
                //  没有更新事件委托列表存在
                else
                {
                    var reactDouble = mDoubleDictionary[key]; //  获得该响应式数据
                    reactDouble.Assign(newData); //  更新该数据
                }
            }
            else
            {
                var reactDouble = U3DFrame.InjectModule.GetInstance<IReactiveStruct<double>>();
                if (mDoubleInitActionDictionary.ContainsKey(key))
                {
                    var initActions = mDoubleInitActionDictionary[key];
                    initActions.ForEach(del => reactDouble.AddOnInitAction(del));
                    mDoubleInitActionDictionary.Remove(key);
                }
                reactDouble.Init(newData);
                mDoubleDictionary.Add(key, reactDouble);
            }
        }

        /// <summary>
        /// 获得一个int32类型值
        /// 如果没有指定的key所对应的数据则会返回int32的最大范围值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public int GetInt(string key)
        {
            if (!mIntDictionary.ContainsKey(key)) return int.MaxValue;
            var result = mIntDictionary[key];
            return result.Value;
        }

        /// <summary>
        /// 获得一个int64类型值
        /// 如果没有指定的key所对应的数据则会返回int64的最大范围值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public long GetLong(string key)
        {
            if (!mLongDictionary.ContainsKey(key)) return long.MaxValue;
            var result = mLongDictionary[key];
            return result.Value;
        }

        /// <summary>
        /// 获得一个字符串类型值
        /// 如果没有指定的key所对应的数据则会返回Null
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetString(string key)
        {
            if (!mStringDictionary.ContainsKey(key)) return null;
            var result = mStringDictionary[key];
            return result.Value;
        }

        /// <summary>
        /// 获得一个浮点类型值
        /// 如果没有指定的key所对应的数据则会返回float的最大范围值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public float GetFloat(string key)
        {
            if (!mFloatDictionary.ContainsKey(key)) return float.MaxValue;
            var result = mFloatDictionary[key];
            return result.Value;
        }

        public double GetDouble(string key)
        {
            if (!mDoubleDictionary.ContainsKey(key)) return double.MaxValue;
            var result = mDoubleDictionary[key];
            return result.Value;
        }

        public void WatchInt(string key, Action<int> update, Action<int> init = null)
        {
            if (init != null)
            {
                if (!mIntInitActionDictionary.ContainsKey(key))
                {
                    mIntInitActionDictionary.Add(key, new List<Action<int>> { init });
                }
                else
                {
                    mIntInitActionDictionary[key].Add(init);
                }
            }
            if (!mIntUpdteActionDictionary.ContainsKey(key))
            {
                mIntUpdteActionDictionary.Add(key, new List<Action<int>> { update });
            }
            else
            {
                mIntUpdteActionDictionary[key].Add(update);
            }
        }

        public void WatchLong(string key, Action<long> update, Action<long> init = null)
        {
            if (init != null)
            {
                if (!mLongInitActionDictionary.ContainsKey(key))
                {
                    mLongInitActionDictionary.Add(key, new List<Action<long>> { init });
                }
                else
                {
                    mLongInitActionDictionary[key].Add(init);
                }
            }
            if (!mLongUpdteActionDictionary.ContainsKey(key))
            {
                mLongUpdteActionDictionary.Add(key, new List<Action<long>> { update });
            }
            else
            {
                mLongUpdteActionDictionary[key].Add(update);
            }
        }

        public void WatchString(string key, Action<string> update, Action<string> init = null)
        {
            if (init != null)
            {
                if (!mStringInitActionDictionary.ContainsKey(key))
                {
                    mStringInitActionDictionary.Add(key, new List<Action<string>> { init });
                }
                else
                {
                    mStringInitActionDictionary[key].Add(init);
                }
            }
            if (!mStringUpdateActionDictionary.ContainsKey(key))
            {
                mStringUpdateActionDictionary.Add(key, new List<Action<string>> { update });
            }
            else
            {
                mStringUpdateActionDictionary[key].Add(update);
            }
        }

        public void WatchFloat(string key, Action<float> update, Action<float> init = null)
        {
            if (init != null)
            {
                if (!mFloatInitActionDictionary.ContainsKey(key))
                {
                    mFloatInitActionDictionary.Add(key, new List<Action<float>> { init });
                }
                else
                {
                    mFloatInitActionDictionary[key].Add(init);
                }
            }
            if (!mFloatUpdateActionDictionary.ContainsKey(key))
            {
                mFloatUpdateActionDictionary.Add(key, new List<Action<float>> { update });
            }
            else
            {
                mFloatUpdateActionDictionary[key].Add(update);
            }
        }

        /// <summary>
        /// 观察一个引用类型数据（非列表、集合、字典、哈希表）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="onDataChanged"></param>
        public void WatchData<T>(string key, Action<T> onDataChanged)
        {
            if (!mObjectDataDictionary.ContainsKey(key))
            {
                mObjectDataDictionary.Add(key, onDataChanged);
            }
        }

        public void SetData<T>(string key, T newData)
        {
            if (!mObjectDataDictionary.ContainsKey(key))
            {
                mObjectDataDictionary.Add(key, newData);
            }
            else
            {
                mObjectDataDictionary[key] = newData;
            }
        }

        private readonly Dictionary<string, object> mObjectDataDictionary = new Dictionary<string, object>();

        /// <summary>
        /// 获取一个引用类型数据（非列表、集合、字典、哈希表）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetData<T>(string key) where T : class, new()
        {
            if (mObjectDataDictionary.ContainsKey(key))
            {
                var result = mObjectDataDictionary[key] as T;
                return result;
            }

#if UNITY_EDITOR || DEBUG
            Debuger.LogError(string.Format("没有找到指定关键字{0}的数据项！", key));
#endif

            return default(T);
        }

        public override string ModuleName
        {
            get
            {
                return ModuleType.Data.ToString();
            }
        }


    }
}