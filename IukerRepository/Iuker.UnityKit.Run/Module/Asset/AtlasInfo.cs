/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/4/8 下午7:25:58
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


namespace Iuker.UnityKit.Run.Module.Asset
{
    /// <summary>
    /// 图集信息
    /// </summary>
    [Serializable]
    public class AtlasInfo : IAssetTypeStr
    {
        public string AssetTypeStr { get; private set; }
        public string AssetName { get; private set; }
        public string MapSon { get; private set; }

        public AtlasInfo(string assetTypeStr, string assetName, string mapSon)
        {
            AssetTypeStr = assetTypeStr;
            AssetName = assetName;
            MapSon = mapSon;
        }
    }
}
