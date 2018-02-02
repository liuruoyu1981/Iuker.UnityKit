/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/02/26 13:08:38
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

using System.Collections.Generic;
using Iuker.Common.Base.Enums;
using Iuker.UnityKit.Run.Base;
using Iuker.UnityKit.Run.Module.Asset;
using UnityEngine;

namespace Iuker.UnityKit.Run.Module.Music
{
    public class DefaultU3dModule_Music : AbsU3dModule, IU3dMusicModule
    {
        public override string ModuleName
        {
            get
            {
                return ModuleType.Music.ToString();
            }
        }

        private readonly DefaultU3dModule_Music.MusicSupportComponent musicSupport = MusicSupportComponent.Init();
        private string currentMusic;

        protected override void onFrameInited()
        {
            assetModule = U3DFrame.GetModule<IU3dAssetModule>();
            mCurrentVolume = musicSupport.AudioSource.volume = U3DFrame.UnityAppConfig.ProjectBaseConfig.MusicVolume;
        }

        private IU3dAssetModule assetModule;

        private readonly Dictionary<string, MusicRef> m_MusicRefs = new Dictionary<string, MusicRef>();

        /// <summary>
        /// 当前音量
        /// </summary>
        private float mCurrentVolume;

        /// <summary>
        /// 释放指定音乐的内存占用
        /// </summary>
        /// <param name="name"></param>
        public void DestroyMusic(string name)
        {

        }

        public void Play(string musicName, bool isCache = true, bool isLoop = true)
        {
            if (musicName == currentMusic) return;

            if (!m_MusicRefs.ContainsKey(musicName))
            {
                var musicRef = assetModule.LoadMusic(musicName);
                PlayMusic(musicRef, musicName, isLoop);

                if (isCache)
                {
                    m_MusicRefs.Add(musicName, musicRef);
                }
                else
                {
                    musicRef.Destroy();
                }
            }
            else
            {
                PlayMusic(m_MusicRefs[musicName], musicName, isLoop);
            }
        }

        void PlayMusic(MusicRef mRef, string musicName, bool isLp = true)
        {
            musicSupport.AudioSource.clip = mRef.Asset;
            musicSupport.AudioSource.loop = isLp;
            musicSupport.AudioSource.Play();
            currentMusic = musicName;
        }

        public void Stop()
        {
            musicSupport.AudioSource.Stop();
        }

        public void ChangeVolume(float value)
        {
            mCurrentVolume = musicSupport.AudioSource.volume = value;
        }

        public void Pause()
        {
            musicSupport.AudioSource.Pause();
        }

        public void Open()
        {
            musicSupport.AudioSource.Play();
        }

        /// <summary>
        /// 获得当前背景音乐音量
        /// </summary>
        public float Volume
        {
            get
            {
                return musicSupport.AudioSource.volume;
            }
        }

        /// <summary>
        /// 音乐播放支持组件
        /// </summary>
        [RequireComponent(typeof(AudioSource))]
        // ReSharper disable once ClassNeverInstantiated.Global
        public sealed class MusicSupportComponent : MonoSingleton<MusicSupportComponent>
        {
            public AudioSource AudioSource { get; private set; }

            public static MusicSupportComponent Init()
            {
                Instance.AudioSource = Instance.GetComponent<AudioSource>();
                return Instance;
            }
        }

    }
}
