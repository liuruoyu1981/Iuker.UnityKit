/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 6/16/2017 19:41
Email: liuruoyu1981@gmail.com
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
using System.Collections.Generic;

namespace Iuker.UnityKit.Editor.Console.Core
{
    [Serializable]
    public class IukerLog
    {
        public string Message { get; private set; }

        public string MessageLower { get; private set; }

        public string File { get; private set; }

        public int Line { get; private set; }

        public int Mode { get; private set; }

        public int InstanceID { get; private set; }

        public List<IukerLogFrame> StackTrace { get; private set; }

        public void SetMessage(string condition)
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

        public void SetFile(string file) => File = file;

        public void SetLine(int line) => Line = line;

        public void SetMode(int mode) => Mode = mode;

        public void SetStackTrace(string condition)
        {
            StackTrace = new List<IukerLogFrame>();

            if (string.IsNullOrEmpty(condition)) return;

            var splits = condition.Split('\n');
            foreach (string t in splits)
            {
                if (string.IsNullOrEmpty(t))
                    continue;
                StackTrace.Add(new IukerLogFrame(t));
            }
        }

        public void SetInstanceID(int instanceID) => InstanceID = instanceID;

        public void FilterStackTrace(List<string> prefixs)
        {
            var newStackTrace = new List<IukerLogFrame>(StackTrace.Count);
            foreach (var frame in StackTrace)
            {
                bool hasPrefix = false;
                foreach (var prefix in prefixs)
                {
                    if (frame.FrameInformation.StartsWith(prefix))
                    {
                        hasPrefix = true;
                        break;
                    }
                }
                if (!hasPrefix)
                    newStackTrace.Add(frame);
            }
            StackTrace = newStackTrace;
        }

        public override string ToString() => $"[IukerLog: Message={Message}, File={File}, Line={Line}, Mode={Mode}, StackTrace={StackTrace}]";
    }
}