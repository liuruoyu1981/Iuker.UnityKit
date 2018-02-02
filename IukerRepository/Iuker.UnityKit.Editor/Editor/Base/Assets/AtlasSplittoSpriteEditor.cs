/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/4/9 下午12:07:06
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
using System.IO;
using Iuker.Common.Utility;
using UnityEditor;
using UnityEngine;


namespace Iuker.UnityKit.Editor.Assets
{
    /// <summary>
    /// 将现有图集拆分为单独的sprite
    /// </summary>
    public class AtlasSplittoSpriteEditor : UnityEditor.Editor
    {
        private static string AtlasToSpritesDir
        {
            get
            {
                return UnityEngine.Application.dataPath + "/__AtlasToSprites/";
            }
        }

        public static void SplitAtlas()
        {
            var objs = Selection.objects;
            foreach (var obj in objs)
            {
                SplitAtlasSingle(obj);
            }

            AssetDatabase.Refresh();
        }

        private static void SplitAtlasSingle(UnityEngine.Object selectObject)
        {
            var fullPath = AssetDatabase.GetAssetPath(selectObject);
            var sprites = AssetDatabase.LoadAllAssetsAtPath(fullPath);

            var outputPath = AtlasToSpritesDir + selectObject.name + "/";
            FileUtility.TryCreateDirectory(outputPath);

            try
            {
                foreach (var texture in sprites)
                {
                    var sprite = texture as Sprite;
                    if (sprite != null)
                    {
                        Texture2D texture2D = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height, sprite.texture.format, false);
                        texture2D.SetPixels(sprite.texture.GetPixels((int)sprite.rect.xMin, (int)sprite.rect.yMin, (int)sprite.rect.width, (int)sprite.rect.height));
                        texture2D.Apply();
                        var spriteOutputPath = outputPath + sprite.name + ".png";
                        File.WriteAllBytes(spriteOutputPath, texture2D.EncodeToPNG());
                    }
                }
            }
            catch (Exception exception)
            {
                if (exception.Message.Contains("is not readable"))
                {
                    EditorUtility.DisplayDialog("错误", "所选择的图集当前没有开启可读选项，请修改后重试！", "确定");
                }
            }

        }
    }
}
