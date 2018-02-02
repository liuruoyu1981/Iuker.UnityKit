interface IJintRawImage {

    ImageName: string;
    DependentGo: IGameObject;
    ViewRoot: IGameObject;
    ToNativeSize: IToNativeSize;
    SetName: ISetTextureName;

}

interface ISetTextureName {

    (textureName: string): void;

}