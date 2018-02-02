using System.Collections.Generic;
using Iuker.Js.Runtime;
using Iuker.Js.Runtime.Descriptors;
using Types = Iuker.Js.Runtime.Types;

namespace Iuker.Js.Native.Object
{
    /// <summary>
    /// Javascript对象实例
    /// </summary>
    public class ObjectInstance
    {

        public ObjectInstance(Engine engine)
        {
            Engine = engine;
            Properties = new MruPropertyCache2<string, PropertyDescriptor>();
        }

        /// <summary>
        /// 执行引擎
        /// </summary>
        public Engine Engine { get; set; }

        /// <summary>
        /// 属性描述符字典
        /// </summary>
        protected IDictionary<string, PropertyDescriptor> Properties { get; private set; }

        /// <summary>
        /// 对象原型
        /// </summary>
        public ObjectInstance Prototype { get; set; }

        /// <summary>
        /// 是否可扩展
        /// 如果为真则可将自己的属性添加到对象
        /// </summary>
        public bool Extensible { get; set; }

        /// <summary>
        /// 标识对象类型的字符串字面量
        /// </summary>
        public virtual string Class => "Object";

        /// <summary>
        /// 获取对象所有的属性
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<KeyValuePair<string, PropertyDescriptor>> GetOwnProperties()
        {
            EnsureInitialized();
            return Properties;
        }

        /// <summary>
        /// 是否拥有某个属性
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public virtual bool HasOwnnProperty(string p)
        {
            EnsureInitialized();
            return Properties.ContainsKey(p);
        }

        /// <summary>
        /// 移除一个属性
        /// </summary>
        /// <param name="p"></param>
        public virtual void RemoveOwnProperty(string p)
        {
            EnsureInitialized();
            Properties.Remove(p);
        }

        /// <summary>
        /// 返回指定命名的属性。
        /// http://www.ecma-international.org/ecma-262/5.1/#sec-8.12.3
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public virtual JsValue Get(string propertyName)
        {
            var desc = GetProperty(propertyName);

            if (desc == PropertyDescriptor.Undefined)
            {
                return JsValue.Undefined;
            }

            if (desc.IsDataDescriptor())
            {
                var val = desc.Value;
                return val ?? Undefined.Instance;
            }

            var getter = desc.Get ?? Undefined.Instance;

            if (getter.IsUndefined())
            {
                return Undefined.Instance;
            }

            // 如果读取器不为空则必须进行读取器调用
            var callable = getter.TryCast<ICallable>();
            return callable.Call(this, Arguments.Empty);
        }

        /// <summary>
        /// 返回对象指定命名的属性描述符，如果不存在则返回undefined
        /// http://www.ecma-international.org/ecma-262/5.1/#sec-8.12.1
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public virtual PropertyDescriptor GetOwnProperty(string propertyName)
        {
            EnsureInitialized();

            PropertyDescriptor x;
            if (Properties.TryGetValue(propertyName, out x))
            {
                /* Spec implementation
                PropertyDescriptor d;
                if (x.IsDataDescriptor())
                {
                    d = new PropertyDescriptor(x.As<DataDescriptor>());
                }
                else
                {
                    d = new PropertyDescriptor(x.As<AccessorDescriptor>());
                }

                return d;
                */

                // optimmized implementation
                return x;
            }

            return PropertyDescriptor.Undefined;
        }

        /// <summary>
        /// 设置指定命名的属性描述符。
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="desc"></param>
        protected virtual void SetOwnProperty(string propertyName, PropertyDescriptor desc)
        {
            EnsureInitialized();
            Properties[propertyName] = desc;
        }

        /// <summary>
        /// 获得指定的具名属性
        /// http://www.ecma-international.org/ecma-262/5.1/#sec-8.12.2
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public PropertyDescriptor GetProperty(string propertyName)
        {
            var prop = GetOwnProperty(propertyName);

            if (prop != PropertyDescriptor.Undefined)
            {
                return prop;
            }

            if (Prototype == null)  //  原型为空则返回Undefined
            {
                return PropertyDescriptor.Undefined;
            }
            // 尝试返回原型中的具名属性
            return Prototype.GetProperty(propertyName);
        }

        /// <summary>
        /// 设置制定的具名属性为目标值
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <param name="throwOnError"></param>
        public virtual void Put(string propertyName, JsValue value, bool throwOnError)
        {
            if (!CanPut(propertyName))
            {
                if (throwOnError)
                {
                    throw new JavaScriptException(Engine.Error.ToString());
                }

                return;
            }

            var ownDesc = GetOwnProperty(propertyName);

            if (ownDesc.IsDataDescriptor())
            {
                ownDesc.Value = value;
                return;

                // as per specification
                // var valueDesc = new PropertyDescriptor(value: value, writable: null, enumerable: null, configurable: null);
                // DefineOwnProperty(propertyName, valueDesc, throwOnError);
                // return;
            }

            // property is an accessor or inherited
            var desc = GetProperty(propertyName);

            if (desc.IsAccessorDescriptor())
            {
                var setter = desc.Set.TryCast<ICallable>();
                setter.Call(new JsValue(this), new[] { value });
            }
            else
            {
                var newDesc = new PropertyDescriptor(value, true, true, true);
                DefineOwnProperty(propertyName, newDesc, throwOnError);
            }
        }

        /// <summary>
        /// 返回一个布尔值，指示是否可以执行具有PropertyName的操作。
        /// http://www.ecma-international.org/ecma-262/5.1/#sec-8.12.4
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public bool CanPut(string propertyName)
        {
            var desc = GetOwnProperty(propertyName);

            if (desc != PropertyDescriptor.Undefined)
            {
                if (desc.IsAccessorDescriptor())
                {
                    if (desc.Set == null || desc.Set.IsUndefined())
                    {
                        return false;
                    }

                    return true;
                }

                return desc.Writable.HasValue && desc.Writable.Value;
            }

            if (Prototype == null)
            {
                return Extensible;
            }

            var inherited = Prototype.GetProperty(propertyName);

            if (inherited == PropertyDescriptor.Undefined)
            {
                return Extensible;
            }

            if (inherited.IsAccessorDescriptor())
            {
                if (inherited.Set == null || inherited.Set.IsUndefined())
                {
                    return false;
                }

                return true;
            }

            if (!Extensible)
            {
                return false;
            }
            return inherited.Writable.HasValue && inherited.Writable.Value;
        }

        /// <summary>
        /// 返回一个布尔值，指示对象是否已具有给定名称的属性。
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public bool HasProperty(string propertyName)
        {
            return GetProperty(propertyName) != PropertyDescriptor.Undefined;
        }

        /// <summary>
        /// 删除指定的具名属性
        /// 如果throwOnError为真则会抛出异常
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="throwOnError"></param>
        /// <returns></returns>
        public virtual bool Delete(string propertyName, bool throwOnError)
        {
            var desc = GetOwnProperty(propertyName);

            if (desc == PropertyDescriptor.Undefined)
            {
                return true;
            }

            if (desc.Configurable.HasValue && desc.Configurable.Value)
            {
                RemoveOwnProperty(propertyName);
                return true;
            }
            if (throwOnError)
            {
                throw new JavaScriptException(Engine.TypeError);
            }

            return false;
        }

        /// <summary>
        /// 默认值
        /// </summary>
        /// <param name="hint"></param>
        /// <returns></returns>
        public JsValue DefaultValue(Types hint)
        {
            EnsureInitialized();

            if (hint == Types.String || hint == Types.None && Class == "Date")
            {
                var toString = Get("toString").TryCast<ICallable>();
                if (toString != null)
                {
                    var str = toString.Call(new JsValue(this), Arguments.Empty);
                    if (str.IsPrimitive())
                    {
                        return str;
                    }
                }

                var valueOf = Get("valueOf").TryCast<ICallable>();
                if (valueOf != null)
                {
                    var val = valueOf.Call(new JsValue(this), Arguments.Empty);
                    if (val.IsPrimitive())
                    {
                        return val;
                    }
                }

                throw new JavaScriptException(Engine.TypeError);
            }

            if (hint == Types.Number || hint == Types.None)
            {
                var valueOf = Get("valueOf").TryCast<ICallable>();
                if (valueOf != null)
                {
                    var val = valueOf.Call(new JsValue(this), Arguments.Empty);
                    if (val.IsPrimitive())
                    {
                        return val;
                    }
                }

                var toString = Get("toString").TryCast<ICallable>();
                if (toString != null)
                {
                    var str = toString.Call(new JsValue(this), Arguments.Empty);
                    if (str.IsPrimitive())
                    {
                        return str;
                    }
                }

                throw new JavaScriptException(Engine.TypeError);
            }

            return ToString();
        }

        /// <summary>
        /// 创建或改变已有的具名属性
        /// 如果throwOnError为真则尝试抛出异常
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="desc"></param>
        /// <param name="throwOnError"></param>
        /// <returns></returns>
        public virtual bool DefineOwnProperty(string propertyName, PropertyDescriptor desc, bool throwOnError)
        {
            var current = GetOwnProperty(propertyName);

            if (current == desc)
            {
                return true;
            }

            if (current == PropertyDescriptor.Undefined)
            {
                if (!Extensible)
                {
                    if (throwOnError)
                    {
                        throw new JavaScriptException(Engine.TypeError);
                    }

                    return false;
                }
                if (desc.IsGenericDescriptor() || desc.IsDataDescriptor())
                {
                    SetOwnProperty(propertyName, new PropertyDescriptor(desc)
                    {
                        Value = desc.Value ?? JsValue.Undefined,
                        Writable = desc.Writable ?? false,
                        Enumerable = desc.Enumerable ?? false,
                        Configurable = desc.Configurable ?? false

                    });
                }
                else
                {
                    SetOwnProperty(propertyName, new PropertyDescriptor(desc)
                    {
                        Get = desc.Get,
                        Set = desc.Set,
                        Enumerable = desc.Enumerable ?? false,
                        Configurable = desc.Configurable ?? false,
                    });
                }

                return true;
            }

            // Step 5
            if (!current.Configurable.HasValue &&
                !current.Enumerable.HasValue &&
                !current.Writable.HasValue &&
                current.Get == null &&
                current.Set == null &&
                current.Value == null)
            {
                return true;
            }

            // Step 6
            if (
                current.Configurable == desc.Configurable &&
                current.Writable == desc.Writable &&
                current.Enumerable == desc.Enumerable &&

                (current.Get == null && desc.Get == null || (current.Get != null && desc.Get != null && ExpressionInterpreter.SameValue(current.Get, desc.Get))) &&
                (current.Set == null && desc.Set == null || (current.Set != null && desc.Set != null && ExpressionInterpreter.SameValue(current.Set, desc.Set))) &&
                (current.Value == null && desc.Value == null || (current.Value != null && desc.Value != null && ExpressionInterpreter.StrictlyEqual(current.Value, desc.Value)))
            )
            {
                return true;
            }

            if (!current.Configurable.HasValue || !current.Configurable.Value)
            {
                if (desc.Configurable.HasValue && desc.Configurable.Value)
                {
                    if (throwOnError)
                    {
                        throw new JavaScriptException(Engine.TypeError);
                    }

                    return false;
                }

                if (desc.Enumerable.HasValue && (!current.Enumerable.HasValue || desc.Enumerable.Value != current.Enumerable.Value))
                {
                    if (throwOnError)
                    {
                        throw new JavaScriptException(Engine.TypeError);
                    }

                    return false;
                }
            }

            if (!desc.IsGenericDescriptor())
            {

                if (current.IsDataDescriptor() != desc.IsDataDescriptor())
                {
                    if (!current.Configurable.HasValue || !current.Configurable.Value)
                    {
                        if (throwOnError)
                        {
                            throw new JavaScriptException(Engine.TypeError);
                        }

                        return false;
                    }

                    if (current.IsDataDescriptor())
                    {
                        SetOwnProperty(propertyName, current = new PropertyDescriptor(
                            get: Undefined.Instance,
                            set: Undefined.Instance,
                            enumerable: current.Enumerable,
                            configurable: current.Configurable
                            ));
                    }
                    else
                    {
                        SetOwnProperty(propertyName, current = new PropertyDescriptor(
                            value: Undefined.Instance,
                            writable: null,
                            enumerable: current.Enumerable,
                            configurable: current.Configurable
                            ));
                    }
                }
                else if (current.IsDataDescriptor() && desc.IsDataDescriptor())
                {
                    if (!current.Configurable.HasValue || current.Configurable.Value == false)
                    {
                        if (!current.Writable.HasValue || !current.Writable.Value && desc.Writable.HasValue && desc.Writable.Value)
                        {
                            if (throwOnError)
                            {
                                throw new JavaScriptException(Engine.TypeError);
                            }

                            return false;
                        }

                        if (!current.Writable.Value)
                        {
                            if (desc.Value != null && !ExpressionInterpreter.SameValue(desc.Value, current.Value))
                            {
                                if (throwOnError)
                                {
                                    throw new JavaScriptException(Engine.TypeError);
                                }

                                return false;
                            }
                        }
                    }
                }
                else if (current.IsAccessorDescriptor() && desc.IsAccessorDescriptor())
                {
                    if (!current.Configurable.HasValue || !current.Configurable.Value)
                    {
                        if ((desc.Set != null && !ExpressionInterpreter.SameValue(desc.Set, current.Set != null ? current.Set : Undefined.Instance))
                            ||
                            (desc.Get != null && !ExpressionInterpreter.SameValue(desc.Get, current.Get != null ? current.Get : Undefined.Instance)))
                        {
                            if (throwOnError)
                            {
                                throw new JavaScriptException(Engine.TypeError);
                            }

                            return false;
                        }
                    }
                }
            }

            if (desc.Value != null)
            {
                current.Value = desc.Value;
            }

            if (desc.Writable.HasValue)
            {
                current.Writable = desc.Writable;
            }

            if (desc.Enumerable.HasValue)
            {
                current.Enumerable = desc.Enumerable;
            }

            if (desc.Configurable.HasValue)
            {
                current.Configurable = desc.Configurable;
            }

            if (desc.Get != null)
            {
                current.Get = desc.Get;
            }

            if (desc.Set != null)
            {
                current.Set = desc.Set;
            }

            return true;

        }

        /// <summary>
        /// 当已知属性已声明时，Put的优化版本
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="writable"></param>
        /// <param name="configurable"></param>
        /// <param name="enumerable"></param>
        public void FastAddProperty(string name, JsValue value, bool writable, bool enumerable, bool configurable)
        {
            SetOwnProperty(name, new PropertyDescriptor(value, writable, enumerable, configurable));
        }

        /// <summary>
        /// 当已知属性已声明时，Put的优化版本
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void FastSetProperty(string name, PropertyDescriptor value)
        {
            SetOwnProperty(name, value);
        }

        /// <summary>
        /// 确保对象已经初始化
        /// Js实例重写该方法
        /// </summary>
        protected virtual void EnsureInitialized() { }

        public override string ToString() => TypeConverter.ToString(this);
    }
}
