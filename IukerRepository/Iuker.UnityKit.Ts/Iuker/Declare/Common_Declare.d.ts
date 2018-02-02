interface IInit {
    (): void;
}

interface IGetContainer {
    (path: string): IGameObject;
}

interface IToNativeSize {
    (): void;
}

interface IList<T> {

    Add: IListAdd<T>;

}

interface IListAdd<T> {

    (element: T): void;
}

declare function parseBoolean(value: string): boolean;
declare function parseListInt(value: string): number[];
declare function parseListFloat(value: string): number[];

//  网络

/**
 * 压入一个待发送的协议消息
 * @param typeName 协议类型名
 * @param message 协议实例
 * @param type 协议逻辑类型
 */
declare function PushSendMessage(typeName: string, message: any, type: number): void;

declare function GetProtoByJs(sonProject: string, protoName: string): any;
