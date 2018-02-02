/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/04/14 12:04:03
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

using System;

namespace Iuker.MoonSharp.Interpreter.Interop
{
    /// <summary>
    /// 用于从脚本中访问给定类型的对象的接口。
    /// Interface used by MoonSharp to access objects of a given type from scripts.
    /// </summary>
    public interface IUserDataDescriptor
    {
        /// <summary>
        /// 获取描述符的名称(通常是描述的类型的名称)。
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 得到这个描述符所引用的类型
        /// </summary>
        Type Type { get; }

        /// <summary>
        /// 执行一个“索引”“获得”操作。
        /// Performs an "index" "get" operation.
        /// </summary>
        /// <param name="script">发出请求的脚本</param>
        /// <param name="obj">对象(如果完成了静态请求，则为null)</param>
        /// <param name="index">The index.</param>
        /// <param name="isDirectIndexing">如果设置为true，那么它将被索引为一个名称，如果是false，则通过括号进行索引。</param>
        /// <returns></returns>
        DynValue Index(Script script, object obj, DynValue index, bool isDirectIndexing);

        /// <summary>
        /// Performs an "index" "set" operation.
        /// </summary>
        /// <param name="script">The script originating the request</param>
        /// <param name="obj">The object (null if a static request is done)</param>
        /// <param name="index">The index.</param>
        /// <param name="value">The value to be set</param>
        /// <param name="isDirectIndexing">If set to true, it's indexed with a name, if false it's indexed through brackets.</param>
        /// <returns></returns>
        bool SetIndex(Script script, object obj, DynValue index, DynValue value, bool isDirectIndexing);

        /// <summary>
        /// 将用户数据转换为字符串
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        string AsString(object obj);

        /// <summary>
        /// 
        /// Gets a "meta" operation on this userdata. If a descriptor does not support this functionality,
        /// it should return "null" (not a nil). 
        /// 
        /// These standard metamethods can be supported (the return value should be a function accepting the
        /// classic parameters of the corresponding metamethod):
        /// __add, __sub, __mul, __div, __div, __pow, __unm, __eq, __lt, __le, __lt, __len, __concat, 
        /// __pairs, __ipairs, __iterator, __call
        /// 
        /// These standard metamethods are supported through other calls for efficiency:
        /// __index, __newindex, __tostring
        /// 
        /// </summary>
        /// <param name="script">The script originating the request</param>
        /// <param name="obj">The object (null if a static request is done)</param>
        /// <param name="metaname">The name of the metamember.</param>
        /// <returns></returns>
        DynValue MetaIndex(Script script, object obj, string metaname);

        /// <summary>
        /// 确定指定对象是否符合指定的对象类型。
        /// Unless a very specific behaviour is needed, the correct implementation is a 
        /// simple " return type.IsInstanceOfType(obj); "
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        bool IsTypeCompatible(Type type, object obj);

    }
}
