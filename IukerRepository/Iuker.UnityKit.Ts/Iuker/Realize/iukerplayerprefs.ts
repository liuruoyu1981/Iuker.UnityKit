namespace Iuker {

    export class IukerPlayerPrefs {

        public static SetInt(key: string, value: number): void {

            IukerPlayerPrefs_SetInt(key, value);
        }

        public static GetInt(key: string): number {

            return IukerPlayerPrefs_GetInt(key);
        }

        public static SetLong(key: string, value: number): void {

            IukerPlayerPrefs_SetLong(key, value);
        }

        public static GetLong(key: string): number {

            return IukerPlayerPrefs_GetLong(key);
        }

        public static SetString(key: string, value: string): void {

            IukerPlayerPrefs_SetString(key, value);
        }

        public static GetString(key: string): string {

            return IukerPlayerPrefs_GetString(key);
        }

        public static SetFloat(key: string, value: number): void {

            IukerPlayerPrefs_SetFloat(key, value);
        }

        public static GetFloat(key: string): number {

            return IukerPlayerPrefs_GetFloat(key);
        }





    }

}