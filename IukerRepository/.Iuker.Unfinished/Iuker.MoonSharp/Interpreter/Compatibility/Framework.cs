/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/03/16 11:06:25
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


using Iuker.MoonSharp.Interpreter.Compatibility.Frameworks.Base;
using MoonSharp.Interpreter.Compatibility.Frameworks;

namespace Iuker.MoonSharp.Interpreter.Compatibility
{
    public static class Framework
    {
        private static FrameworkCurrent s_FrameworkCurrent = new FrameworkCurrent();

        public static FrameworkBase Do { get { return s_FrameworkCurrent; } }
    }
}
