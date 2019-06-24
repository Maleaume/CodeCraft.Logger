namespace CodeCraft.Logger.Formatter
{
    sealed class CriticalLogFormatter : LevelLogFormatter
    {
        public override ElogLevel LogLevel => ElogLevel.Critical;
    }
}
