/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/4/18 上午6:42:29
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
using System.Reflection;


namespace Iuker.MoonSharp.Interpreter.Compatibility.Frameworks.Base
{
    public abstract class FrameworkBase
    {
        public abstract bool StringContainsChar(string str, char chr);

        public abstract bool IsValueType(Type t);

        public abstract Assembly GetAssembly(Type t);

        public abstract Type GetBaseType(Type t);

        public abstract bool IsGenericType(Type t);

        public abstract bool IsGenericTypeDefinition(Type t);

        public abstract bool IsEnum(Type t);

        public abstract bool IsNestedPublic(Type t);

        public abstract bool IsAbstract(Type t);

        public abstract bool IsInterface(Type t);

        public abstract Attribute[] GetCustomAttributes(Type t, bool inherit);

        public abstract Attribute[] GetCustomAttributes(Type t, Type at, bool inherit);

        public abstract Type[] GetInterfaces(Type t);

        public abstract bool IsInstanceOfType(Type t, object o);

        public abstract MethodInfo GetAddMethod(EventInfo ei);

        public abstract MethodInfo GetRemoveMethod(EventInfo ei);

        public abstract MethodInfo GetMethod(PropertyInfo pi);

        public abstract MethodInfo GetSetMethod(PropertyInfo pi);

        public abstract Type GetInterface(Type type, string name);

        public abstract PropertyInfo[] GetProperties(Type type);

        public abstract PropertyInfo GetProperty(Type type, string name);

        public abstract Type[] GetNestedTypes(Type type);

        public abstract EventInfo[] GetEvents(Type type);

        public abstract ConstructorInfo[] GetConstructors(Type type);

        public abstract Type[] GetAssemblyTypes(Assembly asm);

        public abstract MethodInfo[] GetMethods(Type type);

        public abstract FieldInfo[] GetFields(Type t);

        public abstract MethodInfo GetMethod(Type type, string name);

        public abstract Type[] GetGenericArguments(Type t);

        public abstract bool IsAssignableFrom(Type current, Type toCompare);

        public abstract bool IsDbNull(object o);

        public abstract MethodInfo GetMethod(Type resourcesType, string v, Type[] type);
    }
}
