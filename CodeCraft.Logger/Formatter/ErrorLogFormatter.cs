namespace CodeCraft.Logger.Formatter
{
    sealed class ErrorLogFormatter : LevelLogFormatter
    {
        protected override ElogLevel LogLevel => ElogLevel.Error;
    }
}
