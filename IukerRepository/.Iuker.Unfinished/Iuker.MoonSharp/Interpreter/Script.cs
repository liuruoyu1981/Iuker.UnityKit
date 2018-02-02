/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/02/18 22:50:49
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
using Iuker.MoonSharp.Interpreter;
using Iuker.MoonSharp.Interpreter.Debugging;

namespace Iuker.MoonSharp
{
    public class Script
    {

        private List<SourceCode> m_Sources = new List<SourceCode>();












        /// <summary>
        /// 获得全局选项，这是不能定制每个脚本的选项。
        /// </summary>
        public static ScriptGlobalOptions GlobalOptions { get; private set; }

        public SourceCode GetSourceCode(int sourceCodeID) => m_Sources[sourceCodeID];
    }
}
