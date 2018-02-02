using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// 通用调用工具
/// </summary>
public class Debuger
{
    private static bool Enable { get; set; }
    private static Action<string> LogHander { get; set; }
    private static Action<string> WarningHander { get;  set; }
    private static Action<string> ErrorHander { get;  set; }
    private static Action<Exception> ExceptionHander { get;  set; }

    public static void Init(Action<string> logHander, Action<string> warningHander,
        Action<string> errorHander, Action<Exception> exceptionHander = null)
    {
        Enable = true;
        LogHander = logHander;
        WarningHander = warningHander;
        ErrorHander = errorHander;
        ExceptionHander = exceptionHander;
    }

    private static readonly Debuger instance = new Debuger();

    public static void Log(string message)
    {
        instance.Info(message);
    }

    public static void LogWarning(string message)
    {
        instance.Warning(message);
    }

    public static void LogError(string message)
    {
        instance.Error(message);
    }

    public static void LogException(string message)
    {
        instance.Exception(message);
    }

    private bool mIsInited;
    public string Owner { get; private set; }
    public DateTime CreatDate { get; private set; }

    public void Info(string message)
    {
        if (Enable)
        {
            LogHander?.Invoke("C#: " + message);
        }
    }

    public void Warning(string message)
    {
        if (Enable)
        {
            WarningHander?.Invoke("C#: " + message);
        }
    }

    public void Error(string message)
    {
        if (Enable)
        {
            ErrorHander?.Invoke("C#: " + message);
        }
    }

    public void Exception(string exception)
    {
        if (Enable)
        {
            ExceptionHander?.Invoke(new Exception($"C#: {exception}"));
        }
    }
}
