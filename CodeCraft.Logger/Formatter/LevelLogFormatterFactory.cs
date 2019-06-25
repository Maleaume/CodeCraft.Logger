using System;

namespace CodeCraft.Logger.Formatter
{
    public class LevelLogFormatterFactory
    {
        private static readonly Lazy<LevelLogFormatterFactory> instance = new Lazy<LevelLogFormatterFactory>(() => new LevelLogFormatterFactory(), true);
        public static LevelLogFormatterFactory Instance => instance.Value;
        private LevelLogFormatterFactory() { }

        public ILevelLogFormatter Instanciate(ElogLevel logLevel)
        {
            switch (logLevel)
            {
                case ElogLevel.Trace: return new TraceLogFormatter();
                case ElogLevel.Info: return new InfoLogFormatter();
                case ElogLevel.Debug: return new DebugLogFormatter();
                case ElogLevel.Warning: return new WarningFormatter();
                case ElogLevel.Error: return new ErrorLogFormatter();
                case ElogLevel.Critical: return new CriticalLogFormatter();
                default: throw new ArgumentException("ElogLevel value is not valid", "logLevel");
            }
        }
    }
}
