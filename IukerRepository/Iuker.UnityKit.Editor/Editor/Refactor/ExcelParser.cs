/***********************************************************************************************
/*  Author：        liuruoyu1981
/*  CreateDate:     2017/11/27 上午 07:51:57 
/*  Email:          35490136@qq.com
/*  QQCode:         35490136
/*	Machine:		DESKTOP-M1OBR70
/*  CreateNote: 
***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using Excel;
using Iuker.Common;
using Iuker.Common.Constant;
using Iuker.Common.Utility;
using Iuker.UnityKit.Run.Base;
using Iuker.UnityKit.Run.Base.Config;
using Iuker.UnityKit.Run.Base.Config.Develop;

namespace Iuker.UnityKit.Editor
{
    public class ExcelParser
    {
        #region 基础字段

        private readonly List<ExcelFieldInfo> mFieldInfos = new List<ExcelFieldInfo>();
        private readonly string mTxtSeparators = Constant.TxtSeparators[0];
        private readonly StringBuilder mTsScriptSb = new StringBuilder();
        private readonly StringBuilder mTxtSb = new StringBuilder();
        private readonly StringBuilder mCsScriptSb = new StringBuilder();
        private readonly DataTableCollection mTables;
        private readonly DataTable mFirstTable;
        private readonly SonProject mSon;
        private readonly string mClassName;

        #endregion

        #region Common

        public ExcelParser(string excelPath, SonProject son)
        {
            using (var fileStream = File.Open(excelPath, FileMode.Open, FileAccess.Read))
            {
                var excelDataReader = ExcelReaderFactory.CreateOpenXmlReader(fileStream);
                var dataSet = excelDataReader.AsDataSet();
                mFirstTable = dataSet.Tables[0];
                mTables = dataSet.Tables;
            }
            mSon = son;
            mClassName = "LdTable_" + mSon.CompexName + "_" + Path.GetFileNameWithoutExtension(excelPath);
            ParseFieldInfo();
        }

        private void ParseFieldInfo()
        {
            var first = mFirstTable.Rows[0];
            var second = mFirstTable.Rows[1];
            var length = mFirstTable.Columns.Count;
            for (var i = 0; i < length; i++)
            {
                var filedInfo = new ExcelFieldInfo();
                var typeAndNameStr = first[i].ToString();
                var comment = second[i].ToString();
                filedInfo.SetTypeAndName(typeAndNameStr)
                    .SetComment(comment)
                    .SetIndex(i);

                mFieldInfos.Add(filedInfo);
            }
        }

        #endregion

        #region Csharp

        public void ExportCsharpScript()
        {
            mCsScriptSb.AppendCsahrpFileInfo(EditorConstant.HostClientName, EditorConstant.HostClientEmail);
            AppendCsharpNameSpace();
            AppendCsahrpClassHeader();
            AppendCsharpField();
            AppendCsharpCreateEntityMethodCode();
            AppendCsahrpCreateEntitysMethodCode();
            AppendCsharpToTxtMethodCode();
            AppendCsharpDeepCopyMethodCode();
            mCsScriptSb.AppendLine("}");
            var path = mSon.CsLocalDataDir + mClassName + ".cs";
            FileUtility.WriteAllText(path, mCsScriptSb.ToString());
        }

        private void AppendCsharpNameSpace()
        {
            mCsScriptSb.AppendLine("using System;");
            mCsScriptSb.AppendLine("using System.Collections.Generic;");
            mCsScriptSb.AppendLine("using System.Linq;");
            mCsScriptSb.AppendLine("using Iuker.Common;");
            mCsScriptSb.AppendLine("using Iuker.Common.Module.LocalData;");
            mCsScriptSb.AppendLine("using Iuker.Common.Constant;");
            mCsScriptSb.AppendLine("using Iuker.Common.Utility;");
            mCsScriptSb.AppendLine();
            mCsScriptSb.AppendLine(string.Format("namespace {0}", RootConfig.ProjectNameSpace));
            mCsScriptSb.AppendLine("{");
        }

        private void AppendCsahrpClassHeader()
        {
            mCsScriptSb.AppendCsharpNote("本地数据表_" + mClassName, null, null, "    ");
            mCsScriptSb.AppendLine("    [Serializable]");
            mCsScriptSb.AppendLine(string.Format("    public class {0} : IDeepCopyLocalData<{1}>", mClassName,
                mClassName));
            mCsScriptSb.AppendLine("    {");
        }

        private void AppendCsharpField()
        {
            mFieldInfos.ForEach(info =>
            {
                mCsScriptSb.AppendCsharpNote(info.Comment, null, null, "        ");
                mCsScriptSb.AppendLine(string.Format("        public {0} {1};", info.CsharpOriginType, info.Name));
                mCsScriptSb.AppendLine();
            });
        }

        private void AppendCsharpCreateEntityMethodCode()
        {
            mCsScriptSb.AppendLine(string.Format("        public {0} CreateEntity(List<string> row)", mClassName));
            mCsScriptSb.AppendLine("        {");
            mCsScriptSb.AppendLine(string.Format("            {0} entity = new {1}();", mClassName, mClassName));

            mFieldInfos.ForEach(info =>
            {
                mCsScriptSb.AppendLine(string.Format("            {0}", info.CsharpEntityStr));
            });

            mCsScriptSb.AppendLine("            return entity;");
            mCsScriptSb.AppendLine("        }");
            mCsScriptSb.AppendLine();
        }

        private void AppendCsahrpCreateEntitysMethodCode()
        {
            mCsScriptSb.AppendLine(string.Format("        public List<{0}> CreateEntitys(List<string> listObj)",
                mClassName));
            mCsScriptSb.AppendLine("        {");
            mCsScriptSb.AppendLine(string.Format("            var result = new List<{0}>();", mClassName));
            mCsScriptSb.AppendLine("            foreach (var list in listObj)");
            mCsScriptSb.AppendLine("            {");
            mCsScriptSb.AppendLine("                var entityListText = list.Split(Constant.TxtSeparators, StringSplitOptions.None).ToList();");
            mCsScriptSb.AppendLine("                var entity = CreateEntity(entityListText);");
            mCsScriptSb.AppendLine("                result.Add(entity);");
            mCsScriptSb.AppendLine("            }");
            mCsScriptSb.AppendLine("            return result;");
            mCsScriptSb.AppendLine("        }");
            mCsScriptSb.AppendLine();
        }

        private void AppendCsharpToTxtMethodCode()
        {
            mCsScriptSb.AppendCsharpNote("将本地数据对象转换为txt源数据字符串", null, null, "        ");
            mCsScriptSb.AppendLine("        public string ToTxt()");
            mCsScriptSb.AppendLine("        {");
            mCsScriptSb.AppendLine("            string entityStr = string.Empty;");

            mFieldInfos.ForEach(info =>
            {
                mCsScriptSb.AppendLine(string.Format("            entityStr = entityStr + {0} + ", info.Name) + "\"" + mTxtSeparators + "\"" + ";");
            });

            mCsScriptSb.AppendLine("            entityStr = entityStr.Remove(entityStr.Length - 1);");
            mCsScriptSb.AppendLine("            return entityStr;");
            mCsScriptSb.AppendLine("        }");
            mCsScriptSb.AppendLine();
        }

        private void AppendCsharpDeepCopyMethodCode()
        {
            mCsScriptSb.AppendCsharpNote("获得本地数据的一份深度复制副本", null, null, "        ");
            mCsScriptSb.AppendLine(string.Format("        public {0} DeepCopy()", mClassName));
            mCsScriptSb.AppendLine("        {");
            mCsScriptSb.AppendLine("            var buff = SerializeUitlity.Serialize(this);");
            mCsScriptSb.AppendLine(string.Format("            var entity = SerializeUitlity.DeSerialize<{0}>(buff);",
                mClassName));
            mCsScriptSb.AppendLine("            return entity;");
            mCsScriptSb.AppendLine("        }");
            mCsScriptSb.AppendLine("    }");
        }

        #endregion

        #region TypeScript

        public void ExportTypeScriptScript()
        {
            mTsScriptSb.AppendTypeScriptFileNode(EditorConstant.HostClientName, EditorConstant.HostClientEmail,
                "Typescript本地数据表类型");

            mTsScriptSb.AppendLine("namespace Iuker_Project " + "{");
            mTsScriptSb.AppendLine();
            mTsScriptSb.AppendLine(string.Format("    export class {0} implements ILocalDataEntity<{1}> ", mClassName,
                                       mClassName) + "{");
            mTsScriptSb.AppendLine();

            AppendFiledsCode();
            AppendCreateEntityMethodCode();
            AppendCreateEntitysMethodCode();

            mTsScriptSb.AppendLine("    }");
            mTsScriptSb.AppendLine("}");
            mTsScriptSb.AppendLine();
            var path = mSon.TsProjectLocalDataDir + mClassName.ToLower() + ".ts";
            FileUtility.WriteAllText(path, mTsScriptSb.ToString());
            TsProj.AddLine("LocalData" + "\\" + mClassName.ToLower(), mSon).UpdateToFile(mSon.TsProjPath);
        }

        private void AppendFiledsCode()
        {
            mFieldInfos.ForEach(info =>
            {
                mTsScriptSb.AppendLine("        /**");
                mTsScriptSb.AppendLine(string.Format("        *{0}", info.Comment));
                mTsScriptSb.AppendLine("        */");
                mTsScriptSb.AppendLine(string.Format("        public {0}: {1};", info.Name, info.TypeScriptType));
                mTsScriptSb.AppendLine();
            });
        }

        private void AppendCreateEntityMethodCode()
        {
            mTsScriptSb.AppendLine(string.Format("        public CreateEntity(row: string[]): {0} ", mClassName) + "{");
            mTsScriptSb.AppendLine();
            mTsScriptSb.AppendLine(string.Format("            let entity = new {0}();", mClassName));

            mFieldInfos.ForEach(info =>
            {
                mTsScriptSb.AppendLine(string.Format("            entity.{0} = {1}", info.Name,
                    info.TypeScriptEntityStr));
            });

            mTsScriptSb.AppendLine("            return entity;");
            mTsScriptSb.AppendLine("        }");
            mTsScriptSb.AppendLine();
        }

        private void AppendCreateEntitysMethodCode()
        {
            mTsScriptSb.AppendLine(string.Format("        public CreateEntitys(rows: string[]): {0}[] ", mClassName) + "{");
            mTsScriptSb.AppendLine();
            mTsScriptSb.AppendLine(string.Format("            let tables: {0}[] = [];", mClassName));
            mTsScriptSb.AppendLine("            for (let i = 0; i < rows.length; i++) {");
            mTsScriptSb.AppendLine();
            mTsScriptSb.AppendLine("                let row = rows[i];");
            mTsScriptSb.AppendLine(string.Format("                let entityStrArray = row.split('{0}');",
                mTxtSeparators));
            mTsScriptSb.AppendLine("                let entity = this.CreateEntity(entityStrArray);");
            mTsScriptSb.AppendLine("                tables.push(entity);");
            mTsScriptSb.AppendLine("            }");
            mTsScriptSb.AppendLine();
            mTsScriptSb.AppendLine("            return tables;");
            mTsScriptSb.AppendLine("        }");
        }

        #endregion

        #region Lua



        #endregion

        #region Txt

        public void ExportTxts()
        {
            foreach (DataTable t in mTables)
            {
                ExportTxt(t);
            }
        }

        private void ExportTxt(DataTable table)
        {
            for (var i = 2; i < table.Rows.Count; i++)
            {
                var line = string.Empty;
                for (var j = 0; j < table.Columns.Count; j++)
                {
                    var str = table.Rows[i][j].ToString();
                    if (str.IsNullOrEmpty())
                        throw new Exception(string.Format("数据表工作簿{0}的{1}行{2}列位置发现空白单元格！", table.TableName, i, j));

                    str.CleanBr();
                    line = j < table.Columns.Count - 1 ? line + str + mTxtSeparators : line + str;
                }

                if (i < table.Rows.Count - 1)
                {
                    mTxtSb.AppendLine(line);
                }
                else
                {
                    mTxtSb.Append(line);
                }
            }

            var path = mSon.LocalDataTxtDir + mSon.CompexName.ToLower() + "_" + table.TableName.ToLower() + ".txt";
            FileUtility.WriteAllText(path, mTxtSb.ToString());
        }

        #endregion

        #region 二进制





        #endregion

        #region 内部类（Excel字段信息）

        private class ExcelFieldInfo
        {
            public string Name;
            private string OriginType;
            public string Comment;

            public string TypeScriptType
            {
                get
                {
                    switch (OriginType)
                    {
                        case "int":
                            return "number";
                        case "string":
                            return "string";
                        case "List<int>":
                            return "int[]";
                        case "List<float>":
                            return "number[]";
                        case "bool":
                            return "boolean";
                        case "float":
                            return "number";
                        case "List<string>":
                            return "string[]";
                        default:
                            return "";
                    }
                }
            }

            public string CsharpOriginType { get { return OriginType; } }
            public string LuaType;
            private int Index;

            public string CsharpEntityStr
            {
                get
                {
                    switch (OriginType)
                    {
                        case "int":
                            return string.Format("entity.{0} = Convert.ToInt32(row[{1}]);", Name, Index);
                        case "string":
                            return string.Format("entity.{0} = row[{1}];", Name, Index);
                        case "List<int>":
                            return string.Format("entity.{0} = row[{1}].ToListInt();", Name, Index);
                        case "List<float>":
                            return string.Format("entity.{0} = row[{1}.ToListFloat(););", Name, Index);
                        case "bool":
                            return string.Format("entity.{0} = Convert.ToBoolean(row[{1}]);", Name, Index);
                        case "float":
                            return string.Format("entity.{0} = float.Parse(row[{1}]);", Name, Index);
                        case "List<string>":
                            return string.Format("entity.{0} = row[{1}].Split(',').ToList();", Name, Index);
                        default:
                            return "";
                    }
                }
            }

            public string TypeScriptEntityStr
            {
                get
                {
                    switch (OriginType)
                    {
                        case "int":
                            return string.Format("entity.{0} = parseInt(row[{1}]);", Name, Index);
                        case "string":
                            return string.Format("entity.{0} = row[{1}];", Name, Index);
                        case "List<int>":
                            return string.Format("entity.{0} = parseListInt(row[{1}]);", Name, Index);
                        case "List<float>":
                            return string.Format("entity.{0} = parseListFloat(row[{1}]);", Name, Index);
                        case "bool":
                            return string.Format("entity.{0} = parseBoolean(row[{1}]);", Name, Index);
                        case "float":
                            return string.Format("entity.{0} = parseFloat(row[{1}]);", Name, Index);
                        case "List<string>":
                            return string.Format("entity.{0} = row[{1}].split('_');", Name, Index);
                        default:
                            return "";
                    }
                }
            }

            public ExcelFieldInfo SetTypeAndName(string typeAndName)
            {
                var firstArray = typeAndName.Split('_');
                Name = firstArray[1];
                OriginType = firstArray[0];

                return this;
            }

            public ExcelFieldInfo SetComment(string comment)
            {
                Comment = comment;

                return this;
            }

            public ExcelFieldInfo SetIndex(int index)
            {
                Index = index;

                return this;
            }


        }


        #endregion


    }
}
