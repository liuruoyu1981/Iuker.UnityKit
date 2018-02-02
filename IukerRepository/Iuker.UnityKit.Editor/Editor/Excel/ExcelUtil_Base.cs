/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 6/1/2017 15:14:24 PM
Email: liuruoyu1981@gmail.com
***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Excel;
using Iuker.Common.Constant;
using UnityEngine;

namespace Iuker.UnityKit.Editor.Excel
{
    /// <summary>
    /// Excel文件扩展工具
    /// 1. 导出C#数据实体脚本
    /// 2. 导出txt数据源
    /// 3. 导出lua数据源（lua脚本文件）
    /// 4. 导出Json数据源（Json文件）
    /// </summary>
    public static partial class ExcelUtility
    {
        /// <summary>
        /// CN：最大行数
        /// EN：max lines
        /// </summary>
        private static int maxRows;

        /// <summary>
        /// CN：最大列数
        /// EN：max columns
        /// </summary>
        private static int maxColumns;

        /// <summary>
        /// CN：excel表名
        /// EN：excel table n
        /// </summary>
        private static string tableName;

        /// <summary>
        /// CN：列成员类型字典
        /// EN：Members of the column t in the dictionary
        /// </summary>
        private static Dictionary<int, string> filedTypeDictionary = new Dictionary<int, string>();

        /// <summary>
        /// CN：列成员名字字典
        /// EN：Column members names in the dictionary
        /// </summary>
        private static Dictionary<int, string> fieldNameDictionary = new Dictionary<int, string>();

        private static Dictionary<string, string> mFiledDictionary = new Dictionary<string, string>();

        /// <summary>
        /// CN：txt分隔符
        /// EN：txt separator
        /// </summary>
        private static readonly string TxtSeparators = Constant.TxtSeparators[0];

        /// <summary>
        /// CN：成员注释集合
        /// EN：Members of the annotation collection
        /// </summary>
        private static List<string> membersNoteList = new List<string>();

        /// <summary>
        /// CN：本地数据类型表对应的实体类的类名、文件名
        /// EN：Excel table corresponding to the class TimerName of the entity class, and the file TimerName
        /// </summary>
        private static string localDataClassName;

        /// <summary>
        /// 解析第一行，约定为类型
        /// </summary>
        /// <param name="dataSet"></param>
        private static void ParseFirstRow(DataSet dataSet)
        {
            filedTypeDictionary.Clear();
            fieldNameDictionary.Clear();
            var firstRow = dataSet.Tables[0].Rows[0];
            for (var i = 0; i < maxColumns; i++)
            {
                string value;
                if (firstRow[i] is string)
                {
                    value = firstRow[i].ToString();
                }
                else
                {
                    continue;
                }

                var typeArr = value.Split('_');
                filedTypeDictionary.Add(i, typeArr[0]);
                fieldNameDictionary.Add(i, typeArr[1]);
            }
        }


        /// <summary>
        /// 解析第二行，约定为注释行
        /// </summary>
        /// <param name="dataSet"></param>
        private static void ParseSecondRow(DataSet dataSet)
        {
            membersNoteList.Clear();
            var secondRow = dataSet.Tables[0].Rows[1];
            for (var i = 0; i < maxColumns; i++)
            {
                var value = secondRow[i] as string;
                membersNoteList.Add(value);
            }
        }

        /// <summary>
        /// CN：转换一个本地数据类型的excel表文件，返回转换后的DataSet对象
        /// EN：Convert a excel file, returns the transformed DataSet object
        /// </summary>
        /// <returns></returns>
        public static DataSet ConverExcelLocalData(string efp)
        {
            filedTypeDictionary = new Dictionary<int, string>();
            fieldNameDictionary = new Dictionary<int, string>();
            membersNoteList = new List<string>();
            SetLocalDataClassNameAndTxtFileName(efp);
            DataSet dataSet = null;

            try
            {
                using (var fileStream = File.Open(efp, FileMode.Open, FileAccess.ReadWrite))
                {
                    var excelDataReader = ExcelReaderFactory.CreateOpenXmlReader(fileStream);
                    dataSet = excelDataReader.AsDataSet();
                    tableName = dataSet.Tables[0].TableName;

                    var table = dataSet.Tables[0];
                    maxRows = table.Rows.Count;
                    maxColumns = table.Columns.Count;
                    ParseFirstRow(dataSet);
                    ParseSecondRow(dataSet);
                }
            }
            catch (DirectoryNotFoundException exception)
            {
                Debug.LogWarning(exception.Message);
            }

            return dataSet;
        }

        /// <summary>
        /// 导出本地数据脚本文件并返回导出的脚本全路径
        /// </summary>
        /// <param name="path">导出的本地数据脚本全路径</param>
        /// <returns></returns>
        public static string ExportLocalDataClass(string path = null)
        {
            var classPath = path == null ? string.Format(Application.dataPath + "/Scripts/LocalData/" + localDataClassName + ".cs") : string.Format(path + localDataClassName + ".cs");
            if (File.Exists(classPath)) File.Delete(classPath);

            try
            {
                using (var streamWriter = File.CreateText(classPath))
                {
                    CreateLocalDataClass(streamWriter);
                }
            }
            catch (Exception exception)
            {
                Debug.LogError(string.Format("本地数据类型表脚本类文件导出发生异常，异常信息为：{0}", exception.Message));
            }
            Debug.Log(string.Format("Excel class file export succeed，export path is {0}", classPath));
            return classPath;
        }

        /// <summary>
        /// 设置本地数据类型表对应的类名和txt数据源文件名
        /// </summary>
        /// <param n="excelPath"></param>
        /// <param name="excelPath"></param>
        /// <returns></returns>
        private static void SetLocalDataClassNameAndTxtFileName(string excelPath)
        {
            string[] txtNameArr = excelPath.Split('/');
            string name = txtNameArr[txtNameArr.Length - 1].Split('.')[0];
            localDataClassName = "LD_" + name + "Table";
        }







    }
}
