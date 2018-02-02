using System;

namespace Iuker.Js.Runtime.Interop
{
    /// <summary>
    /// 类型转换器接口
    /// </summary>
    public interface ITypeConverter
    {
        object Convert(object value, Type type, IFormatProvider formatProvider);
        bool TryConvert(object value, Type type, IFormatProvider formatProvider, out object converted);

    }
}