/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/04/14 11:01:10
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
using System.Linq;
using Iuker.MoonSharp.Interpreter.DataTypes;

namespace Iuker.MoonSharp.Interpreter.Execution.Scopes
{
    /// <summary>
    /// 一个闭包的范围(upvalues容器)
    /// </summary>
    internal class ClosureContext : List<DynValue>
    {
        /// <summary>
        /// 获得符号数组
        /// </summary>
        public string[] Symbols { get; private set; }

        internal ClosureContext(SymbolRef[] symbols, IEnumerable<DynValue> values)
        {
            Symbols = symbols.Select(s => s.i_Name).ToArray();
            AddRange(values);
        }

        internal ClosureContext()
        {
            Symbols = new string[0];
        }
    }
}
