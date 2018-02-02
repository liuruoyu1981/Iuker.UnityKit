var Iuker;
(function (Iuker) {
    /**
     * 事件模块
     */
    var EventModule = /** @class */ (function () {
        function EventModule() {
        }
        /**
         * 注册一个无需数据的事件处理器
         * @param eventId 事件码
         * @param jsValue Javascript对象
         * @param num 执行次数
         */
        EventModule.WatchEvent = function (eventId, jsValue, num) {
            if (num === void 0) { num = -1; }
            Iuker_EventModule_WatchEventByJint(eventId, jsValue, num);
        };
        /**
         * 注册一个需要数据的事件处理器
         * @param eventId 事件码
         * @param jsValue Javascript
         * @param num 执行次数
         */
        EventModule.WatchEventAsData = function (eventId, jsValue, num) {
            if (num === void 0) { num = -1; }
            Iuker_EventModule_WatchEventByJint(eventId, jsValue, num);
        };
        /**
         * 触发一个事件
         * @param eventId 事件码
         * @param jsValue 事件完成回调函数
         * @param eventData 事件数据
         */
        EventModule.IssEvent = function (eventId, jsValue, eventData) {
            Iuker_Event_IssueEventByJint(eventId, jsValue, eventData);
        };
        return EventModule;
    }());
    Iuker.EventModule = EventModule;
})(Iuker || (Iuker = {}));
//# sourceMappingURL=event_module.js.map