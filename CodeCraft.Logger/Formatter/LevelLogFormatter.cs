using System;

namespace CodeCraft.Logger.Formatter
{
    abstract class LevelLogFormatter : ILevelLogFormatter
    {
        public abstract ElogLevel LogLevel { get; }

        public string FormatLog(string log) => $"[{DateTime.Now:dd/MM/yyyy hh:mm:ss.fff}][{LogLevel}]: {log}";
    }
}
