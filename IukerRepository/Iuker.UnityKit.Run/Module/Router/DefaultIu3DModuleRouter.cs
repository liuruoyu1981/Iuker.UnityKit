using System.Collections.Generic;
using Iuker.Common;
using Iuker.Common.Base;
using Iuker.Common.Base.Enums;
using Iuker.Common.Utility;
using Iuker.UnityKit.Run.Base;
using UnityEngine;

namespace Iuker.UnityKit.Run.Module.Replace
{
#if DEBUG
    /// <summary>
    /// 默认Unity路由模块，用于动态修改应用中的各种调度流程。
    /// 1. 视图构建
    /// 2. 视图行为处理
    /// 3. 通信行为处理
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20171028 10:56:41")]
    [ClassPurposeDesc("默认Unity替换模块，用于动态替换各个行为调度器的实际调度和修改各行为的执行流程", "默认Unity替换模块，用于动态替换各个行为调度器的实际调度和修改各行为的执行流程")]
#endif
    public class DefaultIu3DModule_Router : AbsU3dModule, IU3dRouterModule
    {
        public override string ModuleName
        {
            get
            {
                return ModuleType.Router.ToString();
            }
        }

        protected override void OnHotUpdateComplete()
        {
            base.OnHotUpdateComplete();

            var sons = Bootstrap.Instance.TotalSons;
            foreach (var son in sons)
            {
                var loader = U3DFrame.AssetModule.LoadTextAsset(son.ViewActionReplaceDataName);
                if (loader == null) continue;
                var bytes = loader.Asset.bytes;
                var data = SerializeUitlity.DeSerialize<Dictionary<string, string>>(bytes);
                mActionRouters.Combin(data);
            }
        }

        private readonly Dictionary<string, string> mActionRouters = new Dictionary<string, string>();
        private readonly Dictionary<string, string> mTypeRouters = new Dictionary<string, string>();
        private readonly Dictionary<string, string> mViewContainerRoutes = new Dictionary<string, string>();

        #region 获取路由规则

        public void AddViewTypeRoute(string source, string target)
        {
            if (!mTypeRouters.ContainsKey(source))
            {
                mTypeRouters.Add(source, target);
                return;
            }

            Debug.LogError(string.Format("原视图类型{0}当前已存在路由记录！", source));
        }

        public void AddNetRouter(string source, string target)
        {
            throw new System.NotImplementedException();
        }

        public string GetAssetRoute(string source)
        {
            return mViewContainerRoutes.ContainsKey(source) ? mViewContainerRoutes[source] : null;
        }

        public string GetNetRoute(string source)
        {
            throw new System.NotImplementedException();
        }

        #endregion

        #region 代码直接添加路由规则

        public void AddViewActionRoute(string source, string target)
        {
            if (!mActionRouters.ContainsKey(source))
            {
                mActionRouters.Add(source, target);
                return;
            }

            Debug.LogError(string.Format("原视图行为{0}当前已存在路由记录！", source));
        }

        public string GetActionRoute(string source)
        {
            return !mActionRouters.ContainsKey(source) ? null : mActionRouters[source];
        }

        public string GetTypeRoute(string source)
        {
            return !mTypeRouters.ContainsKey(source) ? null : mTypeRouters[source];
        }

        public void AddViewAssetRoute(string source, string target)
        {
            if (!mViewContainerRoutes.ContainsKey(source))
            {
                mViewContainerRoutes.Add(source, target);
                return;
            }

            Debug.LogError(string.Format("原视图{0}当前已存在路由记录！", source));
        }


        #endregion



    }
}
