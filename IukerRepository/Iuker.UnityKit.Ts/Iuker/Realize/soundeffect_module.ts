namespace Iuker {

    export class SoundEffectModule {

        /**
         *  播放音效
         *  @param {string} name - 要播放的音效资源名。
        */
        public static Play(name: string): void {
            Iuker_SoundEffectModule_DirectPlay(name);
        }

        public static ChangeVolume(value: number): void {

            Iuker_SoundEffectModule_ChangeVolume(value);
        }

        public static DirectPlay(clip: any): void {
            Iuker_SoundEffectModule_DirectPlay(clip);
        }

        public static Close(): void {

            Iuker_SoundEffectModule_Close();
        }

        public static Open(): void {

            Iuker_SoundEffectModule_Open();
        }

    }

}



