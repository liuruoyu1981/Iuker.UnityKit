namespace Iuker.Common.Base.Interfaces
{
#if DEBUG
    /// <summary>
    /// 循环更新对象
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20170922 12:25:44")]
    [InterfacePurposeDesc("循环更新对象", "循环更新对象")]
#endif
    public interface ILoopUpdate
    {
        void Update();

        void LateUpdate();

        void FixedUpdate();
    }
}
