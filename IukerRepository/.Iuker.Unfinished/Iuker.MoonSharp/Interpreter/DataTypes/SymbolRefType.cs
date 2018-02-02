namespace Iuker.MoonSharp.Interpreter.DataTypes
{
    /// <summary>
    /// 潜在左值类型
    /// </summary>
    public enum SymbolRefType
    {
        /// <summary>
        /// The symbol ref of a local variable
        /// </summary>
        Local,
        /// <summary>
        /// The symbol ref of an upvalue variable
        /// </summary>
        Upvalue,
        /// <summary>
        /// The symbol ref of a global variable
        /// </summary>
        Global,
        /// <summary>
        /// The symbol ref of the global environment
        /// </summary>
        DefaultEnv,

    }
}
