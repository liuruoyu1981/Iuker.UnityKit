/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/4/8 下午3:46:24
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
using Iuker.UnityKit.Run.Base;
using Iuker.UnityKit.Run.Module.Asset;
using Iuker.UnityKit.Run.Module.View.MVDA;
using UnityEngine;
using UnityEngine.UI;

namespace Iuker.UnityKit.Run.Module.View.ViewWidget
{
    public class IukImage : Image, IImage
    {
        private IU3dFrame U3DFrame { get; set; }

        public GameObject DependentGo {get { return gameObject; } }

        public GameObject ViewRoot
        {
            get
            {
                return AttachView.RectRoot.gameObject;
            }
        }

        public IukViewWidget Init(IU3dFrame u3DFrame, IView view, IFragment fragment = null)
        {
            U3DFrame = u3DFrame;
            AttachView = view;

            return this;
        }

        public string WidgetToken
        {
            get
            {
                return ViewRoot.name + "_" + gameObject.name;
            }
        }

        public IView AttachView { get; private set; }

        private static readonly Dictionary<string, List<Sprite>> AllSprites =
            new Dictionary<string, List<Sprite>>();

        public string ImageName
        {
            get { return sprite.name; }
            set
            {
                var assetInfo = U3DFrame.AssetModule.GetAssetInfo(value);
                if (assetInfo != null)  //  非精灵表图集类型
                {
                    U3DFrame.AssetModule.AsyncLoad<Sprite>(value, loader =>
                    {
                        sprite = loader.Asset;
                    });
                }
                else //    图集类型
                {
                    var spriteInfo = U3DFrame.AssetModule.GetSpriteInfo(value);
                    var sprites = AllSprites.ContainsKey(spriteInfo.AssetTypeStr) ?
                        AllSprites[spriteInfo.AssetTypeStr] :
                        U3DFrame.AssetModule.SyncLoadAllAsset<Sprite>(spriteInfo.AssetTypeStr);
                    TryCacheSprites(spriteInfo, sprites);
                    var target = sprites.Find(r => r.name == value);
                    sprite = target;
                }
            }
        }

        private void TryCacheSprites(SpriteInfo info, List<Sprite> sprites)
        {
            if (AllSprites.ContainsKey(info.AssetTypeStr)) return;

            AllSprites.Add(info.AssetTypeStr, sprites);
        }

        /// <summary>
        /// 图片颜色
        /// </summary>
        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        public void ToNativeSize()
        {
            SetNativeSize();
        }

        public IImage SetImage(string imageName)
        {
            ImageName = imageName;
            return this;
        }
    }
}
