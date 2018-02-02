/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/02/17 12:58:09
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

using System.IO;
using Iuker.UnityKit.Editor.Excel;
using Iuker.UnityKit.Run.Base.Config;
using Iuker.UnityKit.Run.Base.Config.Develop;
using UnityEditor;
using UnityEngine;


namespace Iuker.UnityKit.Editor
{
    /// <summary>
    /// 本地数据编辑器插件
    /// </summary>
    public class LocalDataEditor
    {
        private static SonProject _son;
        private static string _excelDir;
        private static string _csScriptDir;
        private static string _tsScriptDir;

        [MenuItem("Iuker/本地数据/创建本地数据C#脚本", false, 2)]
        public static void CreateLdList()
        {
            if (!Directory.Exists(_excelDir)) Directory.CreateDirectory(_excelDir);
            if (!Directory.Exists(_csScriptDir)) Directory.CreateDirectory(_csScriptDir);
            AssetDatabase.Refresh();

            var targetExcelPath = EditorUtility.OpenFilePanel("Please select target excel file!", _excelDir, "xlsx");
            if (string.IsNullOrEmpty(targetExcelPath))
            {
                EditorUtility.DisplayDialog("Error", "Don't select excel file!", "Ok");
                return;
            }
            _csScriptDir = EditorUtility.OpenFolderPanel("Please select out dir!", _csScriptDir, "");
            if (string.IsNullOrEmpty(_csScriptDir))
            {
                EditorUtility.DisplayDialog("Error", "Don't select out dir!", "Ok");
                return;
            }
            if (!_csScriptDir.EndsWith("/")) _csScriptDir += "/";

            ExcelUtility.ConverExcelLocalData(targetExcelPath);
            var scriptPath = ExcelUtility.ExportLocalDataClass(_csScriptDir);

            AssetDatabase.Refresh();

            // 打开新创建的数据实体脚本文件
            System.Diagnostics.Process.Start(scriptPath);
        }

        [MenuItem("Iuker/本地数据/创建本地数据TXT数据源", false, 2)]
        public static void CreateLdTxt()
        {
            var sonProject = RootConfig.GetCurrentSonProject();
            var excelFileDir = sonProject.ExcelDir;
            if (!Directory.Exists(excelFileDir))
            {
                excelFileDir = Application.dataPath + "/";
            }

            var txtOutDir = sonProject.LocalDataTxtDir;
            if (!Directory.Exists(txtOutDir))
            {
                txtOutDir = Application.dataPath + "/";
            }

            if (!Directory.Exists(excelFileDir)) Directory.CreateDirectory(excelFileDir);
            if (!Directory.Exists(txtOutDir)) Directory.CreateDirectory(txtOutDir);
            AssetDatabase.Refresh();

            var targetExcelPath = EditorUtility.OpenFilePanel("Please select target excel file!", excelFileDir, "xlsx");
            if (string.IsNullOrEmpty(targetExcelPath))
            {
                EditorUtility.DisplayDialog("Error", "Don't select excel file!", "Ok");
                return;
            }
            txtOutDir = EditorUtility.OpenFolderPanel("Please select out dir!", txtOutDir, "");
            if (string.IsNullOrEmpty(txtOutDir))
            {
                EditorUtility.DisplayDialog("Error", "Don't select out dir!", "Ok");
                return;
            }
            if (!txtOutDir.EndsWith("/")) txtOutDir += "/";

            ExcelUtility.ExportLocalDataTxt(targetExcelPath, txtOutDir);

            //刷新编辑器
            AssetDatabase.Refresh();
        }

        //[MenuItem("Iuker/本地数据/创建本地数据Lua数据源", false, 2)]
        //public static void CreateLdLua()
        //{
        //    var sonProject = RootConfig.GetCurrentSonProject();
        //    var excelFileDir = sonProject.ExcelDir;
        //    var outPath = sonProject.DevelopAssetBundleLuaLocalDataDir;
        //    FileUtility.TryCreateDirectory(excelFileDir);
        //    FileUtility.TryCreateDirectory(outPath);
        //    AssetDatabase.Refresh();

        //    var targetExcelPath = EditorUtility.OpenFilePanel("Please select target excel file!", excelFileDir, "xlsx");
        //    if (string.IsNullOrEmpty(targetExcelPath))
        //    {
        //        EditorUtility.DisplayDialog("Error", "Don't select excel file!", "Ok");
        //        return;
        //    }
        //    outPath = EditorUtility.OpenFolderPanel("Please select out dir!", outPath, "");
        //    if (string.IsNullOrEmpty(outPath))
        //    {
        //        EditorUtility.DisplayDialog("Error", "Don't select out dir!", "Ok");
        //        return;
        //    }
        //    if (!outPath.EndsWith("/")) outPath += "/";

        //    ExcelUtil.ExportLocalDataLua(targetExcelPath, outPath);
        //    //刷新编辑器
        //    AssetDatabase.Refresh();
        //}

    }
}
