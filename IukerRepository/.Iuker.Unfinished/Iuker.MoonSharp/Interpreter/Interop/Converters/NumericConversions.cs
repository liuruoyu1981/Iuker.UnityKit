/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/04/14 12:17:56
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

namespace Iuker.MoonSharp.Interpreter.Interop.Converters
{
    /// <summary>
    /// 处理数值类型转换的静态函数
    /// </summary>
    internal static class NumericConversions
    {
        static NumericConversions()
        {
            NumericTypesOrdered = new Type[]
            {
                typeof(double),
                typeof(decimal),
                typeof(float),
                typeof(long),
                typeof(int),
                typeof(short),
                typeof(sbyte),
                typeof(ulong),
                typeof(uint),
                typeof(ushort),
                typeof(byte),
            };
            NumericTypes = new HashSet<Type>(NumericTypesOrdered);
        }

        /// <summary>
        /// 数字类型的哈希表
        /// </summary>
        internal static readonly HashSet<Type> NumericTypes;

        /// <summary>
        /// 用于转换的数值类型数组
        /// </summary>
        internal static readonly Type[] NumericTypesOrdered;

        /// <summary>
        /// 将一个double值转换为其他类型
        /// MoonSharp（lua）中只有数值类型，映射到C#中即double类型
        /// C#层面的所有数字类型都需转换为double类型，然后置入DynValue对象的Number字段中。
        /// </summary>
        /// <param name="type"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        internal static object DoubleToType(Type type, double d)
        {
            type = Nullable.GetUnderlyingType(type) ?? type;

            if (type == typeof(double)) return d;
            if (type == typeof(sbyte)) return (sbyte)d;
            if (type == typeof(byte)) return (byte)d;
            if (type == typeof(short)) return (short)d;
            if (type == typeof(ushort)) return (ushort)d;
            if (type == typeof(int)) return (int)d;
            if (type == typeof(uint)) return (uint)d;
            if (type == typeof(long)) return (long)d;
            if (type == typeof(ulong)) return (ulong)d;
            if (type == typeof(float)) return (float)d;
            if (type == typeof(decimal)) return (decimal)d;
            return d;
        }

        /// <summary>
        /// 将一个类型对象转换为double
        /// </summary>
        internal static double TypeToDouble(Type type, object d)
        {
            if (type == typeof(double)) return (double)d;
            if (type == typeof(sbyte)) return (double)(sbyte)d;
            if (type == typeof(byte)) return (double)(byte)d;
            if (type == typeof(short)) return (double)(short)d;
            if (type == typeof(ushort)) return (double)(ushort)d;
            if (type == typeof(int)) return (double)(int)d;
            if (type == typeof(uint)) return (double)(uint)d;
            if (type == typeof(long)) return (double)(long)d;
            if (type == typeof(ulong)) return (double)(ulong)d;
            if (type == typeof(float)) return (double)(float)d;
            if (type == typeof(decimal)) return (double)(decimal)d;
            return (double)d;
        }



















    }
}
