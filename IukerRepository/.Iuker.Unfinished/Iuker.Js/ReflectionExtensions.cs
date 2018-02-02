using System;
using System.Linq.Expressions;
using System.Reflection;

#if NETSTANDARD1_3
using System;
using System.Linq;
using System.Reflection;

namespace Jint
{
    internal static class ReflectionExtensions
    {
        internal static bool IsEnum(this Type type)
        {
            return type.GetTypeInfo().IsEnum;
        }

        internal static bool IsGenericType(this Type type)
        {
            return type.GetTypeInfo().IsGenericType;
        }

        internal static bool IsValueType(this Type type)
        {
            return type.GetTypeInfo().IsValueType;
        }

        internal static bool HasAttribute<T>(this ParameterInfo member) where T : Attribute
        {
            return member.GetCustomAttributes<T>().Any();
        }
    }
}
#else

namespace Iuker.Js
{
    internal static class ReflectionExtensions
    {
        internal static bool IsEnum(this Type type)
        {
            return type.IsEnum;
        }

        internal static bool IsGenericType(this Type type)
        {
            return type.IsGenericType;
        }

        internal static bool IsValueType(this Type type)
        {
            return type.IsValueType;
        }

        internal static bool HasAttribute<T>(this ParameterInfo member) where T : Attribute
        {
            return Attribute.IsDefined(member, typeof(T));
        }

        internal static MethodInfo GetMethodInfo(this Delegate d)
        {
            return d.Method;
        }


        //internal static BlockExpression Block(Expression arg0, Expression arg1)
        //{

        //}


        internal static MethodCallExpression Block(this Expression expression, Expression expression1,
            Expression parameter)
        {
            return expression as MethodCallExpression;
        }



    }
}
#endif