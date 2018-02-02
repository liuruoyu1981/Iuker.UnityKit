/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/04/14 12:03:11
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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Iuker.MoonSharp.Interpreter.Compatibility;
using Iuker.MoonSharp.Interpreter.Interop.Attributes;

namespace Iuker.MoonSharp.Interpreter.Interop
{
    /// <summary>
    /// 辅助扩展方法用于简化userdata描述符实现的某些部分
    /// </summary>
    public static class DescriptorHelpers
    {
        public static bool? GetVisbilityFromAttributes(this MemberInfo mi)
        {
            if (mi == null)
                return false;

            MoonSharpVisibleAttribute va =
                mi.GetCustomAttributes(true).OfType<MoonSharpVisibleAttribute>().SingleOrDefault();
            MoonSharpHiddenAttribute ha = mi.GetCustomAttributes(true).OfType<MoonSharpHiddenAttribute>().SingleOrDefault();

            if (va != null && ha != null && va.Visible)
                throw new InvalidOperationException(string.Format("A member ('{0}') can't have discording MoonSharpHiddenAttribute and MoonSharpVisibleAttribute.", mi.Name));
            else if (ha != null)
                return false;
            else if (va != null)
                return va.Visible;
            else
                return null;
        }

        public static bool IsDelegateType(this Type t)
        {
            return Framework.Do.IsAssignableFrom(typeof(Delegate), t);
        }














































































































        /// <summary>
        /// 获取程序集中的类型实现，并捕获ReflectionTypeLoadException。
        /// </summary>
        /// <param name="asm"></param>
        /// <returns></returns>
        public static Type[] SafeGetTypes(this Assembly asm)
        {
            try
            {
                return Framework.Do.GetAssemblyTypes(asm);
            }
            catch (ReflectionTypeLoadException)
            {
                return new Type[0];
            }
        }

        /// <summary>
        /// 获得一个转换方法转换后的名字，以暴露在Lua的脚本中
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetConversionMethedName(this Type type)
        {
            StringBuilder sb = new StringBuilder(type.Name);

            for (int i = 0; i < sb.Length; i++)
                if (!char.IsLetterOrDigit(sb[i])) sb[i] = '_';

            return "__to" + sb.ToString();
        }

        /// <summary>
        /// 按给定类型获取所有实现的类型
        /// </summary>
        /// <param name="t">The t.</param>
        /// <returns></returns>
        public static IEnumerable<Type> GetAllImplementedTypes(this Type t)
        {
            for (Type ot = t; ot != null; ot = Framework.Do.GetBaseType(ot))
                yield return ot;

            foreach (Type it in Framework.Do.GetInterfaces(t))
                yield return it;
        }

        /// <summary>
        /// 确定字符串是否有效的简单标识符(以字母或下划线开始并且只包含字母,数字和下划线)。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsValidSimpleIdentifier(string str)
        {
            if (string.IsNullOrEmpty(str))
                return false;

            if (str[0] != '_' && !char.IsLetter(str[0]))
                return false;

            for (int i = 1; i < str.Length; i++)
                if (str[i] != '_' && !char.IsLetterOrDigit(str[i]))
                    return false;

            return true;
        }

        /// <summary>
        /// 将字符串转换为一个简单标识符（以字母或下划线开始并且只包含字母,数字和下划线）。
        /// Converts the string to a valid simple identifier (starts with letter or underscore
        /// and contains only letters, digits and underscores).
        /// </summary>
        public static string ToValidSimpleIdentifier(string str)
        {
            if (string.IsNullOrEmpty(str))
                return "_";

            if (str[0] != '_' && !char.IsLetter(str[0]))
                str = "_" + str;

            StringBuilder sb = new StringBuilder(str);

            for (int i = 0; i < sb.Length; i++)
                if (sb[i] != '_' && !char.IsLetterOrDigit(sb[i]))
                    sb[i] = '_';

            return sb.ToString();
        }

        /// <summary>
        /// Converts the specified name from underscore_case to camelCase.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static string Camelify(string name)
        {
            StringBuilder sb = new StringBuilder(name.Length);

            bool lastWasUnderscore = false;
            for (int i = 0; i < name.Length; i++)
            {
                if (name[i] == '_' && i != 0)
                {
                    lastWasUnderscore = true;
                }
                else
                {
                    if (lastWasUnderscore)
                        sb.Append(char.ToUpperInvariant(name[i]));
                    else
                        sb.Append(name[i]);

                    lastWasUnderscore = false;
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// 将指定的名称转换为带有大写首字母(某些东西)的名称。
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static string UpperFirstLetter(string name)
        {
            if (!string.IsNullOrEmpty(name))
                return char.ToUpperInvariant(name[0]) + name.Substring(1);

            return name;
        }

    }
}
