/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/02/26 12:46:08
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

using System;
using Iuker.Common.Base.Enums;
using Iuker.UnityKit.Run.Base;
using Iuker.UnityKit.Run.Base.Enums;
using UnityEngine;

namespace Iuker.UnityKit.Run.Module.Video
{
    public class DefaultU3dModule_Video : AbsU3dModule, IU3dVideoModule
    {
        public override string ModuleName { get { return ModuleType.Video.ToString(); } }
        private Action onPlayOver;
        private Action onPlayStart;
        public PlayStatus PlayStatus { get; private set; }

        public void Play(string videoPath, Action onOver, Action onStart = null)
        {
#if PcPlatform
            PlayByPc(videoPath, onOver, onStart);
#endif

#if  MobilePlatform
            PlayByMobile(videoPath);
#endif
        }

        public void Stop()
        {
#if PcPlatform
            StopByPc();
#endif

#if MobilePlatform
            StopByMobile();
#endif
        }

        // 编辑器或者PC、MAC、LINUX
#if PcPlatform

        public DefaultU3dModule_Video()
        {
            VideoSupportComponent.Init(ref videoSupportComponent);
        }

        private readonly VideoSupportComponent videoSupportComponent;
        private MovieTexture movieTexture;
        private AudioClip videoAudioClip;
        private float scale;
        private float playWidth;
        private float playHeight;
        private float playX;

        private void PlayByPc(string videoPath, Action onOver, Action onStart = null)
        {
            if (PlayStatus == PlayStatus.Playing || PlayStatus == PlayStatus.Pause) { Debuger.LogWarning("视频模块使用中"); return; }
            movieTexture = Resources.Load(videoPath) as MovieTexture;
            if (movieTexture == null) { Debuger.LogError("视频加载失败！"); return; }
            scale = (float)Screen.height / movieTexture.height;
            playWidth = movieTexture.width * scale;
            playHeight = movieTexture.height * scale;
            playX = (Screen.width - playWidth) / 2;

            videoAudioClip = movieTexture.audioClip;
            if (videoAudioClip == null) { Debuger.LogWarning("音频片段为空，无法播放!"); return; }

            videoAudioClip = movieTexture.audioClip;
            onPlayOver = onOver;
            onPlayStart = onStart;

            videoSupportComponent.AudioSource.clip = videoAudioClip;
            onPlayStart?.Invoke();
            movieTexture.Play();
            videoSupportComponent.AudioSource.Play();
            PlayStatus = PlayStatus.Playing;
            U3DFrame.EventModule.WatchU3dAppEvent(AppEventType.Update.ToString(), StopCheck);
            U3DFrame.EventModule.WatchU3dAppEvent(AppEventType.OnGUI.ToString(), PlayUseOnGUI);
        }

        private void DefaultOnOver()
        {
            // 清除对电影材质和音频的引用
            movieTexture = null;
            videoAudioClip = null;
            videoSupportComponent.AudioSource.clip = null;

            onPlayOver?.Invoke();
        }

        public void Pause()
        {
            U3DFrame.EventModule.RemoveAppEvent(AppEventType.Update.ToString(), StopCheck);
            U3DFrame.EventModule.RemoveAppEvent(AppEventType.OnGUI.ToString(), PlayUseOnGUI);
            movieTexture.Pause();
            videoSupportComponent.AudioSource.Pause();
            PlayStatus = PlayStatus.Pause;
        }

        private void StopByPc()
        {
            U3DFrame.EventModule.RemoveAppEvent(AppEventType.Update.ToString(), StopCheck);
            U3DFrame.EventModule.RemoveAppEvent(AppEventType.OnGUI.ToString(), PlayUseOnGUI);
            PlayStatus = PlayStatus.Over;
            movieTexture.Stop();
            videoSupportComponent.AudioSource.Stop();
            DefaultOnOver();
        }

        private void PlayUseOnGUI()
        {
            GUI.DrawTexture(new Rect(playX, 0, playWidth, playHeight), movieTexture, ScaleMode.ScaleAndCrop);
        }

        private void StopCheck()
        {
            if (movieTexture == null) return;
            if (movieTexture.isPlaying) return;
            U3DFrame.EventModule.WatchU3dAppEvent(AppEventType.Update.ToString(), StopCheck);
            PlayStatus = PlayStatus.Over;
            StopByPc();
        }

#endif

#if MobilePlatform

        private void PlayByMobile(string videoPath, Action onOver, Action onStart = null)
        {
            onPlayOver = onOver;
            onPlayStart = onStart;

             U3DFrame.StartCoroutine(CoroutinePlay(videoPath));
        }

        private IEnumerator CoroutinePlay(string videoPath)
        {
            if (onPlayStart != null) onPlayStart();
            Handheld.PlayFullScreenMovie(videoPath, Color.black, FullScreenMovieControlMode.CancelOnInput, FullScreenMovieScalingMode.AspectFill);
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();

            if (onPlayOver != null) onPlayOver();
        }

        private void StopByMobile()
        {
        }

#endif


        /// <summary>
        /// 视频播放支持组件
        /// </summary>
        [RequireComponent(typeof(AudioSource))]
        private class VideoSupportComponent : MonoSingleton<VideoSupportComponent>
        {
            private static bool isInited;
            public AudioSource AudioSource { get; private set; }

            public static void Init(ref VideoSupportComponent videoSupport)
            {
                if (isInited) return;
                videoSupport = Instance;
                Instance.AudioSource = Instance.GetComponent<AudioSource>();
                isInited = true;
            }
        }

    }
}
