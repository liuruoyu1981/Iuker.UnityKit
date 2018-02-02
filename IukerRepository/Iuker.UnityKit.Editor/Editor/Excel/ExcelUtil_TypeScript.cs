/***********************************************************************************************
/*  Author：        liuruoyu1981
/*  CreateDate:     2017/11/27 上午 06:37:57 
/*  Email:          35490136@qq.com
/*  QQCode:         35490136
/*	Machine:		DESKTOP-M1OBR70
/*  CreateNote: 
***********************************************************************************************/


/****************************************修改日志***********************************************
/* 	1. 修改日期： 修改人： 修改内容：
/* 	2. 修改日期： 修改人： 修改内容：
/* 	3. 修改日期： 修改人： 修改内容：
/* 	4. 修改日期： 修改人： 修改内容：
/* 	5. 修改日期： 修改人： 修改内容：
****************************************修改日志***********************************************/


using System.Data;
using System.Text;
using Iuker.Common;

namespace Iuker.UnityKit.Editor.Excel
{
    public static partial class ExcelUtility
    {
        private static StringBuilder mSb = new StringBuilder();

        public static void ExportTypeScript(string excelPath, string typeScriptPath)
        {
            var dataSet = ConverExcelLocalData(excelPath);

            mSb.AppendTypeScriptFileNode(EditorConstant.HostClientName,
                       EditorConstant.HostClientEmail, "Typescript本地数据表类型（）");
        }

        private static void AppendFileds()
        {
            foreach (var valuePair in mFiledDictionary)
            {
                mSb.Append(string.Format("{0}: {1};", valuePair.Key, GetTypeScriptDataTypeStr(valuePair.Value)));
            }
        }

        private static void AppendFiledNote(string note)
        {
            mSb.AppendLine("/**");
            mSb.AppendLine("*");
            mSb.AppendLine("*/");
        }

        private static string GetTypeScriptDataTypeStr(string sourceType)
        {
            return null;
        }


        private static void ParseATable(DataTable table)
        {
            var rows = table.Rows;
            for (var i = 2; i < maxRows; i++)
            {
                for (var j = 0; j < maxColumns; j++)
                {
                    var source = rows[i][j].ToString();
                    var columnName = fieldNameDictionary[j];
                    var type = filedTypeDictionary[j];


                }
            }
        }

        private static void GetTypeScriptLine(string type, string cn, string cc)
        {

        }









    }
}
