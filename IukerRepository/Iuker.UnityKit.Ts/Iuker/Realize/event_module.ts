namespace Iuker {

    /**
     * 事件模块
     */
    export class EventModule {

        /**
         * 注册一个无需数据的事件处理器
         * @param eventId 事件码
         * @param jsValue Javascript对象
         * @param num 执行次数
         */
        public static WatchEvent(eventId: string, jsValue: any, num: number = -1): void {

            Iuker_EventModule_WatchEventByJint(eventId, jsValue, num);

        }

        /**
         * 注册一个需要数据的事件处理器
         * @param eventId 事件码
         * @param jsValue Javascript
         * @param num 执行次数
         */
        public static WatchEventAsData(eventId: string, jsValue: any, num: number = -1): void {

            Iuker_EventModule_WatchEventByJint(eventId, jsValue, num);

        }

        /**
         * 触发一个事件
         * @param eventId 事件码
         * @param jsValue 事件完成回调函数
         * @param eventData 事件数据
         */
        public static IssEvent(eventId: string, jsValue: any, eventData: any): void {

            Iuker_Event_IssueEventByJint(eventId, jsValue, eventData);
        }



    }
}