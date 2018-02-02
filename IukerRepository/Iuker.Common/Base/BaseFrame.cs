using System;
using System.Collections.Generic;

namespace Iuker.Common.Base
{
    public class BaseFrame
    {
        /// <summary>
        /// 模块实例字典
        /// </summary>
        protected readonly Dictionary<string, IModule> ModuleDictionary = new Dictionary<string, IModule>();

        /// <summary>
        /// 模块类型字典
        /// </summary>
        protected Dictionary<string, Type> ModuleTypeDictionary = new Dictionary<string, Type>();

        /// <summary>
        /// 获得指定模块实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetModule<T>() where T : class, IModule
        {
            var typeName = typeof(T).Name;
            if (!ModuleDictionary.ContainsKey(typeName))
            {
                throw new Exception(string.Format("指定的{0}模块不存在！", typeName));
            }

            var module = ModuleDictionary[typeName] as T;
            return module;
        }
    }
}