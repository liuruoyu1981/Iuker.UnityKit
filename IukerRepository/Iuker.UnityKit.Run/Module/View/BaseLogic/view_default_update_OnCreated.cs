/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 10/30/2017 7:02:28 AM
Email: liuruoyu1981@gmail.com
***********************************************************************************************/


/*
视图行为处理器脚本，在这里实现视图控件交互、视图生命周期的行为处理逻辑。
*/

using System;
using Iuker.UnityKit.Run.Module.View.MVDA;
using Iuker.UnityKit.Run.Base;
using Iuker.UnityKit.Run.Module.Asset;
using Iuker.UnityKit.Run.Module.HotUpdate;
using Iuker.UnityKit.Run.Module.View.ViewWidget;
using UnityEngine;

/// <summary>
/// 更新视图OnCreated事件默认处理器
/// </summary>
public class view_default_update_OnCreated : IViewActionResponser<IView>
{
    private IU3dFrame mU3DFrame;
    private IViewActionRequest<IView> _viewActionRequest;
    private IView mView;
    private Bootstrap mBootstrap;
    private IU3dHotUpdateModule mHotUpdate;
    private ISlider mSlider;
    private IText mText;


    public IViewActionResponser<IView> Init(IU3dFrame frame, IViewActionRequest<IView> request, IViewModel model)
    {
        mU3DFrame = frame;
        mView = request.ActionRequester.Origin.AttachView;
        mBootstrap = Bootstrap.Instance;
        mHotUpdate = mU3DFrame.GetModule<IU3dHotUpdateModule>();
        mSlider = mView.GetSlider("slider_update");
        mText = mView.GetText("text_progress");

        return this;
    }

    /// <summary>
    /// 行为处理器关注的视图Id
    /// </summary>
    public string ConcernedViewId
    {
        get
        {
            return "view_default_update";
        }
    }

    /// <summary>
    /// 行为处理器关注的视图的开启状态
    /// </summary>
    public bool IsConcernedViewClosed { get; set; }

    public void ProcessRequest(IViewActionRequest<IView> request)
    {
        _viewActionRequest = request;

        if (Application.platform == RuntimePlatform.Android)
        {
            ApkUpdate();
        }
        else
        {
            TryAssetBundleUpdate();
        }
    }


    private void ApkUpdate()
    {
        //  进行整包更新
        if (mBootstrap.UpdateType == AssetUpdateType.Apk)
        {
            mHotUpdate.ApkUpdate();
            mU3DFrame.EventModule.WatchEvent(U3dEventCode.Frame_ApkDownUpdate.Literals, data =>
            {
                var p = float.Parse(data.ToString());
                p = (float)Math.Round(p, 2);
                mSlider.SetValue(p);
                if (!(p >= 0)) return;

                CreateNextView();
            });
        }
        else
        {
            TryAssetBundleUpdate();
        }
    }

    private void CreateNextView()
    {
        var viewId = string.IsNullOrEmpty(Bootstrap.Instance.AfterUpdateViewId)
            ? Bootstrap.Instance.EntryProject.LoginViewId
            : Bootstrap.Instance.AfterUpdateViewId;
        mU3DFrame.ViewModule.CreateView(viewId);
    }

    private void TryAssetBundleUpdate()
    {
        mHotUpdate.Update(UpdateProgressDesc);
    }

    /// <summary>
    /// 约定更新模块返回的描述文字中不包含当前下载进度
    /// 更新视图实时展示的描述文字中的进度由更新视图OnCreated控制器控制
    /// </summary>
    /// <param name="desc"></param>
    /// <param name="progress"></param>
    void UpdateProgressDesc(string desc, float progress)
    {
        if (!mSlider.DependentGo.activeSelf) mSlider.DependentGo.SetActive(true);

        var newDesc = desc + string.Format("  已完成{0}%", (float)Math.Round(progress, 2) * 100);
        mText.Text = newDesc;
        mSlider.SetValue(progress);
    }

    public bool CheckProcessResult()
    {
        return true;
    }

    public void ProcessException(Exception ex)
    {

    }

    public IView Origin { get { return _viewActionRequest.ActionRequester.Origin; } }
}
