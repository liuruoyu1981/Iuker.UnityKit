/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 6/28/2017 14:53
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

namespace Iuker.Common.DataTypes.Collections
{
    public static class ObservableListWrapper<T>
    {
        [ThreadStatic]
        private static readonly Dictionary<string, ObservableList<T>> mListDictionary =
            new Dictionary<string, ObservableList<T>>();

        [ThreadStatic]
        private static readonly Dictionary<string, List<ObservableListEventInfo<T>>> mEventInfoDictionary = new Dictionary<string, List<ObservableListEventInfo<T>>>();

        public static void AddEventInfo(string key, ObservableListEventInfo<T> eventInfo)
        {
            if (!mEventInfoDictionary.ContainsKey(key))
            {
                mEventInfoDictionary.Add(key, new List<ObservableListEventInfo<T>> { eventInfo });
            }
            else
            {
                mEventInfoDictionary[key].Add(eventInfo);
            }
        }

        public static bool IsExist(string key)
        {
            var result = mListDictionary.ContainsKey(key);
            return result;
        }

        private static ObservableList<T> GetList(string key)
        {
            var list = mListDictionary[key];
            return list;
        }

        /// <summary>
        /// 初始化一个可观察列表
        /// </summary>
        /// <param name="key"></param>
        /// <param name="sourceList"></param>
        public static void InitList(string key, IList<T> sourceList)
        {
            var list = new ObservableList<T>();
            if (mEventInfoDictionary.ContainsKey(key))
            {
                var eventInfos = mEventInfoDictionary[key];
                foreach (var info in eventInfos)
                {
                    BindEvent(key, info);
                }
            }

            list.Init(sourceList);  //  将原始列表数据置入可观察列表中
        }

        public static void BindEvent(string key, ObservableListEventInfo<T> eventInfo)
        {
            if (!IsExist(key)) return;

            var list = GetList(key);
            list.WatchOnInit(eventInfo.mOnInit);
            list.WatchOnAdd(eventInfo.mOnAdd);
            list.WatchOnRemove(eventInfo.mOnRemove);
            list.WatchOnInsert(eventInfo.mOnInsert);
            list.WatchOnClear(eventInfo.mOnClear);
        }

        /// <summary>
        /// 清理可观察列表数据容器
        /// 进行内存回收
        /// </summary>
        public static void Clear()
        {
            mListDictionary.Clear();
        }
    }
}