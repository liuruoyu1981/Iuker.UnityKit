namespace Iuker {

    export class DataModule {

        /**
        * 使用指定的key设置一个Int32值。
        * @param {string} key - 数据键。
        * @param {number} newData - 数据值。
        */
        public static SetInt(key: string, newData: number): void {
            Iuker_DataModule_SetInt(key, newData);
        }

        /**
    * 使用指定的key获取一个Int32值。
    * @param {string} key - 数据键。
    * @param {number} newData - 数据值。
    */
        public static GetInt(key: string): number {
            let value = Iuker_DataModule_GetInt(key);
            return value;
        }

        /**
        * 使用指定的key设置一个Int64值。
        * @param {string} key - 数据键。
        * @param {number} newData - 数据值。
        */
        public static SetLong(key: string, newData: number): void {
            Iuker_DataModule_SetLong(key, newData);
        }

        /**
    * 使用指定的key获取一个Int64值。
    * @param {string} key - 数据键。
    * @param {number} newData - 数据值。
    */
        public static GetLong(key: string): number {
            let value = Iuker_DataModule_GetLong(key);
            return value;
        }

        /**
        * 使用指定的key设置一个字符串值。
        * @param {string} key - 数据键。
        * @param {number} newData - 数据值。
        */
        public static SetString(key: string, newData: string): void {
            Iuker_DataModule_SetString(key, newData);
        }

        /**
    * 使用指定的key获取一个字符串值。
    * @param {string} key - 数据键。
    * @param {number} newData - 数据值。
    */
        public static GetString(key: string): string {
            let value = Iuker_DataModule_GetString(key);
            return value;
        }


        /**
    * 使用指定的key设置一个浮点值。
    * @param {string} key - 数据键。
    * @param {number} newData - 数据值。
    */
        public static SetFloat(key: string, newData: number): void {
            Iuker_DataModule_SetFloat(key, newData);
        }

        /**
    * 使用指定的key获取一个浮点值。
    * @param {string} key - 数据键。
    * @param {number} newData - 数据值。
    */
        public static GetFloat(key: string): number {
            let value = Iuker_DataModule_GetFloat(key);
            return value;
        }

        private static mNumberDictionary = {};

        /**
         * 使用指定的Key设置一个数字类型值
         * @param key
         * @param value
         */
        public static TsSetNumber(key: string, value: number): void {

            this.mNumberDictionary[key] = value;
        }

        /**
         * 使用指定的Key获取一个数字类型的值
         * @param key
         */
        public static TsGetNumber(key: string): number {

            return this.mBooleanDictionary[key];
        }

        private static mStringDictionary = {};

        /**
         * 使用指定的Key设置一个字符串类型的值
         * @param key
         * @param value
         */
        public static TsSetString(key: string, value: string): void {

            this.mStringDictionary[key] = value;
        }

        /**
         * 使用指定的Key获取一个字符串类型的值
         * @param key
         */
        public static TsGetString(key: string): string {

            return this.mStringDictionary[key];
        }

        private static mBooleanDictionary = {};

        /**
         * 使用指定的Key设置一个布尔值类型的值
         * @param key
         * @param value
         */
        public static TsSetBoolean(key: string, value: boolean): void {

            this.mBooleanDictionary[key] = value;
        }

        /**
         * 使用指定的Key获取一个布尔值类型的值
         * @param key
         * @param value
         */
        public static TsGetBoolean(key: string): boolean {

            return this.mBooleanDictionary[key];
        }




    }
}
