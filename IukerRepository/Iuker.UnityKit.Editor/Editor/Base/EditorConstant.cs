using Iuker.UnityKit.Editor.Setting;

namespace Iuker.UnityKit.Editor
{
    /// <summary>
    /// 编辑器常量
    /// </summary>
    public static class EditorConstant
    {
        public const string IukerIl8nKey = "IukerEditor";

        #region EditorPrefsKey

        /// <summary>
        /// 本机开发者姓名
        /// </summary>
        public static string HostClientName
        {
            get
            {
                return IukerEditorPrefs.GetString("HostClientName");
            }
        }

        /// <summary>
        /// 本机开发者邮箱
        /// </summary>
        public static string HostClientEmail
        {
            get
            {
                return IukerEditorPrefs.GetString("HostClientEmail");
            }
        }

        /// <summary>
        /// Txuture是否已注册
        /// </summary>
        public static readonly string TexturePakcerReg = "TexturePakcerReg";

        #endregion

        #region 本地http资源服务器

        /// <summary>
        /// 是否已启动本能HTTP资源服务器
        /// </summary>
        //public static readonly string IsLauncherHttpServer = "IsLauncherHttpServer";

        #endregion
    }
}
