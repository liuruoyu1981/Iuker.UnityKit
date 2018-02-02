/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/03/16 10:40:40
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


using Iuker.MoonSharp.Interpreter.Debugging;
using Iuker.MoonSharp.Tree.Lexer;

namespace Iuker.MoonSharp.Interpreter.Execution
{
    /// <summary>
    /// 脚本加载上下文
    /// </summary>
    class ScriptLoadingContext
    {
        public Script Script { get; private set; }
        //public BuildTimeScope Scope { get; set; }
        public SourceCode Source { get; set; }
        public bool Anonymous { get; set; }
        public bool IsDynamicExpression { get; set; }
        public Lexer Lexer { get; set; }

        public ScriptLoadingContext(Script s)
        {
            Script = s;
        }
    }
}
