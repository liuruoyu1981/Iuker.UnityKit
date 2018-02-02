using Iuker.Common.Base;

namespace Iuker.UnityKit.Editor.MVDA
{
#if DEBUG
    /// <summary>
    /// 视图控件路径数据
    /// </summary>
    [CreateDesc("liuruoyu1981","liuruoyu1981@gmail.com","20170913 12:48:39")]
    [ClassPurposeDesc("视图控件路径数据","视图控件路径数据")]
#endif
    public class WidgetPathInfo
    {
        public string Name;
        public string Path;

        public WidgetPathInfo(string name, string path)
        {
            Name = name;
            Path = path;
        }
    }
}
