using System;
using CodeCraft.Logger.Formatter;
using CodeCraft.Logger.ProducerConsumer;

namespace CodeCraft.Logger
{
    public abstract class BaseLogger<T> : ILogger, IDisposable
         where T : ILogProducerConsumer, new()
    {
        private readonly T logProducerConsumer = new T();

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

        protected void EnqueueLog(string log, ILevelLogFormatter levelLogger)
            => logProducerConsumer.Enqueue(levelLogger.FormatLog(log));

        protected abstract void WriteLog(string log);

        public void Trace(string log) => EnqueueLog(log, TraceLogger);
        public void Info(string log) => EnqueueLog(log, InfoLogger);
        public void Debug(string log) => EnqueueLog(log, DebugLogger);
        public void Warn(string log) => EnqueueLog(log, WarningLogger);
        public void Error(string log) => EnqueueLog(log, ErrorLogger);
        public void Critical(string log) => EnqueueLog(log, CriticalLogger);

        public void Dispose() => logProducerConsumer.Dispose();
    }
}
