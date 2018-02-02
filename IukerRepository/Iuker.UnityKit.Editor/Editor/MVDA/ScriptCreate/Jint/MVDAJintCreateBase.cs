using System.Text;

namespace Iuker.UnityKit.Editor.MVDA.ScriptCreate.Jint
{
    public class MVDAJintCreateBase : MVDACsCreater
    {
        protected string ClassName
        {
            get { return mSelectedWidget.name; }
        }

        protected void WriteNameSpaceHeader(StringBuilder sb)
        {
            sb.AppendLine(string.Format("namespace {0} ", mSelectSon.CompexName) + "{");
        }
    }
}