/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 6/1/2017 15:29:56 PM
Email: liuruoyu1981@gmail.com
***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using Iuker.UnityKit.Run.Base.Config.Develop;

namespace Iuker.UnityKit.Editor.Excel
{
    public static partial class ExcelUtility
    {
        /// <summary>
        /// 创建本地数据类型表的对应脚本文件
        /// </summary>
        /// <param name="streamWriter"></param>
        private static void CreateLocalDataClass(StreamWriter streamWriter)
        {
            WriteFileInfo(streamWriter);
            WriteLocalDataNameSpace(streamWriter);
            WriteLocalDataClassName(streamWriter, localDataClassName);
            WriteLocalDataClassMember(streamWriter);
            WriteNote(streamWriter, "从单行转换为实体数据类",
                new List<string> { "row" },
                new List<string> { "单行数据", "数据列数，即类成员数" },
                "        ");
            WriteCreateLocalDataEntity(streamWriter);
            WriteLocalDataParse(streamWriter);
            WriteLocalDataToTxtMethod(streamWriter);
            WriteDeepCopy(streamWriter);
            streamWriter.WriteLine("}");
        }

        /// <summary>
        /// 写入将实体数据对象转换为txt文件的方法代码
        /// </summary>
        /// <param name="streamWriter"></param>
        private static void WriteLocalDataToTxtMethod(StreamWriter streamWriter)
        {
            WriteNote(streamWriter, "将本地数据对象转换为txt源数据字符串", null, null, "        ");
            streamWriter.WriteLine("        public string ToTxt()");
            streamWriter.WriteLine("        {");
            streamWriter.WriteLine("            string entityString = string.Empty;");
            for (var i = 0; i < maxColumns; i++)
            {
                var codeString = GetCodeStringByType(fieldNameDictionary[i], filedTypeDictionary[i]);
                streamWriter.WriteLine("            " + codeString);
            }

            // CN：移除最后得到字符串末尾的=    EN：Remove the = of the string end
            streamWriter.WriteLine("            entityString = entityString.Remove(entityString.Length - 1);");
            streamWriter.WriteLine("            return entityString;");
            streamWriter.WriteLine("        }");
            streamWriter.WriteLine();
        }

        /// <summary>
        /// 通过成员类型获得代码字符串
        /// </summary>
        /// <param name="property"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        private static string GetCodeStringByType(string property, string t)
        {
            var codeString = string.Empty;
            switch (t)
            {
                case "int":
                    codeString = "entityString = entityString + " + property + " + " + "\"" + TxtSeparators + "\"" + ";";
                    break;
                case "string":
                    codeString = "entityString = entityString + " + property + " + " + "\"" + TxtSeparators + "\"" + ";";
                    break;
                case "List<int>":
                    codeString = "entityString = entityString + " + string.Format("{0}.ConvertListIntToString() + " + "\"" + TxtSeparators + "\"" + ";", property);
                    break;
                case "bool":
                    codeString = "entityString = entityString + " + property + ".ToString().ToLower() + " + "\"" + TxtSeparators + "\"" + ";";
                    break;
                case "float":
                    codeString = "entityString = entityString + " + property + " + " + "\"" + TxtSeparators + "\"" + ";";
                    break;
                case "List<string>":
                    codeString = "entityString = entityString + " + string.Format("{0}.ConvertListStringToString() + " + "\"" + TxtSeparators + "\"" + ";", property);
                    break;
            }
            return codeString;
        }

        /// <summary>
        /// 写入解析本地数据源的parse方法
        /// </summary>
        /// <param name="streamWriter"></param>
        private static void WriteLocalDataParse(StreamWriter streamWriter)
        {
            streamWriter.WriteLine(string.Format("        public List<{0}> Parse(List<string> listObj)",
                localDataClassName));
            streamWriter.WriteLine("        {");
            streamWriter.WriteLine(string.Format("            var result = new List<{0}>();", localDataClassName));
            streamWriter.WriteLine("            foreach (var list in listObj)");
            streamWriter.WriteLine("            {");
            streamWriter.WriteLine("                var entityListText = list.Split(Constant.TxtSeparators,StringSplitOptions.None).ToList();");
            streamWriter.WriteLine("                var entity = CreateEntity(entityListText);");
            streamWriter.WriteLine("                result.Add(entity);");
            streamWriter.WriteLine("            }");
            streamWriter.WriteLine("            return result;");
            streamWriter.WriteLine("        }");
            streamWriter.WriteLine();
        }

        private static void WriteFileInfo(StreamWriter streamWriter)
        {
            streamWriter.WriteLine("/***********************************************************************************************");
            streamWriter.WriteLine(string.Format("Author：{0}",
                RootConfig.GetCurrentProject().GetCurrentSonProject().CurrentClientCoder.Name));
            streamWriter.WriteLine("CreateDate: " + DateTime.Now);
            streamWriter.WriteLine(string.Format("Email: {0}",
                RootConfig.GetCurrentProject().GetCurrentSonProject().CurrentClientCoder.Email));
            streamWriter.WriteLine("***********************************************************************************************/");
            streamWriter.WriteLine();
            streamWriter.WriteLine();
            streamWriter.WriteLine("/*");
            streamWriter.WriteLine("该文件由工具自动生成，请勿做任何修改！！！！！！！！！");
            streamWriter.WriteLine("*/");
            streamWriter.WriteLine();
        }

        /// <summary>
        /// 写入本地数据集类型Excel表自动脚本文件的命名空间
        /// </summary>
        /// <param name="streamWriter"></param>
        private static void WriteLocalDataNameSpace(TextWriter streamWriter)
        {
            streamWriter.WriteLine("using System;");
            streamWriter.WriteLine("using System.Collections.Generic;");
            streamWriter.WriteLine("using System.Linq;");
            streamWriter.WriteLine("using Iuker.Common;");
            streamWriter.WriteLine("using Iuker.Common.Module.LocalData;");
            streamWriter.WriteLine("using Iuker.Common.Constant;");
            streamWriter.WriteLine("using Iuker.Common.Utility;");
            streamWriter.WriteLine();

            // 项目命名空间

            streamWriter.WriteLine(string.Format("namespace {0}", RootConfig.ProjectNameSpace));
            streamWriter.WriteLine("{");
        }

        private static void WriteLocalDataClassName(StreamWriter streamWriter, string targetClassName)
        {
            WriteNote(streamWriter, "本地数据表_" + tableName, null, null, "    ");
            // CN：使用Unity5.3版本提供的json序列化工具需要该特性
            streamWriter.WriteLine("    [Serializable]");
            streamWriter.WriteLine("    public class {0} : IDeepCopyLocalData<{0}>", targetClassName);
            streamWriter.WriteLine("    {");
        }

        private static void WriteNote(StreamWriter streamWriter, string note, List<string> paramNameList = null, List<string> paramNoteList = null, string space = null)
        {
            streamWriter.WriteLine(space + "/// <summary>");
            streamWriter.WriteLine(space + "/// " + note);
            streamWriter.WriteLine(space + "/// </summary>");
            //拆注释参数集合
            if (paramNameList == null)
                return;
            if (paramNoteList == null)
            {
                foreach (var paramName in paramNameList)
                {
                    streamWriter.WriteLine(space + "/// <param name=\"" + paramName + "\"></param>");
                }
            }
            else
            {
                for (var i = 0; i < paramNameList.Count; i++)
                {
                    var paramName = paramNameList[i];
                    var paramNoe = paramNoteList[i];
                    streamWriter.WriteLine(space + "/// <param name=\"{0}\">{1}</param>", paramName, paramNoe);
                }
            }
        }

        private static void WriteDeepCopy(StreamWriter streamWriter)
        {
            WriteNote(streamWriter, "获得本地数据的一份深度复制副本", null, null, "        ");
            streamWriter.WriteLine("        public {0} DeepCopy()", localDataClassName);
            streamWriter.WriteLine("        {");
            streamWriter.WriteLine("            var buff = SerializeUitlity.Serialize(this);");
            streamWriter.WriteLine("            {0} entity = SerializeUitlity.DeSerialize<{0}>(buff);", localDataClassName);
            streamWriter.WriteLine("            return entity;");
            streamWriter.WriteLine("        }");
            streamWriter.WriteLine("    }");
        }


        /// <summary>
        /// 写入本地数据集合类型自动脚本文件的类成员
        /// </summary>
        /// <param name="streamWriter"></param>
        private static void WriteLocalDataClassMember(StreamWriter streamWriter)
        {
            for (var i = 0; i < maxColumns; i++)
            {
                WriteNote(streamWriter, membersNoteList[i], null, null, "        ");
                streamWriter.WriteLine("        public " + filedTypeDictionary[i] + " " + fieldNameDictionary[i] + ";");
                streamWriter.WriteLine();
            }

            streamWriter.WriteLine();
        }

        /// <summary>
        /// 写入本地数据集合类型自动脚本文件的创建实体方法
        /// </summary>
        /// <param name="streamWriter"></param>
        private static void WriteCreateLocalDataEntity(StreamWriter streamWriter)
        {
            streamWriter.WriteLine("        public " + localDataClassName + " CreateEntity" + "(List<string> row)");
            streamWriter.WriteLine("        {");
            streamWriter.WriteLine("            " + localDataClassName + " instance = new " + localDataClassName + "();");

            for (var i = 0; i < maxColumns; i++)
            {
                var codeString = ParseLocalDataConvertType(filedTypeDictionary[i], fieldNameDictionary[i], i);
                streamWriter.WriteLine(codeString);
            }

            streamWriter.WriteLine("            return instance;");
            streamWriter.WriteLine("        }");
            streamWriter.WriteLine();
        }


        /// <summary>
        /// 解析本地数据集合类型的数据格式
        /// </summary>
        /// <param name="type"></param>
        /// <param name="left"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static string ParseLocalDataConvertType(string type, string left, int index)
        {
            var indexStr = index.ToString();
            switch (type)
            {
                case "int":
                    return "            instance." + left + " = " + "Convert.ToInt32(row[" + indexStr + "]);";
                case "string":
                    return "            instance." + left + " = " + "row[" + indexStr + "];";
                case "List<int>":
                    return "            instance." + left + " = " +
                           "RyCommonExtend.TransformStringToList(" + left + ", row[" + indexStr + "].Split(',').ToList());";
                case "bool":
                    return "            instance." + left + "=" + "Convert.ToBoolean(row[" + indexStr + "]);";
                case "float":
                    return "            instance." + left + "=" + " float.Parse(row[" + indexStr + "].ToString());";
                case "List<string>":
                    return "            instance." + left + " = " + "row[" + indexStr + "].Split(',').ToList();";
                default:
                    return "";
            }
        }

    }
}
