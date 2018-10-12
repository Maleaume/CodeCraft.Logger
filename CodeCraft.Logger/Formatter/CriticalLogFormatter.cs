namespace CodeCraft.Logger.Formatter
{
    sealed class CriticalLogFormatter : LevelLogFormatter
    {
        protected override ElogLevel LogLevel => ElogLevel.Critical;
    }
}
