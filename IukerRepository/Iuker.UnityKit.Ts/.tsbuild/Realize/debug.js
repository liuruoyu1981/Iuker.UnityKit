var Iuker;
(function (Iuker) {
    /**
 * 日志调试器
 */
    var Debug = /** @class */ (function () {
        function Debug() {
        }
        /**
         * 输出一条普通类型日志
         * @param message
         */
        Debug.Log = function (message) {
            Debuger_Log(message);
        };
        /**
      * 输出一条警告类型日志
      * @param message
      */
        Debug.LogWarning = function (message) {
            Debuger_LogWarning(message);
        };
        /**
         * 输出一条错误类型日志
         * @param message
         */
        Debug.LogError = function (message) {
            Debuger_LogError(message);
        };
        /**
      * 输出一条异常类型日志，该函数会用传入的字符串构建一个异常实例然后抛出。
      * @param message
      */
        Debug.LogException = function (message) {
            Debuger_LogException(message);
        };
        return Debug;
    }());
    Iuker.Debug = Debug;
})(Iuker || (Iuker = {}));
//# sourceMappingURL=debug.js.map