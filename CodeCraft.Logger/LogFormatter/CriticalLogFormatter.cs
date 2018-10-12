namespace CodeCraft.Logger.LogFormatter
{
    sealed class CriticalLogFormatter : LevelLogFormatter
    {
        protected override ElogLevel LogLevel => ElogLevel.Critical;
    }

}
