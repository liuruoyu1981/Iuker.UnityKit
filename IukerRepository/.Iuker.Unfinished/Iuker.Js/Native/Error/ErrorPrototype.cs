/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/05/17 10:58:06
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



namespace Iuker.Js.Native.Error
{
    /// <summary>
    /// javascript错误对象原型
    /// </summary>
    public sealed class ErrorPrototype : ErrorInstance
    {
        private ErrorPrototype(Engine engine, string name) : base(engine, name)
        {
        }


        public static ErrorPrototype CreateProtorypeObject(Engine engine, ErrorConstructor errorConstructor,
            string name)
        {
            var obj = new ErrorPrototype(engine, name) { Extensible = true };
            //obj.FastAddProperty("constructor",errorConstructor,true,false,true);

            return obj;

        }











    }
}
