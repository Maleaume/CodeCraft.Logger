namespace CodeCraft.Logger.Formatter
{
    sealed class WarningFormatter : LevelLogFormatter
    {
        public override ElogLevel LogLevel => ElogLevel.Warning;
    }
}
