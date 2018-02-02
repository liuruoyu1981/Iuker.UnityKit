var Iuker;
(function (Iuker) {
    var MusicModule = /** @class */ (function () {
        function MusicModule() {
        }
        /**
        * 播放音乐。
        * @param {string} name - 要播放的音乐资源名。
        */
        MusicModule.Play = function (name, isCache, isLoop) {
            if (isCache === void 0) { isCache = true; }
            if (isLoop === void 0) { isLoop = true; }
            Iuker_MusicModule_Play(name, isCache, isLoop);
        };
        MusicModule.Stop = function () {
            Iuker_MusicModule_Stop();
        };
        MusicModule.ChangeVolume = function (value) {
            Iuker_MusicModule_ChangeVolume(value);
        };
        MusicModule.Pause = function () {
            Iuker_MusicModule_Pause();
        };
        MusicModule.Open = function () {
            Iuker_MusicModule_Open();
        };
        Object.defineProperty(MusicModule.prototype, "Volume", {
            get: function () {
                return Iuker_MusicModule_Volume();
            },
            enumerable: true,
            configurable: true
        });
        return MusicModule;
    }());
    Iuker.MusicModule = MusicModule;
})(Iuker || (Iuker = {}));
//# sourceMappingURL=music_module.js.map