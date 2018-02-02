﻿/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/04/14 10:49:45
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


using System.Collections.Generic;

namespace Iuker.MoonSharp.Interpreter.DataTypes
{
    /// <summary>
    /// 一个用于容纳回调函数所有参数的容器
    /// </summary>
    public class CallbackArguments
    {
        IList<DynValue> m_Args;
        int m_Count;
        bool m_LastIsTuple = false;


        /// <summary>
        /// 获得参数的数量
        /// </summary>
        public int Count { get { return m_Count; } }

        /// <summary>
        /// 获取或设置一个值，指示是否是一个方法调用。
        /// </summary>
        public bool IsMethodCall { get; private set; }

        /// <summary>
        /// Gets the <see cref="DynValue"/> at the specified index, or Void if not found 
        /// </summary>
        public DynValue this[int index]
        {
            get
            {
                return RawGet(index, true) ?? DynValue.Void;
            }
        }

        /// <summary>
        /// Gets the <see cref="DynValue" /> at the specified index, or null.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="translateVoids">if set to <c>true</c> all voids are translated to nils.</param>
        /// <returns></returns>
        public DynValue RawGet(int index, bool translateVoids)
        {
            DynValue v;

            if (index >= m_Count)
                return null;

            if (!m_LastIsTuple || index < m_Args.Count - 1)
                v = m_Args[index];
            else
                v = m_Args[m_Args.Count - 1].Tuple[index - (m_Args.Count - 1)];

            if (v.Type == DataType.Tuple)
            {
                if (v.Tuple.Length > 0)
                    v = v.Tuple[0];
                else
                    v = DynValue.Nil;
            }

            if (translateVoids && v.Type == DataType.Void)
            {
                v = DynValue.Nil;
            }

            return v;
        }

        /// <summary>
        /// 将参数转换为数组
        /// </summary>
        /// <param name="skip">跳过的元素数量(默认为0)。</param>
        /// <returns></returns>
        public DynValue[] GetArray(int skip = 0)
        {
            if (skip >= m_Count)
                return new DynValue[0];

            DynValue[] vals = new DynValue[m_Count - skip];

            for (int i = skip; i < m_Count; i++)
                vals[i - skip] = this[i];

            return vals;
        }

        /// <summary>
        /// Gets the specified argument as as an argument of the specified type. If not possible,
        /// an exception is raised.
        /// </summary>
        /// <param name="argNum">The argument number.</param>
        /// <param name="funcName">Name of the function.</param>
        /// <param name="type">The type desired.</param>
        /// <param name="allowNil">if set to <c>true</c> nil values are allowed.</param>
        /// <returns></returns>
        public DynValue AsType(int argNum, string funcName, DataType type, bool allowNil = false)
        {
            return this[argNum].CheckType(funcName, type, argNum, allowNil ? TypeValidationFlags.AllowNil | TypeValidationFlags.AutoConvert : TypeValidationFlags.AutoConvert);
        }

        /// <summary>
        /// Gets the specified argument as as an argument of the specified user data type. If not possible,
        /// an exception is raised.
        /// </summary>
        /// <typeparam name="T">The desired userdata type</typeparam>
        /// <param name="argNum">The argument number.</param>
        /// <param name="funcName">Name of the function.</param>
        /// <param name="allowNil">if set to <c>true</c> nil values are allowed.</param>
        /// <returns></returns>
        public T AsUserData<T>(int argNum, string funcName, bool allowNil = false)
        {
            return this[argNum].CheckUserDataType<T>(funcName, argNum, allowNil ? TypeValidationFlags.AllowNil : TypeValidationFlags.None);
        }


        /// <summary>
        /// 转换指定的参数为整数
        /// </summary>
        /// <param name="argNum">The argument number.</param>
        /// <param name="funcName">Name of the function.</param>
        /// <returns></returns>
        public int AsInt(int argNum, string funcName)
        {
            DynValue v = AsType(argNum, funcName, DataType.Number, false);
            double d = v.Number;
            return (int)d;
        }

        /// <summary>
        /// 转换指定的参数为长整数
        /// </summary>
        /// <param name="argNum">The argument number.</param>
        /// <param name="funcName">Name of the function.</param>
        /// <returns></returns>
        public long AsLong(int argNum, string funcName)
        {
            DynValue v = AsType(argNum, funcName, DataType.Number, false);
            double d = v.Number;
            return (long)d;
        }

        /// <summary>
		/// Gets the specified argument as a string, calling the __tostring metamethod if needed, in a NON
		/// yield-compatible way.
		/// </summary>
		/// <param name="executionContext">The execution context.</param>
		/// <param name="argNum">The argument number.</param>
		/// <param name="funcName">Name of the function.</param>
		/// <returns></returns>
		/// <exception cref="ScriptRuntimeException">'tostring' must return a string to '{0}'</exception>
		//public string AsStringUsingMeta(ScriptExecutionContext executionContext, int argNum, string funcName)
  //      {
  //          if ((this[argNum].Type == DataType.Table) && (this[argNum].Table.MetaTable != null) &&
  //              (this[argNum].Table.MetaTable.RawGet("__tostring") != null))
  //          {
  //              var v = executionContext.GetScript().Call(this[argNum].Table.MetaTable.RawGet("__tostring"), this[argNum]);

  //              if (v.Type != DataType.String)
  //                  throw new ScriptRuntimeException("'tostring' must return a string to '{0}'", funcName);

  //              return v.ToPrintString();
  //          }
  //          else
  //          {
  //              return (this[argNum].ToPrintString());
  //          }
  //      }


        /// <summary>
        /// Returns a copy of CallbackArguments where the first ("self") argument is skipped if this was a method call,
        /// otherwise returns itself.
        /// </summary>
        /// <returns></returns>
        //public CallbackArguments SkipMethodCall()
        //{
        //    if (this.IsMethodCall)
        //    {
        //        Slice<DynValue> slice = new Slice<DynValue>(m_Args, 1, m_Args.Count - 1, false);
        //        return new CallbackArguments(slice, false);
        //    }
        //    else return this;
        //}


    }
}
