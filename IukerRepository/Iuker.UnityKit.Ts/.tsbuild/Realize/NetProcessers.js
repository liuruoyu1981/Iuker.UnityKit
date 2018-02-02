var Iuker;
(function (Iuker) {
    /**
     * 网络通信处理器集合
     */
    var NetProcessers = /** @class */ (function () {
        function NetProcessers() {
        }
        /**
         * 通信请求处理器集合
         */
        NetProcessers.Requesters = {};
        /**
         * 通信答复处理器集合
         */
        NetProcessers.Responsers = {};
        return NetProcessers;
    }());
    Iuker.NetProcessers = NetProcessers;
})(Iuker || (Iuker = {}));
//# sourceMappingURL=netprocessers.js.map