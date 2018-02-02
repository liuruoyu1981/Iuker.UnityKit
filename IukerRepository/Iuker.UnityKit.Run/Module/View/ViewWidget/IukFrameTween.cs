/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 7/8/2017 15:44
Email: 35490136@qq.com
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
using Iuker.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Iuker.UnityKit.Run.Module.View.ViewWidget
{
    /// <summary>
    /// 帧动画
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class IukFrameTween : MonoBehaviour
    {
        private Image ImageSource;

        /// <summary>
        /// 当前动画帧索引
        /// </summary>
        private int mCurFrame = 0;
        private float mDelta = 0;

        /// <summary>
        /// 动画速率
        /// </summary>
        public float FPS = 5;

        /// <summary>
        /// 精灵列表
        /// </summary>
        public List<Sprite> SpriteFrames;

        /// <summary>
        /// 当前是否正在播放
        /// </summary>
        public bool IsPlaying = false;

        /// <summary>
        /// 向前播放
        /// </summary>
        public bool IsFoward = true;

        /// <summary>
        /// 是否自动播放
        /// </summary>
        public bool AutoPlay = true;
        public bool Loop = false;

        public int FrameCount
        {
            get
            {
                return SpriteFrames.Count;
            }
        }

        void Awake()
        {
            ImageSource = GetComponent<Image>();
        }

        void Start()
        {
            Extensions.IfElse(AutoPlay, Play, () => { IsPlaying = false; });
        }

        private void SetSprite()
        {
            ImageSource.sprite = SpriteFrames[mCurFrame];
        }

        public void Play()
        {
            IsPlaying = IsFoward = true;
        }

        public void PlayReverse()
        {
            IsFoward = !IsPlaying.Assign(true);
        }

        void Update()
        {
            if (!IsPlaying || 0 == FrameCount) { return; }

            mDelta += Time.deltaTime;
            if (mDelta > 1 / FPS)
            {
                mDelta = 0;
                UpdateFrameCount(); //  更新当前动画帧索引
                mCurFrame.GreaterEqual(FrameCount).TrueDo(PlayOneOver);
                mCurFrame.Less(FrameCount).TrueDo(() =>
                {
                    SetSprite();
                    ImageSource.SetNativeSize();
                });
            }
        }

        private void PlayOneOver()
        {
            if (Loop)
            {
                mCurFrame = 0;
            }
            else
            {
                IsPlaying = false;
                Close();
            }
        }

        private Sprite mCurrentSprite
        {
            get
            {
                return SpriteFrames[mCurFrame];
            }
        }

        private void UpdateFrameCount()
        {
            Extensions.IfElse(IsFoward, () => { mCurFrame++; }, () => { mCurFrame--; });
        }

        private void Close()
        {
            gameObject.SetActive(false);
        }

        public void Pause()
        {
            IsPlaying = false;
        }

        public void Resume()
        {
            Extensions.ExTrueDo(!IsPlaying, () => { IsPlaying = true; });
        }

        public void Stop()
        {
            mCurFrame = 0;
            SetSprite();
            IsPlaying = false;
        }

        public void Rewind()
        {
            mCurFrame = 0;
            SetSprite();
            Play();
        }
    }
}
