var Iuker;
(function (Iuker) {
    /**
     * 业务逻辑管理器集合
     */
    var LogicManagers = /** @class */ (function () {
        function LogicManagers() {
        }
        LogicManagers.GetManager = function (name) {
            var manager = this.Managers[name];
            return manager;
        };
        LogicManagers.Managers = {};
        return LogicManagers;
    }());
    Iuker.LogicManagers = LogicManagers;
})(Iuker || (Iuker = {}));
//# sourceMappingURL=logic_managers.js.map