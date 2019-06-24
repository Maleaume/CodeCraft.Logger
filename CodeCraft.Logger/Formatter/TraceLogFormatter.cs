namespace CodeCraft.Logger.Formatter
{
    sealed class TraceLogFormatter : LevelLogFormatter
    {
        public override ElogLevel LogLevel => ElogLevel.Trace;
    }
}
