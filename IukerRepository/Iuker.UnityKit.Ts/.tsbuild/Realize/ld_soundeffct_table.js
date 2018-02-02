/*
Typescript本地数据基础表
音效表

*/
var Iuker;
(function (Iuker) {
    var ld_soundeffect_table = /** @class */ (function () {
        function ld_soundeffect_table() {
        }
        ld_soundeffect_table.CreateEntity = function (row) {
            var instance = new ld_soundeffect_table();
            instance.ID = parseInt(row[0]);
            instance.ComponentRootName = row[1];
            instance.ComponentName = row[2];
            instance.ActionName = row[3];
            instance.SoundEffectName = row[4];
            instance.Desc = row[5];
            return instance;
        };
        ld_soundeffect_table.CreateEntitys = function (listObj) {
            var tables = [];
            for (var i = 0; i < listObj.length; i++) {
                var list = listObj[i];
                var entityListText = list.split("[__]");
                var entity = ld_soundeffect_table.CreateEntity(entityListText);
                tables.push(entity);
            }
            return tables;
        };
        return ld_soundeffect_table;
    }());
    Iuker.ld_soundeffect_table = ld_soundeffect_table;
})(Iuker || (Iuker = {}));
//# sourceMappingURL=ld_soundeffct_table.js.map