/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/04/14 12:13:04
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

namespace Iuker.MoonSharp.Interpreter.Interop.StandardDescriptors
{
    /// <summary>
    /// 一个用户数据描述符，它聚合多个描述符，并在它们上尝试分派成员。
    /// 例如,对于对象的实现多个接口,但没有描述符是专门注册。
    /// Used, for example, for objects implementing multiple interfaces but for which no descriptor is 
    /// specifically registered.
    /// </summary>
    public class CompositeUserDataDescriptor : IUserDataDescriptor
    {
        public string Name
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Type Type
        {
            get
            {
                throw new NotImplementedException();
            }
        } 

        public string AsString(object obj)
        {
            throw new NotImplementedException();
        }

        public DynValue Index(Script script, object obj, DynValue index, bool isDirectIndexing)
        {
            throw new NotImplementedException();
        }

        public bool IsTypeCompatible(Type type, object obj)
        {
            throw new NotImplementedException();
        }

        public DynValue MetaIndex(Script script, object obj, string metaname)
        {
            throw new NotImplementedException();
        }

        public bool SetIndex(Script script, object obj, DynValue index, DynValue value, bool isDirectIndexing)
        {
            throw new NotImplementedException();
        }
    }
}
