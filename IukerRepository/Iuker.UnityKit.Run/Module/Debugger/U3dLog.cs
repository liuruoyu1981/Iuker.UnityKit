/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 6/29/2017 14:10
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
using UnityEngine;

namespace Iuker.UnityKit.Run.Module.Debugger
{
    /// <summary>
    /// unity日志
    /// </summary>
    [Serializable]
    public class U3dLog
    {
        public string Message { get; private set; }

        public string MessageLower { get; private set; }

        public string File { get; private set; }

        public int Line { get; private set; }

        public int Mode { get; private set; }

        public LogType LogType { get; private set; }

        public int InstanceID { get; private set; }

        public void SetCondition(string condition)
        {
            if (string.IsNullOrEmpty(condition))
            {
                return;
            }

            int index = 0;
            while (index < condition.Length && condition[index++] != '\n')
            {
                Message = condition.Substring(0, index - 1);
            }
            MessageLower = Message.ToLower();
        }

        public U3dLog SetMessage(string message)
        {
            Message = message;
            return this;
        }

        public U3dLog SetLogType(LogType logType)
        {
            LogType = logType;
            return this;
        }

        public void SetFile(string file)
        {
            File = file;
        }

        public void SetLine(int line)
        {
            Line = line;
        }

        public void SetMode(int mode)
        {
            Mode = mode;
        }
    }
}