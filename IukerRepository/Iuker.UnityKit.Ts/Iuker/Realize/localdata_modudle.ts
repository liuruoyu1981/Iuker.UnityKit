namespace Iuker {

    export class LocalDataModule {

        /**
         * 加载指定本地数据类型的数据源然后并返回所有数据
         * @param assetName 数据源资源名
         * @param typeDesc 本地数据类型占位符
         */
        public static GetLocalDataEntitys<T extends ILocalDataEntity<T>>(assetName: string, typeDesc: { new (): T; }): T[] {

            let entity = new typeDesc();

            let lines = Iuker_LocalDataModule_GetEntityStrLines(assetName);
            let entitys = entity.CreateEntitys(lines);

            return entitys;
        }

    }
}