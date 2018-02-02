/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 7/21/2017 11:20:06 AM
Email: liuruoyu1981@gmail.com
***********************************************************************************************/

using System;
using System.Data;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Iuker.UnityKit.Editor.Excel
{
    public static partial class ExcelUtility
    {
        public static void ExportLocalDataLua(string ep, string lp)
        {
            DataSet dataSet = ConverExcelLocalData(ep);
            var lfp = lp + localDataClassName + ".lua";

            if (File.Exists(lfp)) File.Delete(lfp);

            try
            {
                using (StreamWriter sw = File.CreateText(lfp))
                {
                    DataRowCollection rows = dataSet.Tables[0].Rows;

                    // 写入字段注释
                    WriteLuaNote(sw);

                    sw.WriteLine(localDataClassName + " = ");
                    sw.WriteLine("  {");

                    for (int i = 2; i < maxRows; i++)
                    {
                        for (int j = 0; j < maxColumns; j++)
                        {
                            string targetWriteTxt = rows[i][j].ToString();
                            var columnName = fieldNameDictionary[j];
                            var type = filedTypeDictionary[j];

                            if (j == 0)
                            {
                                sw.Write("  [{0}] = {{", i - 2);
                            }
                            else if (j == maxColumns - 1)
                            {
                                var line = GetLuaLineTxt(type, columnName, targetWriteTxt);
                                line = line.Substring(0, line.Length - 1);
                                sw.WriteLine(line + "}" + ",");
                            }
                            else
                            {
                                sw.Write(GetLuaLineTxt(type, columnName, targetWriteTxt));
                            }
                        }
                    }
                    sw.WriteLine("  }");
                }
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
                Debug.LogError("本地数据lua脚本导出发生异常，excel表为：" + tableName);
            }
        }

        /// <summary>
        /// 在lua脚本中写入字段注释
        /// </summary>
        /// <param name="sw"></param>
        private static void WriteLuaNote(StreamWriter sw)
        {
            for (int i = 0; i < membersNoteList.Count; i++)
            {
                var note = membersNoteList[i];
                var n = fieldNameDictionary[i];
                sw.WriteLine("-- {0}__{1}", n, note);
            }
            sw.WriteLine();
            sw.WriteLine();
        }

        /// <summary>
        /// 获得lua数据行输出文本
        /// </summary>
        /// <param name="type"></param>
        /// <param name="cn"></param>
        /// <param name="cc"></param>
        /// <returns></returns>
        private static string GetLuaLineTxt(string type, string cn, string cc)
        {
            var lualine = string.Empty;

            switch (type)
            {
                case "int":
                    lualine = string.Format("{0} = {1},", cn, cc);
                    break;
                case "string":
                    lualine = string.Format("{0} = {1}{2}{1},", cn, "\"", cc);
                    break;
                case "List<int>":
                    lualine = string.Format("{0} = {1},", cn, GetLuaLine_Listint(cc));
                    break;
                case "bool":
                    lualine = string.Format("{0} = {1},", cn, cc);
                    break;
                case "float":
                    lualine = string.Format("{0} = {1},", cn, cc);
                    break;
            }

            return lualine;
        }

        private static string GetLuaLine_Listint(string cc)
        {
            var arr = cc.Split(',');
            var line = arr.Aggregate(string.Empty, (current, s) => current + s + ",");

            line = line.Substring(0, line.Length - 1);
            line = "{ " + line + "}";
            return line;
        }

    }
}
