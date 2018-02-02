namespace Iuker {

    export abstract class AbsMono implements IMono {

        public Root: IGameObject;
        public Attcher: IGameObject;

        public Init(root: IGameObject, attcher: IGameObject): void {

            this.Root = root;
            this.Attcher = attcher;
        }
    }
}