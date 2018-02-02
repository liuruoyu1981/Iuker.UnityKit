using System;
using Iuker.Js.Native.Object;
using Iuker.Js.Runtime;
using Iuker.Js.Runtime.Descriptors;

namespace Iuker.Js.Native.String
{
    /// <summary>
    /// Js字符串
    /// </summary>
    public class StringInstance : ObjectInstance, IPrimitiveInstance
    {
        public StringInstance(Engine engine)
            : base(engine)
        {

        }

        /// <summary>
        /// 重写基类属性
        /// 类型标识字符串（String）
        /// </summary>
        public override string Class => "String";

        /// <summary>
        /// 显示实现IPrimitiveInstance接口
        /// 返回Javascript类型（String）
        /// </summary>
        Types IPrimitiveInstance.Type => Types.String;


        public JsValue PrimitiveValue { get; set; }

        JsValue IPrimitiveInstance.PrimitiveValue => PrimitiveValue;

        /// <summary>
        /// 判断给定的double值是否可以转换为Int
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        private static bool IsInt(double d)
        {
            // 如果d超出double类型的合理范围则返回假
            if (!(d >= long.MinValue) || !(d <= long.MaxValue)) return false;
            var l = (long)d;
            return l >= int.MinValue && l <= int.MaxValue;
        }


        public override PropertyDescriptor GetOwnProperty(string propertyName)
        {
            if (propertyName == "Infinity")
            {
                return PropertyDescriptor.Undefined;
            }

            var desc = base.GetOwnProperty(propertyName);
            if (desc != PropertyDescriptor.Undefined)
            {
                return desc;
            }

            if (propertyName != System.Math.Abs(TypeConverter.ToInteger(propertyName)).ToString())
            {
                return PropertyDescriptor.Undefined;
            }

            var str = PrimitiveValue;
            var dIndex = TypeConverter.ToInteger(propertyName);
            if (!IsInt(dIndex))
                return PropertyDescriptor.Undefined;

            var index = (int)dIndex;
            var len = str.AsString().Length;
            if (len <= index || index < 0)
            {
                return PropertyDescriptor.Undefined;
            }
            var resultStr = str.AsString()[index].ToString();
            return new PropertyDescriptor(new JsValue(resultStr), false, true, false);
        }



    }
}
