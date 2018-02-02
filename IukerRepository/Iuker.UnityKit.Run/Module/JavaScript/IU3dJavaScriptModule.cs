using Iuker.Common;
using Iuker.Common.Base;

namespace Iuker.UnityKit.Run.Module.JavaScript
{
#if DEBUG
    /// <summary>
    /// JavaScript脚本模块
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20170926 14:34:10")]
    [InterfacePurposeDesc("JavaScript脚本模块", "JavaScript脚本模块")]
#endif
    public interface IU3dJavaScriptModule : IModule
    {
        bool Exist(string scriptName);

        void DoFile(string scriptName);

        void DoString(string code);

        object GetGlobalValue(string name);

        object GetJsValueByNameSpace(string name, string nameSpace);

        bool IsMd5Change(string name);

        object Engine { get; }
    }
}
