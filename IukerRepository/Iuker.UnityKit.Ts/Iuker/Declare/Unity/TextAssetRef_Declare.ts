//  ---------------------------------文本（二进制）资源引用----------------------------------------------

interface ITextAssetRef {

    Destroy: ITextAssetRefDestroy;
    Asset: ITextAsset;

}

interface ITextAssetRefDestroy {

    (): void;
}