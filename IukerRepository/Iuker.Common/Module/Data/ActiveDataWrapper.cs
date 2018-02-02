/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/06/17 11:10
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
using Iuker.Common.Base;


namespace Iuker.Common.Module.Data
{
    /// <summary>
    /// 响应式引用数据包装器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class ActiveDataWrapper<T>
    {
        [ThreadStatic]
        private static readonly Dictionary<string, IReactiveClass<T>> DataDictionary = new Dictionary<string, IReactiveClass<T>>();

        public static bool IsExist(string key)
        {
            var result = DataDictionary.ContainsKey(key);
            return result;
        }

        public static IReactiveClass<T> GetData(string key)
        {
            var result = DataDictionary[key];
            return result;
        }

        public static void SetData(string key, IReactiveClass<T> sourceData)
        {
            DataDictionary.Add(key, sourceData);
        }
    }



}
