namespace Iuker {

    /**
 * 日志调试器
 */
    export class Debug {

        /**
         * 输出一条普通类型日志
         * @param message
         */
        public static Log(message: string): void {
            Debuger_Log(message);
        }

        /**
      * 输出一条警告类型日志
      * @param message
      */
        public static LogWarning(message: string): void {
            Debuger_LogWarning(message);
        }


        /**
         * 输出一条错误类型日志
         * @param message
         */
        public static LogError(message: string): void {
            Debuger_LogError(message);
        }

        /**
      * 输出一条异常类型日志，该函数会用传入的字符串构建一个异常实例然后抛出。
      * @param message
      */
        public static LogException(message: string): void {
            Debuger_LogException(message);
        }




    }




}