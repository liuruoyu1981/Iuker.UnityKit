/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/02/27 17:25:28
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


using System;
using System.Collections.Generic;
using System.Linq;
using Iuker.Common.Base.Enums;
using Iuker.Common.Module.LocalData;
using Iuker.Common.Utility;
using Iuker.UnityKit.Run.Base;
using Iuker.UnityKit.Run.Module.Asset;
using UnityEngine;

namespace Iuker.UnityKit.Run.Module.LocalData
{
    /// <summary>
    /// 默认本地数据模块
    /// </summary>
    public class DefaultU3dModule_LocalData : AbsU3dModule, IU3dLocalDataModule
    {
        public override string ModuleName
        {
            get
            {
                return ModuleType.LocalData.ToString();
            }
        }

        protected IU3dAssetModule mAssetModule;

        protected override void OnHotUpdateComplete()
        {
            base.OnHotUpdateComplete();

            mAssetModule = U3DFrame.AssetModule;
        }

        /// <summary>
        /// 获得指定本地数据类型的所有数据
        /// </summary>
        /// <typeparam name="T">泛型本地数据类型</typeparam>
        /// <param name="assetId">资源id</param>
        /// <returns></returns>
        public List<T> GetAllRecord<T>(string assetId) where T : ILocalDataEntity<T>, new()
        {
            byte[] serializeData;
            List<T> deSerializeData;

            if (_serializeDataDictionary.ContainsKey(assetId))
            {
                serializeData = _serializeDataDictionary[assetId];
                deSerializeData = SerializeUitlity.DeSerialize<List<T>>(serializeData);
                return deSerializeData;
            }

            var textRef = mAssetModule.LoadTextAsset(assetId);
            if (textRef == null || textRef.Asset == null)
            {
                Debug.Log(string.Format("资源id{0}的本地数据表加载失败，请检查！", assetId));
                return null;
            }

            var sourceList = textRef.Asset.text.Split(Environment.NewLine.ToCharArray()).ToList();
            var listObj = LdUitlity.ConvertLocalDataTxt(sourceList, false);
            var result = new T().CreateEntitys(listObj);
            serializeData = SerializeUitlity.Serialize(result);
            _serializeDataDictionary.Add(assetId, serializeData);
            deSerializeData = SerializeUitlity.DeSerialize<List<T>>(serializeData);
            return deSerializeData;
        }

        private readonly Dictionary<string, byte[]> _serializeDataDictionary = new Dictionary<string, byte[]>();



    }
}
