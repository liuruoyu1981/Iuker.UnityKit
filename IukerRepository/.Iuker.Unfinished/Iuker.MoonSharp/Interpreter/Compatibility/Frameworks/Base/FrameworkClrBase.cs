﻿#if !(DOTNET_CORE || NETFX_CORE) 

using System;
using System.Reflection;

namespace Iuker.MoonSharp.Interpreter.Compatibility.Frameworks.Base
{
    abstract class FrameworkClrBase : FrameworkReflectionBase
    {
        BindingFlags BINDINGFLAGS_MEMBER = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
        BindingFlags BINDINGFLAGS_INNERCLASS = BindingFlags.Public | BindingFlags.NonPublic;

        public override Type GetTypeInfoFromType(Type t)
        {
            return t;
        }

        /// <summary>
        /// 获取事件的内部方法
        /// </summary>
        /// <param name="ei"></param>
        /// <returns></returns>
        public override MethodInfo GetAddMethod(EventInfo ei)
        {
            return ei.GetAddMethod(true);
        }

        /// <summary>
        /// 获取类型的构造函数数组
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public override ConstructorInfo[] GetConstructors(Type type)
        {
            return type.GetConstructors(BINDINGFLAGS_MEMBER);
        }

        /// <summary>
        /// 获取类型的事件数据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public override EventInfo[] GetEvents(Type type)
        {
            return type.GetEvents(BINDINGFLAGS_MEMBER);
        }

        /// <summary>
        /// 获取类型的所有字段
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public override FieldInfo[] GetFields(Type type)
        {
            return type.GetFields(BINDINGFLAGS_MEMBER);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public override Type[] GetGenericArguments(Type type)
        {
            return type.GetGenericArguments();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pi"></param>
        /// <returns></returns>
        public override MethodInfo GetMethod(PropertyInfo pi)
        {
            return pi.GetGetMethod(true);
        }

        public override Type[] GetInterfaces(Type t)
        {
            return t.GetInterfaces();
        }

        public override MethodInfo GetMethod(Type type, string name)
        {
            return type.GetMethod(name);
        }

        public override MethodInfo[] GetMethods(Type type)
        {
            return type.GetMethods(BINDINGFLAGS_MEMBER);
        }

        public override Type[] GetNestedTypes(Type type)
        {
            return type.GetNestedTypes(BINDINGFLAGS_INNERCLASS);
        }

        public override PropertyInfo[] GetProperties(Type type)
        {
            return type.GetProperties(BINDINGFLAGS_MEMBER);
        }

        public override PropertyInfo GetProperty(Type type, string name)
        {
            return type.GetProperty(name);
        }

        public override MethodInfo GetRemoveMethod(EventInfo ei)
        {
            return ei.GetRemoveMethod(true);
        }

        public override MethodInfo GetSetMethod(PropertyInfo pi)
        {
            return pi.GetSetMethod(true);
        }


        public override bool IsAssignableFrom(Type current, Type toCompare)
        {
            return current.IsAssignableFrom(toCompare);
        }

        public override bool IsInstanceOfType(Type t, object o)
        {
            return t.IsInstanceOfType(o);
        }


        public override MethodInfo GetMethod(Type resourcesType, string name, Type[] types)
        {
            return resourcesType.GetMethod(name, types);
        }

        public override Type[] GetAssemblyTypes(Assembly asm)
        {
            return asm.GetTypes();
        }


    }
}

#endif