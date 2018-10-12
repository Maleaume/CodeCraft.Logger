namespace CodeCraft.Logger.Formatter
{
    sealed class InfoLogFormatter : LevelLogFormatter
    {
        protected override ElogLevel LogLevel => ElogLevel.Info;
    }
}
