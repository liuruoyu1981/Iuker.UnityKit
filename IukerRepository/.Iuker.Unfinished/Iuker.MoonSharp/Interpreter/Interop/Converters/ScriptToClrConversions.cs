/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/04/14 12:18:04
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
using Iuker.MoonSharp.Interpreter.DataTypes;
using Iuker.MoonSharp.Interpreter.Errors;

namespace Iuker.MoonSharp.Interpreter.Interop.Converters
{
    internal class ScriptToClrConversions
    {
        internal const int WEIGHT_MAX_VALUE = 100;
        internal const int WEIGHT_CUSTOM_CONVERTER_MATCH = 100;
        internal const int WEIGHT_EXACT_MATCH = 100;
        internal const int WEIGHT_STRING_TO_STRINGBUILDER = 99;
        internal const int WEIGHT_STRING_TO_CHAR = 98;
        internal const int WEIGHT_NIL_TO_NULLABLE = 100;
        internal const int WEIGHT_NIL_TO_REFTYPE = 100;
        internal const int WEIGHT_VOID_WITH_DEFAULT = 50;
        internal const int WEIGHT_VOID_WITHOUT_DEFAULT = 25;
        internal const int WEIGHT_NIL_WITH_DEFAULT = 25;
        internal const int WEIGHT_BOOL_TO_STRING = 5;
        internal const int WEIGHT_NUMBER_TO_STRING = 50;
        internal const int WEIGHT_NUMBER_TO_ENUM = 90;
        internal const int WEIGHT_USERDATA_TO_STRING = 5;
        internal const int WEIGHT_TABLE_CONVERSION = 90;
        internal const int WEIGHT_NUMBER_DOWNCAST = 99;
        internal const int WEIGHT_NO_MATCH = 0;
        internal const int WEIGHT_NO_EXTRA_PARAMS_BONUS = 100;
        internal const int WEIGHT_EXTRA_PARAMS_MALUS = 2;
        internal const int WEIGHT_BYREF_BONUSMALUS = -10;
        internal const int WEIGHT_VARARGS_MALUS = 1;
        internal const int WEIGHT_VARARGS_EMPTY = 40;

        /// <summary>
        /// 将一个Lua动态对象转换为CLR对象(简单的转换)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal static object DynValueToObject(DynValue value)
        {
            var converter = Script.GlobalOptions.CustomConverters.GetScriptToClrCustomCoversion(value.Type,
                typeof(System.Object));

            if (converter != null)
            {
                var v = converter(value);
                if (v != null)
                    return v;
            }

            switch (value.Type)
            {
                case DataType.Void:
                case DataType.Nil:
                    return null;
                case DataType.Boolean:
                    return value.Boolean;
                case DataType.Number:
                    return value.Number;
                case DataType.String:
                    return value.String;
                case DataType.Function:
                    return value.Function;
                case DataType.Table:
                    return value.Table;
                case DataType.Tuple:
                    return value.Tuple;
                case DataType.UserData:
                    if (value.UserData.Object != null)
                        return value.UserData.Object;
                    else if (value.UserData.Descriptor != null)
                        return value.UserData.Descriptor.Type;
                    else
                        return null;
                case DataType.ClrFunction:
                    return value.Callback;
                default:
                    throw ScriptRuntimeException.ConvertObjectFailed(value.Type);
            }
        }

        /// <summary>
        /// 将一个dyn值转换为一个特定类型的CLR对象
        /// </summary>
        /// <param name="value"></param>
        /// <param name="desiredType"></param>
        /// <param name="defaultValue"></param>
        /// <param name="isOptional"></param>
        /// <returns></returns>
        internal static object DynValueToObjectOfType(DynValue value, Type desiredType, object defaultValue,
            bool isOptional)
        {
            if (desiredType.IsByRef)
                desiredType = desiredType.GetElementType();

            var converter = Script.GlobalOptions.CustomConverters.GetScriptToClrCustomCoversion(value.Type, desiredType);
            if (converter != null)
            {
                var v = converter(value);
                if (v != null) return v;
            }

            if (desiredType == typeof(DynValue))
                return value;

            if (desiredType == typeof(object))
                return DynValueToObject(value);



            return null;
        }












    }
}
