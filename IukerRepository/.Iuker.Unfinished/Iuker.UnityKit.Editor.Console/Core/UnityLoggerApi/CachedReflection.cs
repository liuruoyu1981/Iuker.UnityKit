/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 6/16/2017 19:24
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

using System.Collections.Generic;

namespace Iuker.UnityKit.Editor.Console.Core.UnityLoggerApi
{
    /// <summary>
    /// 已缓存的反射对象
    /// 内部维护一个反射对象字典
    /// </summary>
    public class CachedReflection
    {
        private static readonly Dictionary<string,object> mCache = new Dictionary<string, object>();

        public static T Get<T>(string key) => (T) mCache[key];

        public static void Cache(string key, object value) => mCache[key] = value;

        public static bool Has(string key) => mCache.ContainsKey(key);
    }
}