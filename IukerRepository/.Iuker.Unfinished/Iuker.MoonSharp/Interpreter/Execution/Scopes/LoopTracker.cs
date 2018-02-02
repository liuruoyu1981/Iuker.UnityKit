/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/04/14 11:01:33
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

using Iuker.MoonSharp.Interpreter.DataStructs;
using Iuker.MoonSharp.Interpreter.Execution.VM;

namespace Iuker.MoonSharp.Interpreter.Execution.Scopes
{
    interface ILoop
    {
        void CompileBreak(ByteCode bc);
        bool IsBoundary();
    }


    internal class LoopTracker
    {
        public FastStack<ILoop> Loops = new FastStack<ILoop>(16384);
    }
}
