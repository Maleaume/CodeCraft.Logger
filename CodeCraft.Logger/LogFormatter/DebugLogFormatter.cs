namespace CodeCraft.Logger.LogFormatter
{
    sealed class DebugLogFormatter : LevelLogFormatter
    {
        protected override ElogLevel LogLevel => ElogLevel.Debug;
    }

}
