/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/03/04 17:14:29
Email: 35490136@qq.com
QQCode: 35490136
CreateNote: 
***********************************************************************************************/


/****************************************修改日志***********************************************
1. 修改日期： 修改人： 修改内容：
2. 修改日期： 修改人： 修改内容：
3. 修改日期： 修改人： 修改内容：
4. 修改日期： 修改人： 修改内容：
5. 修改日期： 修改人： 修改内容：
****************************************修改日志***********************************************/

using Iuker.Common;

namespace Iuker.UnityKit.Run.Module.Music
{
    public interface IU3dMusicModule : IModule
    {
        /// <summary>
        /// 播放音乐
        /// </summary>
        /// <param name="musicName">要播放的目标音乐名</param>
        /// <param name="isCache">是否缓存目标音乐资源，如果为真，则在内存占用没有到达当前设备临界值时将会进行缓存处理。</param>
        /// <param name="isLoop">是否循环播放，默认为真。</param>
        void Play(string musicName, bool isCache = true, bool isLoop = true);

        /// <summary>
        /// 停止音乐播放
        /// </summary>
        void Stop();

        /// <summary>
        /// 修改音乐音量
        /// </summary>
        void ChangeVolume(float value);

        /// <summary>
        /// 暂停音乐播放
        /// </summary>
        void Pause();

        /// <summary>
        /// 开启音乐播放
        /// </summary>
        void Open();

        /// <summary>
        /// 获得当前背景音乐音量
        /// </summary>
        float Volume { get; }

        /// <summary>
        /// 释放指定音乐的内存占用
        /// </summary>
        /// <param name="name"></param>
        void DestroyMusic(string name);

    }
}
