/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/07/26 11:26:39
Email: 35490136@qq.com
QQCode: 35490136
CreateNote: 
***********************************************************************************************/


/****************************************修改日志***********************************************
1. 修改日期： 修改人： 修改内容：
2. 修改日期： 修改人： 修改内容：
3. 修改日期： 修改人： 修改内容：
4. 修改日期： 修改人： 修改内容：
5. 修改日期： 修改人： 修改内容：
****************************************修改日志***********************************************/

using System.Collections.Generic;
using System.Text;

namespace Iuker.Common.Utility
{
    /// <summary>
    /// 脚本创建工具
    /// </summary>
    public static class ScriptCreateUitlity
    {
        /// <summary>
        /// 写入一个标准格式的方法的相关代码
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="methadHead"></param>
        /// <param name="methodBody"></param>
        /// <param name="space"></param>
        public static void WriteStandardMethod(StringBuilder sb, string methadHead, string methodBody = "    ", string space = "    ")
        {
            sb.AppendLine(methadHead);
            sb.AppendLine(space + "{");
            sb.AppendLine(space + methodBody);
            sb.AppendLine(space + "}");
            sb.AppendLine();
        }

        /// <summary>
        /// 写入代码注释
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="note"></param>
        /// <param name="paramNameList"></param>
        /// <param name="paramNoteList"></param>
        /// <param name="space">缩进</param>
        public static void WriteNote(StringBuilder sb, string note, List<string> paramNameList = null, List<string> paramNoteList = null,
            string space = "    ")
        {
            sb.AppendLine(string.Format("{0}/// <summary>", space));
            sb.AppendLine(string.Format("{0}/// ", space) + note);
            sb.AppendLine(string.Format("{0}/// </summary>", space));
            //拆注释参数集合
            if (paramNameList == null)
                return;
            if (paramNoteList == null)
            {
                foreach (string paramName in paramNameList)
                {
                    sb.AppendLine(string.Format("{0}/// <param TypeName=\"", space) + paramName + "\"></param>");
                }
            }
            else
            {
                for (int i = 0; i < paramNameList.Count; i++)
                {
                    string paramName = paramNameList[i];
                    string paramNoe = paramNoteList[i];
                    sb.AppendLine(string.Format("{0}/// <param TypeName=\"{1}\">{2}</param>", space, paramName,
                        paramNoe));
                }
            }
        }

        /// <summary>
        /// 写入一行字段、属性定义然后换行
        /// </summary>
        public static void WriteFileAndSpace(StringBuilder sb, string codeContent)
        {
            sb.AppendLine(codeContent);
            sb.AppendLine();
        }

    }
}