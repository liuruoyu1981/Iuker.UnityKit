/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 6/29/2017 14:28
Email: 35490136@qq.com
QQCode: 35490136
CreateNote: 通用调用工具
***********************************************************************************************/


/****************************************修改日志***********************************************
1. 修改日期： 修改人： 修改内容：
2. 修改日期： 修改人： 修改内容：
3. 修改日期： 修改人： 修改内容：
4. 修改日期： 修改人： 修改内容：
5. 修改日期： 修改人： 修改内容：
****************************************修改日志***********************************************/

using System;
using System.Collections.Generic;
using Iuker.Common.Base;
using Iuker.Common.Module.Debugger;
using UnityEngine;

namespace Iuker.UnityKit.Run.Module.Debugger
{
    /// <summary>
    /// 默认日志输出器
    /// </summary>
#if DEBUG
    [ClassPurposeDesc(null, "默认的Unity日志输出器。")]
#endif
    public class DefaultU3dLogger : ILoger
    {
        /// <summary>
        /// 创建者
        /// </summary>
        public string Owner { get; private set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreatDate { get; private set; }

        /// <summary>
        /// 输出器是否已经初始化
        /// </summary>
        private bool mIsInited;

        /// <summary>
        /// 信息日志列表
        /// </summary>
        public List<U3dLog> Infos { get; private set; }

        /// <summary>
        /// 警告日志列表
        /// </summary>
        public List<U3dLog> Warnings { get; private set; }

        /// <summary>
        /// 错误日志列表
        /// </summary>
        public List<U3dLog> Errors { get; private set; }

        /// <summary>
        /// 异常日志列表
        /// </summary>
        public List<U3dLog> Exceptions { get; private set; }

        public void Info(string message)
        {
            Infos.Add(new U3dLog().SetMessage(message).SetLogType(LogType.Log));
        }

        public void Warning(string message)
        {
            Warnings.Add(new U3dLog().SetMessage(message).SetLogType(LogType.Warning));
        }

        public void Error(string message)
        {
            Errors.Add(new U3dLog().SetMessage(message).SetLogType(LogType.Error));
        }

        public void Exception(string exception)
        {
            Errors.Add(new U3dLog().SetMessage(exception).SetLogType(LogType.Exception));
        }

        public ILoger Init(string creater)
        {
            if (mIsInited == false)
            {
                Owner = creater;
                CreatDate = DateTime.Now;
                mIsInited = true;
            }

            return this;
        }
    }
}