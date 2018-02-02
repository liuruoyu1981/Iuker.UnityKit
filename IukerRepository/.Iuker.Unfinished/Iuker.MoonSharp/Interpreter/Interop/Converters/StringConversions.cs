﻿/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/04/14 12:18:13
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
using System.Text;
using Iuker.MoonSharp.Interpreter.DataTypes;
using Iuker.MoonSharp.Interpreter.Errors;

namespace Iuker.MoonSharp.Interpreter.Interop.Converters
{
    internal static class StringConversions
    {
        internal enum  StringSubtype
        {
            None,
            String,
            StringBuilder,
            Char
        }

        internal static StringSubtype GetStringSubtype(Type desiredType)
        {
            if (desiredType == typeof(string))
                return StringSubtype.String;
            else if (desiredType == typeof(StringBuilder))
                return StringSubtype.StringBuilder;
            else if (desiredType == typeof(char))
                return StringSubtype.Char;
            else
                return StringSubtype.None;
        }


        internal static object ConvertString(StringSubtype stringSubType, string str, Type desiredType, DataType dataType)
        {
            switch (stringSubType)
            {
                case StringSubtype.String:
                    return str;
                case StringSubtype.StringBuilder:
                    return new StringBuilder(str);
                case StringSubtype.Char:
                    if (!string.IsNullOrEmpty(str))
                        return str[0];
                    break;
                case StringSubtype.None:
                default:
                    break;
            }

            throw ScriptRuntimeException.ConvertObjectFailed(dataType, desiredType);
        }






    }
}
