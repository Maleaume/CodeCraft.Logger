namespace CodeCraft.Logger.LogFormatter
{
    sealed class InfoLogFormatter : LevelLogFormatter
    {
        protected override ElogLevel LogLevel => ElogLevel.Info;
    }

}
