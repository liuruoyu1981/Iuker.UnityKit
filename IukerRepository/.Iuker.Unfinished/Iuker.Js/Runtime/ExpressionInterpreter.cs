using System;
using Iuker.Js.Native;
using Iuker.Js.Native.Number;
using Iuker.Js.Parser.Ast;
using Iuker.Js.Runtime.Interop;

namespace Iuker.Js.Runtime
{
    /// <summary>
    /// 表达式解释器 
    /// </summary>
    public class ExpressionInterpreter
    {
        private readonly Engine _engine;

        public ExpressionInterpreter(Engine engine)
        {
            _engine = engine;
        }


        private object EvaluateExpression(Expression expression) => _engine.EvaluateExpression(expression);

        public JsValue EvaluateConditionalExpression(ConditionalExpression conditionalExpression)
        {
            var lref = _engine.EvaluateExpression(conditionalExpression.Test);
            //if (TypeConverter.ToBoolean(_engine,get))
        }






















































































































































































































































































































































































































































































        public static bool StrictlyEqual(JsValue x, JsValue y)
        {
            var typea = x.Type;
            var typeb = y.Type;

            if (typea != typeb)
            {
                return false;
            }

            if (typea == Types.Undefined || typea == Types.Null)
            {
                return true;
            }

            if (typea == Types.None)
            {
                return true;
            }

            if (typea == Types.Number)
            {
                var nx = x.AsNumber();
                var ny = y.AsNumber();

                if (double.IsNaN(nx) || double.IsNaN(ny))
                {
                    return false;
                }

                if (nx.Equals(ny))
                {
                    return true;
                }

                return false;
            }

            if (typea == Types.String)
            {
                return x.AsString() == y.AsString();
            }

            if (typea == Types.Boolean)
            {
                return x.AsBoolean() == y.AsBoolean();
            }

            if (typea == Types.Object)
            {
                var xw = x.AsObject() as IObjectWrapper;

                if (xw != null)
                {
                    var yw = y.AsObject() as IObjectWrapper;
                    return Object.Equals(xw.Target, yw.Target);
                }
            }

            return x == y;
        }

        public static bool SameValue(JsValue x, JsValue y)
        {
            var typea = TypeConverter.GetPrimitiveType(x);
            var typeb = TypeConverter.GetPrimitiveType(y);

            if (typea != typeb)
            {
                return false;
            }

            if (typea == Types.None)
            {
                return true;
            }
            if (typea == Types.Number)
            {
                var nx = TypeConverter.ToNumber(x);
                var ny = TypeConverter.ToNumber(y);

                if (double.IsNaN(nx) && double.IsNaN(ny))
                {
                    return true;
                }

                if (nx.Equals(ny))
                {
                    if (nx.Equals(0))
                    {
                        // +0 !== -0
                        return NumberInstance.IsNegativeZero(nx) == NumberInstance.IsNegativeZero(ny);
                    }

                    return true;
                }

                return false;
            }

            if (typea == Types.String)
            {
                return TypeConverter.ToString(x) == TypeConverter.ToString(y);
            }
            if (typea == Types.Boolean)
            {
                return TypeConverter.ToBoolean(x) == TypeConverter.ToBoolean(y);
            }
            return x == y;
        }


    }
}