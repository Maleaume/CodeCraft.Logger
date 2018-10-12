namespace CodeCraft.Logger.LogFormatter
{
    sealed class ErrorLogFormatter : LevelLogFormatter
    {
        protected override ElogLevel LogLevel => ElogLevel.Error;
    }

}
