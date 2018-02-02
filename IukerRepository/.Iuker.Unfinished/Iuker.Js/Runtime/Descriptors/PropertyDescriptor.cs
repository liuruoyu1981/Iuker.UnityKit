using Iuker.Js.Native;

namespace Iuker.Js.Runtime.Descriptors
{
    /// <summary>
    /// Javascript对象属性描述对象 
    /// </summary>
    public class PropertyDescriptor
    {
        /// <summary>
        /// 默认未定义
        /// </summary>
        public static readonly PropertyDescriptor Undefined = new PropertyDescriptor();

        /// <summary>
        /// 构建一个属性描述对象实例（空构造）
        /// </summary>
        public PropertyDescriptor() { }

        /// <summary>
        /// 构建一个属性描述对象实例并设置相关设置
        /// </summary>
        /// <param name="value">需要设置属性的目标JsValue对象</param>
        /// <param name="writable">是否可写</param>
        /// <param name="enumerable">是否可枚举</param>
        /// <param name="configurable">是否可配置</param>
        public PropertyDescriptor(JsValue value, bool? writable, bool? enumerable, bool? configurable)
        {
            Value = value;

            if (writable.HasValue)
            {
                Writable = writable.Value;
            }

            if (enumerable.HasValue)
            {
                Enumerable = enumerable.Value;
            }

            if (configurable.HasValue)
            {
                Configurable = configurable.Value;
            }
        }

        /// <summary>
        /// 构建一个属性描述对象实例并设置属性读写器及可枚举、可配置选项。
        /// </summary>
        /// <param name="get">属性读取器</param>
        /// <param name="set">属性写入器</param>
        /// <param name="enumerable">是否可枚举</param>
        /// <param name="configurable">是否可配置</param>
        public PropertyDescriptor(JsValue get, JsValue set, bool? enumerable = null, bool? configurable = null)
        {
            Get = get;
            Set = set;

            if (enumerable.HasValue)
            {
                Enumerable = enumerable.Value;
            }

            if (configurable.HasValue)
            {
                Configurable = configurable.Value;
            }
        }

        /// <summary>
        /// 使用现有的属性描述对象实例构建一个新的属性描述对象
        /// 新属性描述对象将和源对象保持一致
        /// </summary>
        /// <param name="descriptor"></param>
        public PropertyDescriptor(PropertyDescriptor descriptor)
        {
            Get = descriptor.Get;
            Set = descriptor.Set;
            Value = descriptor.Value;
            Enumerable = descriptor.Enumerable;
            Configurable = descriptor.Configurable;
            Writable = descriptor.Writable;
        }


        public JsValue Get { get; set; }
        public JsValue Set { get; set; }
        public bool? Enumerable { get; set; }
        public bool? Writable { get; set; }
        public bool? Configurable { get; set; }
        public virtual JsValue Value { get; set; }

        /// <summary>
        /// 是否拥有读写器
        /// </summary>
        /// <returns></returns>
        public bool IsAccessorDescriptor()
        {
            if (Get == null && Set == null)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsDataDescriptor()
        {
            if (!Writable.HasValue && Value == null)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// http://www.ecma-international.org/ecma-262/5.1/#sec-8.10.3
        /// </summary>
        /// <returns></returns>
        public bool IsGenericDescriptor()
        {
            return !IsDataDescriptor() && !IsAccessorDescriptor();
        }


        public static PropertyDescriptor ToPropertyDescriptor(Engine engine, JsValue o)
        {

            return null;
        }





    }
}
