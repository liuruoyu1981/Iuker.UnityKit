using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Iuker.Common;
using Iuker.Common.Utility;
using Iuker.UnityKit.Run.Base;
using Iuker.UnityKit.Run.Base.Config;
using UnityEditor;

namespace Iuker.UnityKit.Editor
{
    /// <summary>
    /// 依据约定的的数据ID文本自动生成对应子项目下的
    /// 数据ID脚本（Cs和Ts）
    /// </summary>
    public class IdScriptCreater
    {
        private readonly SonProject mSon;
        private readonly string mIdTypeStr;
        private readonly string mCsPath;
        private readonly string mTsPath;
        private List<string> mNoteLines;
        private List<string> mFieldLines;
        private readonly string mTxtPath;
        private readonly string mNs;

        /// <summary>
        /// 类型前缀
        /// 例如：Net_Success, Net为前缀、Success为消息
        /// </summary>
        private readonly string mPrefsixx;

        public IdScriptCreater(SonProject sonProject, string ns,
            string prefSixx, string txtPath, string idTypeStr, string csPath, string tsPath)
        {
            mTsPath = txtPath;
            mNs = ns;
            mPrefsixx = prefSixx;
            mTxtPath = txtPath;
            mSon = sonProject;
            mIdTypeStr = idTypeStr;
            mCsPath = csPath;
            mTsPath = tsPath;
        }

        public void CreateScript()
        {
            GetNoteAndField();
            AppendCsScript();
            AppendTsScript();
            AssetDatabase.Refresh();
        }

        private void GetNoteAndField()
        {
            if (!File.Exists(mTxtPath))
            {
                EditorUtility.DisplayDialog("错误", "目标Id脚本的源Txt文件不存在！", "确定");
                return;
            }

            var lines = File.ReadAllLines(mTxtPath).ToList();
            mNoteLines = lines.FindAll(l => l.StartsWith("//")).Select(RemoveAllFailChar).ToList();
            mFieldLines = lines.FindAll(l => !l.StartsWith("//") && !string.IsNullOrEmpty(l));
        }

        private void AppendCsScript()
        {
            var sb = new StringBuilder();

            sb.AppendLine(string.Format("namespace {0}", mNs));
            sb.AppendLine("{");
            sb.AppendLine(string.Format("    public static class {0}_{1}", mPrefsixx, mIdTypeStr));
            sb.AppendLine("    {");

            for (var i = 0; i < mNoteLines.Count; i++)
            {
                var note = mNoteLines[i];
                var field = mFieldLines[i];
                sb.WriteNote(note, null, null, "        ");
                sb.WriteFiledOrProperty(string.Format("        public const string {0} = ", field) + "\"" + mPrefsixx + "_"
                    + field + "\"" + ";");
            }

            sb.AppendLine("    }");
            sb.AppendLine("}");

            FileUtility.WriteAllText(mCsPath, sb.ToString());
        }

        private void AppendTsScript()
        {
            var tsScriptName = string.Format("{0}_{1}", mPrefsixx.ToLower(), mIdTypeStr.ToLower());
            var sb = new StringBuilder();

            sb.AppendLine(string.Format("namespace {0} ", mPrefsixx) + "{");
            sb.AppendLine();
            sb.AppendLine(string.Format("    export class {0}_{1} ", mPrefsixx, mIdTypeStr) + "{");
            sb.AppendLine();


            for (var i = 0; i < mNoteLines.Count; i++)
            {
                var note = mNoteLines[i];
                var field = mFieldLines[i];
                sb.WriteTsComment(note);
                sb.AppendLine(string.Format("        public static readonly {0}: string = '{1}_{0}';", field, mPrefsixx));
                sb.AppendLine();
            }

            sb.AppendLine("    }");
            sb.AppendLine("}");

            FileUtility.WriteAllText(mTsPath, sb.ToString());
            TsProj.AddLine(tsScriptName, mSon).UpdateToFile(mSon.TsProjPath);
        }

        private string RemoveAllFailChar(string source)
        {
            var index = 0;

            foreach (var c in source)
            {
                if (c == '/' || c == ' ' || c == '\t') index++;
            }

            var length = source.Length;
            var result = source.Substring(index, length - index);
            return result;
        }



    }
}