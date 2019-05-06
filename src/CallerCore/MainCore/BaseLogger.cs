using System;
using System.Collections.Generic;
using System.Globalization;

namespace CallerCore.MainCore
{
    public static class ErrorObject
    {
        public static string Message { get; set; } = String.Empty;
        public static int CountError { get; set; } = 0;
    }

    public class BaseLogger : ILogger
    {

        protected object objSync = new object();
        public static BaseLogger Logger;
        private int logLevelValue;
        public virtual int RepeatErrorLimit { get; set; } = 100;
        protected IInfoContext info;
        public void InitLog(IInfoContext infoctx)
        {
            info = infoctx;
            Logger = this;
        }
        public void SetLogLevel(LogLevel level)
        {
            logLevelValue = (int)level;
        }

        public virtual void WriteInfo(string level, string message)
        {
            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow.ToString("u", CultureInfo.InvariantCulture)}]{message}");
        }

        public virtual void WriteError(string level, Exception message)
        {
            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow.ToString("u", CultureInfo.InvariantCulture)}]{message.StackTrace}");
        }

        public virtual void Log(LogLevel logLevel, string message)
        {
            if (logLevelValue >= (int)logLevel)
            {
                lock (objSync)
                {
                    WriteInfo(logLevel.ToString(), message);
                }
            }
        }


        public virtual void Log(LogLevel logLevel, Exception e, string functionName)
        {
            if (e.Message != ErrorObject.Message)
            {
                ErrorObject.Message = e.Message;
                ErrorObject.CountError = 0;
            }
            else
            {
                ErrorObject.CountError++;
            }

            if (ErrorObject.CountError <= RepeatErrorLimit)
            {
                var addError = functionName != null ? $"[{functionName}]" : string.Empty;
                if (ErrorObject.CountError == RepeatErrorLimit) addError = $"{addError}[*REPEATED*]";
                Log(logLevel, $"{addError}Exception:{e.Message} StackTrace:{e.StackTrace}");
            }
        }

        public virtual void WriteLogToFile(string message)
        {
            throw new NotImplementedException();
        }
        public void WriteLogToFile(List<string> messages)
        {
            throw new NotImplementedException();
        }

        public virtual void Debug(string message) { Log(LogLevel.Debug, message); }
        public virtual void Debug(string format, params object[] args) { Log(LogLevel.Debug, string.Format(format, args)); }
        public virtual void Info(string message) { Log(LogLevel.Info, message); }
        public virtual void Info(string format, params object[] args) { Log(LogLevel.Info, string.Format(format, args)); }
        public virtual void Warning(string message) { Log(LogLevel.Warning, message); }
        public virtual void Warning(string message, params object[] args) { Log(LogLevel.Warning, string.Format(message, args)); }
        public virtual void Error(Exception e) { Log(LogLevel.Error, e, null); }
        public virtual void Error(Exception e, string functionName) { Log(LogLevel.Error, e, functionName); }
        public virtual void Error(string message) { Log(LogLevel.Error, message); }
        public virtual void Error(string format, params object[] args) { Log(LogLevel.Error, string.Format(format, args)); }

    }

}
