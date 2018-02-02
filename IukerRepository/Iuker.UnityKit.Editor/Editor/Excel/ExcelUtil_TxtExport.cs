/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 6/1/2017 15:31:50 PM
Email: liuruoyu1981@gmail.com
***********************************************************************************************/

using System.Data;
using System.IO;
using Iuker.UnityKit.Run.Base.Config.Develop;
using UnityEngine;

namespace Iuker.UnityKit.Editor.Excel
{
    public static partial class ExcelUtility
    {
        /// <summary>
        /// 导出本地数据类型excel表的txt数据源文件
        /// </summary>
        /// <param name="excelPath">excel文件路径</param>
        /// <param name="txtOutDir"></param>
        /// <param name="specifName">指定的文件名</param>
        public static void ExportLocalDataTxt(string excelPath, string txtOutDir, string specifName = null)
        {
            var dataSet = ConverExcelLocalData(excelPath);
            CreateLocalDataTxt(dataSet.Tables, txtOutDir, specifName);
        }

        private static void CreateLocalDataTxt(DataTableCollection tables, string txtOutDir = null, string specifName = null)
        {
            var targetDir = RootConfig.GetCurrentSonProject().LocalDataTxtDir;
            if (!Directory.Exists(targetDir))
            {
                Directory.CreateDirectory(targetDir);
            }

            foreach (DataTable table in tables)
            {
                string txtOutPath;
                if (txtOutDir == null)
                {
                    txtOutPath = targetDir + "ld_" + table.TableName.ToLower() + ".txt";
                }
                else
                {
                    if (specifName == null)
                    {
                        txtOutPath = txtOutDir + "ld_" + table.TableName.ToLower() + ".txt";
                    }
                    else
                    {
                        txtOutPath = txtOutDir + specifName.ToLower() + ".txt";
                    }
                }

                if (File.Exists(txtOutPath))
                {
                    File.Delete(txtOutPath);
                }

                using (var streamWriter = File.CreateText(txtOutPath))
                {
                    var rows = table.Rows;
                    for (var i = 2; i < maxRows; i++)
                    {
                        string line = null;
                        for (var j = 0; j < maxColumns; j++)
                        {
                            var targetTxt = rows[i][j].ToString();
                            if (string.IsNullOrEmpty(targetTxt))
                            {
                                targetTxt = "-9999";          // 为空，说明这个excel单元格没有填写任何数据，则默认写入-9999表示无数据
                            }
                            targetTxt = targetTxt.Replace("\n", "");    //  去除换行符(protobuf协议字段描述)
                            targetTxt = targetTxt.Replace("\r", "");    //  去除换行符(protobuf协议字段描述)
                            if (j < maxColumns - 1)
                            {
                                // CN：如果不是最后一项，则写入项内容加分隔符 EN：If not the last item, write the item and separator
                                line = line + targetTxt + TxtSeparators;
                            }
                            if (j == maxColumns - 1)
                            {
                                line = line + targetTxt;     // CN：如果是最后一项，则仅仅写入项内容   EN：If the last item, just write the item
                            }
                        }

                        if (i < maxRows - 1) streamWriter.WriteLine(line);       // 判断是否是最后一行，如果是则不写入换行
                        else streamWriter.Write(line);
                    }
                }
                Debug.Log(string.Format("Excel txt file export succeed，export path is {0}", txtOutPath));
            }
        }






    }
}
