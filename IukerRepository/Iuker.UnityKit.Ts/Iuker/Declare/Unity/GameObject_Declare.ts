//  ---------------------------------游戏对象----------------------------------------------

interface IGameObject {
    SetActive: ISetActive;
    name: string;
}

interface ISetActive {
    (reult: boolean): void;
}