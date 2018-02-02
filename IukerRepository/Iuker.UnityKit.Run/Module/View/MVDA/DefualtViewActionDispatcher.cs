/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/3/25 下午7:13:04
Email: 35490136@qq.com
QQCode: 35490136
CreateNote: 视图行为调度器
***********************************************************************************************/


/****************************************修改日志***********************************************
1. 修改日期： 修改人： 修改内容：
2. 修改日期： 修改人： 修改内容：
3. 修改日期： 修改人： 修改内容：
4. 修改日期： 修改人： 修改内容：
5. 修改日期： 修改人： 修改内容：
****************************************修改日志***********************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Iuker.Common.Utility;
using Iuker.UnityKit.Run.Base;
using Iuker.UnityKit.Run.Module.Replace;
using JetBrains.Annotations;
using UnityEngine;


namespace Iuker.UnityKit.Run.Module.View.MVDA
{
    /// <summary>
    /// 默认视图行为调度器
    /// </summary>
    // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
    public class DefualtViewActionDispatcher : IViewActionDispatcher
    {
        private IU3dFrame U3DFrame;
        private IU3dRouterModule mRouterModule;
        private IViewModelDispatcher m_ModelDispatcher;

        public virtual IViewActionDispatcher Init(IU3dFrame u3DFrame)
        {
            U3DFrame = u3DFrame;
            u3DFrame.AddFrameInitedAction(() =>
            {
                mRouterModule = u3DFrame.GetModule<IU3dRouterModule>();
                m_ModelDispatcher = u3DFrame.ViewModule.ModelDispatcher;
            });

            return this;
        }

        /// <summary>
        /// 调度一个视图行为请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <param name="agent"></param>
        public virtual void DispatchRequest<T>(IViewActionRequest<T> request, WidgetActionAgent<T> agent = null)
            where T : IViewElement
        {
            UpdateViewId(request);

            // 设置视图上下文环境对象的当前视图行为唯一码
            U3DFrame.AppContext.ViewContext.ActionToken = request.ActionToken;

            if (request.ViewScriptType == ViewScriptType.WidgetCell)
            {
                DispatcherCellResponser(request, agent);
                return;
            }

            ViewActionResponserWrapper<T>.Init(U3DFrame);
            var isExist = ViewActionResponserWrapper<T>.IsExist(request);
            var responser = isExist
                ? ViewActionResponserWrapper<T>.GetResponser(request.ActionToken)
                : ViewActionResponserWrapper<T>.ActivatorResponser(U3DFrame.ProjectAssemblys, request);

            if (responser == null)
            {
                if (agent != null)
                {
                    agent.NoResponser = true;
                }
                return;
            }

            ExecuteDispatch(responser, isExist, request);
        }

        private void ExecuteDispatch<T>(IViewActionResponser<T> responser, bool isExist, IViewActionRequest<T> request) where T : IViewElement
        {
            // 获得视图数据模型
            var model = m_ModelDispatcher.GetModel(request.ActionRequester.ViewId);  //  获取视图数据实例然后传递给行为处理器

            if (!isExist)        // todo 数据不需反复初始化，界面关闭不影响数据的生命周期,待测试这样的设计是否会有脏数据的问题
            {
                responser.Init(U3DFrame, request, model);
            }
            else
            {
                if (responser.IsConcernedViewClosed)
                {
                    responser.Init(U3DFrame, request, model);   // 重新调用视图行为处理器的初始化方法避免脏数据
                    responser.IsConcernedViewClosed = false;    //  调用完毕后将视图行为处理器的IsConcernedViewClosed置为假
                }
            }

            responser.ProcessRequest(request);  //  调用行为处理器的行为处理函数处理该次行为处理请求
                                                //TransferToTestModule(responser);    // 将控制流程转让给测试模块
        }

        private void UpdateViewId<T>(IViewActionRequest<T> request) where T : IViewElement
        {
            //  如果存在自动建立的脚本替换数据则更新请求Token
            if (mRouterModule.GetActionRoute(request.ActionToken) != null)
            {
                request.ActionToken = mRouterModule.GetActionRoute(request.ActionToken);
            }

            //  如果存在目标视图容器解析脚本存在的情况
            //  则将所有的生命周期请求Token重新解析为原始视图Id结构
            if (request.ViewScriptType == ViewScriptType.ViewPipeline &&
                    mRouterModule.GetAssetRoute(request.ActionRequester.ViewId) != null)
            {
                var sourceViewId = request.ActionRequester.ViewId;
                var targetViewId = mRouterModule.GetAssetRoute(request.ActionRequester.ViewId);
                var newToken = request.ActionToken.Replace(targetViewId, sourceViewId);
                request.ActionToken = newToken;
            }
        }

        /// <summary>
        /// 调度一个模板类型的视图行为处理器
        /// </summary>
        private void DispatcherCellResponser<T>(IViewActionRequest<T> request, WidgetActionAgent<T> agent = null)
            where T : IViewElement
        {
            var responser = ViewActionResponserWrapper<T>.TryGetCellResponser(U3DFrame.ProjectAssemblys, request);
            if (responser == null)
            {
                if (agent != null)
                {
                    agent.NoResponser = true;
                }
                return;
            }

            var model = U3DFrame.ViewModule.ModelDispatcher.GetModel(request.ActionRequester.ViewId);  //  获取视图数据实例然后传递给行为处理器
            responser.Init(U3DFrame, request, model);
            responser.ProcessRequest(request);
        }


        /// <summary>
        /// 将控制流程转让给测试模块
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="responser"></param>
        //private void TransferToTestModule<T>(IViewActionResponser<T> responser) where T : IViewElement
        //{
        //    if (U3DFrame.AppContext.RunMode == U3dAppRunMode.AutoTest)
        //    {
        //        var result = responser.CheckProcessResult();
        //        if (result)
        //        {

        //        }
        //    }
        //}

        // ReSharper disable once ClassNeverInstantiated.Local
        private class ViewActionResponserWrapper<T>
        {
            private static bool sIsInit;
            public static void Init(IU3dFrame frame)
            {
                if (sIsInit) return;

                // 监听视图的关闭事件以以重置视图行为处理器的init函数调用
                frame.EventModule.WatchEvent(U3dEventCode.View_Closed.ToString(), ResetResponsers);

                sIsInit = true;
            }

            /// <summary>
            /// 视图行为处理器列表字典
            /// 一个列表中容纳了对应视图的所有视图行为处理器
            /// </summary>
            private static readonly Dictionary<string, List<IViewActionResponser<T>>> ResponserListDictionary = new Dictionary<string, List<IViewActionResponser<T>>>();

            /// <summary>
            /// 视图行为处理器字典
            /// 以ActionToken作为键存储
            /// </summary>
            private static readonly Dictionary<string, IViewActionResponser<T>> ResponsersDictionary = new Dictionary<string, IViewActionResponser<T>>();

            /// <summary>
            /// 检查请求需求的视图行为处理器当前是否已存在
            /// </summary>
            /// <param name="request"></param>
            /// <returns></returns>
            public static bool IsExist([NotNull] IViewActionRequest<T> request)
            {
                if (request == null)
                    throw new ArgumentNullException("request");

                var isExist = ResponsersDictionary.ContainsKey(request.ActionToken);
                return isExist;
            }

            /// <summary>
            /// 尝试获得Cell模板控件类型的行为处理器脚本
            /// </summary>
            public static IViewActionResponser<T> TryGetCellResponser(List<Assembly> assemblies, IViewActionRequest<T> request)
            {
                var types = GetResponserTypeList(assemblies);
                var targetType = types.Find(r => r.Name == request.ActionToken);
                if (targetType == null)
                {
                    Debug.Log(string.Format("指定的视图行为处理器类型{0}没有找到实例！", request.ActionToken));
                    return null;
                }

                var responser = Activator.CreateInstance(targetType) as IViewActionResponser<T>;
                if (responser == null && !request.ActionToken.Contains("OnPointerEnter"))
                {
                    throw new Exception(string.Format("指定的视图行为处理器{0}", request.ActionToken));
                }
                return responser;
            }

            /// <summary>
            /// 获取一个视图行为处理答复器
            /// 该方法被调用前已检测过答复器是否缓存因此该方法内部不再做缓存检测
            /// </summary>
            /// <param name="token"></param>
            /// <returns></returns>
            public static IViewActionResponser<T> GetResponser(string token)
            {
                var responser = ResponsersDictionary[token];
                return responser;
            }

            /// <summary>
            /// 将指定视图的所有行为处理器都重置为需要进行初始化状态
            /// </summary>
            /// <param name="obj"></param>
            private static void ResetResponsers(object obj)
            {
                var viewId = Convert.ToString(obj);
                if (ResponserListDictionary.ContainsKey(viewId))
                {
                    ResponserListDictionary[viewId].ForEach(responser => responser.IsConcernedViewClosed = true);
                }
            }


            public static IViewActionResponser<T> ActivatorResponser(List<Assembly> assemblies,
                IViewActionRequest<T> request)
            {
                // ReSharper disable once JoinDeclarationAndInitializer
                IViewActionResponser<T> responser;
                responser = ActivatorResponser_OnEditor(assemblies, request);
                return responser;
            }

            /// <summary>
            /// 用于缓存的视图行为处理器类型列表
            /// </summary>
            private static List<Type> sAllResponserTypeList;

            /// <summary>
            /// 当前项目视图行为处理器类型列表
            /// </summary>
            private static List<Type> GetResponserTypeList(List<Assembly> assemblies)
            {
                return sAllResponserTypeList ?? (sAllResponserTypeList =
                           ReflectionUitlity.GetTypeList<IViewActionResponser<T>>(assemblies));
            }

            /// <summary>
            /// 编辑器下激活一个视图行为处理器
            /// </summary>
            /// <param name="assemblies"></param>
            /// <param name="request"></param>
            /// <returns></returns>
            private static IViewActionResponser<T> ActivatorResponser_OnEditor(List<Assembly> assemblies, IViewActionRequest<T> request)
            {
                if (request == null)
                    throw new ArgumentNullException("request");

                var types = GetResponserTypeList(assemblies).
                    FindAll(r => r.Name.StartsWith(request.ActionToken)).OrderByDescending(t => t.Name).ToList();

                if (types.Count == 0)
                {
                    RememberView(request);
                    return null;
                }

                var targetType = types[0];

                var responser = Activator.CreateInstance(targetType) as IViewActionResponser<T>;
                if (responser == null)
                    throw new Exception(string.Format("指定的视图行为处理器{0}创建失败！", request.ActionToken));

                // 如果当前没有对应视图的行为处理器列表则创建一个新的
                if (!ResponserListDictionary.ContainsKey(responser.ConcernedViewId))
                {
                    ResponserListDictionary.Add(responser.ConcernedViewId, new List<IViewActionResponser<T>>());
                }
                // 缓存刚创建的视图行为处理器
                ResponserListDictionary[responser.ConcernedViewId].Add(responser);
                ResponsersDictionary.Add(request.ActionToken, responser);
                // 返回刚创建的视图行为处理器
                return responser;
            }

            private static void RememberView(IViewActionRequest<T> request)
            {
                string refStr = null;
                if (!ViewEventTypeSet.IsLiftType(request.ActionToken, ref refStr)) return;

                var type = ViewEventTypeSet.GetLiftType(refStr);
                var view = (IView)request.ActionRequester.Origin;
                view.Remember(type, false);
            }

            private static class ViewEventTypeSet
            {
                private static readonly List<string> _viewEventList = new List<string>
                {
                    "BeforeCreat",

                    "OnCreated",

                    "BeforeHide",

                    "OnHided",

                    "BeforeActive",

                    "OnActived",

                    "BeforeClose",

                    "OnClosed"
                };

                public static bool IsLiftType(string token, ref string refStr)
                {
                    var typeStr = _viewEventList.Find(token.Contains);
                    if (typeStr == null) return false;

                    refStr = typeStr;
                    return true;
                }

                public static ViewLifeEventType GetLiftType(string type)
                {
                    return _lifeMaps[type];
                }

                private static readonly Dictionary<string, ViewLifeEventType> _lifeMaps = new Dictionary<string, ViewLifeEventType>
                {
                    {"BeforeCreat",ViewLifeEventType.BeforeCreat },
                    {"BeforeActive",ViewLifeEventType.BeforeActive },
                    {"BeforeClose",ViewLifeEventType.BeforeClose },
                    {"BeforeHide",ViewLifeEventType.BeforeHide },
                    {"OnClosed",ViewLifeEventType.OnClosed },
                    {"OnActived",ViewLifeEventType.OnActived },
                    {"OnCreated",ViewLifeEventType.OnCreated },
                    {"OnHided",ViewLifeEventType.OnHided },

                };

            }


        }
    }

}

