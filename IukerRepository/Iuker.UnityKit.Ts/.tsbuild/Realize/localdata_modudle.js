var Iuker;
(function (Iuker) {
    var LocalDataModule = /** @class */ (function () {
        function LocalDataModule() {
        }
        /**
         * 加载指定本地数据类型的数据源然后并返回所有数据
         * @param assetName 数据源资源名
         * @param typeDesc 本地数据类型占位符
         */
        LocalDataModule.GetLocalDataEntitys = function (assetName, typeDesc) {
            var entity = new typeDesc();
            var lines = Iuker_LocalDataModule_GetEntityStrLines(assetName);
            var entitys = entity.CreateEntitys(lines);
            return entitys;
        };
        return LocalDataModule;
    }());
    Iuker.LocalDataModule = LocalDataModule;
})(Iuker || (Iuker = {}));
//# sourceMappingURL=localdata_modudle.js.map