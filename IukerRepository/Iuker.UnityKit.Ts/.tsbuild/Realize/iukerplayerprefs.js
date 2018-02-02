var Iuker;
(function (Iuker) {
    var IukerPlayerPrefs = /** @class */ (function () {
        function IukerPlayerPrefs() {
        }
        IukerPlayerPrefs.SetInt = function (key, value) {
            IukerPlayerPrefs_SetInt(key, value);
        };
        IukerPlayerPrefs.GetInt = function (key) {
            return IukerPlayerPrefs_GetInt(key);
        };
        IukerPlayerPrefs.SetLong = function (key, value) {
            IukerPlayerPrefs_SetLong(key, value);
        };
        IukerPlayerPrefs.GetLong = function (key) {
            return IukerPlayerPrefs_GetLong(key);
        };
        IukerPlayerPrefs.SetString = function (key, value) {
            IukerPlayerPrefs_SetString(key, value);
        };
        IukerPlayerPrefs.GetString = function (key) {
            return IukerPlayerPrefs_GetString(key);
        };
        IukerPlayerPrefs.SetFloat = function (key, value) {
            IukerPlayerPrefs_SetFloat(key, value);
        };
        IukerPlayerPrefs.GetFloat = function (key) {
            return IukerPlayerPrefs_GetFloat(key);
        };
        return IukerPlayerPrefs;
    }());
    Iuker.IukerPlayerPrefs = IukerPlayerPrefs;
})(Iuker || (Iuker = {}));
//# sourceMappingURL=iukerplayerprefs.js.map