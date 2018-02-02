using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Iuker.Common;
using Iuker.Common.Base;
using Iuker.Common.Utility;
using Iuker.UnityKit.Editor.Assets;
using Iuker.UnityKit.Run.Module.Asset;
using Iuker.UnityKit.Run.Base.Config;
using Iuker.UnityKit.Run.Base.Config.Develop;
using UnityEditor;
using UnityEngine;

namespace Iuker.UnityKit.Editor.Base.Assets
{
#if DEBUG
    /// <summary>
    /// 项目图集资源数据创建器
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20170913 22:21:40")]
    [ClassPurposeDesc("项目图集资源创建器", "项目图集资源创建器")]
#endif
    public class SpriteInfoCreater
    {
        public void CreateSpriteInfo(params SonProject[] sons)
        {
            var parentName = sons[0].ParentName;
            var infoDictionary = new Dictionary<string, Dictionary<string, SpriteInfo>>();
            sons.ToList().ForEach(son => CreateSpriteInfo(son, infoDictionary));
            infoDictionary.ForEach((path, infos) => { FileUtility.CreateBinaryFile(path, SerializeUitlity.Serialize(infos)); });
            var commonSonResourcesDir = Application.dataPath +
                                        string.Format("/_{0}/{1}_Common/Resources/", parentName, parentName);
            infoDictionary.ForEach((path, infos) => { FileUtility.CopyFile(path, commonSonResourcesDir); });
            Debug.Log(string.Format("项目{0}下所有子项目的图集数据已创建完毕！", RootConfig.CrtProjectName));
            AssetDatabase.Refresh();
        }

        private void CreateSpriteInfo(SonProject son, IDictionary<string, Dictionary<string, SpriteInfo>> infodDictionary)
        {
            var atlasDir = son.AssetDatabaseAtlasDir;
            if (!Directory.Exists(atlasDir))
            {
                Debug.Log(string.Format("图集目录{0}不存在！", atlasDir));
                return;
            }

            var infos = new Dictionary<string, SpriteInfo>();
            var allAtlas = FileUtility.GetFilePathDictionary(atlasDir, SelectAtlas).FilePathDictionary;
            allAtlas.ForEach((k, v) => CreateSpriteInfos_AssetDataBases(infos, k, v, son.ProjectName));

            if (infodDictionary.ContainsKey(son.SpriteInfoPath))
            {
                infodDictionary[son.SpriteInfoPath].Combin(infos);
            }
            else
            {
                infodDictionary.Add(son.SpriteInfoPath, infos);
            }
        }

        private static void CreateSpriteInfos_AssetDataBases(IDictionary<string, SpriteInfo> dictionary, string atlasName,
            string atlasPath, string mapSon)
        {
            var sprites = IukAssetDataBase.LoadAllSprites(atlasPath);
            foreach (var sprite in sprites)
            {
                var spriteInfo = new SpriteInfo(atlasName, sprite.name, mapSon);
                if (dictionary.ContainsKey(spriteInfo.AssetName))
                {
                    throw new Exception(string.Format("图集{0}有重复命名的精灵{1}，请检查精灵图集然后重试！", atlasName, spriteInfo.AssetName));
                }
                dictionary.Add(spriteInfo.AssetName, spriteInfo);
            }
        }

        private static bool SelectAtlas(string s)
        {
            if (s.Contains("meta")) return false;

            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(s);
            return fileNameWithoutExtension.StartsWith("atlas") && s.EndsWith(".png");
        }













    }
}
