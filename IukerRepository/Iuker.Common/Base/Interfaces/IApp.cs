namespace Iuker.Common.Base.Interfaces
{
#if DEBUG
    /// <summary>
    /// 代表一个应用，应用需要实现自身的循环模式和生命周期。
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20170922 12:22:51")]
    [InterfacePurposeDesc("代表一个应用，应用需要实现自身的循环模式和生命周期。", "代表一个应用，应用需要实现自身的循环模式和生命周期。")]
#endif
    public interface IApp : ILoopUpdate
    {
        /// <summary>
        /// 创建一个对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="name">对象名</param>
        /// <returns></returns>
        T Create<T>(string name = null);


        IFactory<S, T> GetFactory<S, T>() where S : ISubscibe<T>;


    }

    public class SubscibeInt : AbsSubscibe<int>
    {
        public override void Dispose()
        {
        }
    }








}
