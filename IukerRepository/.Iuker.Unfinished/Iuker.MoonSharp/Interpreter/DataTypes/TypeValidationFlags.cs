﻿/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/04/14 10:51:43
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

namespace Iuker.MoonSharp.Interpreter.DataTypes
{
    /// <summary>
    /// Flags to alter the way the DynValue.CheckType and other related functions operate on data types for
    /// validation.
    /// </summary>
    [Flags]
    public enum TypeValidationFlags
    {
        /// <summary>
        /// No type validation specific behaviour
        /// </summary>
        None = 0,
        /// <summary>
        /// Nil and Void values are allowed (and returned by the call)
        /// </summary>
        AllowNil = 0x1,
        /// <summary>
        /// Simple autoconversions are attempted: 
        /// 1) Numbers are convertible to strings
        /// 2) Strings are convertible to numbers if they contain a number
        /// 3) Everything is convertible to boolean (with void and nil converting to 'false', everything else converting to 'true')
        /// Note: if both AutoConvert and AllowNil are specified, nils will NOT be converted to false booleans.
        /// </summary>
        AutoConvert = 0x2,

        /// <summary>
        /// The default : Autoconverting values, no nils.
        /// </summary>
        Default = AutoConvert
    }
}
