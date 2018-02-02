using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Iuker.Common.Base;

namespace Iuker.Common.Utility
{
#if DEBUG
    /// <summary>
    /// IO工具
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20170902 17:45:17")]
    [ClassPurposeDesc("IO工具", "")]
#endif
    public static class FileUtility
    {
        /// <summary>
        /// 文件及目录信息查找结果
        /// </summary>
        public class FileFindResult
        {
            /// <summary>
            /// 所有目录路径列表
            /// </summary>
            public List<string> Dirs { get; set; }

            /// <summary>
            /// 所有文件路径列表
            /// </summary>
            public List<string> FilePaths { get; set; }

            /// <summary>
            /// 所有文件名和文件路径字典
            /// </summary>
            public Dictionary<string, string> FilePathDictionary { get; set; }

            /// <summary>
            /// 目标根路径
            /// </summary>
            public string TargetDir { get; private set; }

            public FileFindResult(string targetDir)
            {
                Dirs = new List<string>();
                FilePaths = new List<string>();
                FilePathDictionary = new Dictionary<string, string>();
                TargetDir = targetDir;
            }
        }

        /// <summary>
        /// 获得指定目录下所有子目录及指定目录
        /// </summary>
        /// <param name="dir">指定目标目录</param>
        /// <param name="dirSelecter">目录选择器</param>
        /// <returns>目录列表</returns>
        public static FileFindResult GetAllDir(string dir, Func<string, bool> dirSelecter = null)
        {
            var findResult = new FileFindResult(dir);
            GetDir(findResult, dir);
            if (dirSelecter != null)
            {
                findResult.Dirs = findResult.Dirs.Where(dirSelecter).ToList();
            }
            return findResult;
        }

        private static FileFindResult GetDir(FileFindResult findResult, string dir)
        {
            if (!findResult.Dirs.Contains(dir)) findResult.Dirs.Add(dir);
            if (!Directory.Exists(dir)) return null;

            if (Directory.GetDirectories(dir).Length <= 0) return findResult;
            var sonDirs = Directory.GetDirectories(dir);
            foreach (var sonDir in sonDirs)
            {
                var tp = string.Empty;
                if (!sonDir.EndsWith("/"))
                {
                    tp = sonDir + "/";
                }
                tp = tp.Replace("\\", "/");
                findResult.Dirs.Add(tp);
                GetDir(findResult, tp);
            }

            return findResult;
        }

        /// <summary>
        /// 获得指定目录及所有子目录下所有文件路径
        /// </summary>
        /// <param name="dir">指定目录</param>
        /// <param name="fileSelecter">文件路径选择器</param>
        /// <param name="dirSelecter">目录路径选择器/param>
        /// <returns>返回文件路径列表</returns>
        public static FileFindResult GetFilePaths(string dir, Func<string, bool> fileSelecter = null, Func<string, bool> dirSelecter = null)
        {
            var info = GetAllDir(dir, dirSelecter);

            foreach (var d in info.Dirs)
            {
                var tp = Directory.GetFiles(d);
                var tm = tp.Select(s => s.Replace("\\", "/")).ToList();
                info.FilePaths.AddRange(tm);
            }

            if (fileSelecter != null)
            {
                info.FilePaths = info.FilePaths.Where(fileSelecter).ToList();
            }
            return info;
        }

        /// <summary>
        /// 获得指定目录下及其所有子目录中所有文件的文件名及文件路径字典
        /// </summary>
        /// <param name="dir">指定目录</param>
        /// <param name="filter">文件过滤器</param>
        /// <param name="dirFilter">目录过滤器</param>
        /// <returns></returns>
        public static FileFindResult GetFilePathDictionary(string dir, Func<string, bool> filter = null, Func<string, bool> dirFilter = null)
        {
            var info = GetFilePaths(dir, filter, dirFilter);
            foreach (var path in info.FilePaths)
            {
                var fileName = path.Split('/').Last().Split('.').First();
                if (info.FilePathDictionary.ContainsKey(fileName))
                {
                    Debuger.LogError(string.Format("目标目录及其子目录下存在同名文件，文件名为：{0}", fileName));
                    continue;
                }
                info.FilePathDictionary.Add(fileName, path);
            }

            return info;
        }

        /// <summary>
        /// 创建一个二进制文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="buffer"></param>
        public static void CreateBinaryFile(string filePath, byte[] buffer)
        {
            EnsureDirExist(filePath);
            FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(buffer);
            bw.Close();
            fs.Close();
        }

        /// <summary>
        /// 获得指定路径或文件的上级路径或目录
        /// </summary>
        /// <param name="p">指定路径</param>
        private static string GetParnetPath(string p)
        {
            var fn = p.Split('/').Last();
            var parentP = p.Replace(fn, "");
            return parentP;
        }

        /// <summary>
        /// 将指定文件移动到目标目录中
        /// 如果目标目录不存在将会自动创建新目录
        /// </summary>
        /// <param name="sourceFile">源文件</param>
        /// <param name="targetDir">目标路径</param>
        public static void Move(string sourceFile, string targetDir)
        {
            var parentPath = GetParnetPath(targetDir);
            if (!Directory.Exists(parentPath))
            {
                Directory.CreateDirectory(parentPath);
            }
            File.Move(sourceFile, targetDir);
        }

        /// <summary>
        /// 尝试创建一个新目录（如果给定的目录路径不存在）
        /// </summary>
        public static bool TryCreateDirectory(string targetDir, bool isHide = false)
        {
            if (Directory.Exists(targetDir)) return true;

            Directory.CreateDirectory(targetDir);
            if (isHide)
            {
                File.SetAttributes(targetDir, FileAttributes.Hidden);
            }

            return false;
        }

        /// <summary>
        /// 将指定的文件拷贝到指定目录
        /// 如果目标路径上已有文件存在则删除已有的然后新建
        /// </summary>
        /// <param name="sourceFilePath"></param>
        /// <param name="targetDir"></param>
        public static void CopyFile(string sourceFilePath, string targetDir)
        {
            EnsureDirExist(targetDir);
            var filename = Path.GetFileName(sourceFilePath);
            if (!targetDir.EndsWith("/"))
            {
                targetDir = targetDir + "/";
            }
            var newPath = targetDir + filename;
            if (File.Exists(newPath))
            {
                File.Delete(newPath);
            }
            File.Copy(sourceFilePath, newPath);
        }

        /// <summary>
        /// 将指定文件拷贝到新的路径,当目标路径上已有文件时，如果复写删除参数为真，则会判断新旧文件的md5值，md5不一样则会覆盖。
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="newPath"></param>
        /// <param name="isDelete"></param>
        /// <param name="beforeAction"></param>
        public static void TryCopy(string sourcePath, string newPath, bool isDelete = false,
            Func<string, string> beforeAction = null)
        {
            //  执行拷贝前委托
            if (beforeAction != null) newPath = beforeAction(newPath);

            if (!File.Exists(newPath))
            {
                EnsureDirExist(newPath);
                File.Copy(sourcePath, newPath);
            }

            if (!isDelete) return;
            var lMd5 = Md5Uitlity.GetFileMd5(sourcePath);
            var rMd5 = Md5Uitlity.GetFileMd5(newPath);
            if (lMd5 == rMd5) return;

            File.Delete(newPath);
            File.Copy(sourcePath, newPath);
        }

        public static void TryDeleteFile(string sourcePath)
        {
            if (File.Exists(sourcePath))
            {
                File.Delete(sourcePath);
            }
        }

        /// <summary>
        /// 尝试在指定的路径上创建一个文本文件
        /// 如果目标路径上已存在文件则创建取消
        /// </summary>
        /// <param name="path"></param>
        /// <param name="content"></param>
        public static void TryWriteAllText(string path, string content)
        {
            EnsureDirExist(path);
            if (!File.Exists(path))
            {
                File.WriteAllText(path, content);
            }
        }

        /// <summary>
        /// 在指定的路径上创建文本文件并写入指定内容
        /// 如果指定的路径上级目录不存在将自动创建对应目录结构
        /// </summary>
        /// <param name="path"></param>
        /// <param name="content"></param>
        public static void WriteAllText(string path, string content)
        {
            EnsureDirExist(path);
            File.WriteAllText(path, content);
        }

        /// <summary>
        /// 删除一个目录及其所有子目录和文件
        /// </summary>
        /// <param name="dirPath"></param>
        public static void DeleteDirectory(string dirPath)
        {
            if (!Directory.Exists(dirPath)) return;
            var di = new DirectoryInfo(dirPath);
            di.Delete(true);
        }

        public static void DeleteAllFile(string dir)
        {
            if (!Directory.Exists(dir)) return;

            var paths = GetFilePaths(dir).FilePaths;
            paths.ForEach(TryDeleteFile);
        }

        public static void TryCopyDirectory(string sourceDir, string targetDir)
        {
            if (Directory.Exists(targetDir)) return;

            CopyDirectory(sourceDir, targetDir);
        }

        /// <summary>
        /// 拷贝一个目录及其所有子目录及文件到目标目录下
        /// </summary>
        /// <param name="sourceDir"></param>
        /// <param name="targetDir"></param>
        /// <param name="isDeleteExist"></param>
        public static void CopyDirectory(string sourceDir, string targetDir, bool isDeleteExist = false)
        {
            var files = GetFilePaths(sourceDir).FilePaths;
            ////  创建所有目录
            CloneDirectoryTree(sourceDir, targetDir);
            // 拷贝所有文件
            foreach (var path in files)
            {
                var newPath = path.Replace(sourceDir, targetDir);
                TryCopy(path, newPath, isDeleteExist);
            }
        }

        /// <summary>
        /// 克隆目标目录及其下所有子目录的目录树结构到目标目录
        /// </summary>
        private static void CloneDirectoryTree(string sourceDir, string targetDir)
        {
            var dirs = GetAllDir(sourceDir).Dirs;
            //  创建所有目录
            foreach (var dir in dirs)
            {
                var newDir = dir.Replace(sourceDir, targetDir);
                TryCreateDirectory(newDir);
            }
        }

        /// <summary>
        /// 同步两个目录
        /// </summary>
        public static void SyncDirectory(string sourceDir, string targetDir, Func<string, bool> selector = null,
            Func<string, string> beforeCopy = null)
        {
            EnsureDirExist(sourceDir);
            EnsureDirExist(targetDir);

            CloneDirectoryTree(sourceDir, targetDir);
            var allFiles = selector == null ? GetFilePaths(sourceDir).FilePaths : GetFilePaths(sourceDir, selector).FilePaths;
            allFiles.ForEach(p => TryCopy(p, p.Replace(sourceDir, targetDir), true, beforeCopy));
        }

        /// <summary>
        /// 给定一个目录或者文件路径
        /// 该方法将确保该目录或文件依赖上级目录是存在的
        /// </summary>
        public static void EnsureDirExist(string targetPath)
        {
            var lastIndex = targetPath.LastIndexOf("/", StringComparison.Ordinal);
            var lastDir = targetPath.Substring(0, lastIndex);
            TryCreateDirectory(lastDir);
        }

        /// <summary>
        /// 打开指定的目录或者文件
        /// </summary>
        /// <param name="fileFullName"></param>
        public static void OpenFolderAndSelectFile(string fileFullName)
        {
            System.Diagnostics.Process.Start(fileFullName);
        }
    }
}
