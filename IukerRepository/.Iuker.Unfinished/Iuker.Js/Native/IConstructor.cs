/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/05/17 10:41:51
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

namespace Iuker.Js.Native
{
    /// <summary>
    /// javascript构造函数接口
    /// </summary>
    public interface IConstructor
    {
        /// <summary>
        /// 调用一个Js对象
        /// </summary>
        /// <param name="thisObject"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        JsValue Call(JsValue thisObject, JsValue[] arguments);

        /// <summary>
        /// 构建一个js对象实例
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        ObjectInstance Construct(JsValue[] arguments);
    }
}
