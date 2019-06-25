using System;

namespace CodeCraft.Logger.ProducerConsumer
{
    public sealed class ConsoleLogProducerConsumer : LogProducerConsumer
    {

        protected override void WriteLog(string log) { Console.WriteLine(log); }
    }

    public delegate void FilePathCompletedEventHandler(object sender, FilePathEventArgs e);
}
