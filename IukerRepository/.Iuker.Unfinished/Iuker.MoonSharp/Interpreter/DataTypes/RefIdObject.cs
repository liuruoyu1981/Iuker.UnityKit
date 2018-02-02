namespace Iuker.MoonSharp.Interpreter.DataTypes
{
    /// <summary>
    /// lua对象基类
    /// 每个对象都具有用于调试的ReferenceId属性
    /// 注意，ReferenceID目前并不是以线程安全的方式分配，所以只有当一次只运行在一个线程上时
    /// ReferenceID才是唯一的
    /// </summary>
    public class RefIdObject
    {
        private static int s_RefIDCouter = 0;
        private int m_RefID = ++s_RefIDCouter;

        /// <summary>
        /// 获得引用标识符
        /// </summary>
        public int ReferenceID => m_RefID;

        /// <summary>
        /// 使用类型名和引用ID获得一个格式字符串
        /// </summary>
        /// <param name="typeString">类型名</param>
        /// <returns></returns>
        public string FormatTypeString(string typeString) => $"{typeString}: {m_RefID:X8}";
    }
}
