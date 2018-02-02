declare function Iuker_LocalDataModule_GetEntityStrLines(assetName: string): string[];

interface ILocalDataEntity<T> {

    CreateEntity: ILocalDataEntity_CreateEntiy<T>;
    CreateEntitys: ILocalDataEntity_CreateEntiys<T>;

}

interface ILocalDataEntity_CreateEntiy<T> {

    (row: string[]): T;

}

interface ILocalDataEntity_CreateEntiys<T> {

    (rows: string[]): T[];

}

