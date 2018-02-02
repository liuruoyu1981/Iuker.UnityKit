/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/05/17 10:40:09
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
using System.Text;
using Iuker.Js.Native;
using Iuker.Js.Native.Error;
using Iuker.Js.Parser;
using Iuker.Js.Parser.Ast;

namespace Iuker.Js.Runtime
{
    /// <summary>
    /// JavaScript异常
    /// </summary>
    public class JavaScriptException : Exception
    {
        private readonly JsValue _errorObject;
        private string _callStack;

        public JavaScriptException(ErrorConstructor errorConstructor) : base("")
        {
            _errorObject = errorConstructor.Construct(Arguments.Empty);
        }

        public JavaScriptException(ErrorConstructor errorConstructor, string message)
            : base(message)
        {
            _errorObject = errorConstructor.Construct(new JsValue[] { message });
        }

        public JavaScriptException(JsValue error)
            : base(GetErrorMessage(error))
        {
            _errorObject = error;
        }

        public JavaScriptException SetCallStack(Engine engine, Location location = null)
        {
            Location = location;
            var sb = new StringBuilder();
            foreach (var cse in engine.CallStack)
            {
                sb.Append(" at ")
                    .Append(cse)
                    .Append("(");

                for (var index = 0; index < cse.CallExpression.Arguments.Count; index++)
                {
                    if (index != 0)
                    {
                        sb.Append(", ");
                    }

                    var arg = cse.CallExpression.Arguments[index];
                    if (arg is IPropertyKeyExpression pke)
                    {
                        sb.Append(pke.GetKey());
                    }
                    else
                    {
                        sb.Append(arg);
                    }
                }

                sb.Append(") @ ")
                    .Append(cse.CallExpression.Location.Source)
                    .Append(" ")
                    .Append(cse.CallExpression.Location.Start.Column)
                    .Append(":")
                    .Append(cse.CallExpression.Location.Start.Line)
                    .AppendLine();
            }
            CallStack = sb.ToString();
            return this;
        }

        private static string GetErrorMessage(JsValue error)
        {
            if (!error.IsObject()) return error.IsString() ? error.AsString() : error.ToString();
            var oi = error.AsObject();
            var message = oi.Get("message").AsString();
            return message;
        }

        public JsValue Error => _errorObject;

        public override string ToString()
        {
            return _errorObject.ToString();
        }

        public string CallStack
        {
            get
            {
                if (_callStack != null)
                    return _callStack;
                if (_errorObject == null)
                    return null;
                if (_errorObject.IsObject() == false)
                    return null;
                var callstack = _errorObject.AsObject().Get("callstack");
                if (callstack == JsValue.Undefined)
                    return null;
                return callstack.AsString();
            }
            set
            {
                _callStack = value;
                if (value != null && _errorObject.IsObject())
                {
                    _errorObject.AsObject()
                        .FastAddProperty("callstack", new JsValue(value), false, false, false);
                }
            }
        }

        public Parser.Location Location { get; set; }

        public int LineNumber => null == Location ? 0 : Location.Start.Line;

        public int Column => null == Location ? 0 : Location.Start.Column;
    }
}
