//  音乐模块

declare function Iuker_MusicModule_Play(name: string, isCahce: boolean, isLoop: boolean): void;
declare function Iuker_MusicModule_Stop(): void;
declare function Iuker_MusicModule_ChangeVolume(value: number): void;
declare function Iuker_MusicModule_Pause(): void;
declare function Iuker_MusicModule_Open(): void;
declare function Iuker_MusicModule_Volume(): number;