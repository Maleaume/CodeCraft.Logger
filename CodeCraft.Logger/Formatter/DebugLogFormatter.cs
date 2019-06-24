namespace CodeCraft.Logger.Formatter
{
    sealed class DebugLogFormatter : LevelLogFormatter
    {
        public override ElogLevel LogLevel => ElogLevel.Debug;
    }
}
