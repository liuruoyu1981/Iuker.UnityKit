/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/04/14 12:02:29
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
using Iuker.MoonSharp.Interpreter.DataStructs;
using Iuker.MoonSharp.Interpreter.DataTypes;

namespace Iuker.MoonSharp.Interpreter.Interop
{
    /// <summary>
    /// 用于转换moonsharp类型和clr类型的一组自定义转换器
    /// 如果转换函数没有指定或返回null，则应用标准的转换路径。
    /// </summary>
    public class CustomConvertersCollection
    {
        private Dictionary<Type, Func<DynValue, object>>[] m_Script2Clr = new Dictionary<Type, Func<DynValue, object>>[(int)LuaTypeExtensions.MaxConvertibleTypes + 1];
        private Dictionary<Type, Func<Script, object, DynValue>> m_Clr2Script = new Dictionary<Type, Func<Script, object, DynValue>>();


        internal CustomConvertersCollection()
        {
            for (int i = 0; i < m_Script2Clr.Length; i++)
            {
                m_Script2Clr[i] = new Dictionary<Type, Func<DynValue, object>>();
            }
        }

        // This needs to be evaluated further (doesn't work well with inheritance)
        //
        // 		private Dictionary<Type, Dictionary<Type, Func<object, object>>> m_Script2ClrUserData = new Dictionary<Type, Dictionary<Type, Func<object, object>>>();
        //
        //public void SetScriptToClrUserDataSpecificCustomConversion(Type destType, Type userDataType, Func<object, object> converter = null)
        //{
        //	var destTypeMap = m_Script2ClrUserData.GetOrCreate(destType, () => new Dictionary<Type, Func<object, object>>());
        //	destTypeMap[userDataType] = converter;

        //	SetScriptToClrCustomConversion(DataType.UserData, destType, v => DispatchUserDataCustomConverter(destTypeMap, v));
        //}

        //private object DispatchUserDataCustomConverter(Dictionary<Type, Func<object, object>> destTypeMap, DynValue v)
        //{
        //	if (v.Type != DataType.UserData)
        //		return null;

        //	if (v.UserData.Object == null)
        //		return null;

        //	Func<object, object> converter;

        //	for (Type userDataType = v.UserData.Object.GetType();
        //		userDataType != typeof(object);
        //		userDataType = userDataType.BaseType)
        //	{
        //		if (destTypeMap.TryGetValue(userDataType, out converter))
        //		{
        //			return converter(v.UserData.Object);
        //		}
        //	}

        //	return null;
        //}

        //public Func<object, object> GetScriptToClrUserDataSpecificCustomConversion(Type destType, Type userDataType)
        //{
        //	Dictionary<Type, Func<object, object>> destTypeMap;

        //	if (m_Script2ClrUserData.TryGetValue(destType, out destTypeMap))
        //	{
        //		Func<object, object> converter;

        //		if (destTypeMap.TryGetValue(userDataType, out converter))
        //		{
        //			return converter;
        //		}
        //	}

        //	return null;
        //}

        /// <summary>
        /// 设置一个将脚本类型转换为CLR类型的自定义转换器。设置null则会移除之前设置的自定义转换器
        /// </summary>
        /// <param name="scriptDataType">脚本数据类型</param>
        /// <param name="clrDataType">CLR数据类型</param>
        /// <param name="conveter">转换器或null</param>
        public void SetScriptToClrCustomConversion(DataType scriptDataType, Type clrDataType,
            Func<DynValue, object> conveter = null)
        {
            if ((int)scriptDataType > m_Script2Clr.Length)
                throw new ArgumentException("scriptDataType");

            Dictionary<Type, Func<DynValue, object>> map = m_Script2Clr[(int)scriptDataType];

            if (conveter == null)
            {
                if (map.ContainsKey(clrDataType))
                {
                    map.Remove(clrDataType);
                }
            }
            else
            {
                map[clrDataType] = conveter;
            }
        }

        /// <summary>
        /// 获取一个从脚本类型到CLR类型的自定义转换器或者null
        /// </summary>
        /// <param name="scriptDataType"></param>
        /// <param name="clrDataType"></param>
        /// <returns></returns>
        public Func<DynValue, object> GetScriptToClrCustomCoversion(DataType scriptDataType, Type clrDataType)
        {
            if ((int)scriptDataType > m_Script2Clr.Length)
                return null;

            Dictionary<Type, Func<DynValue, object>> map = m_Script2Clr[(int)(scriptDataType)];
            return map.GetOrDefault(clrDataType);
        }

        /// <summary>
        /// 设置一个将CLR类型转换为脚本类型的自定义转换器。设置null则会移除之前设置的自定义转换器
        /// </summary>
        /// <param name="clrDataType"></param>
        /// <param name="converter"></param>
        public void SetClrToScriptCustomConversion(Type clrDataType, Func<Script, object, DynValue> converter = null)
        {
            if (converter == null)
            {
                if (m_Clr2Script.ContainsKey(clrDataType))
                    m_Clr2Script.Remove(clrDataType);
            }
            else
            {
                m_Clr2Script[clrDataType] = converter;
            }
        }

        /// <summary>
        /// 设置一个将CLR类型转换为脚本类型的自定义转换器。设置null则会移除之前设置的自定义转换器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="converter"></param>
        public void SetClrToScriptCustomConversion<T>(Func<Script, T, DynValue> converter = null)
        {
            SetClrToScriptCustomConversion(typeof(T), (s, o) => converter(s, (T)o));
        }

        /// <summary>
        /// 从CLR数据类型获得自定义转换器，或null
        /// </summary>
        /// <param name="clrDataType"></param>
        /// <returns></returns>
        public Func<Script, object, DynValue> GetCrlToScriptCustomConversion(Type clrDataType)
        {
            return m_Clr2Script.GetOrDefault(clrDataType);
        }

        /// <summary>
        /// 从CLR数据类型设置自定义转换器。设置null来删除以前的自定义转换器。
        /// </summary>
        /// <param name="clrDataType"></param>
        /// <param name="converter"></param>
        [Obsolete("此方法已弃用，使用接受一个Script参数的重载方法。")]
        public void SetClrToScriptCustomConversion(Type clrDataType, Func<object, DynValue> converter = null)
        {
            SetClrToScriptCustomConversion(clrDataType, (s, o) => converter(o));
        }

        /// <summary>
        /// 从CLR数据类型设置自定义转换器。设置null来删除以前的自定义转换器。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="converter"></param>
        [Obsolete("此方法已弃用，使用接受一个Script参数的重载方法。")]
        public void SetClrToScriptCustomConversion<T>(Func<T, DynValue> converter = null)
        {
            SetClrToScriptCustomConversion(typeof(T), o => converter((T)o));
        }

        /// <summary>
        /// 移除所有转换器。
        /// </summary>
        public void Clear()
        {
            m_Clr2Script.Clear();

            for (int i = 0; i < m_Script2Clr.Length; i++)
                m_Script2Clr[i].Clear();
        }

    }
}
