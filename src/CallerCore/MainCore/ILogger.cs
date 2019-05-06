using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallerCore.MainCore
{

    public enum LogLevel
    {
        All = 5,
        Debug = 4,
        Info = 3,
        Warning = 2,
        Error = 1,
        None = 0
    }

    public interface ILogger
    {
        void Log(LogLevel logLevel, string message);
        void Log(LogLevel logLevel, Exception e, string functionName);
        void Debug(string s);
        void Debug(string format, params object[] args);
        void Info(string s);
        void Info(string format, params object[] args);
        void Warning(string s);
        void Warning(string format, params object[] args);
        void Error(Exception e);
        void Error(Exception e, string functionName);
        void Error(string s);
        void Error(string format, params object[] args);
        void WriteInfo(string level, string message);
        void WriteLogToFile(string message);
        void WriteLogToFile(List<string> messages);
        void WriteError(string level, Exception e);
        void InitLog(IInfoContext info);
        void SetLogLevel(LogLevel level);
        int RepeatErrorLimit { get; set; }

    }
}
