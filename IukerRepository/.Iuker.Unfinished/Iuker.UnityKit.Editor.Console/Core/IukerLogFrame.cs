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

namespace Iuker.UnityKit.Editor.Console.Core
{
    [Serializable]
    public class IukerLogFrame
    {
        public string FrameInformation { get; private set; }

        public string File { get; private set; }

        public int Line { get; private set; }

        public IukerLogFrame(string frameInformation)
        {
            FrameInformation = frameInformation;

            int index = frameInformation.IndexOf("(at", StringComparison.Ordinal);
            if (index == -1)
            {
                return;
            }

            index += 4;
            int begFile = index;
            while (index < frameInformation.Length && frameInformation[index] != ':')
                index++;

            int endFile = index - 1;

            int begLine = index;
            while (index < frameInformation.Length && frameInformation[index] != ')')
                index++;
            int endLine = index - 1;

            int line;
            if (index + 1 != frameInformation.Length ||
                !int.TryParse(frameInformation.Substring(begLine, endLine - begLine + 1), out line))
            {
                File = "";
                Line = 0;
                return;
            }

            File = frameInformation.Substring(begFile, endFile - begFile + 1);
            Line = line;
        }

        public override string ToString()=> $"[IukerLogFrame: FrameInformation={FrameInformation}, File={File}, Line={Line}]";
    }
}