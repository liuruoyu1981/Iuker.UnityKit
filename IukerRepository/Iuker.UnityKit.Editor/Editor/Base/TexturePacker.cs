/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/08/12 21:55:44
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
using System.IO;
using System.Linq;
using Iuker.Common.Utility;
using Iuker.UnityKit.Editor.Setting;
using UnityEngine;

namespace Iuker.UnityKit.Editor.AtlasBuild
{
    /// <summary>
    /// 基于第三方图集打包工具Texturepacker的自动化脚本类
    /// </summary>
    public class TexturePacker
    {
        /// <summary>
        /// 打包图集所允许的最大尺寸
        /// </summary>
        public string MaxSize = "2048";

        /// <summary>
        /// 精灵之间的间隔
        /// </summary>
        public string ShapePadding = "2";

        /// <summary>
        /// 精灵散图所在的源目录
        /// </summary>
        public string SourceDir { get; private set; }

        private const string TempSourceDir = "C:/AtlasOrigin/";
        private const string TempOutPutDir = "C:/AtlasOutPut/";

        /// <summary>
        /// 将要输出的目标目录
        /// </summary>
        public string OUtPutDir { get; private set; }

        /// <summary>
        /// 图集Png文件路径
        /// </summary>
        public string OutPutPngPath
        {
            get
            {
                return string.Format("{0}atlas_{1}_{2}.png", TempOutPutDir, sonProjectCompextName, AtlasClass);
            }
        }

        /// <summary>
        /// 图集精灵表规格描述数据文件路径
        /// </summary>
        public string OutPutTpsheetPath
        {
            get
            {
                return string.Format("{0}atlas_{1}_{2}.tpsheet", TempOutPutDir, sonProjectCompextName, AtlasClass);
            }
        }

        public string BorderPadding  = "0";

        public bool IsDisableRotation  = true;

        public string Opt  = "RGBA8888";

        private string GetCdTexturePackerExeDirCmd
        {
            get
            {
                return string.Format("cd {0}/1_Iuker.UnityKit/.Tools/TexturePacker/bin/", Application.dataPath);
            }
        }


        private static string RegPath
        {
            get
            {
                return Application.dataPath +
                       "/1_Iuker.UnityKit/.Tools/TexturePacker/key.reg";
            }
        }

        private string sonProjectCompextName
        {
            get
            {
                var dirs = OUtPutDir.Split('/');
                var splitStr = dirs[dirs.Length - 2].Split('_');
                var result = splitStr[2] + "_" + splitStr[3];
                result = result.ToLower();
                return result;
            }
        }

        /// <summary>
        /// 图集类型
        /// 以目录名为准
        /// 例如：Atlas_Common，这里Common是图集类型
        /// </summary>
        private string AtlasClass
        {
            get
            {
                var dirs = SourceDir.Split('/');
                var result = dirs[dirs.Length - 2].Split('_').Last();
                return result.ToLower();
            }
        }

        /// <summary>
        /// 打包图集输出图集文件
        /// </summary>
        public void BuildAtlas()
        {
            // 添加破解信息到注册表
            if (!IukerEditorPrefs.GetBool(EditorConstant.TexturePakcerReg))
            {
                CmdUtility.ExecuteReg(RegPath);
                IukerEditorPrefs.SetBool(EditorConstant.TexturePakcerReg, true);
            }

            var switchDiskCmd = Application.dataPath.Split('/').First(); //  切换到当前Unity3D项目所在盘符的Dos命令字符串
            var cdTextureExeCmd = GetCdTexturePackerExeDirCmd;
            var buildCmd = GetBuildAtlasCmd();

            CmdUtility.ExcuteDosCommand
                (
                    Debug.Log,
                    switchDiskCmd,
                    cdTextureExeCmd,
                    buildCmd
                );

            //  从临时目录拷贝回使用目录
            FileUtility.CopyDirectory(TempOutPutDir, OUtPutDir, true);
            FileUtility.DeleteDirectory(TempSourceDir);
            FileUtility.DeleteDirectory(TempOutPutDir);
        }

        /// <summary>
        /// 批量重命名
        /// </summary>
        public TexturePacker BatchRename()
        {
            var allSpritePath = FileUtility.GetFilePaths(SourceDir, s => !s.Contains("meta")).FilePaths;
            allSpritePath = allSpritePath.Where(p => p.EndsWith(".png") || p.EndsWith(".jpg")).ToList();

            for (int i = 0; i < allSpritePath.Count; i++)
            {
                var spritePath = allSpritePath[i];
                var suffix = spritePath.Split('.').Last();
                var cutSuffix = spritePath.Substring(0, spritePath.LastIndexOf('.'));
                var cutFileName = cutSuffix.Substring(0, cutSuffix.LastIndexOf('/'));
                var newPath = string.Format("{0}/{1}_{2}.{3}", cutFileName, AtlasClass, i, suffix);
                File.Move(spritePath, newPath);
            }

            return this;
        }

        /// <summary>
        /// 删除掉重名的散图
        /// </summary>
        public TexturePacker DeleteSameName()
        {
            var paths = GetSourceSpritesList();
            Dictionary<string, string> tempDirectory = new Dictionary<string, string>();
            foreach (var path in paths)
            {
                var filename = path.Split('/').Last().Split('.').First();
                if (tempDirectory.ContainsKey(filename))
                {
                    Debug.Log(string.Format("发现有重名资源，资源名为{0},将进行删除操作！", filename));
                    File.Delete(path);
                    continue;
                }
                tempDirectory.Add(filename, path);
            }
            return this;
        }

        public static TexturePacker Create(string sourceDir, string outPutDir)
        {
            var tex = new TexturePacker
            {
                SourceDir = sourceDir,
                OUtPutDir = outPutDir
            };

            //  删除上次打包图集的可能遗留目录
            FileUtility.DeleteDirectory(TempSourceDir);
            FileUtility.DeleteDirectory(TempOutPutDir);
            //  拷贝散图到临时目录
            FileUtility.CopyDirectory(sourceDir, TempSourceDir);

            return tex;
        }

        #region 链式
        public TexturePacker SetShapePadding(string value)
        {
            ShapePadding = value;
            return this;
        }

        public TexturePacker SetMaxSize(string value)
        {
            MaxSize = value;
            return this;
        }

        public TexturePacker SetBorderPadding(string value)
        {
            BorderPadding = value;
            return this;
        }

        public TexturePacker SetDisableRotation(bool value)
        {
            IsDisableRotation = value;
            return this;
        }

        public TexturePacker SetOutPutFormat(string value)
        {
            Opt = value;
            return this;
        }

        #endregion

        /// <summary>
        /// 获得源目录下所有图片列表
        /// </summary>
        private List<string> GetSourceSpritesList()
        {
            var paths = FileUtility.GetFilePaths(TempSourceDir, s => s.EndsWith(".png") || s.EndsWith(".jpg")).FilePaths;
            return paths;
        }

        private string GetSourceSpritesStr()
        {
            var paths = GetSourceSpritesList();
            string cmd = paths.Aggregate(string.Empty, (current, path) => current + " " + path);
            return cmd;
        }

        private string GetBuildAtlasCmd()
        {
            var spritesCmd = GetSourceSpritesStr();
            var buildCmd = string.Format(
                "TexturePacker {0} --max-size {1} --trim --allow-free-size --format unity-texture2d --shape-padding {2} --border-padding {3} --disable-rotation --algorithm MaxRects --opt {4} --scale 1 --sheet {5} --data {6}",
                spritesCmd, MaxSize, ShapePadding, BorderPadding, Opt, OutPutPngPath, OutPutTpsheetPath);

            Debug.Log("图集输出路径：" + OutPutPngPath);
            Debug.Log("图集数据输出路径：" + OutPutTpsheetPath);
            return buildCmd;
        }

    }
}