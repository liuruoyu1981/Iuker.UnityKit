//  运行时持久化

declare function IukerPlayerPrefs_SetInt(key: string, newData: number): void;
declare function IukerPlayerPrefs_GetInt(key: string): number;

declare function IukerPlayerPrefs_SetLong(key: string, newData: number): void;
declare function IukerPlayerPrefs_GetLong(key: string): number;

declare function IukerPlayerPrefs_SetString(key: string, newData: string): void;
declare function IukerPlayerPrefs_GetString(key: string): string;

declare function IukerPlayerPrefs_SetFloat(key: string, newData: number): void;
declare function IukerPlayerPrefs_GetFloat(key: string): number;