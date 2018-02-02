namespace Iuker {

    export class MusicModule {

        /**
        * 播放音乐。
        * @param {string} name - 要播放的音乐资源名。
        */
        public static Play(name: string, isCache: boolean = true, isLoop: boolean = true): void {
            Iuker_MusicModule_Play(name, isCache, isLoop);
        }

        public static Stop(): void {
            Iuker_MusicModule_Stop();
        }

        public static ChangeVolume(value: number): void {
            Iuker_MusicModule_ChangeVolume(value);
        }

        public static Pause(): void {
            Iuker_MusicModule_Pause();
        }

        public static Open(): void {
            Iuker_MusicModule_Open();
        }

        get Volume(): number {
            return Iuker_MusicModule_Volume();
        }
    }
}