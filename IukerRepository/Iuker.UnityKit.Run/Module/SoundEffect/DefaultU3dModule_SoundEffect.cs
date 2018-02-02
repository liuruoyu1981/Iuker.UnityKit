/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/02/26 13:15:13
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
using System.Linq;
using Iuker.Common;
using Iuker.Common.Base.Enums;
using Iuker.UnityKit.Run.Base;
using Iuker.UnityKit.Run.Base.Config.Develop;
using Iuker.UnityKit.Run.Module.Asset;
using UnityEngine;

namespace Iuker.UnityKit.Run.Module.SoundEffect
{
    public class DefaultU3dModule_SoundEffect : AbsU3dModule, IU3dSoundEffectModule
    {
        /// <summary>
        /// 模块名
        /// </summary>
        public override string ModuleName
        {
            get
            {
                return ModuleType.SoundEffect.ToString();
            }
        }

        /// <summary>
        /// 音效支持Mono组件（单例）
        /// </summary>
        private readonly SoundEffcetSupportComponent mSoundEffcetSupport;

        /// <summary>
        /// 音效表数据字典
        /// </summary>
        private readonly Dictionary<string, LD_SoundEffectTable> mSoundTableDictionary = new Dictionary<string, LD_SoundEffectTable>();

        /// <summary>
        /// 音效播放是否开启
        /// </summary>
        private bool mIsClose;

        private IU3dAssetModule mAssetModule;

        public DefaultU3dModule_SoundEffect()
        {
            SoundEffcetSupportComponent.Init(ref mSoundEffcetSupport);
        }

        protected override void OnHotUpdateComplete()
        {
            base.OnHotUpdateComplete();

            mSoundEffcetSupport.AudioSource.volume = U3DFrame.UnityAppConfig.ProjectBaseConfig.SoundEffectVolume;
            mAssetModule = U3DFrame.AssetModule;
            InitSoundTablesData();
        }

        /// <summary>
        /// 修改当前音效音量
        /// </summary>
        /// <param name="value"></param>
        public void ChangeVolume(float value)
        {
            mSoundEffcetSupport.AudioSource.volume = value;
        }

        private readonly Dictionary<string, SoundRef> mSoundRefs = new Dictionary<string, SoundRef>();

        public bool Play(string soundEffectName)
        {
            if (mIsClose) return false;

            if (mSoundRefs.ContainsKey(soundEffectName))
            {
                mSoundEffcetSupport.AudioSource.PlayOneShot(mSoundRefs[soundEffectName].Asset);
                return true;
            }

            var soundRef = mAssetModule.LoadSound(soundEffectName);
            if (soundRef == null)
            {
                Debug.Log(string.Format("目标音效{0}加载失败！", soundEffectName));
                return false;
            }

            mSoundEffcetSupport.AudioSource.PlayOneShot(soundRef.Asset);
            mSoundRefs.Add(soundEffectName, soundRef);

            return true;
        }

        public void Play(AudioClip sound)
        {
            if (sound == null) return;

            mSoundEffcetSupport.AudioSource.PlayOneShot(sound);
        }

        /// <summary>
        /// 初始化所有目标子项目的音效表数据
        /// </summary>
        private void InitSoundTablesData()
        {
            var sonProjects = RootConfig.GetCurrentProject().AllSonProjects;
            foreach (var sonProject in sonProjects)
            {
                var soundsTxt = U3DFrame.LocalDataModule.GetAllRecord<LD_SoundEffectTable>(sonProject.SoundTableResPath);
                if (soundsTxt == null) continue;

                var sounds = soundsTxt.ToDictionary(table => table.ComponentRootName + "_" + table.ComponentName + "_" + table.ActionName);
                mSoundTableDictionary.Combin(sounds);
            }
        }

        public string GetSoundAssetName(string key)
        {
            return mSoundTableDictionary.ContainsKey(key) ? mSoundTableDictionary[key].SoundEffectName : null;
        }

        /// <summary>
        /// 关闭音效播放
        /// </summary>
        public void Close()
        {
            mIsClose = true;
        }

        /// <summary>
        /// 开启音效播放
        /// </summary>
        public void Open()
        {
            mIsClose = false;
        }

        /// <summary>
        /// 获得当前音效音量
        /// </summary>
        public float Volume
        {
            get
            {
                return mSoundEffcetSupport.AudioSource.volume;
            }
        }

        /// <summary>
        /// 音效支持组件
        /// </summary>
        [RequireComponent(typeof(AudioSource))]
        // ReSharper disable once ClassNeverInstantiated.Global
        public sealed class SoundEffcetSupportComponent : MonoSingleton<SoundEffcetSupportComponent>
        {
            private static bool isInited;
            public AudioSource AudioSource { get; private set; }

            public static void Init(ref SoundEffcetSupportComponent rySoundEffcetSupportComponent)
            {
                if (isInited) return;
                rySoundEffcetSupportComponent = Instance;
                Instance.AudioSource = Instance.GetComponent<AudioSource>();
                isInited = true;
            }
        }
    }
}
