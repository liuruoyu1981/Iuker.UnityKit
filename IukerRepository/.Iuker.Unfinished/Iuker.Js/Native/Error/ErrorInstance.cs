/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/05/17 10:49:04
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


using Iuker.Js.Native.Object;

namespace Iuker.Js.Native.Error
{
    /// <summary>
    /// javasc错误对象
    /// </summary>
    public class ErrorInstance : ObjectInstance
    {
        public ErrorInstance(Engine engine, string name)
            : base(engine)
        {
            //FastAddProperty("name", name, true, false, true);
        }

        public override string Class => "Error";

        public override string ToString()
        {
            //return Engine.Error.PrototypeObject.ToString(this, Arguments.Empty).ToObject().ToString();
            return null;
        }


    }
}
