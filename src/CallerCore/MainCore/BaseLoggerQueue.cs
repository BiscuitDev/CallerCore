using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CallerCore.MainCore
{
    public enum LogStatus
    {
        Stopped,
        Running
    }

    public abstract class BaseLoggerQueue : ILogger
    {
        protected volatile LogStatus LogStatus;
        protected volatile bool ShouldStop;
        protected ConcurrentQueue<string> MessageQueue = new ConcurrentQueue<string>();
        protected AutoResetEvent QueueNotifier = new AutoResetEvent(false);
        public static BaseLoggerQueue LoggerQueue;
        protected int LogLevelValue;
        protected object ProcessLock = new object();
        protected int WaitPause { get; set; } = 600000;
        protected IInfoContext info;
        protected virtual long FileLongLength { get; set; } = 0;
        public virtual int RepeatErrorLimit { get; set; } = 3;
        private Task _task;

        public void Stop()
        {

            try
            {
                ShouldStop = true;
                QueueNotifier.Set();
                if (_task != null)
                    _task.Wait(5000); // wait for the task to complete
            }
            finally
            {
                LogStatus = LogStatus.Stopped;
                _task = null;
            }

        }

        public void Start()
        {
            ShouldStop = false;
            if (_task == null)
            {
                _task = Task.Factory.StartNew(run);
            }

        }

        public virtual void run()
        {
            if (LogStatus == LogStatus.Running)
                return;
            lock (ProcessLock)
            {
                LogStatus = LogStatus.Running;
                string svalue;
                List<string> line = new List<string>();

                while (!ShouldStop)
                {
                    while (MessageQueue.Count == 0 && !ShouldStop)
                    {
                        CheckLogFile();
                        QueueNotifier.WaitOne(WaitPause);
                    }
                    while (MessageQueue.TryDequeue(out svalue))
                    {
                        FileLongLength = FileLongLength + svalue.Length + Environment.NewLine.Length;
                        line.Add(svalue);
                    }
                    if (line.Count > 0)
                    {
                        try
                        {
                            WriteLogToFile(line);
                        }
                        catch (Exception e)
                        {
                            Log(LogLevel.Error, e, null);
                        }
                        line.Clear();
                    }

                }
                LogStatus = LogStatus.Stopped;
            }
        }

        public virtual void InitLog(IInfoContext infoctx)
        {
            LoggerQueue = this;
            info = infoctx;
            Start();
        }

        public void SetLogLevel(LogLevel level)
        {
            LogLevelValue = (int)level;
        }

        public virtual void WriteInfo(string level, string message)
        {
            throw new NotImplementedException();
        }

        public virtual void WriteInfo(string level, List<string> messages)
        {
            throw new NotImplementedException();
        }

        public virtual void WriteError(string level, Exception message)
        {
            throw new NotImplementedException();
        }

        public virtual void WriteError(string level, List<Exception> messages)
        {
            throw new NotImplementedException();
        }

        public virtual void Log(LogLevel logLevel, string message)
        {
            if (LogLevelValue >= (int)logLevel)
            {
                MessageQueue.Enqueue($"[{DateTime.UtcNow.ToString("u", CultureInfo.InvariantCulture)}][{logLevel}]{message}");
                QueueNotifier.Set();
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
                ErrorObject.CountError++;


            if (ErrorObject.CountError <= RepeatErrorLimit)
            {
                var addError = functionName != null ? $"[{functionName}]" : string.Empty;
                if (ErrorObject.CountError == RepeatErrorLimit) addError = $"{addError}[*REPEATED*]";

                Log(logLevel, $"{addError}Exception:{e.Message} StackTrace:{e.StackTrace}");
            }
        }

        public virtual void WriteLogToFile(string message)
        {
            WriteLogToFile(new List<string> { message });
        }

        public virtual void WriteLogToFile(List<string> messages)
        {
            foreach (var message in messages)
                System.Diagnostics.Debug.WriteLine($"{message}");
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


        public abstract string GetPathFileLog();
        public abstract void CheckLogFile();
        public abstract void InitLogFileBytes();


    }
}
