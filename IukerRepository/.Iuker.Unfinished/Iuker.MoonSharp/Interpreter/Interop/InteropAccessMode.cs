namespace Iuker.MoonSharp.Interpreter
{
    /// <summary>
    /// 
    /// </summary>
    public enum InteropAccessMode
    {
        /// <summary>
        /// 不执行优化，每次访问成员时都会使用反射。
        /// 这是最慢的模式，不过如果使用的成员较少则可以节省大量内存
        /// </summary>
        Reflection,

        /// <summary>
        /// 优化会在第一次访问成员时进行
        /// 这将节省所有从未访问过的成员的内存，代价是增加脚本执行时间。
        /// </summary>
        LazyOptimized,

        /// <summary>
        /// 优化会在类型注册时进行
        /// </summary>
        Preoptimized,

        /// <summary>
        /// 优化会在开始注册时以后台线程的形式进行
        /// 如果在优化完成前访问成员，则使用反射。
        /// </summary>
        BackgroundOptimized,

        /// <summary>
        /// 使用固定的描述符
        /// </summary>
        Hardwired,

        /// <summary>
        /// 不进行优化，成员无法访问。
        /// </summary>
        HideMembers,

        /// <summary>
        /// 不允许反射，也不生成代码。当不使用标准的基于反射的描述符的类型（例如类型实现）时，这将作为一个保护措施
        /// </summary>
        NoReflectionAllowed,

        /// <summary>
        /// 使用默认访问模式
        /// </summary>
        Default
    }
}
