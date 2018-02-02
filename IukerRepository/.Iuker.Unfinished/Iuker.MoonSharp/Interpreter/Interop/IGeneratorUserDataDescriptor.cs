/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/04/14 12:03:23
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
    /// An interface for type descriptors having the ability to generate other descriptors on the fly.
    /// </summary>
    public interface IGeneratorUserDataDescriptor : IUserDataDescriptor
    {
        /// <summary>
        /// 
        /// 为指定的类型生成一个新的描述符。
        /// 
        /// The purpose is to allow a mechanism by which a type descriptor can replace itself with another
        /// descriptor for a specific type. For example, descriptors can be created on the fly to support
        /// generic types through this mechanism.
        /// 
        /// The return value should be:
        ///		null - if this descriptor is simply skipped for the specified type
        ///		this - acts as if the descriptor was a vanilla descriptor
        ///		a new descriptor - if a new descriptor should be used in place of this one
        ///		
        /// It's recommended that instances of descriptors are cached for future references. One possible way,
        /// to do the caching is to have the generator register the descriptor through <see cref="UserData.RegisterType"/>. 
        /// In that case, it should query whether the type is exactly registered, through <see cref="UserData.IsTypeRegistered"/>
        /// 
        /// NOTE-1 : the search for descriptors does NOT stop with the descriptor returned by this type, but 
        /// other descriptors (e.g. for interfaces) might still be added.
        /// 
        /// NOTE-2 : the descriptor generation mechanism is not triggered on an exact match of types.
        /// 
        /// NOTE-3 : the method is called in the context of a lock over the descriptors registry so no unpredictable changes to the 
        /// registry can come from other threads during the execution of this method. However this method should not take other 
        /// locks, to avoid deadlocks.
        /// 
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>Null, this object instance or a new descriptor.</returns>
        IUserDataDescriptor Generate(Type type);
    }
}
