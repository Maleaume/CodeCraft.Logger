using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using CodeCraft.Logger.LogFormatter;

namespace CodeCraft.Logger
{
    public enum ElogLevel
    {
        Trace,
        Info,
        Debug,
        Warning,
        Error,
        Critical
    }

    public interface ILogger
    {
        void Trace(string log);
        void Info(string log);
        void Debug(string log);
        void Warn(string log);
        void Error(string log);
        void Critical(string log);
    }

    public abstract class BaseLogger : IDisposable
    {
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

        protected BaseLogger()
        {

        }

        protected string TraceFormat(string log) => TraceLogger.FormatLog(log);
        protected string InfoFormat(string log) => InfoLogger.FormatLog(log);
        protected string DebugFormat(string log) => DebugLogger.FormatLog(log);
        protected string WarnFormat(string log) => WarningLogger.FormatLog(log);
        protected string ErrorFormat(string log) => ErrorLogger.FormatLog(log);
        protected string CriticalFormat(string log) => CriticalLogger.FormatLog(log);
        public virtual  void Dispose()
        {
            terminate.Set();
           
        }
    }

    public class ConsoleLogger : BaseLogger
    {
        delegate string Format(string log);

        private ConcurrentQueue<(string log, ILevelLogFormatter levelLogger)> LogsQueue = new ConcurrentQueue<(string log, ILevelLogFormatter levelLogger)>();
 

        WaitHandle[] WaitHandleEvent;
        Thread loggingThread;
        public override void Dispose()
        {
            base.Dispose();

            continueProcess = false; 
           /* while (!LogsQueue.IsEmpty  )
            {
                LogsQueue.TryDequeue(out (string log, ILevelLogFormatter levelLogger) formatLog);
                Log(formatLog.levelLogger.FormatLog(formatLog.log));
            }*/
        }
        private void Log(string log)
        {
            System.Diagnostics.Debug.WriteLine(log);
        }


        public ConsoleLogger()
        {
            WaitHandleEvent = new WaitHandle[] { hasNewItems, terminate };
            loggingThread = new Thread(new ThreadStart(ProcessQueue))
            {
                IsBackground = true,
                Name = "LoggerThread"
            };
            // this is performed from a bg thread, to ensure the queue is serviced from a single thread
            loggingThread.Start();
        }
        private bool continueProcess = true;
        private void ProcessQueue()
        {
            try
            {
                int i = -1;
                while (true)
                {
                     waiting.Set();
                 
                      i = WaitHandle.WaitAny(WaitHandleEvent,-1);
                    System.Diagnostics.Debug.WriteLine(i);
                    if (i ==1)
                        return;
                   hasNewItems.Reset();
                    waiting.Reset() ;
                    while 
                 (LogsQueue.TryDequeue(out (string log, ILevelLogFormatter levelLogger) formatLog) && continueProcess)
                    {
                        Log(formatLog.levelLogger.FormatLog(formatLog.log));
                    }
                 
                 
                   
 
                }
            }
            catch (ThreadAbortException ex)
            {



            }
        }

        public void Trace(string log) => EnqueueLog(log, TraceLogger);
        public void Info(string log) => EnqueueLog(log, InfoLogger);
        public void Debug(string log) => EnqueueLog(log, DebugLogger);
        public void Warn(string log) => EnqueueLog(log, WarningLogger);
        public void Error(string log) => EnqueueLog(log, ErrorLogger);
        public void Critical(string log) => EnqueueLog(log, CriticalLogger);

        private void EnqueueLog(string log, ILevelLogFormatter levelLogger)
        {
            LogsQueue.Enqueue((log, levelLogger));
            hasNewItems.Set();
        }


      
    }

}
