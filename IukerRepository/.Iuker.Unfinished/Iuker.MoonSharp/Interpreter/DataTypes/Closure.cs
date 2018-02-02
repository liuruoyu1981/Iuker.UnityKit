/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/04/14 10:50:05
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
using Iuker.MoonSharp.Interpreter.Execution.Scopes;

namespace Iuker.MoonSharp.Interpreter.DataTypes
{
    /// <summary>
    /// 一个表示脚本函数的类
    /// </summary>
    public class Closure : RefIdObject, IScriptPrivateResource
    {
        /// <summary>
        /// 基于up取值的闭包类型
        /// Type of closure based on upvalues
        /// </summary>
        public enum UpvaluesType
        {
            /// <summary>
            /// 关闭没有upvalues(因此,从技术上讲,这是一个函数,而不是一个闭包!)
            /// The closure has no upvalues (thus, technically, it's a function and not a closure!)
            /// </summary>
            None,
            /// <summary>
            /// The closure has _ENV as its only upvalue
            /// </summary>
            Environment,
            /// <summary>
            /// The closure is a "real" closure, with multiple upvalues
            /// </summary>
            Closure
        }

        /// <summary>
        /// 在字节码中获取入口点位置。
        /// </summary>
        public int EntryPointByteCodeLocation { get; private set; }

        /// <summary>
        /// 获取该函数的脚本
        /// </summary>
        public Script OwnerScript { get; private set; }

        /// <summary>
        /// 空闭包的快捷方式
        /// </summary>
        private static ClosureContext emptyClosure = new ClosureContext();

        /// <summary>
        /// 当前闭包上下文
        /// </summary>
        internal ClosureContext ClosureContext { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Closure"/> class.
        /// </summary>
        /// <param name="script">The script.</param>
        /// <param name="idx">The index.</param>
        /// <param name="symbols">The symbols.</param>
        /// <param name="resolvedLocals">The resolved locals.</param>
        internal Closure(Script script, int idx, SymbolRef[] symbols, IEnumerable<DynValue> resolvedLocals)
        {
            OwnerScript = script;

            EntryPointByteCodeLocation = idx;

            if (symbols.Length > 0)
                ClosureContext = new ClosureContext(symbols, resolvedLocals);
            else
                ClosureContext = emptyClosure;
        }

        /// <summary>
        /// 使用指定的参数调用此函数
        /// </summary>
        /// <returns></returns>
        public DynValue Call()
        {
            // todo moonsharp
            return null;
        }

        public DynValue Call(params DynValue[] args)
        {
            // todo moonsharp
            return null;
        }

        public ScriptFunctionDelegate GetDelegate()
        {
            return null;
        }








    }
}
