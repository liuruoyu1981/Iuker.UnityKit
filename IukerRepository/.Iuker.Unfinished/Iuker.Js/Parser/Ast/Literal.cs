using Iuker.Js.Native;

namespace Iuker.Js.Parser.Ast
{
    /// <summary>
    /// 字面量表达式
    /// </summary>
    public class Literal : Expression, IPropertyKeyExpression
    {
        public object Value;
        public string Raw;

        public bool Cached;
        public JsValue CachedValue;

        public string GetKey()
        {
            return Value.ToString();
        }
    }
}