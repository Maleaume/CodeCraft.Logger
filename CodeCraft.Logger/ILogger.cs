namespace CodeCraft.Logger
{
    /// <summary>
    /// Logger interface. 
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Max degree of log to generate
        /// </summary>
        ElogLevel MaxLogLevel { get; set; }
 
        void Trace(string log);
        void Info(string log);
        void Debug(string log);
        void Warn(string log);
        void Error(string log);
        void Critical(string log);
    } 
}
