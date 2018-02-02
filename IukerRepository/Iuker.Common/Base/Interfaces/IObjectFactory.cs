namespace Iuker.Common.Base.Interfaces
{
#if DEBUG
    /// <summary>
    ///泛型对象池工厂
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20170902 16:11:32")]
    [InterfacePurposeDesc("泛型对象池工厂", "泛型对象池工厂")]
#endif
    public interface IObjectFactory<out T> where T : class
    {
        T CreateObject();
    }
}
