using System;
using CodeCraft.Logger.Formatter;
using CodeCraft.Logger.ProducerConsumer;

namespace CodeCraft.Logger
{
    public abstract class BaseLogger<T> : ILogger, IDisposable
         where T : ILogProducerConsumer, new()
    {
        protected readonly T logProducerConsumer = new T();

        #region Lazy Objects
        private readonly Lazy<ILevelLogFormatter> traceLogFormatter = new Lazy<ILevelLogFormatter>(() => InstanciateLevelLoger(ElogLevel.Trace), true);
        private readonly Lazy<ILevelLogFormatter> infoLogFormatter = new Lazy<ILevelLogFormatter>(() => InstanciateLevelLoger(ElogLevel.Info), true);
        private readonly Lazy<ILevelLogFormatter> debugLogFormatter = new Lazy<ILevelLogFormatter>(() => InstanciateLevelLoger(ElogLevel.Debug), true);
        private readonly Lazy<ILevelLogFormatter> warningLogFormatter = new Lazy<ILevelLogFormatter>(() => InstanciateLevelLoger(ElogLevel.Warning), true);
        private readonly Lazy<ILevelLogFormatter> errorLogFormatter = new Lazy<ILevelLogFormatter>(() => InstanciateLevelLoger(ElogLevel.Error), true);
        private readonly Lazy<ILevelLogFormatter> criticalLogFormatter = new Lazy<ILevelLogFormatter>(() => InstanciateLevelLoger(ElogLevel.Critical), true);
        #endregion
        #region Accessors
        protected ILevelLogFormatter TraceLogFormatter => traceLogFormatter.Value;
        protected ILevelLogFormatter InfoLogFormatter => infoLogFormatter.Value;
        protected ILevelLogFormatter DebugLogFormatter => debugLogFormatter.Value;
        protected ILevelLogFormatter WarningLogFormatter => warningLogFormatter.Value;
        protected ILevelLogFormatter ErrorLogFormatter => errorLogFormatter.Value;
        protected ILevelLogFormatter CriticalLogFormatter => criticalLogFormatter.Value;
        #endregion

        private static ILevelLogFormatter InstanciateLevelLoger(ElogLevel logLevel)
            => LevelLogFormatterFactory.Instance.Instanciate(logLevel);

        protected void EnqueueLog(string log, ILevelLogFormatter levelLogger)
            => logProducerConsumer.Enqueue(levelLogger.FormatLog(log));

        public void Trace(string log) => EnqueueLog(log, TraceLogFormatter);
        public void Info(string log) => EnqueueLog(log, InfoLogFormatter);
        public void Debug(string log) => EnqueueLog(log, DebugLogFormatter);
        public void Warn(string log) => EnqueueLog(log, WarningLogFormatter);
        public void Error(string log) => EnqueueLog(log, ErrorLogFormatter);
        public void Critical(string log) => EnqueueLog(log, CriticalLogFormatter);

        public void Dispose() => logProducerConsumer.Dispose();
    }
}
