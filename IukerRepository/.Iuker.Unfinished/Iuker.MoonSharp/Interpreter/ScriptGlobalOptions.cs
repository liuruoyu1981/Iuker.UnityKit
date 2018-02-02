/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/04/14 09:19:45
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


using Iuker.MoonSharp.Interpreter.Interop;
using Iuker.MoonSharp.Interpreter.Platforms;

namespace Iuker.MoonSharp.Interpreter
{
    /// <summary>
    /// 包含脚本全局选项的类，这是不能定制每个脚本的选项。
    /// </summary>
    public class ScriptGlobalOptions
    {
        internal ScriptGlobalOptions()
        {
        }

        /// <summary>
        /// 获取或设置自定义转换器。
        /// </summary>
        public CustomConvertersCollection CustomConverters { get; set; }

        /// <summary>
        /// 获取或设置使用的平台访问抽象接口
        /// </summary>
        /// <value>
        /// 当前的平台访问抽象接口
        /// </value>
        public IPlatformAccessor Platform { get; set; }

        /// <summary>
        /// 获取或设置一个值，指示解释器异常是否应该作为嵌套异常被重新抛出。
        /// </summary>
        public bool RethrowExceptionNested { get; set; }


    }
}
