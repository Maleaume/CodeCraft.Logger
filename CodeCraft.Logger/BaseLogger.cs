using System;
using System.Collections.Concurrent;
using System.Threading;
using CodeCraft.Logger.LogFormatter;

namespace CodeCraft.Logger
{


    public abstract class BaseLogger :ILogger,  IDisposable
    {
        readonly WaitHandle[] WaitHandleEvent;
        readonly Thread LoggingThread;

        protected ManualResetEvent hasNewItems = new ManualResetEvent(false);
        protected ManualResetEvent terminate = new ManualResetEvent(false);
        protected ManualResetEvent waiting = new ManualResetEvent(false);

        #region Lazy Objects
        private readonly Lazy<ILevelLogFormatter> traceLogger = new Lazy<ILevelLogFormatter>(() => InstanciateLevelLoger(ElogLevel.Trace), true);
        private readonly Lazy<ILevelLogFormatter> infoLogger = new Lazy<ILevelLogFormatter>(() => InstanciateLevelLoger(ElogLevel.Info), true);
        private readonly Lazy<ILevelLogFormatter> debugLogger = new Lazy<ILevelLogFormatter>(() => InstanciateLevelLoger(ElogLevel.Debug), true);
        private readonly Lazy<ILevelLogFormatter> warningLogger = new Lazy<ILevelLogFormatter>(() => InstanciateLevelLoger(ElogLevel.Warning), true);
        private readonly Lazy<ILevelLogFormatter> errorLogger = new Lazy<ILevelLogFormatter>(() => InstanciateLevelLoger(ElogLevel.Error), true);
        private readonly Lazy<ILevelLogFormatter> criticalLogger = new Lazy<ILevelLogFormatter>(() => InstanciateLevelLoger(ElogLevel.Critical), true);
        #endregion
        #region Accessors
        protected ILevelLogFormatter TraceLogger => traceLogger.Value;
        protected ILevelLogFormatter InfoLogger => infoLogger.Value;
        protected ILevelLogFormatter DebugLogger => debugLogger.Value;
        protected ILevelLogFormatter WarningLogger => warningLogger.Value;
        protected ILevelLogFormatter ErrorLogger => errorLogger.Value;
        protected ILevelLogFormatter CriticalLogger => criticalLogger.Value;
        #endregion

        private static ILevelLogFormatter InstanciateLevelLoger(ElogLevel logLevel)
            => LevelLogFormatterFactory.Instance.Instanciate(logLevel);

        private ConcurrentQueue<string> LogsQueue = new ConcurrentQueue<string>();

        protected BaseLogger()
        {
            WaitHandleEvent = new WaitHandle[] { hasNewItems, terminate };
            LoggingThread = new Thread(new ThreadStart(ProcessQueue))
            {
                IsBackground = true,
                Name = "LoggerThread"
            };
            LoggingThread.Start();
        }

        private void ProcessQueue()
        {
            int i = -1;
            while (true)
            {
                waiting.Set();
                i = WaitHandle.WaitAny(WaitHandleEvent, -1);
                hasNewItems.Reset();
                waiting.Reset();
                DeQueue();
                if (i == 1)
                    return;
            }
        }

        private void DeQueue()
        {
            while (LogsQueue.TryDequeue(out string log))
                WriteLog(log);
        }

        protected void EnqueueLog(string log, ILevelLogFormatter levelLogger)
        {
            LogsQueue.Enqueue(levelLogger.FormatLog(log));
            hasNewItems.Set();
        }

        protected abstract void WriteLog(string log);

        public void Trace(string log) => EnqueueLog(log, TraceLogger);
        public void Info(string log) => EnqueueLog(log, InfoLogger);
        public void Debug(string log) => EnqueueLog(log, DebugLogger);
        public void Warn(string log) => EnqueueLog(log, WarningLogger);
        public void Error(string log) => EnqueueLog(log, ErrorLogger);
        public void Critical(string log) => EnqueueLog(log, CriticalLogger);

        public virtual void Dispose()
        {
            terminate.Set();
            while (LoggingThread.IsAlive) ; 
        }
    }

}
