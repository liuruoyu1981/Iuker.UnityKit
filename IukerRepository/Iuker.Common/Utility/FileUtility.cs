using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Iuker.Common.Base;

namespace Iuker.Common.Utility
{
#if DEBUG
    /// <summary>
    /// IO����
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20170902 17:45:17")]
    [ClassPurposeDesc("IO����", "")]
#endif
    public static class FileUtility
    {
        /// <summary>
        /// �ļ���Ŀ¼��Ϣ���ҽ��
        /// </summary>
        public class FileFindResult
        {
            /// <summary>
            /// ����Ŀ¼·���б�
            /// </summary>
            public List<string> Dirs { get; set; }

            /// <summary>
            /// �����ļ�·���б�
            /// </summary>
            public List<string> FilePaths { get; set; }

            /// <summary>
            /// �����ļ������ļ�·���ֵ�
            /// </summary>
            public Dictionary<string, string> FilePathDictionary { get; set; }

            /// <summary>
            /// Ŀ���·��
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
        /// ���ָ��Ŀ¼��������Ŀ¼��ָ��Ŀ¼
        /// </summary>
        /// <param name="dir">ָ��Ŀ��Ŀ¼</param>
        /// <param name="dirSelecter">Ŀ¼ѡ����</param>
        /// <returns>Ŀ¼�б�</returns>
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
        /// ���ָ��Ŀ¼��������Ŀ¼�������ļ�·��
        /// </summary>
        /// <param name="dir">ָ��Ŀ¼</param>
        /// <param name="fileSelecter">�ļ�·��ѡ����</param>
        /// <param name="dirSelecter">Ŀ¼·��ѡ����/param>
        /// <returns>�����ļ�·���б�</returns>
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
        /// ���ָ��Ŀ¼�¼���������Ŀ¼�������ļ����ļ������ļ�·���ֵ�
        /// </summary>
        /// <param name="dir">ָ��Ŀ¼</param>
        /// <param name="filter">�ļ�������</param>
        /// <param name="dirFilter">Ŀ¼������</param>
        /// <returns></returns>
        public static FileFindResult GetFilePathDictionary(string dir, Func<string, bool> filter = null, Func<string, bool> dirFilter = null)
        {
            var info = GetFilePaths(dir, filter, dirFilter);
            foreach (var path in info.FilePaths)
            {
                var fileName = path.Split('/').Last().Split('.').First();
                if (info.FilePathDictionary.ContainsKey(fileName))
                {
                    Debuger.LogError(string.Format("Ŀ��Ŀ¼������Ŀ¼�´���ͬ���ļ����ļ���Ϊ��{0}", fileName));
                    continue;
                }
                info.FilePathDictionary.Add(fileName, path);
            }

            return info;
        }

        /// <summary>
        /// ����һ���������ļ�
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
        /// ���ָ��·�����ļ����ϼ�·����Ŀ¼
        /// </summary>
        /// <param name="p">ָ��·��</param>
        private static string GetParnetPath(string p)
        {
            var fn = p.Split('/').Last();
            var parentP = p.Replace(fn, "");
            return parentP;
        }

        /// <summary>
        /// ��ָ���ļ��ƶ���Ŀ��Ŀ¼��
        /// ���Ŀ��Ŀ¼�����ڽ����Զ�������Ŀ¼
        /// </summary>
        /// <param name="sourceFile">Դ�ļ�</param>
        /// <param name="targetDir">Ŀ��·��</param>
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
        /// ���Դ���һ����Ŀ¼�����������Ŀ¼·�������ڣ�
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
        /// ��ָ�����ļ�������ָ��Ŀ¼
        /// ���Ŀ��·���������ļ�������ɾ�����е�Ȼ���½�
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
        /// ��ָ���ļ��������µ�·��,��Ŀ��·���������ļ�ʱ�������дɾ������Ϊ�棬����ж��¾��ļ���md5ֵ��md5��һ����Ḳ�ǡ�
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="newPath"></param>
        /// <param name="isDelete"></param>
        /// <param name="beforeAction"></param>
        public static void TryCopy(string sourcePath, string newPath, bool isDelete = false,
            Func<string, string> beforeAction = null)
        {
            //  ִ�п���ǰί��
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
        /// ������ָ����·���ϴ���һ���ı��ļ�
        /// ���Ŀ��·�����Ѵ����ļ��򴴽�ȡ��
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
        /// ��ָ����·���ϴ����ı��ļ���д��ָ������
        /// ���ָ����·���ϼ�Ŀ¼�����ڽ��Զ�������ӦĿ¼�ṹ
        /// </summary>
        /// <param name="path"></param>
        /// <param name="content"></param>
        public static void WriteAllText(string path, string content)
        {
            EnsureDirExist(path);
            File.WriteAllText(path, content);
        }

        /// <summary>
        /// ɾ��һ��Ŀ¼����������Ŀ¼���ļ�
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
        /// ����һ��Ŀ¼����������Ŀ¼���ļ���Ŀ��Ŀ¼��
        /// </summary>
        /// <param name="sourceDir"></param>
        /// <param name="targetDir"></param>
        /// <param name="isDeleteExist"></param>
        public static void CopyDirectory(string sourceDir, string targetDir, bool isDeleteExist = false)
        {
            var files = GetFilePaths(sourceDir).FilePaths;
            ////  ��������Ŀ¼
            CloneDirectoryTree(sourceDir, targetDir);
            // ���������ļ�
            foreach (var path in files)
            {
                var newPath = path.Replace(sourceDir, targetDir);
                TryCopy(path, newPath, isDeleteExist);
            }
        }

        /// <summary>
        /// ��¡Ŀ��Ŀ¼������������Ŀ¼��Ŀ¼���ṹ��Ŀ��Ŀ¼
        /// </summary>
        private static void CloneDirectoryTree(string sourceDir, string targetDir)
        {
            var dirs = GetAllDir(sourceDir).Dirs;
            //  ��������Ŀ¼
            foreach (var dir in dirs)
            {
                var newDir = dir.Replace(sourceDir, targetDir);
                TryCreateDirectory(newDir);
            }
        }

        /// <summary>
        /// ͬ������Ŀ¼
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
        /// ����һ��Ŀ¼�����ļ�·��
        /// �÷�����ȷ����Ŀ¼���ļ������ϼ�Ŀ¼�Ǵ��ڵ�
        /// </summary>
        public static void EnsureDirExist(string targetPath)
        {
            var lastIndex = targetPath.LastIndexOf("/", StringComparison.Ordinal);
            var lastDir = targetPath.Substring(0, lastIndex);
            TryCreateDirectory(lastDir);
        }

        /// <summary>
        /// ��ָ����Ŀ¼�����ļ�
        /// </summary>
        /// <param name="fileFullName"></param>
        public static void OpenFolderAndSelectFile(string fileFullName)
        {
            System.Diagnostics.Process.Start(fileFullName);
        }
    }
}
