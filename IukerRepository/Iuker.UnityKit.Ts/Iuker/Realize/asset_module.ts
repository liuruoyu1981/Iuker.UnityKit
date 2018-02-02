namespace Iuker {

    export class AssetModule {

        public static LoadTextAsset(name: string): ITextAssetRef {

            let ref = Iuker_AssetModule_LoadTextAsset(name);
            return ref;

        }

    }
}