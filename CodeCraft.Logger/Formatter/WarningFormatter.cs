namespace CodeCraft.Logger.Formatter
{
    sealed class WarningFormatter : LevelLogFormatter
    {
        protected override ElogLevel LogLevel => ElogLevel.Warning;
    }
}
