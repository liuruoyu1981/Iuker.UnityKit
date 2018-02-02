using Iuker.Js.Native.Object;
using Iuker.Js.Runtime;
using Iuker.Js.Runtime.Environments;

namespace Iuker.Js.Native.Function
{
    /// <summary>
    /// javascript函数对象
    /// </summary>
    public abstract class FunctionInstance : ObjectInstance, ICallable
    {
        private readonly Engine _engine;

        protected FunctionInstance(Engine engine, string[] parameters, LexicalEnvironment scope, bool strict) : base(engine)
        {
            _engine = engine;
            FormalParameters = parameters;
            Scope = scope;
            Strict = strict;
        }

        /// <summary>
        /// 当一个函数对象被作为函数调用时执行
        /// </summary>
        /// <param name="thisObject"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public abstract JsValue Call(JsValue thisObject, JsValue[] arguments);

        /// <summary>
        /// 词法作用域
        /// </summary>
        public LexicalEnvironment Scope { get; private set; }

        /// <summary>
        /// 将参数格式化并返回格式化后的字符串数组
        /// </summary>
        public string[] FormalParameters { get; private set; }

        /// <summary>
        /// 是否为严格模式
        /// </summary>
        public bool Strict { get; private set; }

        public virtual bool HasInstance(JsValue v)
        {
            var vObj = v.TryCast<ObjectInstance>();
            if (vObj == null)
            {
                return false;
            }

            var po = Get("prototype");
            if (!po.IsObject())
            {
                throw new JavaScriptException(_engine.TypeError,
                    $"Function has non-object prototype '{TypeConverter.ToString(po)}' in instanceof check");
            }

            var o = po.AsObject();

            if (o == null)
            {
                throw new JavaScriptException(_engine.TypeError);
            }

            while (true)
            {
                vObj = vObj.Prototype;

                if (vObj == null)
                {
                    return false;
                }
                if (vObj == o)
                {
                    return true;
                }
            }
        }

        public override string Class => "Function";

        /// <summary>
        /// http://www.ecma-international.org/ecma-262/5.1/#sec-15.3.5.4
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public override JsValue Get(string propertyName)
        {
            var v = base.Get(propertyName);

            var f = v.As<FunctionInstance>();
            if (propertyName == "caller" && f != null && f.Strict)
            {
                throw new JavaScriptException(_engine.TypeError);
            }

            return v;
        }





    }
}
