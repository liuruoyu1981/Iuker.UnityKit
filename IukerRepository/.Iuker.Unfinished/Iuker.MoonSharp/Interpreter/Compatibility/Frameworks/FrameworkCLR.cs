/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/03/16 11:04:53
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


#if !(DOTNET_CORE || NETFX_CORE) && !PCL

using System;
using System.Linq;
using Iuker.MoonSharp.Interpreter.Compatibility.Frameworks.Base;

namespace MoonSharp.Interpreter.Compatibility.Frameworks
{
    class FrameworkCurrent : FrameworkClrBase
    {
        public override bool IsDbNull(object o)
        {
            return o != null && Convert.IsDBNull(o);
        }


        public override bool StringContainsChar(string str, char chr)
        {
            return str.Contains(chr);
        }

        public override Type GetInterface(Type type, string name)
        {
            return type.GetInterface(name);
        }
    }
}

#endif
