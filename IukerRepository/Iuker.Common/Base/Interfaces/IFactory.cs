namespace Iuker.Common.Base.Interfaces
{
#if DEBUG
    /// <summary>
    /// 对象创建工厂
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20170922 12:32:01")]
    [InterfacePurposeDesc("对象创建工厂", "对象创建工厂")]
#endif
    public interface IFactory<S, T> where S : ISubscibe<T>
    {
        /// <summary>
        /// 创建一个可订阅对象
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        ISubscibe<T> Create(string name);

        /// <summary>
        /// 初始化一个可订阅对象创建工厂
        /// </summary>
        /// <param name="app">工厂对象所依赖的应用实例</param>
        /// <param name="size"></param>
        /// <returns></returns>
        IFactory<S, T> InitFactory(IApp app, int size);


    }
}
