namespace CodeCraft.Logger.Formatter
{
    public interface ILevelLogFormatter
    {
        string FormatLog(string log); 
        ElogLevel LogLevel { get; }
    }
}
