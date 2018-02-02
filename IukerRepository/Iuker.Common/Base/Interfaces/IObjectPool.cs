namespace Iuker.Common.Base.Interfaces
{
#if DEBUG
    /// <summary>
    /// 泛型对象池
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20170902 14:44:30")]
    [InterfacePurposeDesc("泛型对象池", "泛型对象池")]
#endif
    public interface IObjectPool<T> where T : class
    {
        T Take();

        void Restore(T t);

        int UseCount { get; }

        void Clear();
    }
}
