namespace CodeCraft.Logger.Formatter
{
    sealed class DebugLogFormatter : LevelLogFormatter
    {
        protected override ElogLevel LogLevel => ElogLevel.Debug;
    }
}
