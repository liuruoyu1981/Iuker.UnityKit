/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/04/14 11:01:44
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
using Iuker.MoonSharp.Interpreter.DataTypes;

namespace Iuker.MoonSharp.Interpreter.Execution.Scopes
{
    internal class RuntimeScopeFrame
    {
        public List<SymbolRef> DebugSymbols { get; private set; }
        public int Count { get { return DebugSymbols.Count; } }
        public int ToFirstBlock { get; internal set; }

        public RuntimeScopeFrame()
        {
            DebugSymbols = new List<SymbolRef>();
        }

        public override string ToString()
        {
            return string.Format("ScopeFrame : #{0}", Count);
        }
    }
}
