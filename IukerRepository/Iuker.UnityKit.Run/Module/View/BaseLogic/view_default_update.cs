/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 11/10/2017 3:17:51 PM
Email: liuruoyu1981@gmail.com
***********************************************************************************************/


/*
*/

using Iuker.UnityKit.Run.Module.View.MVDA;
using Iuker.UnityKit.Run.Module.View.ViewWidget;

public class view_default_update : AbsViewBase
{
    // 视图行为字符串常量，避免临时字符串反复构建消耗。
    private const string _onBeforeCreatActionToken = "view_default_update_BeforeCreat";
    private const string _onCreatedActionToken = "view_default_update_OnCreated";
    private const string _onBeforeActiveActionToken = "view_default_update_BeforeActive";
    private const string _onActivedActionToken = "view_default_update_OnActived";
    private const string _onBeforeHideActionToken = "view_default_update_BeforeHide";
    private const string _onHidedActionToken = "view_default_update_OnHided";
    private const string _onBeforeCloseActionToken = "view_default_update_BeforeClose";
    private const string _onClosedActionToken = "view_default_update_OnClosed";
    private const string _onCreatedDrawActionToken = "view_default_update_Draw_OnCreated";
    private const string _onActivedDrawActionToken = "view_default_update_Draw_OnActived";
    private const string _onHideDrawActionToken = "view_default_update_Draw_OnHide";
    private const string _onCloseDrawActionToken = "view_default_update_Draw_OnClose";

    #region 视图控件字段
    #endregion

    protected override void InitViewWidgets()
    {
        // 容器对象

        // 按钮对象

        // 文本对象
        InitViewWidget<IText>("text_progress").AddTo("text_progress", TextDictionary);

        // 输入框对象

        // Image对象

        // RawImage对象

        // Toggle对象

        // Slider对象
        InitViewWidget<ISlider>("slider_update").AddTo("slider_update", SliderDictionary);

        // TabGroup对象

        // ListView对象

    }


    #region 视图生命周期
    protected override void BeforeCreat()
    {

        base.BeforeCreat();
        var onCreateRequest = U3DFrame.InjectModule.GetInstance<IViewActionRequest<IView>>().Init(this, _onBeforeCreatActionToken, ViewScriptType.ViewPipeline);
        Issue(onCreateRequest);

    }

    protected override void OnCreated()
    {

        base.OnCreated();
        var onCreatedRequest = U3DFrame.InjectModule.GetInstance<IViewActionRequest<IView>>().Init(this, _onCreatedActionToken, ViewScriptType.ViewPipeline);
        Issue(onCreatedRequest);
        var onCreatedDrawRequest = U3DFrame.InjectModule.GetInstance<IViewActionRequest<IView>>()
            .Init(this, _onCreatedDrawActionToken, ViewScriptType.ViewDraw);
        Issue(onCreatedDrawRequest);

    }

    protected override void BeforeActive()
    {

        base.BeforeActive();
        var onActiveRequest = U3DFrame.InjectModule.GetInstance<IViewActionRequest<IView>>().Init(this, _onBeforeActiveActionToken, ViewScriptType.ViewPipeline);
        Issue(onActiveRequest);

    }

    protected override void OnActived()
    {

        base.OnActived();
        var onActivedRequest = U3DFrame.InjectModule.GetInstance<IViewActionRequest<IView>>().Init(this, _onActivedActionToken, ViewScriptType.ViewPipeline);
        Issue(onActivedRequest);
        var onActivedDrawRequest = U3DFrame.InjectModule.GetInstance<IViewActionRequest<IView>>()
            .Init(this, _onActivedDrawActionToken, ViewScriptType.ViewDraw);
        Issue(onActivedDrawRequest);

    }

    protected override void BeforeHide()
    {

        base.BeforeHide();
        var onHideRequest = U3DFrame.InjectModule.GetInstance<IViewActionRequest<IView>>().Init(this, _onBeforeHideActionToken, ViewScriptType.ViewPipeline);
        Issue(onHideRequest);
        var onHideDrawRequest = U3DFrame.InjectModule.GetInstance<IViewActionRequest<IView>>()
            .Init(this, _onHideDrawActionToken, ViewScriptType.ViewDraw);
        Issue(onHideDrawRequest);

    }

    protected override void OnHided()
    {

        base.OnHided();
        var onHidedRequest = U3DFrame.InjectModule.GetInstance<IViewActionRequest<IView>>().Init(this, _onHidedActionToken, ViewScriptType.ViewPipeline);
        Issue(onHidedRequest);

    }

    protected override void BeforeClose()
    {

        base.BeforeClose();
        var onCloseRequest = U3DFrame.InjectModule.GetInstance<IViewActionRequest<IView>>().Init(this, _onBeforeCloseActionToken, ViewScriptType.ViewPipeline);
        Issue(onCloseRequest);
        var onCloseDrawRequest = U3DFrame.InjectModule.GetInstance<IViewActionRequest<IView>>()
            .Init(this, _onCloseDrawActionToken, ViewScriptType.ViewDraw);
        Issue(onCloseDrawRequest);

    }

    protected override void OnClosed()
    {

        base.OnClosed();
        var onClosedRequest = U3DFrame.InjectModule.GetInstance<IViewActionRequest<IView>>().Init(this, _onClosedActionToken, ViewScriptType.ViewPipeline);
        Issue(onClosedRequest);

    }

    #endregion
}
