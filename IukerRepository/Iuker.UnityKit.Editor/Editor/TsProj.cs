using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Iuker.Common;
using Iuker.Common.Utility;
using Iuker.UnityKit.Run.Base.Config;
using Iuker.UnityKit.Run.Base.Config.Develop;
using UnityEditor;
using UnityEngine;

namespace Iuker.UnityKit.Run.Base
{
    /// <summary>
    /// Visual Studio环境下TypeScript项目文件
    /// 1. 用于支持在Unity下创建Ts相关的自动脚本模板时，自动将新建的模板加入到Ts项目中。
    /// 2. 同上，自动移除已在Unity中删除的Ts脚本。
    /// </summary>
    public class TsProj
    {
        private List<string> AllLines { get; set; }
        private int mLastLine;

        [MenuItem("Iuker/快捷菜单/Ts项目文件测试")]
        public static void TsProjTest()
        {
            var tsP = new TsProj();
            tsP.Init(RootConfig.GetCurrentSonProject());
        }

        private void Init(SonProject son)
        {
            if (son == null) return;
            var existTsFiles = FileUtility.GetFilePaths(son.TsProjectDir, s => !s.Contains("meta")).FilePaths;
            existTsFiles = existTsFiles.OrderBy(n => n).ToList();

            var path = son.TsProjPath;
            AllLines = File.ReadAllLines(path).ToList();

            if (existTsFiles.Count > 0)
            {
                // 查找已有的最后一行
                var lastScriptName = existTsFiles.Last().FileName();
                mLastLine = AllLines.FindLastIndex(r => r.Contains(lastScriptName));
            }

            if (existTsFiles.Count != 0 && mLastLine != -1) return;

            for (var index = 0; index < AllLines.Count; index++)
            {
                var line = AllLines[index];
                if (line.Contains("<TypeScriptCompile Include"))
                {
                    mLastLine = index;
                }
            }
        }

        private string CreateNewLineStr(string path, string parentDir = "Project")
        {
            var result = "    <TypeScriptCompile Include=" + "\"" + parentDir + "\\" + string.Format("{0}.ts", path) + "\"" + " />";
            return result;
        }

        public static TsProj AddLine(string name, SonProject son, string parentDir = "Project")
        {
            son = son ?? RootConfig.GetCurrentSonProject();
            var tsP = new TsProj();
            tsP.Init(son);
            tsP.Instert(name, parentDir);

            return tsP;
        }

        public static TsProj AddLines(List<string> names, SonProject son, string parentDir = "Project")
        {
            son = son ?? RootConfig.GetCurrentSonProject();
            var tsP = new TsProj();
            tsP.Init(son);

            foreach (var name in names)
            {
                tsP.Instert(name, parentDir);
            }

            return tsP;
        }

        private void Instert(string name, string parentDir)
        {
            if (name == "") return;

            var newLine = CreateNewLineStr(name, parentDir);
            if (AllLines.Exists(l => l == newLine))
            {
                Debug.Log(string.Format("目标Typescript脚本{0}在目标Ts项目中已存在，添加操作取消！", name));
                return;
            }

            AllLines.Insert(mLastLine + 1, newLine);
            mLastLine++;
        }

        public void UpdateToFile(string path)
        {
            var sb = new StringBuilder();
            foreach (var line in AllLines)
            {
                sb.AppendLine(line);
            }

            var content = sb.ToString();
            File.WriteAllText(path, content);
        }







    }
}