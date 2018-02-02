/*
Typescript本地数据基础表
音效表

*/
namespace Iuker {

    export class ld_soundeffect_table {

        /**
         * 编号
         */
        public ID: number;
        public ComponentRootName: string;
        public ComponentName: string;
        public ActionName: string;
        public SoundEffectName: string;
        public Desc: string;

        public static CreateEntity(row: string[]): ld_soundeffect_table {

            let instance = new ld_soundeffect_table();
            instance.ID = parseInt(row[0]);
            instance.ComponentRootName = row[1];
            instance.ComponentName = row[2];
            instance.ActionName = row[3];
            instance.SoundEffectName = row[4];
            instance.Desc = row[5];

            return instance;
        }

        public static CreateEntitys(listObj: string[]): ld_soundeffect_table[] {

            let tables: ld_soundeffect_table[] = [];
            for (let i = 0; i < listObj.length; i++) {

                let list = listObj[i];
                let entityListText = list.split("[__]");
                let entity = ld_soundeffect_table.CreateEntity(entityListText);
                tables.push(entity);
            }

            return tables;
        }
    }

}


