using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Iuker.Common;
using Iuker.Common.Utility;
using Iuker.UnityKit.Run;
using Iuker.UnityKit.Run.Base;
using Iuker.UnityKit.Run.Base.Config;
using UnityEditor;
using UnityEngine;

namespace Iuker.UnityKit.Editor
{
    public class MyAssetIdCreater
    {
        #region 字段

        private readonly SonProject mSon;
        private readonly StringBuilder mCsSb = new StringBuilder();
        private readonly StringBuilder mTsSb = new StringBuilder();
        private readonly string mCsOutPutPath;
        private readonly string mTsOutPutPath;
        private readonly Dictionary<string, string> mPaths = new Dictionary<string, string>();
        private readonly Dictionary<string, List<string>> mTypeAssets = new Dictionary<string, List<string>>();

        #endregion

        #region 静态调用入口

        public static void CreateScript(SonProject son, string csPath = null, string tsPath = null)
        {
            var creater = new MyAssetIdCreater(son, csPath, tsPath);
            creater.ScanAssetByType(son);
            creater.AppendAssetIdClass();
            creater.WriteToFile();
        }

        #endregion

        #region 实例函数

        private void WriteToFile()
        {
            FileUtility.WriteAllText(mCsOutPutPath, mCsSb.ToString());
            FileUtility.WriteAllText(mTsOutPutPath, mTsSb.ToString());
            TsProj.AddLine(mSon.CompexName.ToLower() + "_assetid", mSon).UpdateToFile(mSon.TsProjPath);
        }

        private MyAssetIdCreater(SonProject son, string csPath, string tsPath)
        {
            mSon = son;
            mCsOutPutPath = csPath ?? mSon.CsAssetIdPath;
            mTsOutPutPath = tsPath ?? mSon.TsAssetIdPath;
        }

        private void ScanAssetByType(SonProject son)
        {
            var dirs = FileUtility.GetAllDir(son.AssetDataBaseDir, s => s != son.AssetDataBaseDir).Dirs;
            foreach (var dir in dirs)
            {
                var type = ParseAssetType(dir, son);
                if (type == "Atlas")
                {
                    var aPs = FileUtility.GetFilePathDictionary(dir, s => !s.Contains(".meta") && !s.Contains(".tpsheet")).FilePathDictionary;
                    aPs.ForEach(LoadAtlas);
                    continue;
                }

                var paths = Directory.GetFiles(dir).Where(
                    s => !s.Contains("meta") && !s.Contains("~") && !s.Contains(".xlsx")
                         && !s.EndsWith(".gitKeep.txt")).ToList();

                var nameDictionary = new Dictionary<string, string>();
                foreach (var p in paths)
                {
                    var name = Path.GetFileNameWithoutExtension(p);
                    if (nameDictionary.ContainsKey(name))
                        throw new Exception(string.Format("发现重复的资源{0}！", name));

                    nameDictionary.Add(name, p);
                }
                AddTypeList(type, nameDictionary.Keys.ToList());
                mPaths.Combin(nameDictionary);
            }
        }

        private string ParseAssetType(string dir, SonProject son)
        {
            var parentDir = dir.Replace(son.AssetDataBaseDir, "").Split('/').First();
            var typeArr = parentDir.Split('_');
            var assetIdType = typeArr.Length == 0 ? dir : typeArr.Last();
            return assetIdType;
        }

        private void AddTypeList(string type, List<string> typeFns)
        {
            AssetNameCheck(typeFns);
            type = string.Format("AssetId_{0}_{1}", mSon.CompexName, type);

            if (!mTypeAssets.ContainsKey(type))
            {
                mTypeAssets.Add(type, typeFns);
            }
            else
            {
                var exitFns = mTypeAssets[type];
                foreach (var fn in typeFns)
                {
                    if (exitFns.Contains(fn))
                        throw new Exception(string.Format("发现重复文件__ {0} __，脚本创建中止！", fn));

                    exitFns.Add(fn);
                }
            }
        }

        private void LoadAtlas(string fn, string path)
        {
            List<string> sps = null;

            var assetsPath = path.AssetsPath();
            var objs = AssetDatabase.LoadAllAssetsAtPath(assetsPath);
            if (objs != null)
            {
                sps = objs.OfType<Sprite>().Select(s => s.name).ToList();
            }

            if (sps == null)
            {
                path = path.Replace(mSon.AssetDataBaseDir, "");
                path = path.Remove(path.LastIndexOf(".", StringComparison.Ordinal));
                sps = Resources.LoadAll<Sprite>(path).Select(s => s.name).ToList();
            }

            AssetNameCheck(sps);
            AddTypeList(string.Format("Atlas_{0}", fn.Split('_').Last()), sps);
        }

        private void AssetNameCheck(List<string> paths)
        {
            var notOks = new List<string>();

            foreach (var path in paths)
            {
                var fileName = path.FileName();
                if (!char.IsDigit(fileName.First()) && !fileName.Contains("+") && !fileName.Contains("-") &&
                    !fileName.Contains("=") && !fileName.Contains(".")) continue;

                notOks.Add(path);
            }

            if (notOks.Count <= 0) return;

            notOks.ForEach(s => Debug.LogError(string.Format("发现命名不合法的资源：{0}！", s)));
            throw new Exception(string.Format("发现命名不合法的资源一共{0}个，脚本创建中止！", notOks.Count));
        }

        private void AppendAssetIdClass()
        {
            mCsSb.AppendCsahrpFileInfo(EditorConstant.HostClientName, EditorConstant.HostClientEmail, "资源Id脚本，请勿修改该脚本！");
            mTsSb.AppendCsahrpFileInfo(EditorConstant.HostClientName, EditorConstant.HostClientEmail, "资源Id脚本，请勿修改该脚本！");
            mTsSb.AppendLine(string.Format("namespace {0} ", mSon.CompexName) + "{");

            foreach (var valuePair in mTypeAssets)
            {
                mCsSb.AppendLine(string.Format("public static class {0}", valuePair.Key) + "\r\n{");
                mTsSb.AppendLine(string.Format("    export class {0}", valuePair.Key) + " {");
                mTsSb.AppendLine();
                foreach (var fn in valuePair.Value)
                {
                    mCsSb.AppendLine(string.Format("    public static readonly string {0} = ", fn) + "\"" + fn + "\";");
                    mTsSb.AppendLine(string.Format("        public static readonly {0}: string = ", fn) + "\"" + fn + "\";");
                }
                mCsSb.AppendLine("}\r\n");
                mTsSb.AppendLine();
                mTsSb.AppendLine("    }\r\n");
            }

            mTsSb.AppendLine("}\r\n");
        }

        #endregion

    }
}
