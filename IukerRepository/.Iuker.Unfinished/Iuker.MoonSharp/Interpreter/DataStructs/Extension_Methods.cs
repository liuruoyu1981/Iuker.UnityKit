/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/04/14 10:47:28
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
using System.Collections.Generic;

namespace Iuker.MoonSharp.Interpreter.DataStructs
{
    /// <summary>
    /// 在整个项目中使用的扩展方法。
    /// </summary>
    internal static class Extension_Methods
    {
        /// <summary>
        /// 从字典获取一个值，或者返回默认值
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static TValue GetOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key)
        {
            TValue v;

            if (dictionary.TryGetValue(key, out v))
                return v;

            return default(TValue);
        }

        /// <summary>
        /// 从字典中获取一个值，或者创建它
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <param name="creator"></param>
        /// <returns></returns>
        public static TValue GetOrCreate<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key,
            Func<TValue> creator)
        {
            TValue v;

            if (!dictionary.TryGetValue(key, out v))
            {
                v = creator();
                dictionary.Add(key, v);
            }

            return v;
        }
    }
}
