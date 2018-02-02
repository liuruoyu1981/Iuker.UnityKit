namespace Iuker {

    /**
     * 业务逻辑管理器集合
     */
    export class LogicManagers {

        public static Managers = {};

        public static GetManager<T>(name: string): T {

            let manager = this.Managers[name];
            return manager as T;

        }
    }
}