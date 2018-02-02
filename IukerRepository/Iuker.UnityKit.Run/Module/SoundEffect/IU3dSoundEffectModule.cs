/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/03/04 17:16:15
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
using UnityEngine;

namespace Iuker.UnityKit.Run.Module.SoundEffect
{
    public interface IU3dSoundEffectModule : IModule
    {
        /// <summary>
        /// 改变音效音量
        /// </summary>
        /// <param name="value"></param>
        void ChangeVolume(float value);

        /// <summary>
        /// 播放音效
        /// </summary>
        bool Play(string soundEffectName);

        /// <summary>
        /// 直接播放音效
        /// </summary>
        /// <param name="sound"></param>
        void Play(AudioClip sound);

        /// <summary>
        /// 关闭音效
        /// </summary>
        void Close();

        /// <summary>
        /// 开启音效
        /// </summary>
        void Open();

        /// <summary>
        /// 获得当前音效音量
        /// </summary>
        float Volume { get; }

        string GetSoundAssetName(string key);
    }
}
