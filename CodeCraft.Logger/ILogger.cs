namespace CodeCraft.Logger
{
    public interface ILogger
    {
        ElogLevel MaxLogLevel { get; set; }

        void Trace(string log);
        void Info(string log);
        void Debug(string log);
        void Warn(string log);
        void Error(string log);
        void Critical(string log);
    } 
}
