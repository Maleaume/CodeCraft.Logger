using System;

namespace CodeCraft.Logger.LogFormatter
{
    abstract class LevelLogFormatter : ILevelLogFormatter
    {
        protected abstract ElogLevel LogLevel { get; }

        public string FormatLog(string log) => $"[{DateTime.Now}][{LogLevel}]: {log}";
    }

}
