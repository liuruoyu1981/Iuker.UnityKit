using Iuker.MoonSharp.Interpreter.DataTypes;

namespace Iuker.MoonSharp.Interpreter
{
    /// <summary>
    /// 该类包装了一个CLR函数
    /// </summary>
    public sealed class CallbackFunction : RefIdObject
    {
        /// <summary>
        /// 默认的访问优化模式
        /// 优化会在第一次访问成员时进行
        /// </summary>
        private static InteropAccessMode m_DefaultAccessMode = InteropAccessMode.LazyOptimized;

        /// <summary>
        /// 获得函数的名字
        /// </summary>
        public string Name { get; private set; }


    }
}
