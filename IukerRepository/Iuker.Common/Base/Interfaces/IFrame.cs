namespace Iuker.Common.Base.Interfaces
{
#if DEBUG
    /// <summary>
    ///框架接口
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20170902 15:06:54")]
    [InterfacePurposeDesc("框架接口", "框架接口")]
#endif
    public interface IFrame
    {
        /// <summary>
        /// 获得一个模块
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetModule<T>() where T : class, IModule;

        /// <summary>
        /// 框架初始化
        /// </summary>
        void Init();

    }
}
