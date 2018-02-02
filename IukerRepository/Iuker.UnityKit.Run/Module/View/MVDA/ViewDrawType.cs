using Iuker.Common.Base;

namespace Iuker.UnityKit.Run.Module.View.MVDA
{
#if DEBUG
    /// <summary>
    /// 视图动画绘制类型，类似于视图生命周期类型。
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20170926 10:32:56")]
    [EmumPurposeDesc("视图动画绘制类型，类似于视图生命周期类型。", "视图动画绘制类型，类似于视图生命周期类型。")]
#endif
    public enum ViewDrawType
    {
        OnCreated,

        OnActived,

        BeforeHide,

        BeforeClose,
    }
}
