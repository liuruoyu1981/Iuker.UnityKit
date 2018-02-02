using System;
using System.Diagnostics;
using System.IO;
using Iuker.Common.Base;

namespace Iuker.Common.Utility
{
#if DEBUG
    /// <summary>
    ///命令行工具
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20170902 17:26:01")]
    [ClassPurposeDesc("命令行工具", "命令行工具")]
#endif
    public static class CmdUtility
    {
        private static Action<string> mPrintAction;

        /// <summary>
        /// 执行Dos命令列表
        /// </summary>
        /// <param name="printAction"></param>
        /// <param name="cmdArray"></param>
        public static void ExcuteDosCommand(Action<string> printAction, params string[] cmdArray)
        {
            mPrintAction = printAction;

            try
            {
                Process p = new Process();
                p.StartInfo.FileName = "cmd";
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardInput = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.CreateNoWindow = false;
                p.OutputDataReceived += sortProcess_OutputDataReceived;
                p.Start();
                StreamWriter cmdWriter = p.StandardInput;
                p.BeginOutputReadLine();
                foreach (var cmd in cmdArray)
                {
                    cmdWriter.WriteLine(cmd);
                }
                cmdWriter.Close();
                p.WaitForExit();
                p.Close();
            }
            catch (Exception ex)
            {
                Debuger.LogError("执行命令失败，请检查输入的命令是否正确！异常信息为：" + ex.Message);
            }
        }

        private static void sortProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!String.IsNullOrEmpty(e.Data))
            {
                mPrintAction(e.Data);
            }
        }

        /// <summary>  
        /// 执行注册表导入  
        /// </summary>  
        /// <param name="regPath">注册表文件路径</param>  
        public static void ExecuteReg(string regPath)
        {
            if (File.Exists(regPath))
            {
                regPath = @"""" + regPath + @"""";
                Process.Start("regedit", string.Format("/s {0}", regPath));
            }
        }







    }
}
