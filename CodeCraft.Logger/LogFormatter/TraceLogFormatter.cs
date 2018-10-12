namespace CodeCraft.Logger.LogFormatter
{
    sealed class TraceLogFormatter : LevelLogFormatter
    {
        protected override ElogLevel LogLevel => ElogLevel.Trace;
    }

}
