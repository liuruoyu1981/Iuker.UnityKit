/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2/12/2017 10:39:27
Email: 35490136@qq.com
QQCode: 35490136
CreateNote: 通用调用工具
***********************************************************************************************/


/****************************************修改日志***********************************************
1. 修改日期： 修改人： 修改内容：
2. 修改日期： 修改人： 修改内容：
3. 修改日期： 修改人： 修改内容：
4. 修改日期： 修改人： 修改内容：
5. 修改日期： 修改人： 修改内容：
****************************************修改日志***********************************************/



namespace Iuker.UnityKit.Editor.Assets
{
    /// <summary>
    /// 资源导入检查器
    /// </summary>
    public class IukerPostprocessor : UnityEditor.AssetPostprocessor
    {
        ////模型导入之前调用  
        //public void OnPreprocessModel()
        //{
        //    Debug.Log("OnPreprocessModel=" + this.assetPath);
        //}

        ////模型导入之前调用  
        //public void OnPostprocessModel(GameObject go)
        //{
        //    Debug.Log("OnPostprocessModel=" + go.name);
        //}

        ////纹理导入之前调用，针对入到的纹理进行设置  
        //public void OnPreprocessTexture()
        //{
        //    Debug.Log("OnPreProcessTexture=" + this.assetPath);
        //    TextureImporter impor = this.assetImporter as TextureImporter;
        //    impor.textureFormat = TextureImporterFormat.ARGB32;
        //    impor.maxTextureSize = 512;
        //    impor.textureType = TextureImporterType.Advanced;
        //    impor.mipmapEnabled = false;

        //}

        ////文理导入之后
        //public void OnPostprocessTexture(Texture2D tex)
        //{
        //    Debug.Log("OnPostProcessTexture=" + this.assetPath);
        //}

        ////音频导入之前
        //public void OnPreprocessAudio()
        //{
        //    Debug.Log("OnPreprocessAudio");

        //    AudioImporter audio = this.assetImporter as AudioImporter;
        //    //audio.com = AudioImporterFormat.Compressed;
        //}

        ////音频导入之后
        //public void OnPostprocessAudio(AudioClip clip)
        //{
        //    Debug.Log("OnPostprocessAudio=" + clip.name);
        //}



        //所有的资源的导入，删除，移动，都会调用此方法，注意，这个方法是static的  
        public static void OnPostprocessAllAssets(string[] importedAsset, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            AutoUpdateAssetIdAndAssetInfo.TryUpdate(importedAsset);
            AutoUpdateAssetIdAndAssetInfo.TryUpdate(deletedAssets);
            AutoUpdateAssetIdAndAssetInfo.TryUpdate(movedAssets);
            AutoUpdateAssetIdAndAssetInfo.TryUpdate(movedFromAssetPaths);
        }



    }
}