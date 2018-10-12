namespace CodeCraft.Logger.LogFormatter
{
   sealed class WarningFormatter : LevelLogFormatter
    {
        protected override ElogLevel LogLevel => ElogLevel.Warning;
    }

}
