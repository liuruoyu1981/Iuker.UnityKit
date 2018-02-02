/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/03/04 17:12:55
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
using Iuker.Common;

namespace Iuker.UnityKit.Run.Module.Video
{
    public interface IU3dVideoModule : IModule
    {
        /// <summary>
        /// 播放视频
        /// </summary>
        /// <param name="videoPath">视频绝对路径</param>
        /// <param name="onOver">视频播放结束处理委托</param>
        /// <param name="onStart">视频播放开始处理委托</param>
        void Play(string videoPath, Action onOver, Action onStart = null);

        /// <summary>
        /// 停止播放视频
        /// </summary>
        void Stop();
    }
}
