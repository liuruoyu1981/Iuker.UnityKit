var Iuker;
(function (Iuker) {
    var DataModule = /** @class */ (function () {
        function DataModule() {
        }
        /**
        * 使用指定的key设置一个Int32值。
        * @param {string} key - 数据键。
        * @param {number} newData - 数据值。
        */
        DataModule.SetInt = function (key, newData) {
            Iuker_DataModule_SetInt(key, newData);
        };
        /**
    * 使用指定的key获取一个Int32值。
    * @param {string} key - 数据键。
    * @param {number} newData - 数据值。
    */
        DataModule.GetInt = function (key) {
            var value = Iuker_DataModule_GetInt(key);
            return value;
        };
        /**
        * 使用指定的key设置一个Int64值。
        * @param {string} key - 数据键。
        * @param {number} newData - 数据值。
        */
        DataModule.SetLong = function (key, newData) {
            Iuker_DataModule_SetLong(key, newData);
        };
        /**
    * 使用指定的key获取一个Int64值。
    * @param {string} key - 数据键。
    * @param {number} newData - 数据值。
    */
        DataModule.GetLong = function (key) {
            var value = Iuker_DataModule_GetLong(key);
            return value;
        };
        /**
        * 使用指定的key设置一个字符串值。
        * @param {string} key - 数据键。
        * @param {number} newData - 数据值。
        */
        DataModule.SetString = function (key, newData) {
            Iuker_DataModule_SetString(key, newData);
        };
        /**
    * 使用指定的key获取一个字符串值。
    * @param {string} key - 数据键。
    * @param {number} newData - 数据值。
    */
        DataModule.GetString = function (key) {
            var value = Iuker_DataModule_GetString(key);
            return value;
        };
        /**
    * 使用指定的key设置一个浮点值。
    * @param {string} key - 数据键。
    * @param {number} newData - 数据值。
    */
        DataModule.SetFloat = function (key, newData) {
            Iuker_DataModule_SetFloat(key, newData);
        };
        /**
    * 使用指定的key获取一个浮点值。
    * @param {string} key - 数据键。
    * @param {number} newData - 数据值。
    */
        DataModule.GetFloat = function (key) {
            var value = Iuker_DataModule_GetFloat(key);
            return value;
        };
        /**
         * 使用指定的Key设置一个数字类型值
         * @param key
         * @param value
         */
        DataModule.TsSetNumber = function (key, value) {
            this.mNumberDictionary[key] = value;
        };
        /**
         * 使用指定的Key获取一个数字类型的值
         * @param key
         */
        DataModule.TsGetNumber = function (key) {
            return this.mBooleanDictionary[key];
        };
        /**
         * 使用指定的Key设置一个字符串类型的值
         * @param key
         * @param value
         */
        DataModule.TsSetString = function (key, value) {
            this.mStringDictionary[key] = value;
        };
        /**
         * 使用指定的Key获取一个字符串类型的值
         * @param key
         */
        DataModule.TsGetString = function (key) {
            return this.mStringDictionary[key];
        };
        /**
         * 使用指定的Key设置一个布尔值类型的值
         * @param key
         * @param value
         */
        DataModule.TsSetBoolean = function (key, value) {
            this.mBooleanDictionary[key] = value;
        };
        /**
         * 使用指定的Key获取一个布尔值类型的值
         * @param key
         * @param value
         */
        DataModule.TsGetBoolean = function (key) {
            return this.mBooleanDictionary[key];
        };
        DataModule.mNumberDictionary = {};
        DataModule.mStringDictionary = {};
        DataModule.mBooleanDictionary = {};
        return DataModule;
    }());
    Iuker.DataModule = DataModule;
})(Iuker || (Iuker = {}));
//# sourceMappingURL=data_module.js.map