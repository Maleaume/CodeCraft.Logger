namespace CodeCraft.Logger.Formatter
{
    sealed class TraceLogFormatter : LevelLogFormatter
    {
        protected override ElogLevel LogLevel => ElogLevel.Trace;
    }
}
