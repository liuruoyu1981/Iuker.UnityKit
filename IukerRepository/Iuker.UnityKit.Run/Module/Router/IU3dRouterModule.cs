using Iuker.Common;
using Iuker.Common.Base;

namespace Iuker.UnityKit.Run.Module.Replace
{
#if DEBUG
    /// <summary>
    /// 替换模块接口
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20171028 11:01:47")]
    [InterfacePurposeDesc("替换模块接口", "替换模块接口")]
#endif
    public interface IU3dRouterModule : IModule
    {
        #region 获取路由规则

        /// <summary>
        /// 替换视图预制件
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        string GetAssetRoute(string source);

        /// <summary>
        /// 获取视图行为处理路由记录
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        string GetActionRoute(string source);

        /// <summary>
        /// 替换视图解析脚本（容器脚本）
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        string GetTypeRoute(string source);

        /// <summary>
        /// 获取通信行为处理路由记录
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        string GetNetRoute(string source);

        #endregion

        #region 代码直接添加路由规则

        void AddViewActionRoute(string source, string target);
        void AddViewAssetRoute(string source, string target);
        void AddViewTypeRoute(string source, string target);
        void AddNetRouter(string source, string target);

        #endregion



    }
}
