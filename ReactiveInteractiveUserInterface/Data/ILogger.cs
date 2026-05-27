using System;

namespace TP.ConcurrentProgramming.Data
{
    public enum LogLevel
    {
        Emergency = 0,
        Alert = 1,
        Critical = 2,
        Error = 3,
        Warning = 4,
        Notice = 5,
        Info = 6,
        Debug = 7
    }

    internal interface ILogger : IDisposable
    {
        void Log(LogLevel level, string message);
    }
}