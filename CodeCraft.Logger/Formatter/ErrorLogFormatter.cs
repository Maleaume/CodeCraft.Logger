namespace CodeCraft.Logger.Formatter
{
    sealed class ErrorLogFormatter : LevelLogFormatter
    {
        public override ElogLevel LogLevel => ElogLevel.Error;
    }
}
