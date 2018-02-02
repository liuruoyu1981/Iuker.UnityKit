interface IViewRef {

    Destroy: IViewRefDestroy;
    Asset: IGameObject;

}

interface IViewRefDestroy {

    (): void;
}