namespace Iuker.Js.Native.String
{
    /// <summary>
    /// 字符串属性
    /// http://www.ecma-international.org/ecma-262/5.1/#sec-15.5.4
    /// </summary>
    public sealed class StringPrototype : StringInstance
    {
        private StringPrototype(Engine engine)
            : base(engine)
        {
            
        }

        public static StringPrototype CreatePrototypeObject(Engine engine, StringConstructor stringConstructor)
        {
            var obj = new StringConstructor(engine);
            obj.Extensible = true;

            obj.Prototype = engine.Function.PrototypeObject;
            


        }






































































































        // http://msdn.microsoft.com/en-us/library/system.char.iswhitespace(v=vs.110).aspx
        // http://en.wikipedia.org/wiki/Byte_order_mark
        const char BOM_CHAR = '\uFEFF';
        const char MONGOLIAN_VOWEL_SEPARATOR = '\u180E';

        private static bool IsWhiteSpaceEx(char c)
        {
            return
                char.IsWhiteSpace(c) ||
                c == BOM_CHAR ||
                // In .NET 4.6 this was removed from WS based on Unicode 6.3 changes
                c == MONGOLIAN_VOWEL_SEPARATOR;
        }

        /// <summary>
        /// 去掉空白字符
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string TrimEndEx(string s)
        {
            if (s.Length == 0)
                return string.Empty;

            var i = s.Length - 1;
            while (i >= 0)
            {
                if (IsWhiteSpaceEx(s[i]))
                    i--;
                else
                    break;
            }
            return i >= 0 ? s.Substring(0, i + 1) : string.Empty;
        }

        public static string TrimEx(string s)=> TrimEndEx(TrimStartEx(s));
        public static string TrimStartEx(string s)
        {
            if (s.Length == 0)
                return string.Empty;

            var i = 0;
            while (i < s.Length)
            {
                if (IsWhiteSpaceEx(s[i]))
                    i++;
                else
                    break;
            }
            return i >= s.Length ? string.Empty : s.Substring(i);
        }

    }
}
