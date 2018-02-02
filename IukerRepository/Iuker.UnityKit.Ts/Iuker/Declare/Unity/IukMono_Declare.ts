
interface IMonoInit {

    (root: IGameObject, attcher: IGameObject): void;

}


interface IMono {

    Init: IMonoInit;

}