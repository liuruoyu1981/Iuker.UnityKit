using System;
using System.Collections.Generic;
using System.Text;
using Iuker.Common.Utility;

namespace Iuker.Common.Base
{
#if DEBUG
    /// <summary>
    /// Csharp脚本创建字符串追加器
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20170913 14:00:38")]
    [ClassPurposeDesc("Csharp脚本创建字符串追加器", "Csharp脚本创建字符串追加器")]
#endif
    public class CsharpScriptAppender : IDisposable
    {
        private readonly StringBuilder StringBuilder = new StringBuilder();
        private readonly string mNameSpace;

        public string CodeContent
        {
            get
            {
                return StringBuilder.ToString();
            }
        }

        public void Dispose()
        {
            if (string.IsNullOrEmpty(mNameSpace))
            {
                StringBuilder.AppendLine("}");
            }
            else
            {
                StringBuilder.AppendLine("    " + "}");
                StringBuilder.AppendLine("}");
            }

            WriteAllText(mWritePath);
        }

        private string mWritePath;

        /// <summary>
        /// 设置脚本的创建路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public CsharpScriptAppender SetWritePath(string path)
        {
            mWritePath = path;
            return this;
        }

        public CsharpScriptAppender(string clientCoder, string email, string name, string[] nameSpaces = null, string superName = null, string nameSpace = null, string type = "class", string visit = "public", params string[] notes)
        {
            mNameSpace = nameSpace;
            WriteFileInfo(clientCoder, email, notes);
            if (string.IsNullOrEmpty(mNameSpace))
            {
                if (nameSpaces != null && nameSpaces.Length > 0)
                {
                    WriteNameSpace(nameSpaces);
                }
                StringBuilder.Append(string.Format("{0} {1} {2}", visit, type, name));
                if (!string.IsNullOrEmpty(superName))
                {
                    Append(string.Format(" : {0}", superName));
                }
                AppendLine();
                AppendLine("{");
            }
            else
            {
                if (nameSpaces != null && nameSpaces.Length > 0)
                {
                    WriteNameSpace(nameSpaces);
                }
                AppendLine(string.Format("namespace {0}", mNameSpace));
                AppendLine("{");
                Append(string.Format("    {0} {1} {2}", visit, type, name));
                if (!string.IsNullOrEmpty(superName))
                {
                    Append(string.Format(" : {0}", superName));
                }
                AppendLine();
                AppendLine("    {");
            }
        }

        #region StringBuilder接口

        public void Append(String value)
        {
            StringBuilder.Append(value);
        }

        public void AppendLine(string value)
        {
            StringBuilder.AppendLine(value);
        }

        public void AppendLine()
        {
            StringBuilder.AppendLine("");
        }

        #endregion

        #region 代码块写入接口

        /// <summary>
        /// 写入脚本文件的命名空间
        /// </summary>
        /// <param name="namespaces"></param>
        private void WriteNameSpace(params string[] namespaces)
        {
            foreach (var ns in namespaces)
            {
                AppendLine(ns);
            }
            AppendLine();
        }

        /// <summary>
        /// 写入一个方法代码块
        /// </summary>
        /// <param name="methodHead"></param>
        /// <param name="methodBody"></param>
        /// <param name="braceSpace"></param>
        /// <param name="bodySpace"></param>
        public void WriteMethod(string methodHead, string methodBody = "    ",
            string braceSpace = "    ", string bodySpace = "        ")
        {
            if (!string.IsNullOrEmpty(mNameSpace)) braceSpace = "        ";
            if (!string.IsNullOrEmpty(mNameSpace)) bodySpace = "            ";
            AppendLine(braceSpace + methodHead);
            AppendLine(braceSpace + "{");
            AppendLine(bodySpace + methodBody);
            AppendLine(braceSpace + "}");
            AppendLine();
        }

        /// <summary>
        /// 写入一个方法代码块
        /// 该方法将接受一个委托，用于写入方法体的代码块字符串
        /// </summary>
        /// <param name="methodHead"></param>
        /// <param name="methodBodyAction"></param>
        /// <param name="braceSpace"></param>
        public void WriteMethod(string methodHead, Action<bool> methodBodyAction,
            string braceSpace = "    ")
        {
            braceSpace = string.IsNullOrEmpty(mNameSpace) ? braceSpace : braceSpace + "    ";
            AppendLine(braceSpace + methodHead);
            AppendLine(braceSpace + "{");
            if (methodBodyAction != null)
            {
                methodBodyAction(string.IsNullOrEmpty(mNameSpace));
            }
            AppendLine(braceSpace + "}");
            AppendLine();
        }

        /// <summary>
        /// 写入一行代码字符串
        /// 该方法在位于一个函数体内时调用
        /// 默认的缩进为8个字符串
        /// </summary>
        /// <param name="codeText"></param>
        /// <param name="isInNameSpace"></param>
        /// <param name="retract"></param>
        public void AppendLineInMethod(string codeText, bool isInNameSpace = true, string retract = "        ")
        {
            AppendLine(isInNameSpace ? "    " + retract + codeText : retract + codeText);
        }

        private void WriteFileInfo(string client, string email, params string[] notes)
        {
            AppendLine("/***********************************************************************************************");
            AppendLine(string.Format("Author：{0}", client));
            AppendLine("CreateDate: " + Constant.Constant.DateAndTime);
            AppendLine(string.Format("Email: {0}", email));
            AppendLine("***********************************************************************************************/");
            AppendLine();
            AppendLine();
            AppendLine("/*");
            foreach (var note in notes)
            {
                AppendLine(note);
            }
            AppendLine("*/");
            AppendLine();
        }

        /// <summary>
        /// 写入一个字段或属性的代码字符串
        /// </summary>
        /// <param name="content"></param>
        public void WriteFiledOrProperty(string content)
        {
            var space = string.IsNullOrEmpty(mNameSpace) ? "    " : "        ";
            AppendLine(space + content);
            AppendLine();
        }


        /// <summary>
        /// 写入注释
        /// </summary>
        /// <param name="note"></param>
        /// <param name="paramNameList">参数名列表</param>
        /// <param name="paramNoteList">参数注释文字列表</param>
        /// <param name="space">缩进空格符文本</param>
        public void WriteNote(string note, List<string> paramNameList = null, List<string> paramNoteList = null, string space = null)
        {
            AppendLine(space + "/// <summary>");
            AppendLine(space + "/// " + note);
            AppendLine(space + "/// </summary>");
            //拆注释参数集合
            if (paramNameList == null)
                return;
            if (paramNoteList == null)
            {
                foreach (var paramName in paramNameList)
                {
                    AppendLine(space + "/// <param name=\"" + paramName + "\"></param>");
                }
            }
            else
            {
                for (var i = 0; i < paramNameList.Count; i++)
                {
                    var paramName = paramNameList[i];
                    var paramNoe = paramNoteList[i];
                    AppendLine(string.Format(space + "/// <param name=\"{0}\">{1}</param>", paramName, paramNoe));
                }
            }
        }

        private void WriteAllText(string path)
        {
            FileUtility.WriteAllText(path, CodeContent);
        }

        #endregion




    }
}
