using Iuker.Common.Base;

namespace Run.Iuker.Common.Module.Asset
{
#if DEBUG
    /// <summary>
    /// 可回收对象
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20170911 16:08:14")]
    [InterfacePurposeDesc("可回收对象", "可回收对象")]
#endif
    public interface IRecycle
    {
        /// <summary>
        /// 回收一个对象
        /// </summary>
        void Recycle();
    }
}
