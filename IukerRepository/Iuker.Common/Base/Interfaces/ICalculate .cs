namespace Iuker.Common.Base.Interfaces
{
#if DEBUG
    /// <summary>
    /// 可计算对象
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20170922 09:38:09")]
    [InterfacePurposeDesc("可计算对象", "可计算对象")]
#endif
    public interface ICalculate<T> where T : struct
    {
        /// <summary>
        /// 相减
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        T Minus(T left, T right);

        /// <summary>
        /// 相加
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>

        T Plus(T left, T right);






    }
}
