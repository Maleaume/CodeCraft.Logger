using System;

namespace CodeCraft.Logger
{ 
    public class ConsoleLogger : BaseLogger
    {
        protected override void WriteLog(string log) => Console.WriteLine(log);
    }
}
