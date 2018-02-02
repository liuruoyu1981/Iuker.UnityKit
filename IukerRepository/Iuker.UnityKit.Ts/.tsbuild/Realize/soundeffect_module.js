var Iuker;
(function (Iuker) {
    var SoundEffectModule = /** @class */ (function () {
        function SoundEffectModule() {
        }
        /**
         *  播放音效
         *  @param {string} name - 要播放的音效资源名。
        */
        SoundEffectModule.Play = function (name) {
            Iuker_SoundEffectModule_DirectPlay(name);
        };
        SoundEffectModule.ChangeVolume = function (value) {
            Iuker_SoundEffectModule_ChangeVolume(value);
        };
        SoundEffectModule.DirectPlay = function (clip) {
            Iuker_SoundEffectModule_DirectPlay(clip);
        };
        SoundEffectModule.Close = function () {
            Iuker_SoundEffectModule_Close();
        };
        SoundEffectModule.Open = function () {
            Iuker_SoundEffectModule_Open();
        };
        return SoundEffectModule;
    }());
    Iuker.SoundEffectModule = SoundEffectModule;
})(Iuker || (Iuker = {}));
//# sourceMappingURL=soundeffect_module.js.map