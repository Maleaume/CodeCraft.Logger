namespace CodeCraft.Logger.Formatter
{
    sealed class InfoLogFormatter : LevelLogFormatter
    {
        public override ElogLevel LogLevel => ElogLevel.Info;
    }
}
