using System;
using CodeCraft.Logger.Formatter;
using CodeCraft.Logger.ProducerConsumer;

namespace CodeCraft.Logger
{
    public abstract class BaseLogger<T> : ILogger, IDisposable
         where T : ILogProducerConsumer, new()
    {
        private bool disposed = false;
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

        protected void Produce(string log, ILevelLogFormatter levelLogger)
            => logProducerConsumer.Produce(levelLogger.FormatLog(log));

        public void Trace(string log) => Produce(log, TraceLogFormatter);
        public void Info(string log) => Produce(log, InfoLogFormatter);
        public void Debug(string log) => Produce(log, DebugLogFormatter);
        public void Warn(string log) => Produce(log, WarningLogFormatter);
        public void Error(string log) => Produce(log, ErrorLogFormatter);
        public void Critical(string log) => Produce(log, CriticalLogFormatter);

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                // Dispose of resources held by this instance.
                logProducerConsumer.Dispose();
                disposed = true; 

            }
        }
        // Dispose of resources held by this instance.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


            // Disposable types implement a finalizer.
            ~BaseLogger() => Dispose(false);
    }
}
